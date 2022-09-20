USE [LibrarySystem]
GO
/****** Object:  StoredProcedure [dbo].[DeleteBook]    Script Date: 3/9/2021 9:41:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	1. Update Publication Table
	2. Add any author not currently existing in author table
	3. Update PublicationAuthor
	4. Update PublicationPublisher
*/


ALTER PROCEDURE [dbo].[UpdateBook]
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
	 @PublicationID INT
AS
BEGIN TRY
	BEGIN TRANSACTION
	--Update Publication Table first
	UPDATE Publication 
	SET Title = @Title,
		CongressCatalogNumber = @CongressCatalogNumber,
		ISBN = @ISBN,
		PublicationDescription = @PublicationDescription,
		PageNum = @PageNum,
		Edition = @Edition,
		DeweyDecimalNumber = @DeweyDecimalNumber,
		PublicationDate = @PublicationDate
	WHERE Publication.PublicationID = @PublicationID

	--Create temp table via CTE to handle books with multiple authors
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

	DECLARE @currentAuthorCount INT, @updateAuthorCount INT;
	SELECT @currentAuthorCount = COUNT(*) FROM PublicationAuthor WHERE PublicationAuthor.PublicationID = @PublicationID;
	SELECT @updateAuthorCount = COUNT(*) FROM #temp_author;

	--Check if there is change in authors being associated to publication=
	IF (EXISTS (SELECT #temp_author.FirstName, #temp_author.MiddleName, #temp_author.LastName FROM #temp_author
				WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName))))
					OR (@currentAuthorCount != @updateAuthorCount)
	BEGIN
		INSERT Author(FirstName, MiddleName, LastName)
		SELECT #temp_author.FirstName, #temp_author.MiddleName, #temp_author.LastName FROM #temp_author
		WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName));
	
		DELETE FROM PublicationAuthor WHERE PublicationID = @PublicationID;
		;WITH PubID AS (SELECT @PublicationID AS UpdatePubID),
		 InputAuthorIDs(authID) AS (
		SELECT AuthorID FROM Author, #temp_author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName))
		INSERT INTO PublicationAuthor(Author, PublicationID)
		SELECT authID, UpdatePubID FROM InputAuthorIDs CROSS JOIN PubID
	END
	
	IF EXISTS (SELECT PublisherID FROM PublicationPublisher WHERE PublicationPublisher.PublicationID = @PublicationID AND PublicationPublisher.PublisherID != @PublisherID)
	BEGIN 
		UPDATE PublicationPublisher SET PublisherID = @PublisherID WHERE PublicationID = @PublicationID
	END
	COMMIT TRANSACTION
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
	
	INSERT INTO LibraryDebug(ErrorMessage, ErrorTime) VALUES (@DebugString, GETDATE())

	ROLLBACK TRANSACTION
END CATCH


EXEC UpdateBook @Authors = 'George,,Orwell', @DebugString = '', @PublisherID = 7,  @ISBN = 9780451524935, @PublicationDate =  '1961-01-01', @Title = 'Nineteen Eighty-Four', @CongressCatalogNumber = '8672843684', 
	@PublicationDescription = 'Thematically, Nineteen Eighty-Four centres on the consequences of totalitarianism, mass surveillance, and repressive regimentation of persons and behaviours within society.[2][3] Orwell, himself a democratic socialist, modelled the authoritarian government in the novel after Stalinist Russia.[2][3][4] More broadly, the novel examines the role of truth and facts within politics and the ways in which they are manipulated.',
	@DeweyDecimalNumber = '823.912', @PageNum = 328, @PublicationID = 5;

EXEC UpdateBook @Authors = 'George,,Orwell', @DebugString = '', @PublisherID = 7,  @ISBN = 1123456782345, @PublicationDate =  '1961-01-01', @Title = 'Nineteen Eighty-Four', @CongressCatalogNumber = '8672843684', 
	@PublicationDescription = 'Thematically, Nineteen Eighty-Four centres on the consequences of totalitarianism, mass surveillance, and repressive regimentation of persons and behaviours within society.[2][3] Orwell, himself a democratic socialist, modelled the authoritarian government in the novel after Stalinist Russia.[2][3][4] More broadly, the novel examines the role of truth and facts within politics and the ways in which they are manipulated.',
	@DeweyDecimalNumber = '823.912', @PageNum = 328, @PublicationID = 5;

CREATE TABLE LibraryDebug
(
	ErrorMessage VARCHAR(MAX), 
	ErrorTime DATETIME
);

select * from LibraryDebug

SELECT * FROM Author
SELECT * FROM PublicationAuthor
SELECT * FROM Publication

GO
DECLARE @Authors varchar(500) = 'George,,Orwell', 
		@DebugString varchar(500) = '',
		@PublisherID INT = 7,  
		@ISBN VARCHAR(13) = 9780451524935, 
		@PublicationDate DATETIME =  '1961-01-01',
		@Title VARCHAR(255)= 'Nineteen Eighty-Four', 
		@CongressCatalogNumber VARCHAR(25) = '8672843684', 
		@PublicationDescription VARCHAR(MAX) = 'Thematically, Nineteen Eighty-Four centres on the consequences of totalitarianism, mass surveillance, and repressive regimentation of persons and behaviours within society.[2][3] Orwell, himself a democratic socialist, modelled the authoritarian government in the novel after Stalinist Russia.[2][3][4] More broadly, the novel examines the role of truth and facts within politics and the ways in which they are manipulated.',
		@DeweyDecimalNumber VARCHAR(25) = '823.912', 
		@PageNum INT = 328,
		@PublicationID INT = 5,
		@Edition INT = NULL;

	UPDATE Publication 
	SET Title = @Title,
		CongressCatalogNumber = @CongressCatalogNumber,
		ISBN = @ISBN,
		PublicationDescription = @PublicationDescription,
		PageNum = @PageNum,
		Edition = @Edition,
		DeweyDecimalNumber = @DeweyDecimalNumber,
		PublicationDate = @PublicationDate
	WHERE Publication.PublicationID = @PublicationID

	--Create temp table via CTE to handle books with multiple authors
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
	DECLARE @currentAuthorCount INT, @updateAuthorCount INT;
	SELECT @currentAuthorCount = COUNT(*) FROM PublicationAuthor WHERE PublicationAuthor.PublicationID = @PublicationID;
	SELECT @updateAuthorCount = COUNT(*) FROM #temp_author;
	--Check if there is change in authors being associated to publication=
	IF EXISTS (SELECT #temp_author.FirstName, #temp_author.MiddleName, #temp_author.LastName FROM #temp_author
				WHERE NOT EXISTS 
				(SELECT AuthorID FROM Author, PublicationAuthor WHERE ((PublicationAuthor.PublicationID = @PublicationID AND PublicationAuthor.Author = Author.AuthorID) 
																		AND Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName)))
																		OR (@currentAuthorCount != @updateAuthorCount)

					
	BEGIN
		SELECT * FROM #temp_author;

		INSERT Author(FirstName, MiddleName, LastName)
		SELECT #temp_author.FirstName, #temp_author.MiddleName, #temp_author.LastName FROM #temp_author
		WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName));
	
		DELETE FROM PublicationAuthor WHERE PublicationID = @PublicationID;
		;WITH PubID AS (SELECT @PublicationID AS UpdatePubID),
		 InputAuthorIDs(authID) AS (
		SELECT AuthorID FROM Author, #temp_author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName))
		INSERT INTO PublicationAuthor(Author, PublicationID)
		SELECT authID, UpdatePubID FROM InputAuthorIDs CROSS JOIN PubID
	
	END
	
	IF EXISTS (SELECT PublisherID FROM PublicationPublisher WHERE PublicationPublisher.PublicationID = @PublicationID AND PublicationPublisher.PublisherID != @PublisherID)
	BEGIN 
		UPDATE PublicationPublisher SET PublisherID = @PublisherID WHERE PublicationID = @PublicationID
	END

	SELECT * FROM Author;
	SELECT * FROM PublicationAuthor;