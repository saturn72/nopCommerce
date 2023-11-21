﻿-- MySql
CREATE TABLE StoreSnapshot(
	Id int NOT NULL AUTO_INCREMENT,
	CreatedOnUtc TIMESTAMP NOT NULL DEFAULT (UTC_TIMESTAMP),
    Version INT NOT NULL,
    Json LONGTEXT NOT NULL,
    PRIMARY KEY (Id)
);

-- Sql Server 

CREATE TABLE [dbo].[StoreSnapshot](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOnUtc] [datetime2](7) NULL,
	[Version] [int] NOT NULL,
	[Json] [nvarchar](MAX) NULL,
CONSTRAINT [PK_StoreSnapshot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[StoreSnapshot] ADD  CONSTRAINT [DF_StoreSnapshot_CreatedOnUtc]  DEFAULT (getutcdate()) FOR [CreatedOnUtc]
GO
