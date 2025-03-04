create database  SafeDriveCertDB

-- Create the Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL CHECK (Role IN ('Student', 'Teacher', 'TrafficPolice')),
    Class VARCHAR(50) NULL,          -- Only applicable for students
    School VARCHAR(100) NULL,        -- Only applicable for students
    Phone VARCHAR(15) NULL
);
GO

-- Create the Courses table
CREATE TABLE Courses (
    CourseID INT IDENTITY(1,1) PRIMARY KEY,
    CourseName VARCHAR(100) NOT NULL,
    TeacherID INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    CONSTRAINT FK_Courses_Teacher FOREIGN KEY (TeacherID) REFERENCES Users(UserID)
);
GO

-- Create the Registrations table
CREATE TABLE Registrations (
    RegistrationID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    CourseID INT NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    Comments NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Registrations_User FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Registrations_Course FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);
GO

-- Create the Exams table
CREATE TABLE Exams (
    ExamID INT IDENTITY(1,1) PRIMARY KEY,
    CourseID INT NOT NULL,
    -- "Date" is a reserved word in SQL Server, so we use ExamDate instead
    ExamDate DATE NOT NULL,
    Room VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Exams_Course FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);
GO

-- Create the Results table
CREATE TABLE Results (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    ExamID INT NOT NULL,
    UserID INT NOT NULL,
    Score DECIMAL(5,2) NOT NULL,
    PassStatus BIT NOT NULL,  -- Use BIT for boolean values: 0 = false, 1 = true
    CONSTRAINT FK_Results_Exam FOREIGN KEY (ExamID) REFERENCES Exams(ExamID),
    CONSTRAINT FK_Results_User FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- Create the Certificates table
CREATE TABLE Certificates (
    CertificateID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    IssuedDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    CertificateCode VARCHAR(50) UNIQUE,
    CONSTRAINT FK_Certificates_User FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- Create the Notifications table
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    SentDate DATETIME DEFAULT GETDATE(),
    IsRead BIT DEFAULT 0,  -- 0 = unread, 1 = read
    CONSTRAINT FK_Notifications_User FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO
