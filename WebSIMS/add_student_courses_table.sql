-- Script to add StudentCourses table
-- Run this script directly in your database

CREATE TABLE [StudentCourses] (
    [StudentCourseID] int NOT NULL IDENTITY(1,1),
    [StudentID] int NOT NULL,
    [CourseID] int NOT NULL,
    [EnrollmentDate] datetime2 NULL,
    [Grade] nvarchar(max) NULL,
    CONSTRAINT [PK_StudentCourses] PRIMARY KEY ([StudentCourseID])
);

-- Add foreign key constraints
ALTER TABLE [StudentCourses] ADD CONSTRAINT [FK_StudentCourses_Students_StudentID] 
    FOREIGN KEY ([StudentID]) REFERENCES [Students] ([StudentID]) ON DELETE CASCADE;

ALTER TABLE [StudentCourses] ADD CONSTRAINT [FK_StudentCourses_Courses_CourseID] 
    FOREIGN KEY ([CourseID]) REFERENCES [Courses] ([CourseID]) ON DELETE CASCADE;

-- Add indexes for better performance
CREATE INDEX [IX_StudentCourses_StudentID] ON [StudentCourses] ([StudentID]);
CREATE INDEX [IX_StudentCourses_CourseID] ON [StudentCourses] ([CourseID]);
