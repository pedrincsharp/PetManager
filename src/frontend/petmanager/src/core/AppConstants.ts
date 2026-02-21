/**
 * Constantes de Aplicação
 * Enums, tipos e constantes do sistema
 */

export enum UserRole {
  Administrator = 0,
  Attendant = 1,
  Veterinarian = 2,
}

export const ROLE_LABELS: Record<UserRole, string> = {
  [UserRole.Administrator]: "Administrador",
  [UserRole.Attendant]: "Atendente",
  [UserRole.Veterinarian]: "Veterinário",
};

export const USER_ROLES = [
  { value: UserRole.Administrator, label: ROLE_LABELS[UserRole.Administrator] },
  { value: UserRole.Attendant, label: ROLE_LABELS[UserRole.Attendant] },
  { value: UserRole.Veterinarian, label: ROLE_LABELS[UserRole.Veterinarian] },
];

export enum UserStatus {
  Active = "Active",
  Inactive = "Inactive",
  Suspended = "Suspended",
}

export const STATUS_LABELS: Record<UserStatus, string> = {
  [UserStatus.Active]: "Ativo",
  [UserStatus.Inactive]: "Inativo",
  [UserStatus.Suspended]: "Suspenso",
};

// Timeouts em milissegundos
export const TIMEOUTS = {
  SHORT: 3000, // 3 segundos
  MEDIUM: 5000, // 5 segundos
  LONG: 10000, // 10 segundos
  VERY_LONG: 30000, // 30 segundos
};

// Regex para validações
export const REGEX: Record<string, RegExp> = {
  EMAIL: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
  PHONE: /^[\d\s\-()]+$/,
  USERNAME: /^[a-zA-Z0-9_-]{3,20}$/,
  PASSWORD:
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
};

// Mensagens de validação
export const VALIDATION_MESSAGES = {
  EMAIL_INVALID: "Email inválido",
  PHONE_INVALID: "Telefone inválido",
  USERNAME_INVALID:
    "Username deve ter 3-20 caracteres (letras, números, _ e -)",
  PASSWORD_WEAK:
    "Senha deve ter 8+ caracteres, incluindo maiúscula, minúscula, número e símbolo",
  REQUIRED: "Campo obrigatório",
  MIN_LENGTH: (min: number) => `Mínimo ${min} caracteres`,
  MAX_LENGTH: (max: number) => `Máximo ${max} caracteres`,
};
