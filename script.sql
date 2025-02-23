CREATE DATABASE UsersDb;
GO

USE UsersDb;
GO

-- Create the Users table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(200) NOT NULL,
    Role INT NOT NULL
);
GO

INSERT INTO Users (Name, Email, PasswordHash, Role)
VALUES 
    ('John Lorem', 'john@example.com', 'qwerty123', 1),
    ('Jane Smithane', 'jane@example.com', 'qwerty124', 2);
GO