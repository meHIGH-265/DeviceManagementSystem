-- Create database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDB')
BEGIN
    CREATE DATABASE DeviceManagementDB;
END
GO

-- Use database
USE DeviceManagementDB;
GO

/*
-- Create test database if it doesn't exist
-- The test database will be used for tests only so that the main database isn't affected during testing
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDB_Test')
BEGIN
    CREATE DATABASE DeviceManagementDB_Test;
END
GO

-- Use database
USE DeviceManagementDB_Test;
GO
*/

-- USERS TABLE
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1),
		CONSTRAINT PK_Users PRIMARY KEY (Id),
        Name NVARCHAR(100) NOT NULL,
        Role NVARCHAR(50) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL,
		CONSTRAINT UQ_Users_Email UNIQUE (Email),
        PasswordHash NVARCHAR(255) NOT NULL
    );
END
GO

-- DEVICES TABLE
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
    CREATE TABLE Devices (
        Id INT IDENTITY(1,1),
		CONSTRAINT PK_Devices PRIMARY KEY (Id),
        Name NVARCHAR(100) NOT NULL,
        Manufacturer NVARCHAR(100) NOT NULL,
        Type NVARCHAR(50) NOT NULL,
        OS NVARCHAR(50) NOT NULL,
        OSVersion NVARCHAR(50),
        Processor NVARCHAR(100),
        RAM NVARCHAR(50),
        Description NVARCHAR(500),
        AssignedUserId INT NULL,
        CONSTRAINT FK_Devices_Users FOREIGN KEY (AssignedUserId)
            REFERENCES Users(Id)
            ON DELETE SET NULL
    );
END
GO
