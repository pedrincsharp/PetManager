/**
 * Componente Alert - Para mostrar mensagens de sucesso, erro ou aviso
 */

import "./Alert.css";

export type AlertType = "success" | "error" | "warning" | "info";

interface AlertProps {
  type: AlertType;
  message: string;
  onClose?: () => void;
  autoClose?: boolean;
  duration?: number;
}

export function Alert({
  type,
  message,
  onClose,
  autoClose = true,
  duration = 5000,
}: AlertProps) {
  // Auto-close se configurado
  if (autoClose && onClose) {
    setTimeout(onClose, duration);
  }

  const getBackgroundColor = (): string => {
    switch (type) {
      case "success":
        return "bg-green-100 border-green-400";
      case "error":
        return "bg-red-100 border-red-400";
      case "warning":
        return "bg-yellow-100 border-yellow-400";
      case "info":
        return "bg-blue-100 border-blue-400";
      default:
        return "bg-gray-100 border-gray-400";
    }
  };

  const getTextColor = (): string => {
    switch (type) {
      case "success":
        return "text-green-800";
      case "error":
        return "text-red-800";
      case "warning":
        return "text-yellow-800";
      case "info":
        return "text-blue-800";
      default:
        return "text-gray-800";
    }
  };

  const getBorderColor = (): string => {
    switch (type) {
      case "success":
        return "border-green-400";
      case "error":
        return "border-red-400";
      case "warning":
        return "border-yellow-400";
      case "info":
        return "border-blue-400";
      default:
        return "border-gray-400";
    }
  };

  return (
    <div
      className={`
        alert-container
        border-l-4 ${getBorderColor()}
        ${getBackgroundColor()}
        ${getTextColor()}
        p-4 mb-4 rounded
        flex justify-between items-center
        animate-slideIn
      `}
      role="alert"
    >
      <span className="font-medium">{message}</span>
      {onClose && (
        <button
          onClick={onClose}
          className="ml-4 text-xl leading-none opacity-75 hover:opacity-100"
          aria-label="Fechar alerta"
        >
          ×
        </button>
      )}
    </div>
  );
}

export default Alert;
