/**
 * Cliente HTTP para chamadas à API
 * Responsável por gerenciar requisições, autenticação e refresh de tokens
 */

import { ApiResponse, TokenResponseDto } from "../../application/dtos/auth.dto";
import TokenManager from "../../core/TokenManager";
import { environment } from "../../config/environment";

class ApiClient {
  private baseUrl: string;
  private isRefreshing = false;
  private refreshSubscribers: Array<(token: string) => void> = [];

  constructor() {
    this.baseUrl = environment.apiBaseUrl;
  }

  /**
   * Faz uma requisição GET
   */
  async get<T>(endpoint: string, withAuth = true): Promise<T> {
    return this.request<T>(endpoint, "GET", undefined, withAuth);
  }

  /**
   * Faz uma requisição POST
   */
  async post<T>(endpoint: string, data?: unknown, withAuth = true): Promise<T> {
    return this.request<T>(endpoint, "POST", data, withAuth);
  }

  /**
   * Faz uma requisição PUT
   */
  async put<T>(endpoint: string, data?: unknown, withAuth = true): Promise<T> {
    return this.request<T>(endpoint, "PUT", data, withAuth);
  }

  /**
   * Faz uma requisição DELETE
   */
  async delete<T>(endpoint: string, withAuth = true): Promise<T> {
    return this.request<T>(endpoint, "DELETE", undefined, withAuth);
  }

  /**
   * Requisição genérica
   */
  private async request<T>(
    endpoint: string,
    method: string,
    data?: unknown,
    withAuth = true,
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;
    const headers: HeadersInit = {
      "Content-Type": "application/json",
    };

    if (withAuth) {
      const token = TokenManager.getAccessToken();
      if (token) {
        headers[environment.authHeader] = `Bearer ${token}`;
      }
    }

    try {
      const response = await fetch(url, {
        method,
        headers,
        body: data ? JSON.stringify(data) : undefined,
      });

      // Se token expirou, tenta renovar
      if (response.status === 401 && withAuth) {
        return this.handleTokenExpiry<T>(endpoint, method, data, withAuth);
      }

      if (!response.ok) {
        throw new ApiError(response.status, response.statusText);
      }

      return (await response.json()) as T;
    } catch (error) {
      throw this.handleError(error);
    }
  }

  /**
   * Gerencia a expiração do token e faz refresh
   */
  private async handleTokenExpiry<T>(
    endpoint: string,
    method: string,
    data: unknown,
    withAuth: boolean,
  ): Promise<T> {
    // Se já está tentando renovar, aguarda
    if (this.isRefreshing) {
      return new Promise((resolve, reject) => {
        this.refreshSubscribers.push(() => {
          this.request<T>(endpoint, method, data, withAuth)
            .then(resolve)
            .catch(reject);
        });
      });
    }

    this.isRefreshing = true;

    try {
      const refreshToken = TokenManager.getRefreshToken();
      if (!refreshToken) {
        throw new Error("Refresh token não encontrado");
      }

      const response = await fetch(`${this.baseUrl}/auth/refresh`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ refreshToken }),
      });

      if (!response.ok) {
        TokenManager.clearTokens();
        throw new Error("Falha ao renovar token");
      }

      const result = (await response.json()) as ApiResponse<TokenResponseDto>;
      const newTokens = result.data;

      if (!newTokens) {
        throw new Error("Falha ao renovar token");
      }

      TokenManager.setTokens({
        accessToken: newTokens.accessToken,
        refreshToken: newTokens.refreshToken,
        expiresAt: new Date(newTokens.expiresAt),
      });

      // Notifica subscribers
      this.refreshSubscribers.forEach((cb) => cb(newTokens.accessToken));
      this.refreshSubscribers = [];

      // Retry original request
      return this.request<T>(endpoint, method, data, withAuth);
    } catch (error) {
      TokenManager.clearTokens();
      throw error;
    } finally {
      this.isRefreshing = false;
    }
  }

  /**
   * Processa erros de requisição
   */
  private handleError(error: unknown): Error {
    if (error instanceof ApiError) {
      return error;
    }
    if (error instanceof Error) {
      return error;
    }
    return new Error("Erro desconhecido na requisição");
  }
}

export class ApiError extends Error {
  constructor(
    public status: number,
    public statusText: string,
  ) {
    super(`API Error: ${status} ${statusText}`);
    this.name = "ApiError";
  }
}

export default new ApiClient();
