Use LibrarySystem;

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
		@PublicationDate AS DATETIME = '1800-01-01',
		@Title AS VARCHAR(255) = 'ParadiseLost',
		@CongressCatalogNumber AS CHAR(12) = NULL,
		@PublicationDescription AS VARCHAR(MAX),
		@DeweyDecimalNumber AS VARCHAR(25) = NULL,
		@PageNum AS INT = 100,
		@Edition AS INT = NULL,
		@Publisher AS VARCHAR(30) = 'Penguin Books USA';
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



--DATA RESET
SELECT * FROM Publication;
DELETE FROM Publication WHERE PublicationID = 1;
DBCC CHECKIDENT ('[Publication]', RESEED, 0)
GO


SELECT * FROM Author;
DELETE FROM Author WHERE AuthorID IN (1,2,3)
DBCC CHECKIDENT ('[Author]', RESEED, 0)
GO


SELECT * FROM PublicationAuthor
DELETE FROM PublicationAuthor WHERE PublicationID = 1

SELECT * FROM PublicationPublisher
DELETE FROM PublicationPublisher WHERE PublicationID = 1;

EXEC InsertBook @Authors = 'William,,Golding', @DebugString = '', @PublisherID = 4,  @ISBN = 1123456782345, @PublicationDate =  '1954-01-01', @Title = 'Lord of the Flies', @CongressCatalogNumber = '1234567891', 
	@PublicationDescription = 'The book focuses on a group of British boys stranded on an uninhabited island and their disastrous attempt to govern themselves. Themes include the tension between groupthink and individuality, between rational and emotional reactions, and between morality and immorality.',
	@DeweyDecimalNumber = '398.024G', @PageNum = 224, @Publisher = 'Faber and Faber';


EXEC InsertBook @Authors = 'Ramez,,Elmasri; Shamkant, B, Navathe', @DebugString = '', @PublisherID = 6,  @ISBN = 9780133970777, @PublicationDate =  '2015-07-08', @Title = 'Fundamentals of Database Systems', @CongressCatalogNumber = '9102345678', 
	@PublicationDescription = 'This book introduces the fundamental concepts necessary for designing, using, and implementing database systems and database applications. Our presentation stresses the fundamentals of database modeling and design, the languages and models provided by the database management systems, and database system implementation techniques.',
	@DeweyDecimalNumber = '123.012N', @PageNum = 1280 , @Publisher = 'Pearson';


EXEC InsertBook @Authors = 'George,,Orwell', @DebugString = '', @PublisherID = 7,  @ISBN = 9780451524935, @PublicationDate =  '1961-01-01', @Title = 'Nineteen Eighty-Four', @CongressCatalogNumber = '8672843684', 
	@PublicationDescription = 'Thematically, Nineteen Eighty-Four centres on the consequences of totalitarianism, mass surveillance, and repressive regimentation of persons and behaviours within society.[2][3] Orwell, himself a democratic socialist, modelled the authoritarian government in the novel after Stalinist Russia.[2][3][4] More broadly, the novel examines the role of truth and facts within politics and the ways in which they are manipulated.',
	@DeweyDecimalNumber = '823.912', @PageNum = 328;


SELECT * FROM Publication

INSERT INTO Publisher(PublisherName)
VALUES('Random House')


DELETE FROM PublicationPublisher WHERE PublicationPublisher.PublicationID = 6
DELETE FROM PublicationAuthor WHERE PublicationAuthor.PublicationID = 6
DELETE FROM Publication WHERE PublicationID = 6

EXEC GetBookInfo;