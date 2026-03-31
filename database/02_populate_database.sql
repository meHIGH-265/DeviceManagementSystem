-- Use database
USE DeviceManagementDB;
GO

-- Clear existing data (safe order because of FK)
DELETE FROM Devices;
DELETE FROM Users;

-- Reset identity counters (optional but nice)
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Devices', RESEED, 0);

-- Insert Users
INSERT INTO Users (Name, Role, Location, Email, PasswordHash)
VALUES
('Alice Johnson', 'Admin', 'Cluj', 'alice@example.com', 'hashed_password_1'),
('Bob Smith', 'Employee', 'Bucharest', 'bob@example.com', 'hashed_password_2'),
('Charlie Brown', 'Employee', 'Berlin', 'charlie@example.com', 'hashed_password_3');

-- Insert Devices
INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description, AssignedUserId)
VALUES
('iPhone 15 Pro', 'Apple', 'Phone', 'iOS', '17', 'A17 Pro', '8GB', 'Apple flagship smartphone', 1),
('Galaxy S23', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 2', '8GB', 'High-end Android device', 2),
('iPad Air', 'Apple', 'Tablet', 'iPadOS', '16', 'M1', '8GB', 'Lightweight tablet', NULL),
('Surface Pro 9', 'Microsoft', 'Tablet', 'Windows', '11', 'Intel i7', '16GB', '2-in-1 business device', 3);

-- See the sample data
SELECT * FROM Users;
SELECT * FROM Devices;
