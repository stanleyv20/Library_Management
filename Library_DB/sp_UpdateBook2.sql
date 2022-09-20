USE [LibrarySystem];

SELECT * FROM Publication;
SELECT * FROM PublicationAuthor;
SELECT * FROM Author
EXEC GetBookInfo

DECLARE @PublicationID INT = 5,
		@PublicationAuthor VARCHAR(500) = 'George,,Orwell;Stanley,John,Varghese',
		@PublisherID INT

	--Create temp table via CTE to handle books with multiple authors
	IF OBJECT_ID('tempdb..#temp_author') IS NOT NULL
    DROP TABLE #temp_author;

		WITH AuthorCTE AS
			(SELECT FirstName = PARSENAME(SplitString, 3),
					MiddleName = PARSENAME(SplitString, 2),
					LastName = PARSENAME(SplitString, 1)
			FROM (
					SELECT SplitString = REPLACE(RTRIM(LTRIM(VALUE)), ',', '.')
					FROM STRING_SPLIT(@PublicationAuthor, ';')
				 ) A
			)
	SELECT FirstName, MiddleName, LastName INTO #temp_author FROM AuthorCTE;
	SELECT * FROM #temp_author;

	--Check if there is change in authors being associated to publication=
	IF EXISTS (SELECT #temp_author.FirstName, #temp_author.MiddleName, #temp_author.LastName FROM #temp_author
				WHERE NOT EXISTS (SELECT AuthorID FROM Author WHERE (Author.FirstName = #temp_author.FirstName AND (Author.MiddleName = #temp_author.MiddleName OR Author.MiddleName IS NULL) AND Author.LastName = #temp_author.LastName)))
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
