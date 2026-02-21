/**
 * Página de Login
 * Tela para autenticação de usuários
 */

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Alert from "../shared/Alert";
import { useAlert } from "../shared/useAlert";
import authApplicationService from "../../application/services/authApplicationService";
import "./LoginPage.css";

export function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { alerts, removeAlert, error, warning, success } = useAlert();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!username || !password) {
      warning("Por favor, preencha todos os campos");
      return;
    }

    setIsLoading(true);

    try {
      await authApplicationService.login(username, password);
      success("Login realizado com sucesso!");

      // Redireciona após 1 segundo
      setTimeout(() => {
        navigate("/dashboard");
      }, 1000);
    } catch (err) {
      const errorMessage =
        err instanceof Error ? err.message : "Erro desconhecido";

      // Identifica o tipo de erro
      if (errorMessage.includes("401") || errorMessage.includes("Invalid")) {
        error("Usuário ou senha incorretos");
      } else if (errorMessage.includes("500")) {
        error("Erro interno do servidor. Tente novamente mais tarde.");
      } else if (
        errorMessage.includes("Network") ||
        errorMessage.includes("fetch")
      ) {
        error("Erro de conexão. Verifique sua internet.");
      } else {
        warning(errorMessage);
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-wrapper">
        {/* Logo/Header */}
        <div className="login-header">
          <h1 className="login-title">PetManager</h1>
          <p className="login-subtitle">Sistema de Gestão Veterinária</p>
        </div>

        {/* Alertas */}
        <div className="login-alerts">
          {alerts.map((alert) => (
            <Alert
              key={alert.id}
              type={alert.type}
              message={alert.message}
              onClose={() => removeAlert(alert.id)}
            />
          ))}
        </div>

        {/* Formulário */}
        <form onSubmit={handleLogin} className="login-form">
          <div className="form-group">
            <label htmlFor="username" className="form-label">
              Usuário
            </label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Digite seu usuário"
              className="form-input"
              disabled={isLoading}
              autoFocus
            />
          </div>

          <div className="form-group">
            <label htmlFor="password" className="form-label">
              Senha
            </label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Digite sua senha"
              className="form-input"
              disabled={isLoading}
            />
          </div>

          <button type="submit" className="login-button" disabled={isLoading}>
            {isLoading ? "Entrando..." : "Entrar"}
          </button>
        </form>

        {/* Footer */}
        <div className="login-footer">
          <p className="footer-text">
            Sessão expira em 60 minutos de inatividade
          </p>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
