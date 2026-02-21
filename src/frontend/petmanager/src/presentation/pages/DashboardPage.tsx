/**
 * Componente Dashboard (placeholder)
 * Página inicial após autenticação
 */

import { useNavigate } from "react-router-dom";
import authApplicationService from "../../application/services/authApplicationService";
import "./DashboardPage.css";

export function DashboardPage() {
  const navigate = useNavigate();

  const handleLogout = () => {
    authApplicationService.logout();
    navigate("/login");
  };

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>Dashboard</h1>
        <button onClick={handleLogout} className="logout-button">
          Sair
        </button>
      </div>

      <div className="dashboard-content">
        <p>Bem-vindo ao PetManager!</p>
        <p>
          Esta é uma página placeholder. Use como base para construir o
          dashboard.
        </p>
      </div>
    </div>
  );
}

export default DashboardPage;
