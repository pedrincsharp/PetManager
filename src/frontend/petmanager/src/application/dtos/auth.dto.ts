/**
 * DTOs (Data Transfer Objects) para autenticação
 */

export interface LoginRequestDto {
  username: string;
  password: string;
}

export interface TokenResponseDto {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface ApiResponse<T> {
  status: string;
  message: string;
  data: T | null;
}

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
  expiresAt: Date;
}
