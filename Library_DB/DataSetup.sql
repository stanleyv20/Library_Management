USE [LibrarySystem];

INSERT INTO UserType(UserType)
VALUES 
	('UndergraduateStudent'),
	('GraduateStudent'),
	('Faculty');

--INSERT INTO LibraryItem()

INSERT INTO Publication()
VALUES;

INSERT INTO PublicationType(FineRate,FineLimit,PublicationDescription)
VALUES 
(0.5, 50.00, 'PhysicalBook');

DELETE FROM PublicationType WHERE PublicationTypeID = 4;
SELECT * FROM PublicationType;

DBCC CHECKIDENT ('[PublicationType]', RESEED, 0)
GO

INSERT INTO Publication(PublicationTypeID, Title, CongressCatalogNumber, PublicationDate, ISBN, PublicationDescription, PageNum, DeweyDecimalNumber)
VALUES 
	(1, 'Anna Karenina', '000000000001', CONVERT(datetime, '2017-08-25'),'0000000000001', 
	'A complex novel in eight parts, with more than a dozen major characters, it is spread over more than 800 pages (depending on the translation and publisher), typically contained in two volumes. It deals with themes of betrayal, faith, family, marriage, Imperial Russian society, desire, and rural vs. city life. The plot centers on an extramarital affair between Anna and dashing cavalry officer Count Alexei Kirillovich Vronsky that scandalizes the social circles of Saint Petersburg and forces the young lovers to flee to Italy in a search for happiness. After they return to Russia, their lives further unravel.',
	864, '523.43K');

INSERT INTO Publication(PublicationTypeID, Title, AuthorID, CongressCatalogNumber, PublisherId, PublicationDate, ISBN, PublicationDescription, PageNum, DeweyDecimalNumer)
VALUES 
	(1, 'I, Robot', 1, '000000000002', 1, '19501202 01:11:09 AM', '0000000000002', 
	'I, Robot is a fixup novel of science fiction short stories or essays by American writer Isaac Asimov. The stories are woven together by a framing narrative in which the fictional Dr. Susan Calvin tells each story to a reporter (who serves as the narrator) in the 21st century. Although the stories can be read separately, they share a theme of the interaction of humans, robots, and morality, and when combined they tell a larger story of Asimov''s fictional history of robotics.',
	253, '123.45A');

SELECT * FROM Publication;
DELETE FROM Publication WHERE PublicationID = 21

Alter TABLE Publication
ADD ISBN INT

ALTER TABLE Publication ADD PRIMARY KEY (ISBN)

ALTER TABLE Publication DROP COLUMN PublicationDate;
ALTER TABLE Publication ADD PublicationDate DATETIME CHECK (PublicationDate <= CURRENT_TIMESTAMP AND PublicationDate > CAST('1753-1-1' AS DATETIME))

SELECT * FROM Publication

Alter TABLE Publication
DROP COLUMN ISBN 

DELETE FROM Publication WHERE PublicationID = 4;

DELETE FROM PublicationPublisher WHERE PublisherID = 1

INSERT INTO Author(FirstName, LastName)
VALUES
	('Isaac', 'Asimov'),
	('Ayn', 'Rand'),
	('Leo', 'Tolstoy');

SELECT * FROM Publication;

INSERT INTO Publisher(PublisherName)
VALUES ('SomePublisher');

ALTER TABLE Publication
DROP COLUMN ISBN 

DBCC CHECKIDENT ('[Publication]', RESEED, 0)
GO

DROP TABLE CD;
DROP TABLE DVD;
DROP TABLE ElectronicBook;
DROP TABLE Journal;
DROP TABLE LibraryItem;
DROP TABLE Magazine;
DROP TABLE PublicationAuthor;
DROP TABLE PublicationPublisher;
DROP TABLE LoanItem
DROP TABLE Publication


ALTER TABLE Publication
ADD FOREIGN KEY (PublisherID) REFERENCES Publisher(PublisherID);

SELECT * FROM Author
SELECT * FROM PublicationType;

SELECT * FROM PublicationAuthor
INSERT INTO PublicationAuthor VALUES (3,1);

DBCC CHECKIDENT ('[Publication]', RESEED, 0)
GO

DELETE FROM Publication WHERE PublicationID = 2;
SELECT * FROM PublicationAuthor;

SELECT * FROM Publisher;

SELECT * FROM Publication;
SELECT * FROM PublicationPublisher;
INSERT INTO PublicationPublisher VALUES(1,1);

DBCC CHECKIDENT ('[Author]', RESEED, 0)
GO

SELECT * FROM PublicationType

select * from Publication


SELECT * FROM Author;

DELETE FROM Author WHERE AuthorID IN (7,8,9)
DBCC CHECKIDENT ('[Author]', RESEED, 0)
GO

ALTER TABLE Author ADD CONSTRAINT UQ_Fname_Mname_Lname UNIQUE(FirstName, MiddleName, LastName);

ALTER TABLE Publication
ADD CONSTRAINT df_Date
DEFAULT NULL for PublicationDate;

Insert INTO Publisher(PublisherName) VALUES('Faber and Faber');
SELECT * FROM Publisher

Insert INTO Publisher(PublisherName) VALUES('Pearson');
SELECT * FROM Publisher



;WITH AuthorGroup(PublicationID, PublicationAuthor) AS
		(SELECT P.PublicationID, A.FirstName + ',' + ISNULL(A.MiddleName, ',') + A.LastName
		FROM Publication P INNER JOIN PublicationAuthor PB ON P.PublicationID = PB.PublicationID
		INNER JOIN Author A ON PB.Author = A.AuthorID
		WHERE PB.Author = A.AuthorID)
SELECT PublicationID, STUFF((SELECT ';' + PublicationAuthor FROM AuthorGroup 
								WHERE PublicationID = AG.PublicationID
								ORDER BY PublicationAuthor
								FOR XML PATH('')),1,1,'') AS PublicationAuthor
FROM AuthorGroup AG
GROUP BY PublicationID

EXEC GetBookInfo

EXEC GetPublishers

UPDATE Publication
SET Edition = 7
WHERE PublicationID = 4

EXEC InsertBook @Authors = 'Stanley,John,Varghese', @DebugString = '', @PublisherID = 1, @ISBN = '1123456782345', @PublicationDate = '19501202 01:11:09 AM', @Title = 'Duplicate ISBN',
	@CongressCatalogNumber = '9187346278', @PublicationDescription = 'A book with duplicate ISBN', @DeweyDecimalNumber = '916.21', @PageNum = 100, @Edition = 0;