﻿CREATE TABLE [dbo].[DataTypes] (
    [DataTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Type]       NVARCHAR (255)   NOT NULL,
    [ProjectId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [UC_DataTypes_Name] UNIQUE NONCLUSTERED ([Type] ASC)
);

