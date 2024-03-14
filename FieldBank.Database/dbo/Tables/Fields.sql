CREATE TABLE [dbo].[Fields] (
    [FieldId]            UNIQUEIDENTIFIER NOT NULL,
    [Label]              NVARCHAR (255)   NOT NULL,
    [PageId]             UNIQUEIDENTIFIER NOT NULL,
    [InputTypeId]        UNIQUEIDENTIFIER NOT NULL,
    [DataTypeId]         UNIQUEIDENTIFIER NOT NULL,
    [Length]             BIGINT           NOT NULL,
    [IsRequired]         BIT              NOT NULL,
    [IsReadonly]         BIT              NOT NULL,
    [ValidationMessage]  NVARCHAR (MAX)   NULL,
    [InformationMessage] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_FieldId] PRIMARY KEY CLUSTERED ([FieldId] ASC),
    CONSTRAINT [FK_Fields_DataTypeId] FOREIGN KEY ([DataTypeId]) REFERENCES [dbo].[DataTypes] ([DataTypeId]),
    CONSTRAINT [FK_Fields_InputTypeId] FOREIGN KEY ([InputTypeId]) REFERENCES [dbo].[InputTypes] ([InputTypeId]),
    CONSTRAINT [FK_Fields_PageId] FOREIGN KEY ([PageId]) REFERENCES [dbo].[Pages] ([PageId]),
    CONSTRAINT [UC_Fields_Label] UNIQUE NONCLUSTERED ([Label] ASC)
);

