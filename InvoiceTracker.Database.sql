IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    IF SCHEMA_ID(N'security') IS NULL EXEC(N'CREATE SCHEMA [security];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    IF SCHEMA_ID(N'setting') IS NULL EXEC(N'CREATE SCHEMA [setting];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    CREATE TABLE [security].[Roles] (
        [RoleId] int NOT NULL IDENTITY,
        [RoleName] nvarchar(50) NOT NULL,
        [Description] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(100) NULL,
        [ModifiedDate] datetime2 NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    CREATE TABLE [setting].[setting] (
        [SettingId] int NOT NULL IDENTITY,
        [Key] nvarchar(max) NOT NULL,
        [Value] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        [ModifiedDate] datetime2 NULL,
        CONSTRAINT [PK_setting] PRIMARY KEY ([SettingId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    CREATE TABLE [security].[Users] (
        [UserId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [IsLocked] bit NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [LastLoginUtc] datetime2 NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        [ModifiedDate] datetime2 NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [security].[Roles] ([RoleId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'CreatedBy', N'CreatedDate', N'Description', N'IsActive', N'ModifiedBy', N'ModifiedDate', N'RoleName') AND [object_id] = OBJECT_ID(N'[security].[Roles]'))
        SET IDENTITY_INSERT [security].[Roles] ON;
    EXEC(N'INSERT INTO [security].[Roles] ([RoleId], [CreatedBy], [CreatedDate], [Description], [IsActive], [ModifiedBy], [ModifiedDate], [RoleName])
    VALUES (1, N''System'', ''2026-11-08T17:49:23.9804615Z'', N''System Administrator'', CAST(1 AS bit), N''System'', ''2026-11-08T17:49:23.9804993Z'', N''Admin''),
    (2, N''System'', ''2026-11-08T17:49:23.9805503Z'', N''Regular user'', CAST(1 AS bit), N''System'', ''2026-11-08T17:49:23.9805504Z'', N''User''),
    (3, N''System'', ''2026-11-08T17:49:23.9805507Z'', N''Invoice Manager'', CAST(1 AS bit), N''System'', ''2026-11-08T17:49:23.9805508Z'', N''InvoiceManager''),
    (4, N''System'', ''2026-11-08T17:49:23.9805510Z'', N''Invoice Agent'', CAST(1 AS bit), N''System'', ''2026-11-08T17:49:23.9805511Z'', N''Invoice Agent'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'CreatedBy', N'CreatedDate', N'Description', N'IsActive', N'ModifiedBy', N'ModifiedDate', N'RoleName') AND [object_id] = OBJECT_ID(N'[security].[Roles]'))
        SET IDENTITY_INSERT [security].[Roles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SettingId', N'CreatedBy', N'CreatedDate', N'IsActive', N'Key', N'ModifiedBy', N'ModifiedDate', N'Value') AND [object_id] = OBJECT_ID(N'[setting].[setting]'))
        SET IDENTITY_INSERT [setting].[setting] ON;
    EXEC(N'INSERT INTO [setting].[setting] ([SettingId], [CreatedBy], [CreatedDate], [IsActive], [Key], [ModifiedBy], [ModifiedDate], [Value])
    VALUES (1, N''System'', ''2026-11-08T17:49:23.9810531Z'', CAST(1 AS bit), N''EcryptionKey'', NULL, ''2026-11-08T17:49:23.9810532Z'', N''X9#mP7@vL2!qR8$zN5&wK1*Tj6^Ys3%Hu9+Cg4=Db7?Fm2!Qr8@Wx5#Lp1$Zn6^Vk3&Te0'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SettingId', N'CreatedBy', N'CreatedDate', N'IsActive', N'Key', N'ModifiedBy', N'ModifiedDate', N'Value') AND [object_id] = OBJECT_ID(N'[setting].[setting]'))
        SET IDENTITY_INSERT [setting].[setting] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [security].[Users] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108174924_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261108174924_Initial', N'10.0.14');
END;

COMMIT;
GO

