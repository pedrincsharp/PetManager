/**
 * TokenManager - Singleton para gerenciar tokens de autenticação
 * Responsável por armazenar, validar e renovar tokens JWT
 */

import { AuthTokens } from "../application/dtos/auth.dto";

class TokenManager {
  private static instance: TokenManager;
  private tokens: AuthTokens | null = null;
  private refreshTimeout: ReturnType<typeof setTimeout> | null = null;
  private refreshCallbacks: Array<(tokens: AuthTokens) => void> = [];

  private constructor() {
    this.loadTokensFromStorage();
  }

  /**
   * Obtém a instância singleton do TokenManager
   */
  static getInstance(): TokenManager {
    if (!TokenManager.instance) {
      TokenManager.instance = new TokenManager();
    }
    return TokenManager.instance;
  }

  /**
   * Define os tokens e inicia o monitoramento de expiração
   */
  setTokens(tokens: AuthTokens): void {
    this.tokens = tokens;
    this.saveTokensToStorage(tokens);
    this.setupRefreshTimer();
  }

  /**
   * Obtém o token de acesso atual
   */
  getAccessToken(): string | null {
    if (!this.tokens) return null;

    if (this.isTokenExpired(this.tokens.expiresAt)) {
      return null;
    }

    return this.tokens.accessToken;
  }

  /**
   * Obtém o refresh token
   */
  getRefreshToken(): string | null {
    return this.tokens?.refreshToken || null;
  }

  /**
   * Obtém todos os tokens
   */
  getTokens(): AuthTokens | null {
    return this.tokens;
  }

  /**
   * Verifica se o token de acesso expirou
   */
  isTokenExpired(expiresAt?: Date): boolean {
    const expiredAt = expiresAt || this.tokens?.expiresAt;
    if (!expiredAt) return true;

    const expiresDate = new Date(expiredAt);
    // Considera expirado 5 minutos antes da data real para evitar edge cases
    const bufferTime = 5 * 60 * 1000;
    return new Date().getTime() > expiresDate.getTime() - bufferTime;
  }

  /**
   * Verifica se há um refresh token válido
   */
  hasValidRefreshToken(): boolean {
    return !this.isTokenExpired() || this.tokens?.refreshToken !== null;
  }

  /**
   * Limpa os tokens (logout)
   */
  clearTokens(): void {
    this.tokens = null;
    this.clearRefreshTimer();
    localStorage.removeItem("petmanager_tokens");
  }

  /**
   * Registra callback para quando tokens forem renovados
   */
  onTokenRefreshed(callback: (tokens: AuthTokens) => void): () => void {
    this.refreshCallbacks.push(callback);
    return () => {
      this.refreshCallbacks = this.refreshCallbacks.filter(
        (cb) => cb !== callback,
      );
    };
  }

  /**
   * Notifica todos os callbacks de renovação de tokens
   */
  private notifyTokenRefreshed(): void {
    if (this.tokens) {
      this.refreshCallbacks.forEach((callback) => callback(this.tokens!));
    }
  }

  /**
   * Configura um timer para renovar o token antes de expirar
   */
  private setupRefreshTimer(): void {
    this.clearRefreshTimer();

    if (!this.tokens) return;

    const expiresAt = new Date(this.tokens.expiresAt);
    const now = new Date();
    const timeUntilExpiry = expiresAt.getTime() - now.getTime();

    // Renova token 10 minutos antes de expirar
    const refreshTime = Math.max(timeUntilExpiry - 10 * 60 * 1000, 1000);

    this.refreshTimeout = setTimeout(() => {
      this.notifyTokenRefreshed();
    }, refreshTime);
  }

  /**
   * Limpa o timer de renovação
   */
  private clearRefreshTimer(): void {
    if (this.refreshTimeout) {
      clearTimeout(this.refreshTimeout);
      this.refreshTimeout = null;
    }
  }

  /**
   * Carrega tokens do localStorage
   */
  private loadTokensFromStorage(): void {
    try {
      const stored = localStorage.getItem("petmanager_tokens");
      if (stored) {
        const tokens = JSON.parse(stored) as AuthTokens;
        tokens.expiresAt = new Date(tokens.expiresAt);

        // Apenas carrega se ainda não expirou (com buffer)
        if (!this.isTokenExpired(tokens.expiresAt)) {
          this.tokens = tokens;
          this.setupRefreshTimer();
        } else {
          localStorage.removeItem("petmanager_tokens");
        }
      }
    } catch (error) {
      console.error("Erro ao carregar tokens do localStorage:", error);
      localStorage.removeItem("petmanager_tokens");
    }
  }

  /**
   * Salva tokens no localStorage
   */
  private saveTokensToStorage(tokens: AuthTokens): void {
    try {
      localStorage.setItem("petmanager_tokens", JSON.stringify(tokens));
    } catch (error) {
      console.error("Erro ao salvar tokens no localStorage:", error);
    }
  }
}

export default TokenManager.getInstance();
