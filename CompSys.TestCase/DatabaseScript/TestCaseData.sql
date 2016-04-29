USE [TestCaseData]
GO

/****** Object:  Table [dbo].[Table_GetOrAdd]    Script Date: 29.04.2016 22:53:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Table_GetOrAdd](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[Table_AddOrUpdate](
	[Id] [int] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_Id_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Table_Transfer](
	[Id] [int] NOT NULL,
	[Balance] [money] NULL,
 CONSTRAINT [PK_Id_2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE PROCEDURE [dbo].[sp_GetOrAdd] 
	@name varchar(50), 
	@id int output
AS
BEGIN
	SET NOCOUNT ON;

	IF @name IS NOT NULL
	BEGIN
		BEGIN TRAN;

		INSERT INTO [dbo].[Table_GetOrAdd] ([Name]) SELECT @name
		WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Table_GetOrAdd] WHERE [Name] = @name);

		SELECT @id = Id
		FROM [dbo].[Table_GetOrAdd] --WITH(TABLOCKX)
		WHERE [Name] = @name;
		/*
		IF @id IS NULL
		BEGIN
			INSERT INTO dbo.Table_GetOrAdd
			(Name)
			VALUES
			(@name);

			SELECT @id = SCOPE_IDENTITY();
		END
		*/
		COMMIT TRAN;
	END
END

GO

CREATE PROCEDURE [dbo].[sp_AddOrUpdate] 
	@id int, 
	@value int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @updated int;

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;

	UPDATE [dbo].[Table_AddOrUpdate]
	SET [Value] = @value
	WHERE Id = @id;

	SET @updated = @@ROWCOUNT

	IF @updated = 0 
	BEGIN
		INSERT INTO [dbo].[Table_AddOrUpdate]
		([Id], [Value])
		VALUES
		(@id, @value);
	END

	COMMIT TRAN;
END

GO

CREATE PROCEDURE [dbo].[sp_Transfer] 
	@id1 int,
	@id2 int, 
	@amount money
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sourceAmount MONEY

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
	BEGIN TRAN;

	SELECT Id
	FROM Table_Transfer WITH(ROWLOCK, XLOCK)
	WHERE Id = @id1 or Id = @id2;

	SELECT @sourceAmount = Balance
	FROM Table_Transfer
	WHERE Id = @id1

	IF @sourceAmount >= @amount
	BEGIN
		UPDATE [dbo].[Table_Transfer]
		SET [Balance] = [Balance] - @amount
		WHERE Id = @id1;

		UPDATE [dbo].[Table_Transfer]
		SET [Balance] = [Balance] + @amount
		WHERE Id = @id2;
	END

	COMMIT TRAN;
END

GO

