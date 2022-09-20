/*
	Written by Stanley Varghese for CS6360.MS1, assignment 3, starting February 19th, 2021.
	NetID: sjv140030
*/

CREATE DATABASE LibrarySystem;
--DROP DATABASE LibrarySystem;
USE [LibrarySystem];

CREATE TABLE LibraryUser(
	UserTypeID INT NOT NULL,
	FOREIGN KEY (UserTypeID) REFERENCES UserType(UserTypeID),
	UtdID CHAR(10) PRIMARY KEY,
	NetID VARCHAR(20),
	EmailAddress VARCHAR(500),
	Department INT,
	RentalTimeLimit INT,
	Gender VARCHAR(10),
	--Need to add computed column for fines
);

ALTER TABLE LibraryUser ADD UserFine DECIMAL(4,4);

CREATE TABLE UserType(
	UserTypeID INT IDENTITY(1,1) PRIMARY KEY,
	UserType VARCHAR(50) NOT NULL UNIQUE 
);

CREATE TABLE Phone(
	UserID CHAR(10) NOT NULL,
	PhoneNumber VARCHAR(20) NOT NULL CHECK (PhoneNumber LIKE '[0-9]'),
	PhoneNumberType VARCHAR(30) NOT NULL,
	UNIQUE(UserID, PhoneNumberType),
	FOREIGN KEY (UserID) REFERENCES LibraryUser(UtdID) ON DELETE CASCADE
);

CREATE TABLE UserAddress(
	UtdID CHAR(10) NOT NULL,
	MailingAddress VARCHAR(500) NOT NULL,
	AddressType VARCHAR(20),
	UNIQUE(UtdID, AddressType),
	FOREIGN KEY (UtdID) REFERENCES LibraryUser(UtdID) ON DELETE CASCADE
);

CREATE TABLE Author(
	AuthorID INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	MiddleName VARCHAR(50), --Nullable since not everyone has a middle name
	LastName VARCHAR(50) NOT NULL,
	CONSTRAINT UQ_Fname_Mname_Lname UNIQUE(FirstName, MiddleName, LastName)
);

CREATE TABLE LibraryItem(
	LibItemID INT IDENTITY(1,1) PRIMARY KEY,
	PublicationID INT,
	ItemAvailability BIT,
	CheckOutElibigility BIT,
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID)
);

CREATE TABLE Publication(
	PublicationID INT IDENTITY(1,1) NOT NULL UNIQUE,
	PublicationTypeID INT NOT NULL,
	Title VARCHAR(255) NOT NULL, --Not using VARCHAR(MAX) in case indexing is needed on this column to improve retrieval speed
	CongressCatalogNumber CHAR(12), --Library of congress catalog number
	PublicationDate DATE CHECK (PublicationDate <= CURRENT_TIMESTAMP AND PublicationDate > CAST('1753-1-1' AS DATETIME)), --Ensuring that publication date is not in the future and is before 400CE
	ISBN CHAR(13) NOT NULL UNIQUE,
	PublicationDescription VARCHAR(MAX), --May not want to use MAX length to restrict users from regularly inserting large amounts of data?
	PageNum INT,
	Edition INT,
	DeweyDecimalNumber VARCHAR(25),
	FOREIGN KEY (PublicationTypeID) REFERENCES PublicationType(PublicationTypeID),
	PRIMARY KEY (ISBN)
);

CREATE TABLE PublicationAuthor(
	Author INT NOT NULL,
	PublicationID INT NOT NULL,
	FOREIGN KEY (Author) REFERENCES AUTHOR(AuthorID),
	FOREIGN KEY (PublicationID) REFERENCES PUBLICATION(PublicationID)
);

CREATE TABLE PublicationPublisher(
	PublisherID INT NOT NULL,
	PublicationID INT NOT NULL,
	FOREIGN KEY (PublisherID) REFERENCES Publisher(PublisherID),
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID)
);

CREATE TABLE PublicationType(
	PublicationTypeID INT IDENTITY (1,1) PRIMARY KEY,
	PublicationDescription VARCHAR(30) NOT NULL,
	FineRate DECIMAL(4,4) DEFAULT 0.5, --Default fine rate is currently $0.50 per day but can be set for each publication type if needed in the future
	FineLimit DECIMAL(4,4)
);

CREATE TABLE ElectronicBook(
	PublicationID INT NOT NULL UNIQUE,
	EBookURL VARCHAR(500) NOT NULL, --Not sure on restrictions for URL length
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID),
);

CREATE TABLE Journal(
	PublicationID INT NOT NULL UNIQUE,
	PublicationFrequency CHAR(30) NOT NULL,
	JournalVolume INT NOT NULL,
	ImpactFactor  DECIMAL(5,5),
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID),
)

CREATE TABLE Magazine(
	PublicationID INT NOT NULL UNIQUE,
	ISSN INT NOT NULL UNIQUE, --International Standard Serial Number
	PublicationFrequency CHAR(30) NOT NULL,
	MagazineCategory CHAR(30),
	MagazineEditor CHAR(30),
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID),
)

CREATE TABLE Publisher(
	PublisherID INT IDENTITY (1,1) PRIMARY KEY,
	PublisherName CHAR(100) NOT NULL UNIQUE
)

CREATE TABLE CD(
	PublicationID INT NOT NULL UNIQUE,
	CdFormat CHAR(20) NOT NULL,
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID),
)

CREATE TABLE DVD(
	PublicationID INT NOT NULL UNIQUE,
	RunTime INT, --Film runtime in minutes
	ProductionCompany CHAR(30),
	FilmLanguage CHAR(30),
	FilmRleaseDate DATE, 
	FOREIGN KEY (PublicationID) REFERENCES Publication(PublicationID),
)

CREATE TABLE LoanTerms(
	UserTypeID INT NOT NULL UNIQUE,
	ItemLimit INT NOT NULL,
	TimeLimit INT NOT NULL,
	FOREIGN KEY (UserTypeID) REFERENCES UserType(UserTypeID),
);

CREATE TABLE LoanItem(
	LibItemID INT NOT NULL UNIQUE,
	LibraryUserID CHAR(10) NOT NULL,
	ItemDueDate DATE NOT NULL,
	LoanStartDate DATE NOT NULL, 
	FOREIGN KEY (LibItemID) REFERENCES LibraryItem(LibItemID),
	FOREIGN KEY (LibraryUserID) REFERENCES LibraryUser(UtdID)
);

