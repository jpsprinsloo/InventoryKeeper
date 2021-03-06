
USE [InventoryKeeper]
GO
/****** Object:  UserDefinedFunction [dbo].[MonthToDays365]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE FUNCTION [dbo].[MonthToDays365] (@month int)
  RETURNS int
  WITH SCHEMABINDING
  AS
  -- converts the given month (0-12) to the corresponding number of days into the year (by end of month)
  -- this function is for non-leap years
  BEGIN 
  RETURN
      CASE @month
          WHEN 0 THEN 0
          WHEN 1 THEN 31
          WHEN 2 THEN 59
          WHEN 3 THEN 90
          WHEN 4 THEN 120
          WHEN 5 THEN 151
          WHEN 6 THEN 181
          WHEN 7 THEN 212
          WHEN 8 THEN 243
          WHEN 9 THEN 273
          WHEN 10 THEN 304
          WHEN 11 THEN 334
          WHEN 12 THEN 365
          ELSE 0
      END
  END
  

GO
/****** Object:  UserDefinedFunction [dbo].[MonthToDays366]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  CREATE FUNCTION [dbo].[MonthToDays366] (@month int)
  RETURNS int 
  WITH SCHEMABINDING
  AS
  -- converts the given month (0-12) to the corresponding number of days into the year (by end of month)
  -- this function is for leap years
  BEGIN 
  RETURN
      CASE @month
          WHEN 0 THEN 0
          WHEN 1 THEN 31
          WHEN 2 THEN 60
          WHEN 3 THEN 91
          WHEN 4 THEN 121
          WHEN 5 THEN 152
          WHEN 6 THEN 182
          WHEN 7 THEN 213
          WHEN 8 THEN 244
          WHEN 9 THEN 274
          WHEN 10 THEN 305
          WHEN 11 THEN 335
          WHEN 12 THEN 366
          ELSE 0
      END
  END
  

GO
/****** Object:  UserDefinedFunction [dbo].[MonthToDays]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  CREATE FUNCTION [dbo].[MonthToDays] (@year int, @month int)
  RETURNS int
  WITH SCHEMABINDING
  AS
  -- converts the given month (0-12) to the corresponding number of days into the year (by end of month)
  -- this function is for non-leap years
  BEGIN 
  RETURN 
      -- determine whether the given year is a leap year
      CASE 
          WHEN (@year % 4 = 0) and ((@year % 100  != 0) or ((@year % 100 = 0) and (@year % 400 = 0))) THEN dbo.MonthToDays366(@month)
          ELSE dbo.MonthToDays365(@month)
      END
  END
  

GO
/****** Object:  UserDefinedFunction [dbo].[DateToTicks]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  CREATE FUNCTION [dbo].[DateToTicks] (@year int, @month int, @day int)
  RETURNS bigint
  WITH SCHEMABINDING
  AS
  -- converts the given year/month/day to the corresponding ticks
  BEGIN 
  RETURN CONVERT(bigint, (((((((@year - 1) * 365) + ((@year - 1) / 4)) - ((@year - 1) / 100)) + ((@year - 1) / 400)) + dbo.MonthToDays(@year, @month - 1)) + @day) - 1) * 864000000000;
  END
  

GO
/****** Object:  UserDefinedFunction [dbo].[TimeToTicks]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  CREATE FUNCTION [dbo].[TimeToTicks] (@hour int, @minute int, @second int)  
  RETURNS bigint 
  WITH SCHEMABINDING
  AS 
  -- converts the given hour/minute/second to the corresponding ticks
  BEGIN 
  RETURN (((@hour * 3600) + CONVERT(bigint, @minute) * 60) + CONVERT(bigint, @second)) * 10000000
  END
  

GO
/****** Object:  UserDefinedFunction [dbo].[DateTimeToTicks]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  CREATE FUNCTION [dbo].[DateTimeToTicks] (@LastEditDate datetime)
  RETURNS bigint
  WITH SCHEMABINDING
  AS
  -- converts the given datetime to .NET-compatible ticks
  -- see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemdatetimeclasstickstopic.asp
  BEGIN 
  RETURN 
      dbo.DateToTicks(DATEPART(yyyy, @LastEditDate), DATEPART(mm, @LastEditDate), DATEPART(dd, @LastEditDate)) +
      dbo.TimeToTicks(DATEPART(hh, @LastEditDate), DATEPART(mi, @LastEditDate), DATEPART(ss, @LastEditDate)) +
      (CONVERT(bigint, DATEPART(ms, @LastEditDate)) * CONVERT(bigint,10000));
  END
  

GO
/****** Object:  Table [dbo].[InventoryItems]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NOT NULL,
	[ItemDescr] [varchar](100) NOT NULL,
	[SN] [varchar](100) NOT NULL,
	[Photo] [varbinary](max) NULL,
	[LastEditDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Triggers] UNIQUE NONCLUSTERED 
(
	[Id] ASC,
	[SN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ListItems]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NULL,
	[ListItemName] [varchar](100) NOT NULL,
	[ListItemDescr] [varchar](250) NULL,
	[ListItemValue] [varchar](250) NULL,
	[LastEditDate] [datetime] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_ListItem] UNIQUE NONCLUSTERED 
(
	[TypeId] ASC,
	[ListItemName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[InventoryItems] ADD  DEFAULT (getdate()) FOR [LastEditDate]
GO
ALTER TABLE [dbo].[ListItems] ADD  DEFAULT (getdate()) FOR [LastEditDate]
GO
ALTER TABLE [dbo].[ListItems] ADD  DEFAULT ((1)) FOR [IsEnabled]
GO
ALTER TABLE [dbo].[InventoryItems]  WITH CHECK ADD  CONSTRAINT [FK_InventoryItem_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[ListItems] ([Id])
GO
ALTER TABLE [dbo].[InventoryItems] CHECK CONSTRAINT [FK_InventoryItem_TypeId]
GO
ALTER TABLE [dbo].[ListItems]  WITH CHECK ADD  CONSTRAINT [FK_ListItem_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[ListItems] ([Id])
GO
ALTER TABLE [dbo].[ListItems] CHECK CONSTRAINT [FK_ListItem_TypeId]
GO
/****** Object:  StoredProcedure [dbo].[IK_Add_InventoryItem]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Add_InventoryItem]
	@TypeId INT = null,
	@ItemDescr varchar(100),
	@SN varchar(100),
	@Photo varbinary(max)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[InventoryItems] (TypeId, ItemDescr, SN, Photo, LastEditDate)
    VALUES(@TypeId, @ItemDescr, @SN, @Photo, GETDATE())

    SELECT SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[IK_Add_ListItem]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Add_ListItem]
	@TypeId INT = null,
	@ListItemName varchar(100),
	@ListItemDescr varchar(250),
	@ListItemValue varchar(250),
	@IsEnabled bit
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ListItems] (TypeId, ListItemName, ListItemDescr, ListItemValue, LastEditDate, IsEnabled)
    VALUES(@TypeId, @ListItemName, @ListItemDescr, @ListItemValue, GETDATE(), @IsEnabled)

    SELECT SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[IK_Delete_InventoryItem]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Delete_InventoryItem] 
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM InventoryItems WHERE Id = @Id)
		RETURN 1;
		
	DELETE FROM InventoryItems WHERE Id = @Id

	RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[IK_Delete_ListItem]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Delete_ListItem] 
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM ListItems WHERE Id = @Id)
		RETURN 1;
		
	DELETE FROM ListItems WHERE Id = @Id

	RETURN 0;
END
GO
/****** Object:  StoredProcedure [dbo].[IK_GetAllListItems]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[IK_GetAllListItems]
AS
BEGIN
	SELECT * FROM ListItems WHERE IsEnabled = 1
END

GO
/****** Object:  StoredProcedure [dbo].[IK_GetInventoryItemById]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[IK_GetInventoryItemById]
	@Id int 
AS
BEGIN
	SELECT 
		Id, 
		TypeId, 
		ItemDescr, 
		SN, 
		Photo, 
		[dbo].[DateTimeToTicks](LastEditDate) AS Age 
	FROM InventoryItems 
	WHERE Id = @Id
END


GO
/****** Object:  StoredProcedure [dbo].[IK_GetListItemById]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                
CREATE PROCEDURE [dbo].[IK_GetListItemById]
	@Id int = 0
AS
BEGIN
	SELECT 
		Id,
		TypeId,
		ListItemName,
		ListItemDescr,
		ListItemValue,
		IsEnabled,
		[dbo].[DateTimeToTicks](LastEditDate) AS Age  
	FROM ListItems 
	WHERE Id = @Id AND IsEnabled = 1
END


GO
/****** Object:  StoredProcedure [dbo].[IK_GetListItemByName]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[IK_GetListItemByName]
	@Name varchar(100) 
AS
BEGIN
	SELECT 
		Id,
		TypeId,
		ListItemName,
		ListItemDescr,
		ListItemValue,
		IsEnabled,
		[dbo].[DateTimeToTicks](LastEditDate) AS Age 
	FROM ListItems 
	WHERE ListItemName = @Name AND IsEnabled = 1
END


GO
/****** Object:  StoredProcedure [dbo].[IK_GetListItemsByTypeName]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[IK_GetListItemsByTypeName]
	@TypeName varchar(100) 
AS
BEGIN
	SELECT 
		l.Id,
		l.TypeId,
		l.ListItemName,
		l.ListItemDescr,
		l.ListItemValue,
		l.IsEnabled,
		[dbo].[DateTimeToTicks](l.LastEditDate) AS Age
	FROM ListItems l
	INNER JOIN ListItems t ON t.Id = l.TypeId
	WHERE t.ListItemName = @TypeName AND l.IsEnabled = 1
END

GO
/****** Object:  StoredProcedure [dbo].[IK_Search_InventoryItems]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Search_InventoryItems]
(
	/* Optional Filters for Dynamic Search*/
	@SearchCriteria VARCHAR(250) = '',
	@TypeId int = 0,
	/*– Pagination Parameters */
	@DisplayStart INT = 0,
	@PageSize INT = 10,
	/*– Sorting Parameters */
	@OrderField INT = 0,
	@SortOrder NVARCHAR(4)='ASC'
)
AS
BEGIN
    /*–Declaring Local Variables corresponding to parameters for modification */
    DECLARE @lCriteria VARCHAR(250),
  
    @lFirstRec INT,
    @lLastRec INT,
    @lTotalRows INT

    /*Setting Local Variables*/
    SET @lCriteria = LTRIM(RTRIM(@SearchCriteria))

	SET @lFirstRec = @DisplayStart
    SET @lLastRec = (@DisplayStart + @PageSize + 1)
    SET @lTotalRows = @lFirstRec - @lLastRec + 1

    ; WITH CTE_Results
    AS (
    SELECT ROW_NUMBER() OVER (ORDER BY
		CASE WHEN (@OrderField = 0)
                    THEN i.LastEditDate
        END DESC,
        CASE WHEN (@OrderField = 1 AND @SortOrder='ASC')
                    THEN t.ListItemName
        END ASC,
        CASE WHEN (@OrderField = 1 AND @SortOrder='DESC')
                    THEN t.ListItemName
        END DESC,

        CASE WHEN (@OrderField = 2 AND @SortOrder='ASC')
                    THEN i.ItemDescr
        END ASC,
        CASE WHEN @OrderField = 2 AND @SortOrder='DESC'
                    THEN i.ItemDescr
        END DESC,

        CASE WHEN @OrderField = 3 AND @SortOrder='ASC'
                THEN i.SN
        END ASC,
        CASE WHEN @OrderField = 3 AND @SortOrder='DESC'
                THEN i.SN
        END DESC

    ) AS ROWNUM,

		Count(*) over () AS TotalCount, 
		i.Id AS Id,
		i.ItemDescr,
		t.ListItemName AS TypeName,
		i.SN,
		i.Photo
	FROM  [dbo].[InventoryItems] i
	INNER JOIN [dbo].[ListItems] t on t.Id = i.TypeId
	WHERE (i.TypeId = @TypeId or @TypeId = 0)
		AND (i.ItemDescr like '%' + @lCriteria + '%' OR i.SN LIKE '%' + @lCriteria + '%' OR @lCriteria = '')
		

)
SELECT
    CPC.TotalCount,
    CPC.ROWNUM,
    CPC.Id,
	CPC.ItemDescr,
	CPC.SN,
	CPC.TypeName,
	CPC.Photo
