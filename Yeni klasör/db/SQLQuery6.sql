CREATE TABLE [dbo].[Yorumlar] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Text]    NVARCHAR (250) NOT NULL,
    [OTarihi] DATETIME       DEFAULT (getdate()) NOT NULL,
    [TarifId] INT            NOT NULL,
    [UserId]  NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Resimler] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (250) NOT NULL,
    [TarifId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Malzemeler] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (250) NOT NULL,
    [TarifId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[YapimAsamalari] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (500) NOT NULL,
    [TarifId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Kategoriler] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Tarifler] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (250) NOT NULL,
    [YSuresi]    NVARCHAR (250) NOT NULL,
    [Porsiyon]   NVARCHAR (250) NOT NULL,
    [Durum]      BIT            DEFAULT ((0)) NOT NULL,
    [OTarihi]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [KategoriId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
ALTER TABLE [dbo].[Malzemeler]  WITH CHECK ADD FOREIGN KEY([TarifId])
REFERENCES [dbo].[Tarifler] ([Id])
GO
ALTER TABLE [dbo].[YapimAsamalari]  WITH CHECK ADD FOREIGN KEY([TarifId])
REFERENCES [dbo].[Tarifler] ([Id])
GO
ALTER TABLE [dbo].[Resimler]  WITH CHECK ADD FOREIGN KEY([TarifId])
REFERENCES [dbo].[Tarifler] ([Id])
GO
ALTER TABLE [dbo].[Tarifler]  WITH CHECK ADD FOREIGN KEY([KategoriId])
REFERENCES [dbo].[Kategoriler] ([Id])
GO
ALTER TABLE [dbo].[Yorumlar]  WITH CHECK ADD FOREIGN KEY([TarifId])
REFERENCES [dbo].[Tarifler] ([Id])
GO