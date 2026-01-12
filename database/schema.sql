-- EMS Database Schema
-- Energy Management System

CREATE DATABASE IF NOT EXISTS ems_db;
USE ems_db;

-- Users table
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    FullName VARCHAR(100),
    IsActive BOOLEAN DEFAULT TRUE,
    IsSuperuser BOOLEAN DEFAULT FALSE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_username (Username),
    INDEX idx_email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Power Stations table
CREATE TABLE IF NOT EXISTS PowerStations (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Latitude DOUBLE,
    Longitude DOUBLE,
    Capacity DOUBLE COMMENT 'kWh',
    Status VARCHAR(20) DEFAULT 'active',
    Description TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_name (Name),
    INDEX idx_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Devices table
CREATE TABLE IF NOT EXISTS Devices (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PowerStationId INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    DeviceType VARCHAR(50) NOT NULL,
    Manufacturer VARCHAR(100),
    Model VARCHAR(100),
    SerialNumber VARCHAR(100) UNIQUE,
    Specifications TEXT COMMENT 'JSON string',
    InstallationDate DATETIME,
    WarrantyExpiry DATETIME,
    Status VARCHAR(20) DEFAULT 'offline',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (PowerStationId) REFERENCES PowerStations(Id) ON DELETE CASCADE,
    INDEX idx_power_station (PowerStationId),
    INDEX idx_device_type (DeviceType),
    INDEX idx_serial_number (SerialNumber)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Device Monitoring table
CREATE TABLE IF NOT EXISTS DeviceMonitoring (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DeviceId INT NOT NULL,
    BatteryLevel DOUBLE COMMENT 'percentage',
    Temperature DOUBLE COMMENT 'celsius',
    Voltage DOUBLE COMMENT 'volts',
    Current DOUBLE COMMENT 'amperes',
    PowerOutput DOUBLE COMMENT 'watts',
    EnergyGenerated DOUBLE COMMENT 'kWh',
    EnergyConsumed DOUBLE COMMENT 'kWh',
    Status VARCHAR(20),
    ErrorCode VARCHAR(50),
    RecordedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE CASCADE,
    INDEX idx_device (DeviceId),
    INDEX idx_recorded_at (RecordedAt)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Events table
CREATE TABLE IF NOT EXISTS Events (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DeviceId INT,
    PowerStationId INT,
    EventType VARCHAR(20) NOT NULL,
    Severity VARCHAR(20),
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    Resolved BOOLEAN DEFAULT FALSE,
    ResolvedAt DATETIME,
    ResolvedBy VARCHAR(100),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_device (DeviceId),
    INDEX idx_power_station (PowerStationId),
    INDEX idx_event_type (EventType),
    INDEX idx_resolved (Resolved),
    INDEX idx_created_at (CreatedAt)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Factory Tests table
CREATE TABLE IF NOT EXISTS FactoryTests (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DeviceId INT NOT NULL,
    TestName VARCHAR(100) NOT NULL,
    TestType VARCHAR(50),
    Status VARCHAR(20) DEFAULT 'pending',
    Result TEXT COMMENT 'JSON string with test results',
    TestedBy VARCHAR(100),
    TestedAt DATETIME,
    Notes TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE CASCADE,
    INDEX idx_device (DeviceId),
    INDEX idx_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Construction Processes table
CREATE TABLE IF NOT EXISTS ConstructionProcesses (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PowerStationId INT NOT NULL,
    Phase VARCHAR(100) NOT NULL,
    Status VARCHAR(20) DEFAULT 'planning',
    StartDate DATETIME,
    EndDate DATETIME,
    ExpectedCompletion DATETIME,
    ResponsiblePerson VARCHAR(100),
    Description TEXT,
    Documents TEXT COMMENT 'JSON array of document URLs',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (PowerStationId) REFERENCES PowerStations(Id) ON DELETE CASCADE,
    INDEX idx_power_station (PowerStationId),
    INDEX idx_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Statistics table
CREATE TABLE IF NOT EXISTS Statistics (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PowerStationId INT,
    StatisticType VARCHAR(50) NOT NULL,
    Period VARCHAR(20),
    PeriodStart DATETIME,
    PeriodEnd DATETIME,
    Value DOUBLE,
    Unit VARCHAR(20),
    Data TEXT COMMENT 'JSON string with detailed data',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_power_station (PowerStationId),
    INDEX idx_statistic_type (StatisticType),
    INDEX idx_period (Period),
    INDEX idx_created_at (CreatedAt)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
