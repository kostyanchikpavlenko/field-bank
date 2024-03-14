CREATE TABLE [dbo].[Projects] (
    [ProjectId]     UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (255)   NOT NULL,
    [ProjectTypeId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ProjectId] PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_ProjectTypes_ProjectTypeId] FOREIGN KEY ([ProjectTypeId]) REFERENCES [dbo].[ProjectTypes] ([ProjectTypeId]),
    CONSTRAINT [UC_Projects_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

