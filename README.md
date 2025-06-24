# TaskSystem – README

## Setup Instructions

### Requirements
- Windows 10 or higher
- IIS (Internet Information Services) enabled
- SQL Server (Express or higher)
- .NET Framework 4.7.2+ or .NET Hosting Bundle 6.0+
- .NET Desktop Runtime 6.0+ (for WPF and WinForms clients)

### Structure
```
Distribution
  1.- Server-IIS             --> Backend Web API + SignalR (for IIS deployment)
  2.- TaskSystem.ClientWPF   --> WPF client for managing tasks
  3.- CatalogAdmin.WinForms  --> WinForms tool for managing users and priorities
  4.- TaskSystem_DB.sql      --> Database creation script
  5.- README.md
```

### Setup Steps

#### 1. Configure SQL Server Database

1. Open SQL Server Management Studio
2. Execute the provided `TaskSystem_DB.sql` script
3. Confirm that the following tables are created:
   - Users
   - Tasks
   - Priorities

> You can insert a user manually or use the Catalog WinForms tool to manage users.

#### 2. Deploy the Backend on IIS

- Open **IIS Manager**
- Create a new website pointing to `Server-IIS` folder
- Suggested URL: `http://localhost:5000`
- Update the connection string in `Web.config` to point to your SQL Server instance
- Test with: `http://localhost:5000/api/users`

#### 3. Run the WPF Client

- Navigate to `TaskSystem.ClientWPF`
- Run `TaskSystem.Client.exe`
- Log in using a valid user

#### 4. Run the WinForms Catalog Tool

- Navigate to `CatalogAdmin.WinForms`
- Run `TaskSystemCatalogAdmin.exe`
- Use this tool to manage users and priorities

#### 5. Configure API Base URL in Clients

In both clients, modify `App.config`:

```xml
<appSettings>
  <add key="ApiBaseUrl" value="http://localhost:5000/api/" />
</appSettings>
```

## Communication Protocol Choice

### Protocols: **HTTP (Web API) + SignalR (WebSockets)**

- **Web API (HTTP + JSON):** For CRUD operations
- **SignalR:** For real-time updates (task locking, synchronization)

### Reasoning:
- Low latency communication
- Avoids polling
- Common choice for real-time .NET applications

---

## Design Patterns Used

- **Singleton** – for `ApiService` and `SignalRService`
- **MVVM** – used in WPF client
- **DTOs (Data Transfer Objects)** – to control API output and prevent circular references
- **Observer Pattern** – SignalR events notify the UI
- **Service Layer abstraction** – logic separated from EF context

---

## Sample Credentials

```
Create a new user using Catalog app, before to use Client App
```