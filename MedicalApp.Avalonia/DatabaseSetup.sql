-- Medical Appointment Booking System Database Setup
-- Run this script in SQL Server Management Studio or Azure Data Studio

-- Create database if it doesn't exist
IF DB_ID('MedicalDB') IS NULL
BEGIN
    CREATE DATABASE MedicalDB;
    PRINT 'Database MedicalDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database MedicalDB already exists.';
END
GO

USE MedicalDB;
GO

-- Drop existing tables if they exist (for clean setup)
IF OBJECT_ID('dbo.Appointments', 'U') IS NOT NULL
    DROP TABLE dbo.Appointments;
IF OBJECT_ID('dbo.Doctors', 'U') IS NOT NULL
    DROP TABLE dbo.Doctors;
IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL
    DROP TABLE dbo.Patients;
GO

-- Create Doctors table
CREATE TABLE dbo.Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT NOT NULL DEFAULT 1
);
GO

-- Create Patients table
CREATE TABLE dbo.Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(200) NOT NULL
);
GO

-- Create Appointments table
CREATE TABLE dbo.Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(400) NULL,
    CONSTRAINT FK_Appointments_Doctors FOREIGN KEY (DoctorID) REFERENCES dbo.Doctors(DoctorID),
    CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID)
);
GO

-- Insert sample doctors
INSERT INTO dbo.Doctors (FullName, Specialty, Availability) VALUES
('Dr. Alice Mensah', 'Cardiology', 1),
('Dr. Bernard Owusu', 'Dermatology', 1),
('Dr. Clara Boateng', 'Pediatrics', 0),
('Dr. David Addo', 'Orthopedics', 1),
('Dr. Elizabeth Kufuor', 'Neurology', 1),
('Dr. Francis Ampah', 'General Medicine', 1);
GO

-- Insert sample patients
INSERT INTO dbo.Patients (FullName, Email) VALUES
('John Doe', 'john.doe@email.com'),
('Mary Smith', 'mary.smith@email.com'),
('Kwame Asante', 'kwame.asante@email.com'),
('Ama Osei', 'ama.osei@email.com'),
('Kofi Mensah', 'kofi.mensah@email.com');
GO

-- Insert sample appointments
INSERT INTO dbo.Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES
(1, 1, DATEADD(day, 1, GETDATE()), 'Regular checkup'),
(2, 2, DATEADD(day, 2, GETDATE()), 'Skin consultation'),
(4, 3, DATEADD(day, 3, GETDATE()), 'Follow-up appointment'),
(5, 4, DATEADD(day, 4, GETDATE()), 'Initial consultation');
GO

-- Verify the setup
SELECT 'Doctors' as TableName, COUNT(*) as RecordCount FROM dbo.Doctors
UNION ALL
SELECT 'Patients', COUNT(*) FROM dbo.Patients
UNION ALL
SELECT 'Appointments', COUNT(*) FROM dbo.Appointments;
GO

PRINT 'Database setup completed successfully!';
PRINT 'You can now run the Medical Appointment Booking System.';
GO 