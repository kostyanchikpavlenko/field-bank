CREATE TABLE [dbo].[InputTypes] (
    [InputTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Type]        NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_InputTypeId] PRIMARY KEY CLUSTERED ([InputTypeId] ASC),
    CONSTRAINT [UC_InputTypes_Name] UNIQUE NONCLUSTERED ([Type] ASC)
);

