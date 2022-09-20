USE [LibrarySystem]
/*
	1. Delete from PublicationPublisher
	2. Delete from PublicationAuthor
	3. Delete from Publication
*/
SELECT * FROM Publication
SELECT * FROM PublicationPublisher
SELECT * FROM PublicationAuthor
SELECT * FROM Author;

USE [LibrarySystem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].DeleteBook
	@PublicationID INT
AS
BEGIN TRY
	BEGIN TRANSACTION;
		DELETE FROM PublicationPublisher WHERE PublicationPublisher.PublicationID = @PublicationID;
		SELECT Author INTO #AuthorIDTemp FROM PublicationAuthor WHERE PublicationAuthor.PublicationID = @PublicationID;
		DELETE FROM PublicationAuthor WHERE PublicationAuthor.PublicationID = @PublicationID;
		DELETE FROM Publication WHERE Publication.PublicationID = @PublicationID;

		DECLARE @CurrentAuthorID INT;
		WHILE EXISTS (SELECT * FROM #AuthorIDTemp)
		BEGIN
			SELECT TOP 1 @CurrentAuthorID = Author FROM #AuthorIDTemp;
			IF((SELECT COUNT(PublicationID) FROM PublicationAuthor WHERE Author = @CurrentAuthorID) < 1)
			BEGIN
				DELETE FROM Author WHERE Author.AuthorID = @CurrentAuthorID;
			END
			DELETE FROM #AuthorIDTemp WHERE Author = @CurrentAuthorID;
		END
		DROP TABLE #AuthorIDTemp;

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
	SELECT @ErrorMessage AS Debug ;
	ROLLBACK TRANSACTION
END CATCH
	
