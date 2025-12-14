# 📡 SignalR Integration Guide — Talos Backend

Este documento describe los pasos necesarios para finalizar la integración de SignalR dentro del backend de Talos.  
Actualmente, SignalR ya está configurado, el hub está operativo y existen pruebas funcionales usando Postman.  
Lo que falta es integrar SignalR dentro de los flujos reales del dominio (templates, compatibilidad, usuarios, etc.).

---

## 📝 Crear este archivo en tu proyecto

```bash
# Crear archivo
echo "" > signalr-integration-guide.md

# Abrirlo (Linux/Mac)
nano signalr-integration-guide.md

# Abrirlo (Windows PowerShell)
notepad signalr-integration-guide.md
```

---

## ✅ 1. Estado actual (lo ya implementado)
✔ Hub creado

Application/RealTime/NotificationsHub.cs

```csharp
public class NotificationsHub : Hub { }
```
✔ Servicio de notificaciones

Application/RealTime/NotificationService.cs
```csharp
await _hub.Clients.All.SendAsync("templateCreated", data);
```

✔ Routing configurado en Program.cs
```csharp
app.MapHub<NotificationsHub>("/hubs/notifications");
```
✔ Pruebas manuales funcionales

Se puede enviar un mensaje desde:

POST `/api/NotificationTest/send`

Y el cliente HTML (`test-signalr.html`) recibe WebSockets correctamente.

---

## 🚧 2. Trabajo pendiente (lo que se debe implementar)

La integración real requiere conectar SignalR con los flujos reales del sistema.

---

### 2.1. Invocar `NotificationService` desde los Controllers reales

Hay dos flujos principales donde se deben disparar notificaciones:

---

#### **A. Cuando se crea un template**

**Endpoint:**

`POST /api/templates`
Debe ejecutar:
```csharp
await _notificationService.NotifyTemplateCreated(userName, templateName);
```

---

## B. Cuando se actualizan compatibilidades

**Endpoint:**

`GET /api/compatibility/{package}/{version}`

o cualquier otro que modifique compatibilidades.

Debe ejecutar:
```csharp
await _notificationService.NotifyCompatibilityUpdated(package, version);
```

---

## 📌 IMPORTANTE
Si el controller actual no tiene acceso al servicio, inyectarlo:

```csharp
private readonly NotificationService _notification;

public TemplateController(NotificationService notification, ...)
{
    _notification = notification;
}
```

---

## 2.2. Configurar notificaciones por usuario (opcional, pero recomendado)

Crear grupos de SignalR:

```csharp
public override async Task OnConnectedAsync()
{
    var user = Context.GetHttpContext().Request.Query["user"];
    await Groups.AddToGroupAsync(Context.ConnectionId, user);
}
```

Enviar solo a un usuario:

```csharp
await _hub.Clients.Group(user).SendAsync("templateCreated", data);
```

---

## 2.3. Implementar el Cliente Real (Web / Frontend)

Ejemplo en JavaScript:

```csharp
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notifications")
    .build();

connection.on("templateCreated", data => {
    console.log("Nuevo template:", data);
});

connection.on("compatibilityUpdated", data => {
    console.log("Compatibilidad actualizada:", data);
});

connection.start();
```

Con autenticación:

```csharp
.withUrl("/hubs/notifications", {
    accessTokenFactory: () => authToken
});
```

---

## 2.4. Integración con el sistema de autenticación (JWT)

Program.cs

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs/notifications"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
```

---

## 2.5. Deploy Ready Configuration

En producción:

- Habilitar WebSockets en el hosting (Nginx/IIS/Reverse Proxy).

- Cambiar el cliente a **HTTPS**.

- Configurar **CORS** correctamente.

Ejemplo Nginx (para WebSockets):

```
location /hubs/ {
    proxy_pass http://localhost:5000;
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection "Upgrade";
    proxy_set_header Host $host;
}
```

---

## 🧪 3. Pruebas finales necesarias

- ✔ Crear un template desde frontend → recibir notificación

- ✔ Actualizar compatibilidades → notificación en tiempo real

- ✔ Reconexión automática del cliente

- ✔ Persistencia de conexión cuando cambia de vista

- ✔ Notificaciones por usuario (si se activan grupos)

---

## 🧩 4. Archivos que deben ser modificados


| Archivo | Acción |
|---------|--------|
| **Controllers/TemplateController.cs** | Invocar notificaciones al crear templates |
| **Controllers/CompatibilityController.cs** | Invocar notificaciones al actualizar compatibilidad |
| **NotificationsHub.cs** | (Opcional) Manejar grupos por usuario |
| **frontend** | Implementar conexión real al Hub |
| **Program.cs** | (Opcional) Integrar JWT con SignalR |


---

## 🟢 5. Resultado esperado cuando se termine

Cuando se complete la integración, el sistema podrá:

✨ Notificar en tiempo real cuando un usuario cree un template

✨ Avisar cambios de compatibilidad

✨ Enviar notificaciones a todos o solo a un usuario

✨ Trabajar con WebSockets en producción

✨ Integrarse con JWT para seguridad