FROM CTE_Results AS CPC
WHERE ROWNUM > @lFirstRec AND ROWNUM < @lLastRec
ORDER BY ROWNUM ASC

END
GO
/****** Object:  StoredProcedure [dbo].[IK_Search_ListItems]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Search_ListItems]
(
	/* Optional Filters for Dynamic Search*/
	@SearchCriteria VARCHAR(250) = '',
	/*– Pagination Parameters */
	@DisplayStart INT = 0,
	@PageSize INT = 10,
	/*– Sorting Parameters */
	@OrderField INT = 0,
	@SortOrder NVARCHAR(4)='ASC'
)
AS
BEGIN
    /*–Declaring Local Variables corresponding to parameters for modification */
    DECLARE @lCriteria VARCHAR(250),
  
    @lFirstRec INT,
    @lLastRec INT,
    @lTotalRows INT

    /*Setting Local Variables*/
    SET @lCriteria = LTRIM(RTRIM(@SearchCriteria))

	SET @lFirstRec = @DisplayStart
    SET @lLastRec = (@DisplayStart + @PageSize + 1)
    SET @lTotalRows = @lFirstRec - @lLastRec + 1

    ; WITH CTE_Results
    AS (
    SELECT ROW_NUMBER() OVER (ORDER BY
		CASE WHEN (@OrderField = 0)
                    THEN l.LastEditDate
        END DESC,
        CASE WHEN (@OrderField = 1 AND @SortOrder='ASC')
                    THEN t.ListItemName
        END ASC,
        CASE WHEN (@OrderField = 1 AND @SortOrder='DESC')
                    THEN t.ListItemName
        END DESC,

        CASE WHEN (@OrderField = 2 AND @SortOrder='ASC')
                    THEN l.ListItemName
        END ASC,
        CASE WHEN @OrderField = 2 AND @SortOrder='DESC'
                    THEN l.ListItemName
        END DESC,

        CASE WHEN @OrderField = 3 AND @SortOrder='ASC'
                THEN l.ListItemDescr
        END ASC,
        CASE WHEN @OrderField = 3 AND @SortOrder='DESC'
                THEN l.ListItemDescr
        END DESC

    ) AS ROWNUM,

		Count(*) over () AS TotalCount, 
		l.Id AS Id,
		l.ListItemName,
		l.ListItemDescr,
		t.ListItemName AS TypeName
	FROM  [dbo].[ListItems] l
	LEFT JOIN [dbo].[ListItems] t on t.Id = l.TypeId
	where (l.ListItemName like '%' + @lCriteria + '%' OR l.ListItemDescr LIKE '%' + @lCriteria + '%' OR @lCriteria = '')	

)
SELECT
    CPC.TotalCount,
    CPC.ROWNUM,
    CPC.Id,
	CPC.ListItemName,
	CPC.ListItemDescr,
	CPC.TypeName
