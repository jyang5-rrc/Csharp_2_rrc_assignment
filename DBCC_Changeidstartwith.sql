-- [table GradePointStates]
BEGIN TRANSACTION;
SELECT *
INTO TempGradePointStates 
FROM GradePointStates
WHERE 1 = 0;  -- This ensures no data is copied, only the structure

SET IDENTITY_INSERT TempGradePointStates ON;

INSERT INTO TempGradePointStates (GradePointStateId, LowerLimit, UpperLimit, TuitionRateFactor, Discriminator)
SELECT GradePointStateId, LowerLimit, UpperLimit, TuitionRateFactor, Discriminator FROM GradePointStates;

SET IDENTITY_INSERT TempGradePointStates OFF;

DELETE FROM GradePointStates;


DBCC CHECKIDENT ('GradePointStates', RESEED, 0);


-- Assuming GradePointStates has an identity column,
-- and other columns which we will call Col1, Col2, ..., ColN.
INSERT INTO GradePointStates (LowerLimit, UpperLimit, TuitionRateFactor, Discriminator)
SELECT LowerLimit, UpperLimit, TuitionRateFactor, Discriminator
FROM TempGradePointStates;


DROP TABLE TempGradePointStates;




COMMIT TRANSACTION;
GO

-- [table Students]
BEGIN TRANSACTION;
DBCC CHECKIDENT (Students, RESEED, 100)

INSERT INTO Students (GradePointStateId, AcademicProgramId
      ,StudentNumber
      ,FirstName
      ,LastName
      ,Address
      ,City
      ,Province
      ,DateCreated
	  ,GradePointAverage
      ,OustandingFees
      ,Notes
       ) VALUES(
				1, 
				2, 
				10000000, 
				'Zelda', 
				'Zimmerman', 
				'160 Princess Street', 
				'Winnipeg', 
				'MB', 
				'2023-10-29',
				Null,
				0.00,
				'New Student'
				);

INSERT INTO Students (GradePointStateId, AcademicProgramId
      ,StudentNumber
      ,FirstName
      ,LastName
      ,Address
      ,City
      ,Province
      ,DateCreated
	  ,GradePointAverage
      ,OustandingFees
      ,Notes
       ) VALUES(
				1, 
				3, 
				10000001, 
				'Yancy', 
				'Yale', 
				'555 Main Street', 
				'Winnipeg', 
				'MB', 
				'2023-10-29',
				Null,
				500.00,
				'New Student'
				);

COMMIT TRANSACTION;
GO



-- [table Courses]

BEGIN TRANSACTION;

SELECT *
INTO TempCourses
FROM Courses
WHERE 1 = 0;  -- This ensures no data is copied, only the structure

SET IDENTITY_INSERT TempCourses ON;

INSERT INTO TempCourses (CourseId, AcademicProgramId, CourseNumber, Title, CreditHours, TuitionAmount, Notes, AssignmentWeight, ExamWeight, MaximumAttempts, Discriminator)
SELECT CourseId, AcademicProgramId, CourseNumber, Title, CreditHours, TuitionAmount, Notes, AssignmentWeight, ExamWeight, MaximumAttempts, Discriminator FROM Courses;

SET IDENTITY_INSERT TempCourses OFF;

DELETE FROM Courses;


DBCC CHECKIDENT ('Courses', RESEED, 100);


-- Assuming GradePointStates has an identity column,
-- and other columns which we will call Col1, Col2, ..., ColN.
INSERT INTO Courses (AcademicProgramId, CourseNumber, Title, CreditHours, TuitionAmount, Notes, AssignmentWeight, ExamWeight, MaximumAttempts, Discriminator)
SELECT AcademicProgramId, CourseNumber, Title, CreditHours, TuitionAmount, Notes, AssignmentWeight, ExamWeight, MaximumAttempts, Discriminator
FROM TempCourses;


DROP TABLE TempCourses;




COMMIT TRANSACTION;
GO


-- [table Registrations]

BEGIN TRANSACTION;

DBCC CHECKIDENT (Registrations, RESEED, 100)

INSERT INTO Registrations (StudentId
      ,CourseId
      ,RegistrationNumber
      ,RegistrationDate
      ,Grade
      ,Notes
       ) VALUES(
				101, 
				103, 
				100,
				'2023-10-29',
				Null,
				'Online Registration'
				);

INSERT INTO Registrations (StudentId
      ,CourseId
      ,RegistrationNumber
      ,RegistrationDate
      ,Grade
      ,Notes
       ) VALUES(
				102, 
				102, 
				101,
				'2023-10-29',
				Null,
				'Online Registration'
				);

COMMIT TRANSACTION;
GO