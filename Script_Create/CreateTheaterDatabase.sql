/****** Object:  Database [TheaterDatabase]    Script Date: 08/07/2021 22:36:44 ******/
CREATE DATABASE [TheaterDatabase]
GO

USE [TheaterDatabase]
GO

/****** Object:  Table [dbo].[Credentials]    Script Date: 08/07/2021 22:57:28 ******/
CREATE TABLE [dbo].[Credentials](
	[CredentialID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](250) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Credentials] PRIMARY KEY CLUSTERED 
(
	[CredentialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Movies]    Script Date: 08/07/2021 22:57:51 ******/
CREATE TABLE [dbo].[Movies](
	[MovieID] [int] IDENTITY(1,1) NOT NULL,
	[Tittle] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Duration] [int] NOT NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
(
	[MovieID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Rooms]    Script Date: 08/07/2021 22:58:14 ******/
CREATE TABLE [dbo].[Rooms](
	[RoomID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[SeatsNumber] [int] NOT NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Sections]    Script Date: 08/07/2021 22:56:12 ******/
CREATE TABLE [dbo].[Sections](
	[SectionID] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Value] [decimal](10, 2) NOT NULL,
	[AnimationType] [int] NOT NULL,
	[AudioType] [int] NOT NULL,
	[MovieID] [int] NOT NULL,
	[RoomID] [int] NOT NULL,
 CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Sections]  WITH CHECK ADD  CONSTRAINT [FK_Sections_Movies] FOREIGN KEY([MovieID])
REFERENCES [dbo].[Movies] ([MovieID])
GO

ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_Sections_Movies]
GO

ALTER TABLE [dbo].[Sections]  WITH CHECK ADD  CONSTRAINT [FK_Sections_Rooms] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Rooms] ([RoomID])
GO

ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_Sections_Rooms]
GO

INSERT INTO [dbo].[Rooms] ([Name], [SeatsNumber]) VALUES
('Sala 1', 42),
('Sala 2', 50),
('Sala 3', 30),
('Sala 4', 102)
GO

INSERT INTO [dbo].[Credentials] ([Login], [Password]) VALUES
('admin', 'P@ssw0rd')
GO





