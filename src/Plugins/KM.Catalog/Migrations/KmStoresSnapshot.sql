﻿-- MySql
CREATE TABLE KmStoresSnapshot(
	Id int NOT NULL AUTO_INCREMENT,
	CreatedOnUtc TIMESTAMP NOT NULL DEFAULT (UTC_TIMESTAMP),
    Version INT NOT NULL,
    Data LONGTEXT NOT NULL,
    PRIMARY KEY (Id)
);

-- Sql Server 

CREATE TABLE [dbo].[KmStoresSnapshot](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOnUtc] [datetime2](7) NULL,
	[Version] [int] NOT NULL,
	[Data] [nvarchar](MAX) NULL,
CONSTRAINT [PK_KmStoresSnapshot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[KmStoresSnapshot] ADD  CONSTRAINT [DF_KmStoresSnapshot_CreatedOnUtc]  DEFAULT (getutcdate()) FOR [CreatedOnUtc]
GO
