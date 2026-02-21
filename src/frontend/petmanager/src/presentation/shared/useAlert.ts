/**
 * Hook customizado para gerenciar alertas
 */

import { useState, useCallback } from "react";
import { AlertType } from "./Alert";

interface AlertState {
  type: AlertType;
  message: string;
  id: number;
}

export function useAlert() {
  const [alerts, setAlerts] = useState<AlertState[]>([]);
  const [nextId, setNextId] = useState(0);

  const showAlert = useCallback(
    (type: AlertType, message: string, duration?: number) => {
      const id = nextId;
      setNextId((prev) => prev + 1);
      setAlerts((prev) => [...prev, { type, message, id }]);

      // Auto-remove após duração
      if (duration !== 0) {
        const timeout = setTimeout(() => {
          removeAlert(id);
        }, duration || 5000);

        return () => clearTimeout(timeout);
      }
    },
    [nextId],
  );

  const removeAlert = useCallback((id: number) => {
    setAlerts((prev) => prev.filter((alert) => alert.id !== id));
  }, []);

  const success = useCallback(
    (message: string, duration?: number) => {
      showAlert("success", message, duration);
    },
    [showAlert],
  );

  const error = useCallback(
    (message: string, duration?: number) => {
      showAlert("error", message, duration);
    },
    [showAlert],
  );

  const warning = useCallback(
    (message: string, duration?: number) => {
      showAlert("warning", message, duration);
    },
    [showAlert],
  );

  const info = useCallback(
    (message: string, duration?: number) => {
      showAlert("info", message, duration);
    },
    [showAlert],
  );

  return {
    alerts,
    showAlert,
    removeAlert,
    success,
    error,
    warning,
    info,
  };
}
