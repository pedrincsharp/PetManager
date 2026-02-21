/\*\*

- Exemplos de Uso - Sistema de Autenticação
-
- Copie e adapte estes exemplos ao criar novos componentes
  \*/

// ============================================
// 1. USAR AUTENTICAÇÃO EM UM NOVO COMPONENTE
// ============================================

import authApplicationService from '@/application/services/authApplicationService';
import { useAlert } from '@/presentation/shared/useAlert';

function MyComponent() {
const { error, success } = useAlert();

// Verificar se está autenticado
if (authApplicationService.isAuthenticated()) {
// Usuário está autenticado
}

// Obter token de acesso
const token = authApplicationService.getAccessToken();
if (token) {
// Usar token em requisições
}
}

// ============================================
// 2. FAZER LOGIN PARTIR DE UM FORMULÁRIO
// ============================================

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authApplicationService from '@/application/services/authApplicationService';
import { useAlert } from '@/presentation/shared/useAlert';

function LoginForm() {
const [username, setUsername] = useState('');
const [password, setPassword] = useState('');
const [isLoading, setIsLoading] = useState(false);
const navigate = useNavigate();
const { error, success } = useAlert();

const handleSubmit = async (e: React.FormEvent) => {
e.preventDefault();
setIsLoading(true);

    try {
      await authApplicationService.login(username, password);
      success('Login realizado!');
      navigate('/dashboard');
    } catch (err) {
      error(err instanceof Error ? err.message : 'Erro ao fazer login');
    } finally {
      setIsLoading(false);
    }

};

return (
<form onSubmit={handleSubmit}>
{/_ Seu formulário _/}
</form>
);
}

// ============================================
// 3. MONITORAR RENOVAÇÃO DE TOKENS
// ============================================

import { useEffect } from 'react';
import TokenManager from '@/core/TokenManager';

function MyComponent() {
useEffect(() => {
const unsubscribe = TokenManager.onTokenRefreshed((newTokens) => {
console.log('Token foi renovado automaticamente');
// Faça algo quando token é renovado
});

    return () => unsubscribe();

}, []);
}

// ============================================
// 4. FAZER REQUISIÇÕES COM AUTENTICAÇÃO
// ============================================

import apiClient from '@/infrastructure/api/apiClient';
import { ApiResponse } from '@/application/dtos/auth.dto';

async function fetchUserData() {
try {
// ApiClient adiciona automaticamente header de autenticação
const response = await apiClient.get<ApiResponse<any>>(
'/user/profile',
true // withAuth = true (padrão)
);

    if (response.code === '200') {
      const userData = response.data;
      // Use os dados
    }

} catch (error) {
console.error('Erro ao buscar dados:', error);
}
}

// ============================================
// 5. IMPLEMENTAR LOGOUT
// ============================================

import authApplicationService from '@/application/services/authApplicationService';
import { useNavigate } from 'react-router-dom';

function UserMenu() {
const navigate = useNavigate();

const handleLogout = () => {
authApplicationService.logout();
navigate('/login');
};

return (
<button onClick={handleLogout}>
Sair
</button>
);
}

// ============================================
// 6. CRIAR ROTA PROTEGIDA
// ============================================

import ProtectedRoute from '@/presentation/components/ProtectedRoute';
import MyPage from '@/presentation/pages/MyPage';

// Em App.tsx
export function App() {
return (
<Routes>
<Route path="/protected" element={<ProtectedRoute element={<MyPage />} />} />
</Routes>
);
}

// ============================================
// 7. MOSTRAR ALERTAS DE ERROS
// ============================================

import { useAlert } from '@/presentation/shared/useAlert';

function MyComponent() {
const { alerts, removeAlert, error, warning, success, info } = useAlert();

const handleClick = () => {
success('Ação realizada com sucesso!');
warning('Cuidado com isso!');
error('Erro ao processar');
info('Informação importante');
};

return (
<div>
{/_ Renderize alertas _/}
{alerts.map(alert => (
<Alert
key={alert.id}
type={alert.type}
message={alert.message}
onClose={() => removeAlert(alert.id)}
/>
))}

      <button onClick={handleClick}>Mostrar Alertas</button>
    </div>

);
}

// ============================================
// 8. TRATAR DIFERENTES TIPOS DE ERRO
// ============================================

import { useAlert } from '@/presentation/shared/useAlert';
import { HTTP_STATUS, ERROR_MESSAGES } from '@/core/ErrorConstants';

function handleApiError(error: Error) {
const { error: showError, warning: showWarning } = useAlert();

if (error.message.includes('401')) {
showError(ERROR_MESSAGES.INVALID_CREDENTIALS);
} else if (error.message.includes('500')) {
showError(ERROR_MESSAGES.SERVER_ERROR);
} else if (error.message.includes('Network')) {
showError(ERROR_MESSAGES.NETWORK_ERROR);
} else {
showWarning(ERROR_MESSAGES.UNKNOWN_ERROR);
}
}

// ============================================
// 9. INTERCEPTAR ERROS DE TOKEN EXPIRADO
// ============================================

// ApiClient já implementa isso automaticamente!
// Quando um token expira (401), o apiClient:
// 1. Usa refreshToken para obter novo accessToken
// 2. Tenta novamente a requisição original
// 3. Se refresh falhar, limpa tokens e redireciona a /login

// ============================================
// 10. CRIAR NOVO SERVIÇO DE NEGÓCIO
// ============================================

// infrastructure/services/clientService.ts
import apiClient from '@/infrastructure/api/apiClient';
import { ApiResponse } from '@/application/dtos/auth.dto';

interface Client {
id: string;
name: string;
email: string;
}

class ClientService {
async getClients(): Promise<Client[]> {
const response = await apiClient.get<ApiResponse<Client[]>>(
'/client',
true
);
return response.data || [];
}

async createClient(data: Partial<Client>): Promise<Client> {
const response = await apiClient.post<ApiResponse<Client>>(
'/client',
data,
true
);
if (!response.data) throw new Error(response.message);
return response.data;
}
}

export default new ClientService();

// application/services/clientApplicationService.ts
import clientService from '@/infrastructure/services/clientService';

class ClientApplicationService {
async getClients() {
return await clientService.getClients();
}

async createClient(name: string, email: string) {
return await clientService.createClient({ name, email });
}
}

export default new ClientApplicationService();
