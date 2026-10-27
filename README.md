# SignalRArchitecture: Chat en tiempo real con SignalR y .NET 10

Taller universitario de aplicaciones en tiempo real. Solución con tres proyectos:

| Proyecto | Rol |
|---|---|
| `SignalR.Hub` | Servidor (ASP.NET Core Web API). Aloja el `ChatHub` y reparte los mensajes. |
| `SignalR.Client` | Interfaz web (Razor Pages + JavaScript). Se conecta al Hub. |
| `SignalR.Shared` | Contratos compartidos (`ChatMessage`). |

## Requisitos

- SDK de .NET 10 (`dotnet --version`)
- PowerShell y VS Code
- LibMan CLI:
  ```powershell
  dotnet tool install -g Microsoft.Web.LibraryManager.Cli
  ```

## Cómo ejecutarlo

1. Clonar el repositorio y entrar a la carpeta:
   ```powershell
   git clone <URL-DEL-REPO>
   cd SignalRArchitecture
   ```
2. Descargar la librería JavaScript de SignalR (se ejecuta dentro de `SignalR.Client`):
   ```powershell
   cd SignalR.Client
   libman restore
   cd ..
   ```
3. Restaurar y compilar desde la raíz de la solución:
   ```powershell
   dotnet restore
   dotnet build
   ```
4. Si el navegador desconfía del certificado HTTPS (una sola vez):
   ```powershell
   dotnet dev-certs https --trust
   ```
5. **Terminal 1**, iniciar el servidor (dejarlo corriendo):
   ```powershell
   dotnet run --project SignalR.Hub --urls https://localhost:7001
   ```
6. **Terminal 2**, iniciar el cliente:
   ```powershell
   dotnet run --project SignalR.Client --urls https://localhost:7002
   ```
7. Abrir `https://localhost:7002` en **dos pestañas**, enviar un mensaje desde una y comprobar que aparece en ambas.

## Configuración importante

| Elemento | Valor |
|---|---|
| Puerto del servidor | 7001 |
| Puerto del cliente | 7002 |
| Endpoint del Hub | `/chatHub` |
| Método invocado | `SendMessage` |
| Evento recibido | `ReceiveMessage` |
| Origen permitido en CORS | `https://localhost:7002` |

Si cambias un puerto, actualiza también CORS en `SignalR.Hub/Program.cs` y la URL en `SignalR.Client/wwwroot/js/chat.js`.

## Limitaciones

- No hay almacenamiento persistente: al recargar la página se pierde el historial.
- El nombre de usuario lo escribe el cliente; no es una identidad verificada. En producción se necesita autenticación y autorización en el servidor.