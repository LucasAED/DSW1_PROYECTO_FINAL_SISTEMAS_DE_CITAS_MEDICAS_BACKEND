-- 1. Borrar la base de datos si ya existe (para empezar limpio)
DROP DATABASE IF EXISTS medical_db;

-- 2. Crear la base de datos
CREATE DATABASE medical_db;
USE medical_db;

-- 3. Crear tabla de Doctores
CREATE TABLE Doctors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(50) NOT NULL,
    Cmp VARCHAR(20) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL,
    IsAvailable BOOLEAN DEFAULT 1,
    ShiftStart VARCHAR(5) DEFAULT '08:00',
    ShiftEnd VARCHAR(5) DEFAULT '17:00'
);

-- 4. Crear tabla de Citas
CREATE TABLE Appointments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientName VARCHAR(100) NOT NULL,
    PatientDNI VARCHAR(20) NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status VARCHAR(20) DEFAULT 'Programada',
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id) ON DELETE CASCADE
);

-- 5. INSERTAR DATOS PARA LA DEMO (¡ESTO ES LO IMPORTANTE!)
INSERT INTO Doctors (FullName, Specialty, Cmp, Email, IsAvailable, ShiftStart, ShiftEnd)
VALUES 
('Dr. Gregory House', 'Diagnóstico', 'CMP-00001', 'house@hospital.com', 1, '10:00', '18:00'),
('Dra. Lisa Cuddy', 'Endocrinología', 'CMP-55200', 'cuddy@hospital.com', 1, '06:00', '14:00'),
('Dr. Derek Shepherd', 'Neurocirugía', 'CMP-90900', 'derek@hospital.com', 1, '22:00', '06:00');