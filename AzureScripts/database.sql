USE [master]
GO
/****** Object:  Database [dispatchDb]    Script Date: 1/30/2017 9:22:31 AM ******/
CREATE DATABASE [dispatchDb]
GO
ALTER DATABASE [dispatchDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dispatchDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dispatchDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dispatchDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dispatchDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dispatchDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dispatchDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [dispatchDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dispatchDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dispatchDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dispatchDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dispatchDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dispatchDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dispatchDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dispatchDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dispatchDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dispatchDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dispatchDb] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [dispatchDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dispatchDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [dispatchDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dispatchDb] SET  MULTI_USER 
GO
ALTER DATABASE [dispatchDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dispatchDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [dispatchDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [dispatchDb]
GO
/****** Object:  Table [dbo].[drones]    Script Date: 1/30/2017 9:22:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[drones](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[description] [nvarchar](max) NULL,
	[droneName] [nvarchar](max) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Index [createdAt]    Script Date: 1/30/2017 9:22:36 AM ******/
CREATE CLUSTERED INDEX [createdAt] ON [dbo].[drones]
(
	[createdAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[images]    Script Date: 1/30/2017 9:22:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[images](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[uri] [nvarchar](max) NULL,
	[latitude] [float] NULL,
	[droneId] [nvarchar](max) NULL,
	[longitude] [nvarchar](max) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Index [createdAt]    Script Date: 1/30/2017 9:22:37 AM ******/
CREATE CLUSTERED INDEX [createdAt] ON [dbo].[images]
(
	[createdAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[iris]    Script Date: 1/30/2017 9:22:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[iris](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[projectId] [nvarchar](max) NULL,
	[objectName] [nvarchar](max) NULL,
	[irisUri] [nvarchar](max) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Index [createdAt]    Script Date: 1/30/2017 9:22:37 AM ******/
CREATE CLUSTERED INDEX [createdAt] ON [dbo].[iris]
(
	[createdAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[iris_images]    Script Date: 1/30/2017 9:22:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[iris_images](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[imageId] [nvarchar](max) NULL,
	[accuracy] [float] NULL,
	[objectName] [nvarchar](max) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Index [createdAt]    Script Date: 1/30/2017 9:22:37 AM ******/
CREATE CLUSTERED INDEX [createdAt] ON [dbo].[iris_images]
(
	[createdAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[drones] ADD  CONSTRAINT [DF_drones_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[drones] ADD  CONSTRAINT [DF_drones_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[drones] ADD  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[images] ADD  CONSTRAINT [DF_images_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[images] ADD  CONSTRAINT [DF_images_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[images] ADD  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[iris] ADD  CONSTRAINT [DF_iris_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[iris] ADD  CONSTRAINT [DF_iris_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[iris] ADD  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[iris_images] ADD  CONSTRAINT [DF_iris_images_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[iris_images] ADD  CONSTRAINT [DF_iris_images_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[iris_images] ADD  DEFAULT ((0)) FOR [deleted]
GO
USE [master]
GO
ALTER DATABASE [dispatchDb] SET  READ_WRITE 
GO
