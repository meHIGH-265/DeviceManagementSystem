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
('Charlie Brown', 'Employee', 'Berlin', 'charlie@example.com', 'hashed_password_3'),
('Mihai Secosan', 'Junior', 'Cluj-Napoca', 'secosanmihaisebastian@gmail.com', 'pass1234');

-- Insert Devices
INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description, AssignedUserId)
VALUES
('iPhone 15 Pro', 'Apple', 'Phone', 'iOS', '17', 'A17 Pro', '8GB', 'Apple flagship smartphone', 1),
('Galaxy S23', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 2', '8GB', 'High-end Android device', 2),
('iPad Air', 'Apple', 'Tablet', 'iPadOS', '16', 'M1', '8GB', 'Lightweight tablet', 3),
('Surface Pro 9', 'Microsoft', 'Tablet', 'Windows', '11', 'Intel i7', '16GB', '2-in-1 business device', NULL),
('Pixel 8', 'Google', 'Phone', 'Android', '14', 'Google Tensor G3', '8GB', 'Google flagship phone', NULL),
('OnePlus 11', 'OnePlus', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 2', '16GB', 'Performance-focused phone', NULL),
('Xiaomi 13', 'Xiaomi', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 2', '12GB', 'Premium Android device', NULL),
('MacBook Air M2', 'Apple', 'Laptop', 'macOS', '13', 'M2', '8GB', 'Lightweight laptop', NULL),
('MacBook Pro 14', 'Apple', 'Laptop', 'macOS', '13', 'M2 Pro', '16GB', 'Professional laptop', NULL),
('Dell XPS 13', 'Dell', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Compact ultrabook', NULL),
('HP Spectre x360', 'HP', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Convertible laptop', NULL),
('Lenovo ThinkPad X1 Carbon', 'Lenovo', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Business ultrabook', NULL),
('Samsung Galaxy Tab S9', 'Samsung', 'Tablet', 'Android', '13', 'Snapdragon 8 Gen 2', '8GB', 'High-end Android tablet', NULL),
('iPad Pro 12.9', 'Apple', 'Tablet', 'iPadOS', '16', 'M2', '16GB', 'Professional tablet', NULL),
('Asus ROG Phone 7', 'Asus', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 2', '16GB', 'Gaming smartphone', NULL),
('Nokia X30', 'Nokia', 'Phone', 'Android', '13', 'Snapdragon 695', '6GB', 'Mid-range eco-friendly phone', NULL),
('Sony Xperia 1 V', 'Sony', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 2', '12GB', 'Camera-focused smartphone', NULL),
('Google Pixel Tablet', 'Google', 'Tablet', 'Android', '13', 'Google Tensor G2', '8GB', 'Smart home tablet', NULL),
('Microsoft Surface Laptop 5', 'Microsoft', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Premium laptop', NULL);

-- See the sample data
SELECT * FROM Users;
SELECT * FROM Devices;
