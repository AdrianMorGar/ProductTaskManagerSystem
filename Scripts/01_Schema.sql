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
GO

CREATE TABLE [Productos] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Stock] int NOT NULL,
    [Categoria] nvarchar(50) NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Tareas] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(250) NOT NULL,
    [EstaCompletada] bit NOT NULL,
    [ProductoId] int NOT NULL,
    CONSTRAINT [PK_Tareas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tareas_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Tareas_ProductoId] ON [Tareas] ([ProductoId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251216132522_InitialCreate', N'8.0.22');
GO

COMMIT;
GO

