CREATE TABLE [dbo].[Images] (
    [Id]     UNIQUEIDENTIFIER CONSTRAINT [DF_Images_Id] DEFAULT (newid()) NOT NULL,
    [Path]   NVARCHAR (255)   NOT NULL,
    [Length] INT              NOT NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC)
);

