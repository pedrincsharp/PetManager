/**
 * App.tsx - Componente raiz da aplicação
 * Configura routing, autenticação e layout global
 */

import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { useEffect } from "react";
import LoginPage from "./presentation/pages/LoginPage";
import DashboardPage from "./presentation/pages/DashboardPage";
import ProtectedRoute from "./presentation/components/ProtectedRoute";
import TokenManager from "./core/TokenManager";
import "./App.css";

function App() {
  useEffect(() => {
    // Monitora renovação automática de tokens
    const unsubscribe = TokenManager.onTokenRefreshed(() => {
      console.log("Token renovado automaticamente");
    });

    return () => unsubscribe();
  }, []);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/dashboard"
          element={<ProtectedRoute element={<DashboardPage />} />}
        />
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
