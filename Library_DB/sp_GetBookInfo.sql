USE [LibrarySystem]
GO

CREATE PROCEDURE [dbo].[GetBookInfo]
AS
BEGIN TRY
 ;WITH AuthorGroup(PublicationID, PublicationAuthor) AS
		(SELECT P.PublicationID, A.FirstName + ',' + ISNULL(A.MiddleName, '') + ',' + A.LastName
		FROM Publication P INNER JOIN PublicationAuthor PB ON P.PublicationID = PB.PublicationID
		INNER JOIN Author A ON PB.Author = A.AuthorID
		WHERE PB.Author = A.AuthorID)
 SELECT DISTINCT P.PublicationID, P.Title AS Title, STUFF((SELECT ';' + PublicationAuthor FROM AuthorGroup 
								WHERE PublicationID = AG.PublicationID
								ORDER BY PublicationAuthor
								FOR XML PATH('')),1,1,'') AS PublicationAuthor, 
		P.ISBN AS ISBN, P.PublicationDate AS PublicationDate, P.PublicationDescription AS [Description]
 FROM Publication P INNER JOIN PublicationAuthor PB ON P.PublicationID = PB.PublicationID
		INNER JOIN Author A ON PB.Author = A.AuthorID
		INNER JOIN AuthorGroup AG ON AG.PublicationID = P.PublicationID
END TRY
BEGIN CATCH
END CATCH 
GO