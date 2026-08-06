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
CREATE TABLE [dbo].[Product] (
    [Id] int NOT NULL IDENTITY,
    [ProductName] nvarchar(255) NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    [CreatedOn] datetime NOT NULL,
    [ModifiedBy] nvarchar(100) NULL,
    [ModifiedOn] datetime NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[User] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(50) NOT NULL,
    [CreatedOn] datetime NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[Item] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Item_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[RefreshToken] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Token] nvarchar(500) NOT NULL,
    [Expires] datetime NOT NULL,
    [Created] datetime NOT NULL,
    [CreatedByIp] nvarchar(100) NOT NULL,
    [Revoked] datetime NULL,
    [RevokedByIp] nvarchar(100) NULL,
    [ReplacedByToken] nvarchar(500) NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshToken_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Item_ProductId] ON [dbo].[Item] ([ProductId]);

CREATE INDEX [IX_RefreshToken_UserId] ON [dbo].[RefreshToken] ([UserId]);

CREATE UNIQUE INDEX [IX_User_Username] ON [dbo].[User] ([Username]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260805181302_InitialCreate', N'10.0.10');

COMMIT;
GO