FROM CTE_Results AS CPC
WHERE ROWNUM > @lFirstRec AND ROWNUM < @lLastRec
ORDER BY ROWNUM ASC

END
GO
/****** Object:  StoredProcedure [dbo].[IK_Update_InventoryItem]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Update_InventoryItem]
	@Id INT = null,
	@TypeId INT = null,
	@ItemDescr varchar(100),
	@SN varchar(100),
	@Photo varbinary(max),
	@Age BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE InventoryItems SET 
		TypeId = @TypeId,
		ItemDescr = @ItemDescr, 
		SN = @SN, 
		Photo = @Photo,  
		LastEditDate = GETDATE()
    WHERE Id = @Id AND [dbo].[DateTimeToTicks](LastEditDate) = @Age 

    IF @@ROWCOUNT = 0
        SELECT 0
    ELSE
		SELECT @Id
END
GO
/****** Object:  StoredProcedure [dbo].[IK_Update_ListItem]    Script Date: 13/04/2018 13:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IK_Update_ListItem]
	@Id INT = null,
	@TypeId INT = null,
	@ListItemName varchar(100),
	@ListItemDescr varchar(250),
	@ListItemValue varchar(250),
	@IsEnabled BIT,
	@Age BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE ListItems SET 
		TypeId = @TypeId,
		ListItemName = @ListItemName, 
		ListItemDescr = @ListItemDescr, 
		ListItemValue = @ListItemValue,  
		LastEditDate = GETDATE()
    WHERE Id = @Id AND [dbo].[DateTimeToTicks](LastEditDate) = @Age 

    IF @@ROWCOUNT = 0
        SELECT 0
    ELSE
		SELECT @Id
END
GO

INSERT INTO [dbo].[ListItems] ([TypeId],[ListItemName],[ListItemDescr],[ListItemValue],[LastEditDate],[IsEnabled])
VALUES(null,'Hardware','Hardware','',GETDATE(),1)
GO

INSERT INTO [dbo].[ListItems] ([TypeId],[ListItemName],[ListItemDescr],[ListItemValue],[LastEditDate],[IsEnabled])
VALUES(null,'Software','Software','',GETDATE(),1)
GO

INSERT INTO [dbo].[ListItems] ([TypeId],[ListItemName],[ListItemDescr],[ListItemValue],[LastEditDate],[IsEnabled])
VALUES((SELECT Id FROM dbo.ListItems WHERE ListItemName = 'Hardware'),'Mouse','Mouse','',GETDATE(),1)
GO

INSERT INTO [dbo].[ListItems] ([TypeId],[ListItemName],[ListItemDescr],[ListItemValue],[LastEditDate],[IsEnabled])
VALUES((SELECT Id FROM dbo.ListItems WHERE ListItemName = 'Software'),'VS2015','Visual Studio 2015','',GETDATE(),1)
GO

INSERT INTO [dbo].[ListItems] ([TypeId],[ListItemName],[ListItemDescr],[ListItemValue],[LastEditDate],[IsEnabled])
VALUES((SELECT Id FROM dbo.ListItems WHERE ListItemName = 'Software'),'VS2017','Visual Studio 2017','',GETDATE(),1)
GO