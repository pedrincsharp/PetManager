/**
 * Serviço de Autenticação - Camada de Infraestrutura
 * Interface com a API de autenticação
 */

import apiClient from "../api/apiClient";
import {
  ApiResponse,
  LoginRequestDto,
  TokenResponseDto,
} from "../../application/dtos/auth.dto";

class AuthService {
  /**
   * Realiza login com username e password
   */
  async login(username: string, password: string): Promise<TokenResponseDto> {
    const loginRequest: LoginRequestDto = { username, password };

    try {
      const response = await apiClient.post<ApiResponse<TokenResponseDto>>(
        "/auth/login",
        loginRequest,
        false,
      );

      if (response.status !== "200" || !response.data) {
        throw new Error(response.message || "Falha ao fazer login");
      }

      return response.data;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
      throw new Error("Erro ao fazer login");
    }
  }

  /**
   * Obtém token inicial com API Key
   */
  async getInitialToken(apiKey: string): Promise<TokenResponseDto> {
    try {
      const response = await apiClient.post<ApiResponse<TokenResponseDto>>(
        "/auth/token",
        { apiKey },
        false,
      );

      if (response.status !== "200" || !response.data) {
        throw new Error(response.message || "Falha ao obter token");
      }

      return response.data;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
      throw new Error("Erro ao obter token");
    }
  }

  /**
   * Renova o token de acesso usando o refresh token
   */
  async refreshToken(refreshToken: string): Promise<TokenResponseDto> {
    try {
      const response = await apiClient.post<ApiResponse<TokenResponseDto>>(
        "/auth/refresh",
        { refreshToken },
        false,
      );

      if (response.status !== "200" || !response.data) {
        throw new Error(response.message || "Falha ao renovar token");
      }

      return response.data;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
      throw new Error("Erro ao renovar token");
    }
  }
}

export default new AuthService();
