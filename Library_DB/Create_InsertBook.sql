USE [LibrarySystem];
GO

CREATE PROCEDURE [dbo].[InsertBook]
	 @SP_Status INT = 0,
	 @Authors VARCHAR(500),
	 @DebugString VARCHAR(500),
	 @PublisherID INT,
	 @ISBN VARCHAR(13),
	 @PublicationDate DATETIME,
	 @Title VARCHAR(255),
	 @CongressCatalogNumber VARCHAR(25), 
	 @PublicationDescription VARCHAR(MAX),
	 @DeweyDecimalNumber varchar(25),
	 @PageNum INT,
	 @Edition INT = NULL,
	 @Publisher VARCHAR(30)
AS
BEGIN TRY
	BEGIN TRANSACTION;

	IF OBJECT_ID('tempdb..#temp_author') IS NOT NULL
    DROP TABLE #temp_author;

		WITH AuthorCTE AS
	(SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (
			SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
			FROM STRING_SPLIT(@Authors, ';')
			) A
	)
	SELECT FirstName, MiddleName, LastName INTO #temp_author FROM AuthorCTE;
	SELECT * FROM #temp_author;
	
	INSERT Author(FirstName, MiddleName, LastName)
	SELECT #temp_author.FirstName, #temp_author.MiddleName, #temp_author.LastName FROM #temp_author
	WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName));

	IF NOT EXISTS (SELECT PublisherID FROM Publisher WHERE PublisherID = @PublisherID)
	BEGIN
		RAISERROR ('Submited Publisher Not Found', 11, 1); --Message,Severity,State
	END

	INSERT INTO Publication(PublicationTypeID, Title, CongressCatalogNumber, ISBN, PublicationDescription, PageNum, Edition, DeweyDecimalNumber, PublicationDate )
	VALUES(1, @Title, @CongressCatalogNumber, @ISBN, @PublicationDescription, @PageNum, @Edition, @DeweyDecimalNumber, @PublicationDate);

	DECLARE @AddedPublicationID int = (SELECT PublicationID FROM Publication WHERE ISBN = @ISBN);
	

	IF OBJECT_ID('tempdb..#temp_authorID') IS NOT NULL
    DROP TABLE #temp_authorID;

	SELECT AuthorID INTO #temp_authorID 
	FROM Author, #temp_author
	WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName)
	SELECT * FROM #temp_authorID;

	;WITH newPubID AS (SELECT @AddedPublicationID AS AddedPublicationID)
	INSERT INTO PublicationAuthor(Author, PublicationID)
	SELECT AuthorID, AddedPublicationID FROM #temp_authorID CROSS JOIN newPubID
	
	IF NOT EXISTS (SELECT PublisherID FROM Publisher WHERE PublisherName = @Publisher)
		INSERT INTO Publisher(PublisherName) VALUES(@Publisher);

	INSERT INTO PublicationPublisher(PublisherID, PublicationID) 
	VALUES((SELECT PublisherID FROM Publisher WHERE Publisher.PublisherName = @Publisher), @AddedPublicationID)

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH 
	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
	SELECT
		@ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
	
	SET @SP_Status = -1;
	SET @DebugString = ERROR_MESSAGE();
	SELECT @DebugString AS DEBUG;
	--SELECT ERROR_LINE() AS ErrorLine, ERROR_Message() AS ErrorMessage
	
	ROLLBACK TRANSACTION
END CATCH 
