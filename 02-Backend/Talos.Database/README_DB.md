# Talos 

## Creación del esquema DB en MySQL 
### Database :
 
```sql
CREATE DATABASE Talos;
USE Talos;
```
### Tablas :

- **PackageManager**
    ```sql
   CREATE TABLE PackageManager (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(150) UNIQUE NOT NULL
    -- Ejemplos: 'npm', 'apt', 'snap', 'pip', 'brew');
    ```

- **User**

    ```sql
    CREATE TABLE User (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserName VARCHAR(100) UNIQUE NOT NULL,
    Email VARCHAR(150) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    AvatarUrl TEXT,
    SignalrConnectionId VARCHAR(255),
    LastConnectionId VARCHAR(255),
    IsOnline BOOLEAN DEFAULT FALSE,
    LastSeenAt TIMESTAMP,
    LastIpAddress VARCHAR(45),
    Tier ENUM('free', 'business', 'personal') DEFAULT 'free',
    PrivateTemplateLimit INT DEFAULT 2,
    CurrentPrivateTemplates INT DEFAULT 0,
    EmailVerified BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    DeletedAt TIMESTAMP NULL,
    Role VARCHAR(100),
    
    INDEX IdxUserTier (Tier),
    INDEX IdxUserOnline (IsOnline));
    ```

- **Package**
    ```sql
    CREATE TABLE Package (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) UNIQUE NOT NULL,
    ShortName VARCHAR(100),
    PackageManagerId INT NOT NULL,
    RepositoryUrl TEXT,
    OfficialDocumentationUrl TEXT,
    LastScrapedAt TIMESTAMP NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    LatestVersion VARCHAR(50),
    
    FOREIGN KEY (PackageManagerId) REFERENCES PackageManager(Id));
    ```

- **PackageVersion**

    ```sql
    CREATE TABLE PackageVersion (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    PackageId INT NOT NULL,
    Version VARCHAR(50) NOT NULL,
    ReleaseDate TIMESTAMP NULL,
    IsDeprecated BOOLEAN DEFAULT FALSE,
    DeprecationMessage TEXT,
    DownloadUrl TEXT,
    ReleaseNotesUrl TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE KEY UkPackageVersion (PackageId, Version),
    FOREIGN KEY (PackageId) REFERENCES Package(Id) ON DELETE CASCADE);
    ```

- **Compatibility**

    ```sql
    CREATE TABLE Compatibility (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    SourcePackageVersionId INT NOT NULL,
    TargetPackageId INT NOT NULL,
    TargetVersionConstraint VARCHAR(100) NOT NULL,
    CompatibilityType ENUM('required', 'recommended', 'optional', 'conflict') DEFAULT 'required',
    CompatibilityScore INT DEFAULT 100,
    ConfidenceLevel ENUM('high', 'medium', 'low') DEFAULT 'medium',
    DetectedBy ENUM('n8n_scraper', 'manual', 'user_report', 'ai_analysis') DEFAULT 'n8n_scraper',
    DetectionDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Notes TEXT,
    IsActive BOOLEAN DEFAULT TRUE,
    
    UNIQUE KEY UkCompatibilityUnique (SourcePackageVersionId, TargetPackageId, TargetVersionConstraint),
    FOREIGN KEY (SourcePackageVersionId) REFERENCES PackageVersion(Id) ON DELETE CASCADE,
    FOREIGN KEY (TargetPackageId) REFERENCES Package(Id) ON DELETE CASCADE);
    ```

- **Template**

    ```sql
    CREATE TABLE Template (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    TemplateName VARCHAR(150),
    Slug VARCHAR(255) UNIQUE,
    IsPublic BOOLEAN DEFAULT FALSE,
    LicenseType VARCHAR(50),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (UserId) REFERENCES User(Id) ON DELETE CASCADE);
    ```
- **TemplateDependencies**

    ```sql
    CREATE TABLE TemplateDependencies (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    TemplateId INT NOT NULL,
    PackageId INT NOT NULL,
    VersionConstraint VARCHAR(100),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (TemplateId) REFERENCES Template(Id) ON DELETE CASCADE,
    FOREIGN KEY (PackageId) REFERENCES Package(Id));
    ```

- **Tag**

    ```sql
    CREATE TABLE Tag (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) UNIQUE NOT NULL,
    Description TEXT,
    Color VARCHAR(7) DEFAULT '#3B82F6',
    IsSystemTag BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP);
    ```
    **Tags del Sistema:**

    - `system` - Notificaciones del sistema

    - `template` - Actualizaciones de templates

    - `compatibility` - Cambios en compatibilidades

    - `security` - Alertas de seguridad

    - `update` - Actualizaciones disponibles


