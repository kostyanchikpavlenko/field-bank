CREATE TABLE [dbo].[ProjectTypes] (
    [ProjectTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_ProjectTypeId] PRIMARY KEY CLUSTERED ([ProjectTypeId] ASC),
    CONSTRAINT [UC_ProjectTypes_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

