# DOCUMENTO OFICIAL DE REQUISITOS DO SISTEMA

---

# 1. CONTROLE DO DOCUMENTO

| Item          | Informação                                     |
| ------------- | ---------------------------------------------- |
| Projeto       | Sistema Web de Gestão para Clínica Veterinária |
| Versão        | 1.0                                            |
| Data          | 20/02/2026                                     |
| Elaborado por | Analista de Negócios                           |
| Aprovado por  | ************\_************                     |
| Status        | Em aprovação                                   |

---

# 2. HISTÓRICO DE ALTERAÇÕES

| Versão | Data       | Descrição da Alteração | Responsável          |
| ------ | ---------- | ---------------------- | -------------------- |
| 1.0    | 20/02/2026 | Criação do documento   | Analista de Negócios |

---

# 3. VISÃO GERAL DO PROJETO

## 3.1 Objetivo

Este documento tem como objetivo descrever os requisitos funcionais e não funcionais do Sistema Web de Gestão para Clínica Veterinária, garantindo alinhamento entre as áreas envolvidas e servindo como base para desenvolvimento, testes e validação.

O sistema visa centralizar a gestão operacional e financeira da clínica, promovendo organização, controle e melhoria na experiência dos clientes.

---

## 3.2 Escopo do Projeto

O sistema contemplará os seguintes módulos:

1. Gestão de Usuários
2. Gestão de Clientes
3. Gestão de Pets
4. Gestão de Serviços
5. Agendamentos
6. Pagamentos
7. Dashboard Gerencial

---

## 3.3 Público-Alvo

- Administrador
- Atendente
- Veterinário

---

# 4. REQUISITOS FUNCIONAIS

---

## 4.1 Gestão de Usuários

| Código | Descrição                                                                                        |
| ------ | ------------------------------------------------------------------------------------------------ |
| RF01   | O sistema deverá permitir cadastro de usuários internos.                                         |
| RF02   | O sistema deverá permitir definição de perfil de acesso (Administrador, Atendente, Veterinário). |
| RF03   | O sistema deverá permitir login e logout.                                                        |
| RF04   | O sistema deverá restringir funcionalidades conforme perfil de acesso.                           |

---

## 4.2 Gestão de Clientes

| Código | Descrição                                                                                |
| ------ | ---------------------------------------------------------------------------------------- |
| RF05   | O sistema deverá permitir cadastrar clientes com nome, CPF, telefone, e-mail e endereço. |
| RF06   | O sistema deverá permitir editar cadastro de clientes.                                   |
| RF07   | O sistema deverá permitir inativar clientes.                                             |
| RF08   | O sistema deverá permitir consulta de clientes por nome, CPF ou telefone.                |

---

## 4.3 Gestão de Pets

| Código | Descrição                                                                                         |
| ------ | ------------------------------------------------------------------------------------------------- |
| RF09   | O sistema deverá permitir cadastrar pets vinculados a um cliente.                                 |
| RF10   | O sistema deverá permitir registrar nome, espécie, raça, porte, data de nascimento e observações. |
| RF11   | O sistema deverá permitir edição dos dados do pet.                                                |
| RF12   | O sistema deverá permitir visualização do histórico de serviços realizados.                       |

---

## 4.4 Gestão de Serviços

| Código | Descrição                                                                             |
| ------ | ------------------------------------------------------------------------------------- |
| RF13   | O sistema deverá permitir cadastro de serviços oferecidos pela clínica.               |
| RF14   | O sistema deverá permitir definir nome, descrição, valor e tempo estimado do serviço. |
| RF15   | O sistema deverá permitir ativar ou inativar serviços.                                |

---

## 4.5 Agendamentos

| Código | Descrição                                                                                                      |
| ------ | -------------------------------------------------------------------------------------------------------------- |
| RF16   | O sistema deverá permitir realizar agendamento vinculando cliente, pet, serviço, profissional, data e horário. |
| RF17   | O sistema não deverá permitir conflito de horário para o mesmo profissional.                                   |
| RF18   | O sistema deverá permitir alteração de agendamentos.                                                           |
| RF19   | O sistema deverá permitir cancelamento de agendamentos.                                                        |
| RF20   | O sistema deverá impedir cancelamento com menos de 2 horas do horário marcado.                                 |
| RF21   | O sistema deverá permitir visualização da agenda por dia, semana e profissional.                               |

---

## 4.6 Pagamentos

| Código | Descrição                                                                                              |
| ------ | ------------------------------------------------------------------------------------------------------ |
| RF22   | O sistema deverá permitir registrar pagamentos.                                                        |
| RF23   | O sistema deverá registrar valor pago, forma de pagamento, data e status.                              |
| RF24   | O sistema deverá impedir novo agendamento caso o cliente possua pagamento pendente superior a 30 dias. |

---

## 4.7 Dashboard Gerencial

| Código | Descrição                                                                                                                                                              |
| ------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| RF25   | O sistema deverá apresentar indicadores como total de agendamentos no mês, faturamento mensal, serviço mais realizado e profissional com maior volume de atendimentos. |
| RF26   | O sistema deverá permitir filtro por período.                                                                                                                          |

---

# 5. REQUISITOS NÃO FUNCIONAIS

---

## 5.1 Segurança

| Código | Descrição                                                                |
| ------ | ------------------------------------------------------------------------ |
| RNF01  | O sistema deverá exigir autenticação para acesso.                        |
| RNF02  | O sistema deverá garantir controle de acesso conforme perfil do usuário. |
| RNF03  | Os dados deverão ser armazenados de forma segura.                        |

---

## 5.2 Desempenho

| Código | Descrição                                                                 |
| ------ | ------------------------------------------------------------------------- |
| RNF04  | O sistema deverá responder em até 3 segundos em condições normais de uso. |
| RNF05  | O sistema deverá suportar múltiplos usuários simultâneos.                 |

---

## 5.3 Usabilidade

| Código | Descrição                                              |
| ------ | ------------------------------------------------------ |
| RNF06  | O sistema deverá possuir interface intuitiva.          |
| RNF07  | O sistema deverá ser responsivo para desktop e tablet. |

---

## 5.4 Disponibilidade

| Código | Descrição                                                                            |
| ------ | ------------------------------------------------------------------------------------ |
| RNF08  | O sistema deverá estar disponível 24 horas por dia, exceto em manutenção programada. |

---

## 5.5 Manutenibilidade

| Código | Descrição                                                  |
| ------ | ---------------------------------------------------------- |
| RNF09  | O sistema deverá permitir futuras expansões e integrações. |

---

# 6. BANCO DE DADOS

O sistema deverá utilizar banco de dados relacional.

Tecnologias sugeridas:

- PostgreSQL  
  ou
- SQL Server

O banco deverá garantir:

- Integridade referencial
- Controle transacional
- Consistência dos dados

---

# 7. FORA DO ESCOPO

- Aplicativo mobile
- Integração com sistemas externos
- Emissão de nota fiscal
- Integração com meios de pagamento online

---

# 8. CRITÉRIOS DE ACEITE

O sistema será considerado aprovado quando:

- Todos os requisitos funcionais forem implementados
- Perfis de acesso funcionarem corretamente
- Não houver conflito de agendamentos
- Indicadores do dashboard apresentarem dados consistentes
- Cadastros e consultas operarem corretamente
