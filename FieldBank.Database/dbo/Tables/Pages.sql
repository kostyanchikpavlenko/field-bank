CREATE TABLE [dbo].[Pages] (
    [PageId]    UNIQUEIDENTIFIER NOT NULL,
    [Name]      NVARCHAR (255)   NOT NULL,
    [ProjectId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PageId] PRIMARY KEY CLUSTERED ([PageId] ASC),
    CONSTRAINT [FK_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]),
    CONSTRAINT [UC_Pages_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

