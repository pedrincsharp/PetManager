/**
 * Componente ProtectedRoute
 * Rota protegida que requer autenticação
 */

import { Navigate } from "react-router-dom";
import authApplicationService from "../../application/services/authApplicationService";

interface ProtectedRouteProps {
  element: React.ReactElement;
}

export function ProtectedRoute({ element }: ProtectedRouteProps) {
  const isAuthenticated = authApplicationService.isAuthenticated();

  return isAuthenticated ? element : <Navigate to="/login" replace />;
}

export default ProtectedRoute;
