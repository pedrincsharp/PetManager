/**
 * Serviço de Autenticação - Camada de Aplicação
 * Lógica de negócio de autenticação
 */

import authService from "../../infrastructure/services/authService";
import TokenManager from "../../core/TokenManager";
import { AuthTokens } from "../dtos/auth.dto";

class AuthApplicationService {
  /**
   * Realiza o login e armazena os tokens
   */
  async login(username: string, password: string): Promise<AuthTokens> {
    const tokenResponse = await authService.login(username, password);

    const tokens: AuthTokens = {
      accessToken: tokenResponse.accessToken,
      refreshToken: tokenResponse.refreshToken,
      expiresAt: new Date(tokenResponse.expiresAt),
    };

    TokenManager.setTokens(tokens);
    return tokens;
  }

  /**
   * Verifica se o usuário está autenticado
   */
  isAuthenticated(): boolean {
    const token = TokenManager.getAccessToken();
    return !!token;
  }

  /**
   * Realiza logout
   */
  logout(): void {
    TokenManager.clearTokens();
  }

  /**
   * Obtém o token de acesso atual
   */
  getAccessToken(): string | null {
    return TokenManager.getAccessToken();
  }
}

export default new AuthApplicationService();
