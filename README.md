# OrderDispatch

Микросервисная система управления заказами и их автоматического распределения по кухням на базе .NET, YARP, RabbitMQ, PostgreSQL и Keycloak.

## Архитектура и стек

- **ASP.NET Core** — REST API сервисы (CQRS + Vertical Slice Architecture, `MediatR`)
- **YARP (Yet Another Reverse Proxy)** — API Gateway с единым агрегированным Swagger/OpenAPI
- **PostgreSQL 17 + Entity Framework Core** — изолированная БД на каждый сервис (*Database per Service*)
- **RabbitMQ + MassTransit** — асинхронный обмен событиями и запросы-ответы
- **Keycloak** — аутентификация (OAuth 2.0 / OIDC, JWT Bearer) и ролевая иерархия
- **NUnit** — модульные тесты бизнес-логики (`OrderDispatch.Tests`)
- **Docker Compose** — контейнеризация всех сервисов и инфраструктуры

## Структура решения

- **`Gateway`** — единая точка входа (`http://localhost:5000`), маршрутизация трафика и агрегированный Swagger UI
- **`UserService`** — регистрация, вход (JWT), просмотр профиля и назначение ролей с проверкой иерархии через Keycloak Admin API
- **`OrderService`** — создание, получение, оплата и отмена заказов, стейт-машина переходов (`OrderStateMachine`)
- **`KitchenService`** — управление кухнями, персоналом, расчётом нагрузки и очередью заказов (`KitchenStateMachine`)
- **`OrderDispatch.Contracts`** — общие DTO, контракты событий/запросов RabbitMQ, константы ролей (`AppRoles`) и механизм авторизации по рангу (`AuthorizeMinRankAttribute`, `MinRankAuthorizationHandler`)
- **`OrderDispatch.Tests`** — unit-тесты для стейт-машин заказов и кухни, а также иерархии ролей

## Ролевая иерархия

Доступ к эндпоинтам контролируется по минимальному рангу роли через `[AuthorizeMinRank]`:

1. `super-admin` (ранг 6)
2. `company-owner` (ранг 5)
3. `branch-manager` (ранг 4)
4. `chef` (ранг 3)
5. `courier` (ранг 2)
6. `customer` (ранг 1)

Вышестоящая роль наследует доступ к эндпоинтам нижестоящих ролей и может назначать только должности строго ниже своего ранга.

## Быстрый старт (Docker)

### 1. Скопируйте шаблон переменных окружения:
   ```bash
   cp .env.example .env
   ```
   *(Укажите `KEYCLOAK_ADMIN_CLIENT_SECRET` из настроек сервисного клиента в Keycloak).*
   
### 2. Настройка Keycloak
Запустите контейнер Keycloak (`docker compose up -d keycloak`) и откройте админ-панель `http://localhost:8080` (логин/пароль по умолчанию: `admin` / `admin`):

1. **Создайте Realm:** `OrderDispatchRealm`.
2. **Создайте роли реалма** (*Realm roles*): `super-admin`, `company-owner`, `branch-manager`, `chef`, `courier`, `customer`.
   - В *Realm settings ➔ User registration ➔ Default roles* добавьте роль `customer` (чтобы выдавалась автоматически при регистрации).
3. **Создайте клиента для входа пользователей (`api-gateway`):**
   - *Clients ➔ Create client* ➔ Client ID: `api-gateway`.
   - *Client authentication:* `OFF`, включите **Direct access grants**.
4. **Создайте сервисного клиента для `UserService` (`user-service-admin`):**
   - *Clients ➔ Create client* ➔ Client ID: `user-service-admin`.
   - *Client authentication:* `ON`, включите **Service accounts roles**.
   - Во вкладке **Credentials** скопируйте `Client secret` и вставьте его в `.env` в `KEYCLOAK_ADMIN_CLIENT_SECRET`.
   - Во вкладке **Service account roles** ➔ *Assign role ➔ Client roles* ➔ выберите `realm-management` и назначьте роли **`manage-users`** и **`view-realm`**.
5. *(Опционально для тестов)* Создайте первого пользователя в **Users** (заполнив Email, First/Last name, `Email verified: ON`, постоянный пароль) и выдайте ему роль `super-admin` во вкладке *Role mapping*.

### 3. Запуск всех сервисов
```bash
docker compose up -d --build
```
После запуска миграции БД применятся автоматически.
### Полезные адреса (Development)

- **Swagger UI (API Gateway):** `http://localhost:5000/swagger`
- **Keycloak Admin Console:** `http://localhost:8080`
- **RabbitMQ Management UI:** `http://localhost:15672`
- **Health Checks:** `/health` на Gateway (`http://localhost:5000/health`) и на каждом микросервисе

## Запуск тестов

```bash
dotnet test
```