- **Notification**

    ```sql
    CREATE TABLE Notification (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    TagId INT,
    Title VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    Payload JSON,
    IsRead BOOLEAN DEFAULT FALSE,
    IsArchived BOOLEAN DEFAULT FALSE,
    Priority ENUM('low', 'medium', 'high', 'critical') DEFAULT 'medium',
    ExpiresAt TIMESTAMP NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ReadAt TIMESTAMP NULL,
    ArchivedAt TIMESTAMP NULL,
    
    FOREIGN KEY (UserId) REFERENCES User(Id) ON DELETE CASCADE,
    FOREIGN KEY (TagId) REFERENCES Tag(Id) ON DELETE SET NULL,
    INDEX IdxNotificationUser (UserId),
    INDEX IdxNotificationRead (IsRead),
    INDEX IdxNotificationArchived (IsArchived),
    INDEX IdxNotificationCreated (CreatedAt));
    ```

- **UserNotificationPreference**

    ```sql
    CREATE TABLE UserNotificationPreference (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    TagId INT NOT NULL,
    ViaEmail BOOLEAN DEFAULT TRUE,
    ViaPush BOOLEAN DEFAULT TRUE,
    ViaWeb BOOLEAN DEFAULT TRUE,
    IsMuted BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    UNIQUE KEY UkUserTagPreference (UserId, TagId),
    FOREIGN KEY (UserId) REFERENCES User(Id) ON DELETE CASCADE,
    FOREIGN KEY (TagId) REFERENCES Tag(Id) ON DELETE CASCADE);
    ```

### Relaciones entre Tablas :

- Relación de la tabla `PackageVersion`(**N**) con la tabla `Package`(**1**)
        
    ---
 
- Relación de la tabla `Compatibility`(**N**) con la tabla `PackageVersion`(**1**) 

    ---

- Relación de la tabla `Compatibility`(**N**) con la tabla `Package`(**1**) 

    ---

- Relación de la tabla `Package`(**N**) con la tabla `PackageManager`(**1**) 

    ---

- Relación de la tabla `TemplateDependencies`(**N**) con la tabla `Package`(**1**) 

    ---

- Relación de la tabla `TemplateDependencies`(**N**) con la tabla `Template`(**1**)

    ---

- Relación de la tabla `Template`(**N**) con la tabla `User`(**1**)  

    ---

- Relación de la tabla `UserNotificationPreference`(**N**) con la tabla `User`(**1**) 

    ---

- Relación de la tabla `Notification`(**N**) con la tabla `User`(**1**) 

    ---

- Relación de la tabla `UserNotificationPreference`(**N**) con la tabla `Tag`(**1**) 

    ---

- Relación de la tabla `Notification`(**N**) con la tabla `Tag`(**1**) 

    ---

- Relación de la tabla `Follow`(**N**) con la tabla `User`(**1**) 

    ---

- Relación de la tabla `Post`(**N**) con la tabla `User`(**1**) 

    ---

## Foreign Keys Explicadas:

1. `Package.PackageManagerId` → `PackageManager.Id`

- Un paquete pertenece a un gestor de paquetes

- Un gestor de paquetes puede tener muchos paquetes

2. `PackageVersion.PackageId` → `Package.Id` **(ON DELETE CASCADE)**

- Una versión pertenece a un paquete

- Si se elimina un paquete, se eliminan todas sus versiones

3. `Compatibility.SourcePackageVersionId` → `PackageVersion.Id` **(ON DELETE CASCADE)**

- Una compatibilidad tiene como origen una versión específica

- Si se elimina la versión, se eliminan sus compatibilidades

4. `Compatibility.TargetPackageId` → `Package.Id` **(ON DELETE CASCADE)**

- Una compatibilidad tiene como destino un paquete

- Si se elimina el paquete destino, se eliminan las compatibilidades

5. `Template.UserId` → `User.Id` **(ON DELETE CASCADE)**

- Un template pertenece a un usuario

- Si se elimina un usuario, se eliminan sus templates

6. `TemplateDependencies.TemplateId` → `Template.Id` **(ON DELETE CASCADE)**

- Una dependencia pertenece a un template

- Si se elimina un template, se eliminan sus dependencias

7. `TemplateDependencies.PackageId` → `Package.Id`

- Una dependencia referencia a un paquete

- No se puede eliminar un paquete si está siendo usado en dependencias

8. `Notification.UserId` → `User.Id` **(ON DELETE CASCADE)**

- Una notificación pertenece a un usuario

- Si se elimina un usuario, se eliminan sus notificaciones

9. `Notification.TagId` → `Tag.Id` **(ON DELETE SET NULL)**

- Una notificación puede tener una categoría (tag)

- Si se elimina el tag, la notificación mantiene su registro con TagId = NULL

10. `UserNotificationPreference.UserId` → `User.Id` **(ON DELETE CASCADE)**

- Una preferencia pertenece a un usuario

- Si se elimina un usuario, se eliminan sus preferencias

11. `UserNotificationPreference.TagId` → `Tag.Id` **(ON DELETE CASCADE)**

- Una preferencia referencia a un tag

- Si se elimina un tag, se eliminan las preferencias asociadas

---

