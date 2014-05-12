/****** Object:  Table [dbo].[Documents] ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Documents]') AND type in (N'U'))
DROP TABLE [dbo].[Documents]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextBlocks]') AND type in (N'U'))
DROP TABLE [dbo].[TextBlocks]
GO

/****** Object:  Table [dbo].[Documents] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Documents](
	[guid] [uniqueidentifier] NULL,
	[hash] [uniqueidentifier] NULL,
	[title] [nvarchar](400) NULL,
	[description] [nvarchar](400) NULL,
	[snippet] [nvarchar](1000) NULL,
	[category] [nvarchar](400) NULL,
	[link] [varchar](400) NULL,
	[responseUrl] [varchar](400) NULL,
	[urlKey] [varchar](400) NULL,
	[time] [datetime] NULL,
	[pubDate] [char](100) NULL,
	[mimeType] [varchar](80) NULL,
	[charSet] [varchar](40) NULL,
	[contentLength] [int] NULL,
	[domainName] [varchar](100) NULL,
	[bprBoilerplateCharCount] [int] NULL,
	[bprContentCharCount] [int] NULL,
	[unseenContentCharCount] [int] NULL,
	[rev] [int] NULL,
	[fileName] [varchar](100) NULL,
	[siteId] [nvarchar](100) NULL,
	CONSTRAINT [UQ_Documents_id] UNIQUE NONCLUSTERED 
	(
		[guid] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = ON, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[TextBlocks] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TextBlocks](
	[docGuid] [uniqueidentifier] NOT NULL,
	[hashCodes] [varbinary](max) NOT NULL,
	[hashCodesBase64] [text] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO