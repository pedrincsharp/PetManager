/**
 * Constantes de códigos de erro HTTP
 */

export const HTTP_STATUS = {
  OK: 200,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  INTERNAL_SERVER_ERROR: 500,
  SERVICE_UNAVAILABLE: 503,
};

export const ERROR_TYPES = {
  VALIDATION: "VALIDATION",
  AUTHENTICATION: "AUTHENTICATION",
  AUTHORIZATION: "AUTHORIZATION",
  NOT_FOUND: "NOT_FOUND",
  SERVER_ERROR: "SERVER_ERROR",
  NETWORK_ERROR: "NETWORK_ERROR",
  UNKNOWN: "UNKNOWN",
};

export const ERROR_MESSAGES = {
  INVALID_CREDENTIALS: "Usuário ou senha incorretos",
  SESSION_EXPIRED: "Sua sessão expirou. Faça login novamente",
  UNAUTHORIZED: "Você não tem permissão para acessar este recurso",
  SERVER_ERROR: "Erro interno do servidor. Tente novamente mais tarde",
  NETWORK_ERROR: "Erro de conexão. Verifique sua internet",
  VALIDATION_ERROR: "Dados inválidos. Verifique os campos",
  UNKNOWN_ERROR: "Erro desconhecido. Tente novamente",
};
