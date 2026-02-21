/**
 * Configuração de ambiente da aplicação
 * Contém URLs, chaves e constantes de comportamento
 */

const API_BASE_URL =
  import.meta.env.VITE_API_URL || "http://localhost:5225/api";
const API_KEY =
  import.meta.env.VITE_API_KEY ||
  "84989c9b29ab33524b3d54ef8d3eb8cad50e8de61cfab7d33b74f1afd25bd44b";

// Tempo de expiração da sessão em minutos
const SESSION_TIMEOUT_MINUTES = 60;

// Header de autenticação
const AUTH_HEADER = "Authorization";

export const environment = {
  apiBaseUrl: API_BASE_URL,
  apiKey: API_KEY,
  sessionTimeout: SESSION_TIMEOUT_MINUTES,
  authHeader: AUTH_HEADER,
};
