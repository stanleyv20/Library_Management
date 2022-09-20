
USE [LibrarySystem];

CREATE PROCEDURE InsertBook(
	@PublicationTypeID INT = 1,
	@Title VARCHAR(255), 
	@Author VARCHAR(255),
	@ISBN CHAR(13), 
	@PublicationDescription VARCHAR(MAX), 
	@CongressCatalogNumber CHAR(12), 
	@Dewey VARCHAR(25),
	@PublisherID INT,
	@PublicationDate DATETIME,  
	@PageNum INT = NULL,
	@Edition INT = NULL
) AS
BEGIN
	BEGIN TRY
		EXEC InsertAuthor @Author;


	END TRY
	BEGIN CATCH
		SELECT
			@ErrorMessage = ERROR_MESSAGE(), 
			@ErrorSeverity = ERROR_SEVERITY(), 
			@ErrorState = ERROR_STATE();
	END CATCH;
END;




DECLARE @TestString VARCHAR(500)= 'Mark,,Twain; George,,Orwell;Edgar, Allan, Poe';
WITH AuthorCTE 
	AS (SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
		FROM STRING_SPLIT(@TestString, ';'))
	SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM AuthorCTE



	DECLARE @TestString VARCHAR(500)= 'Mark,,Twain; George,,Orwell;Edgar, Allan, Poe';
	SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
		FROM STRING_SPLIT(@TestString, ';')) A



	DECLARE @TestString VARCHAR(500)= 'Mark,,Twain; George,,Orwell;Edgar, Allan, Poe';
	WITH AuthorCTE AS
	(SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (
			SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
			FROM STRING_SPLIT(@TestString, ';')
			) A
	)
	INSERT INTO Author
	SELECT * FROM AuthorCTE WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE Author.FirstName = AuthorCTE.FirstName AND Author.MiddleName = AuthorCTE.MiddleName AND Author.LastName = AuthorCTE.LastName)

	
	SELECT * FROM Author

ALTER TABLE Author
ADD LastName VARCHAR(50);

EXEC sp_RENAME 'Author.Name' , 'FirstName', 'COLUMN'



	WITH AuthorCTE AS
	(SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (
			SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
			FROM STRING_SPLIT(@TestString, ';')
			) A
	)
	INSERT INTO Author
	SELECT * FROM AuthorCTE WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE Author.FirstName = AuthorCTE.FirstName AND Author.MiddleName = AuthorCTE.MiddleName AND Author.LastName = AuthorCTE.LastName)


	ALTER PROCEDURE dbo.InsertAuthor @AuthorString VARCHAR(255)
	AS
	WITH AuthorCTE AS
	(SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (
			SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
			FROM STRING_SPLIT(@AuthorString, ';')
			) A
	)
	INSERT INTO Author
	SELECT * FROM AuthorCTE WHERE NOT EXISTS (
				(SELECT AuthorID FROM Author WHERE (Author.FirstName = AuthorCTE.FirstName AND (Author.MiddleName = AuthorCTE.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = AuthorCTE.LastName))
		)
	GO

--DELETE FROM Author WHERE AuthorID IN (4,5,6)

EXEC InsertAuthor 'Mark,,Twain; George,,Orwell;Edgar, Allan, Poe';

EXEC InsertAuthor 'George,,Orwell';
SELECT * FROM Author;

DELETE FROM Author WHERE AuthorID IN (4,5, 6);

SELECT * FROM Author;
DELETE FROM Author WHERE AuthorID IN (1,2,3)
DELETE FROM PublicationAuthor WHERE PublicationAuthor.Author IN (1,2,3)

DBCC CHECKIDENT ('[Author]', RESEED, 0)
GO

ALTER TABLE Author
ALTER COLUMN FirstName VARCHAR(50) NOT NULL;

ALTER TABLE Author
ALTER COLUMN LastName VARCHAR(50) NOT NULL;

DECLARE @TestString VARCHAR(500)= 'Mark,,Twain; George,,Orwell;Edgar, Allan, Poe';
	WITH AuthorCTE AS
	(SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (
			SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
			FROM STRING_SPLIT(@TestString, ';')
			) A
	)
	SELECT * FROM AuthorCTE WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE (Author.FirstName = AuthorCTE.FirstName AND (Author.MiddleName = AuthorCTE.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = AuthorCTE.LastName))

/*
	1. Insert authors that do not exist
	2. Insert Publisher if not exist -- not needed
	3. Insert record into publication
	4. Insert PublicationAuthors
	5. Insert PublicationPublisher
*/

GO
DECLARE @SP_Status AS INT = 0,
		@TestString AS VARCHAR(500)= 'Mark,,Twain; George,,Orwell;Edgar, Allan, Poe',
		@DebugString AS VARCHAR(500),
		@PublisherID AS INT = 1,
		@ISBN AS VARCHAR(13) = '1234567890123',
		@Title AS VARCHAR(255) = 'ParadiseLost',
		@PublicationDate DATETIME = '19501202 01:11:09 AM', 
		@CongressCatalogNumber AS CHAR(12) = NULL,
		@PublicationDescription AS VARCHAR(MAX),
		@DeweyDecimalNumber AS VARCHAR(25) = NULL,
		@PageNum AS INT = 100,
		@Edition AS INT = NULL;
BEGIN TRY
	BEGIN TRANSACTION;
		WITH AuthorCTE AS
	(SELECT FirstName = PARSENAME(SplitString, 3),
		   MiddleName = PARSENAME(SplitString, 2),
		   LastName = PARSENAME(SplitString, 1)
	FROM (
			SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
			FROM STRING_SPLIT(@TestString, ';')
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

	INSERT INTO Publication(PublicationTypeID, Title, CongressCatalogNumber, PublicationDate, ISBN, PublicationDescription, PageNum, Edition, DeweyDecimalNumber)
	VALUES(1, @Title, @CongressCatalogNumber, @PublicationDate, @ISBN, @PublicationDescription, @PageNum, @Edition, @DeweyDecimalNumber);

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
	SELECT ERROR_LINE() AS ErrorLine;
	
	ROLLBACK TRANSACTION
END CATCH 


