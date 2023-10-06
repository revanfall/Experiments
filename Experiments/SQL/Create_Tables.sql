CREATE TABLE Options(
	Id INT PRIMARY KEY IDENTITY (1, 1),
    ExpOption VARCHAR (50) NOT NULL,
);


CREATE TABLE Experiments(
	ExperimentId INT PRIMARY KEY IDENTITY (1, 1),
	ExperimentKey VARCHAR(50) NOT NULL,
	AssignedValue INT,
	CreatedDate DateTime
);

ALTER TABLE Experiments
ADD CONSTRAINT FK_Experiments_Options FOREIGN KEY (AssignedValue)
REFERENCES Options(Id);

CREATE TABLE ClientDevices(
	DeviceId INT PRIMARY KEY IDENTITY (1, 1),
	Token VARCHAR(50) NOT NULL,
	ExperimentId INT
);

ALTER TABLE ClientDevices
ADD CONSTRAINT FK_ClientDevices_Experiments FOREIGN KEY (ExperimentId)
REFERENCES Experiments(ExperimentId);
