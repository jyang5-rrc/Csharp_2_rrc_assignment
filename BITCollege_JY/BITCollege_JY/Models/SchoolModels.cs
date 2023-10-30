using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Data.Entity;
using BITCollege_JY.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;

namespace BITCollege_JY.Models
{
    /// <summary>
    /// Student Model. represents the Student table in database
    /// </summary>
    public class Student
    {
        //primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        //foreign key
        [Required]
        [ForeignKey("GradePointState")]
        public int GradePointStateId { get; set; }

        //foreign key
        [Required]
        [ForeignKey("AcademicProgram")]
        public int? AcademicProgramId { get; set; }

        //[Required]
        //[Range(10000000, 99999999, ErrorMessage = "Student Number must be between 10000000 and 99999999")]
        [Display(Name = "Student\nNumber")]
        [DisplayFormat(DataFormatString = "{0}\n")]
        public long StudentNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [DisplayFormat(DataFormatString = "{0}\n", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DisplayFormat(DataFormatString = "{0}\n", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [RegularExpression("^(N[BLSTU]|[AMN]B|[BQ]C|ON|PE|SK|YT)", ErrorMessage = "Valid Province must be entered.")]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Grade Point\nAverage", ShortName = "Grade Point\nAverage")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        [Range(0, 4.5, ErrorMessage = "Grade Point Average must be between 0 and 4.5.")]
        public double? GradePointAverage { get; set; }

        [Required]
        [Display(Name = "Fees")]
        [DisplayFormat(DataFormatString = "${0:F2}", ApplyFormatInEditMode = false)]
        public double OustandingFees { get; set; }

        public string Notes { get; set; }

        //read-only
        [Display(Name = "Name")]
        public string FullName 
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Display(Name = "Address")]
        public string FullAddress
        {
            get
            {
                return String.Format("{0} {1} {2}", Address, City, Province);
            }
        }

        //navigation properties
        public virtual GradePointState GradePointState { get; set; }
        public virtual AcademicProgram AcademicProgram { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }

        //Create a private instance of the data context at the class level
        private BITCollege_JYContext db = new BITCollege_JYContext();

        //method : ChangeState():void connect to the state pattern
        public void ChangeState()
        {
            // Get the initial state based on GradePointStateId
            GradePointState currentState = db.GradePointStates.Find(GradePointStateId);
            
            // Check for state changes iteratively
            bool stateChanged = true;

            while (stateChanged)
            {
                // Call the StateChangeCheck method on the current state
                currentState.StateChangeCheck(this);

                // Retrieve the updated state based on GradePointStateId
                GradePointState UpdatedState = db.GradePointStates.Find(GradePointStateId);

                // Check if the state has changed
                stateChanged = UpdatedState.GradePointStateId != currentState.GradePointStateId;

                // Update the current state
                currentState = UpdatedState;
            }
        }

        //method SetNextStudentNumber():void
        public void SetNextStudentNumber()
        {
            String discriminator = "NextStudent";

            // get the nextNumber from nextNumber method
            long? nextNumber = StoredProcedures.NextNumber(discriminator);

            // set the next course number
            if (nextNumber != null)
            {
                StudentNumber = (long)nextNumber;
            }
            else
            {
                throw new Exception("Unable to generate next student number");
            }

            
        }

    }  


    /// <summary>
    /// Course Model. represents the Course table in database
    /// </summary>
    public abstract class Course
    {
        //Primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        //Foreign Key
        [ForeignKey("AcademicProgram")]
        public int? AcademicProgramId { get; set; }

        //[Required]
        [Display(Name = "Course\nNumber")]
        public string CourseNumber { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Credit\nHours\n")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public double CreditHours { get; set; }

        [Required]
        [Display(Name = "Tuition")]
        [DisplayFormat(DataFormatString = "${0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public double TuitionAmount { get; set; }

        //read only
        [Display(Name = "Course\nType")]
        public string CourseType 
        {
            get
            {
                string fullCourseType = this.GetType().Name;
                int index = fullCourseType.IndexOf("Course", StringComparison.OrdinalIgnoreCase);
                string courseType = index >= 0 ? fullCourseType.Substring(0, index) : fullCourseType;
                return courseType;
            }
        }

        public string Notes { get; set; }

        //navigation properties
        public virtual AcademicProgram AcademicProgram { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }

        //method : SetNextCourseNumber():void
        public abstract void SetNextCourseNumber();
        

    }


    /// <summary>
    /// GradedCourse Model. represents the GradedCourse table in database
    /// </summary>
    public class GradedCourse : Course
    {
        [Required]
        [Display(Name = "Assignments")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double AssignmentWeight { get; set; }

        [Required]
        [Display(Name = "Exams")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double ExamWeight  { get; set; }

        //override method : SetNextCourseNumber():void
        public override void SetNextCourseNumber()
        {
            String discriminator = "NextGradedCourse";
            
            // get the nextNumber from nextNumber method
            long? nextNumber = StoredProcedures.NextNumber(discriminator);

            // set the next course number
            CourseNumber = String.Format("G-{0:D6}", nextNumber);
        }

    }

    /// <summary>
    /// MasteryCourse Model. represents the MasteryCourse table in database
    /// </summary>
    public class MasteryCourse : Course
    {
        [Required]
        [Display(Name = "Maximum\nAttempts")]
        public int MaximumAttempts { get; set; }

        //override method : SetNextCourseNumber():void
        public override void SetNextCourseNumber()
        {
            String discriminator = "NextMasteryCourse";

            // get the nextNumber from nextNumber method
            long? nextNumber = StoredProcedures.NextNumber(discriminator);

            // set the next course number
            CourseNumber = String.Format("M-{0:D5}", nextNumber);
        }
    }

    /// <summary>
    /// AuditCourse Model. represents the AuditCourse table in database
    /// </summary>
    public class AuditCourse : Course 
    {
        //override method : SetNextCourseNumber():void
        public override void SetNextCourseNumber()
        {
            String discriminator = "NextAuditCourse";

            // get the nextNumber from nextNumber method
            long? nextNumber = StoredProcedures.NextNumber(discriminator);

            // set the next course number
            CourseNumber = String.Format("A-{0:D4}", nextNumber);
        }
    }


    /// <summary>
    /// Registration Model. represents the Registration table in database
    /// </summary>
    public class Registration
    {
        //Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        //Foreign Key
        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        //Foreign Key
        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        //[Required]
        [Display(Name = "Registration\nNumber", ShortName = "Registration\nNumber")]
        public long RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime RegistrationDate { get; set; }

        [DisplayFormat(NullDisplayText = "Ungraded", ApplyFormatInEditMode = true)]
        [Range(0,1)]
        public double? Grade { get; set; }


        public string Notes { get; set; }

        //Navigation properties
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }

        //method : SetNextRegistrationNumber():void
        public void SetNextRegistrationNumber()
        {
            String discriminator = "NextRegistration";

            // get the nextNumber from nextNumber method
            long? nextNumber = StoredProcedures.NextNumber(discriminator);

            // set the next Registration Number
            RegistrationNumber = (long)nextNumber;
        }

    }

    /// <summary>
    /// AcademicProgram Model. represents the AcademicProgram table in database
    /// </summary>
    public class AcademicProgram
    {
        //Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AcademicProgramId { get; set; }

        [Required]
        [Display(Name = "Program")]
        public string ProgramAcornym { get; set; }

        [Required]
        [Display(Name = "Program\nName", ShortName = "Program Name")]
        public string Description { get; set; }

        //navigation properties
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }


    /// <summary>
    /// GradePointState Model. represents the GradePointState table in database
    /// </summary>
    public abstract class GradePointState
    {
        //Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GradePointStateId { get; set; }

        [Required]
        [Display(Name = "Lower\nLimit")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public double LowerLimit { get; set; }

        [Required]
        [Display(Name = "Upper\nLimit")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public double UpperLimit { get; set; }

        [Required]
        [Display(Name = "Tuition Rate\nFactor")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public double TuitionRateFactor { get; set; }


        //read-only
        [Display(Name = "State")]
        public string Description
        {
            get
            {
                string fullState = this.GetType().Name;
                int index = fullState.IndexOf("State", StringComparison.OrdinalIgnoreCase);
                string state = index >= 0 ? fullState.Substring(0, index) : fullState;
                return state;
            }
        }

        //navigation properties
        public virtual ICollection<Student> Students { get; set; }

        //method:TuitionRateAdjustment(student: Student): double
        public abstract double TuitionRateAdjustment(Student student);

        //method: StateChangeCheck(student: Student): void
        public abstract void StateChangeCheck(Student student);

        //Create a protected static variable of data context object 
        protected static BITCollege_JYContext db = new BITCollege_JYContext();


    }

    /// <summary>
    /// SuspendedState Model. represents the SuspendedState table in database
    /// </summary>
    public class SuspendedState : GradePointState
    {
        //singleton pattern : single instance
        private static SuspendedState suspendedState;// It is a filed to store the only instance of the class

        //private constructor
        private SuspendedState() {

            //set the inherited auto-implemented properties
            this.LowerLimit = 0.00;
            this.UpperLimit = 1.00;
            this.TuitionRateFactor = 1.1;

        }

        //public method to get the only instance of the class
        public static SuspendedState GetInstance()
        {
            if (suspendedState == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    suspendedState = db.SuspendedState.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (suspendedState == null)
                    {
                        suspendedState = new SuspendedState();
                        db.SuspendedState.Add(suspendedState);
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return suspendedState;
        }

        //Tuition RateAdjustment method
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRateAdjustment = this.TuitionRateFactor;
            
            if (student.GradePointAverage < 0.75) 
            {
                if (student.GradePointAverage < 0.50)
                {
                    tuitionRateAdjustment += 0.05;
                }
                else
                {
                    tuitionRateAdjustment += 0.02;
                }
            }
            
            return tuitionRateAdjustment;
        }

        //StateChangeCheck(student:Student): void method
        public override void StateChangeCheck(Student student)
        {
            // if the student's GPA is less than or equal to 1.0
            if (student.GradePointAverage > UpperLimit)
            {
                //change the student's state to ProbationState
                GradePointState nextState = ProbationState.GetInstance();
                student.GradePointStateId = nextState.GradePointStateId;
                db.SaveChanges();
            }
            
        }


    }

    /// <summary>
    /// probationState Model. represents the probationState table in database
    /// </summary>
    public class ProbationState : GradePointState
    {
        //single instance
        private static ProbationState probationState;

        //private constructor
        private ProbationState() {
            
            //set the inherited auto-implemented properties
            this.LowerLimit = 1.00;
            this.UpperLimit = 2.00;
            this.TuitionRateFactor = 1.075;
        }

        //public method to get the only instance of the class
        public static ProbationState GetInstance()
        {
            if (probationState == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    probationState = db.ProbationState.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (probationState == null)
                    {
                        probationState = new ProbationState();
                        db.ProbationState.Add(probationState);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return probationState;
        }

        //Tuition RateAdjustment method
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRateAdjustment = this.TuitionRateFactor;

            //If the Student has completed 5 or more courses, tuition for each newly registered
            //course is increased by only 3.5 %
            int completedCourseCount = student.Registrations.Count(registration => registration.Grade != null);
            //int completedCourseCount = student.Registrations.Where(registration => registration.Grade != null).Count();

            if (completedCourseCount >= 5)
            {
                tuitionRateAdjustment -= 0.04;
            }
           

            return tuitionRateAdjustment;
        }

        //StateChangeCheck(student:Student): void method
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage > UpperLimit)
            {
                //change the student's state to GoodStandingState
                GradePointState nextState = RegularState.GetInstance();
                student.GradePointStateId = nextState.GradePointStateId;
                db.SaveChanges();
            }
            else if (student.GradePointAverage < LowerLimit)
            {
                //change the student's state to SuspendedState
                GradePointState nextState = SuspendedState.GetInstance();
                student.GradePointStateId = nextState.GradePointStateId;
                db.SaveChanges();
            }
            
        }

    }

    /// <summary>
    /// RegularState Model. represents the RegularState table in database
    /// </summary>
    public class RegularState : GradePointState
    {
        //single instance
        private static RegularState regularState;

        //private constructor
        private RegularState() {

            //set the inherited auto-implemented properties
            this.LowerLimit = 2.00;
            this.UpperLimit = 3.70;
            this.TuitionRateFactor = 1.0;
        }

        //public method to get the only instance of the class
        public static RegularState GetInstance()
        {
            if (regularState == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    regularState = db.RegularState.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (regularState == null)
                    {
                        regularState = new RegularState();
                        db.RegularState.Add(regularState);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return regularState;
        }

        //Tuition RateAdjustment method
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRateAdjustment = this.TuitionRateFactor;

            return tuitionRateAdjustment;
        }

        //StateChangeCheck(student:Student): void method
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage > UpperLimit)
            {
                //change the student's state to Dean'sListState
                GradePointState nextState = HonoursState.GetInstance();
                student.GradePointStateId = nextState.GradePointStateId;
                db.SaveChanges();
            }
            else if (student.GradePointAverage < LowerLimit)
            {
                //change the student's state to ProbationState
                GradePointState nextState = ProbationState.GetInstance();
                student.GradePointStateId = nextState.GradePointStateId;
                db.SaveChanges();
            }
            
        }

    }

    /// <summary>
    /// HonoursState Model. represents the HonoursState table in database
    /// </summary>
    public class HonoursState : GradePointState
    {
        //single instance
        private static HonoursState honoursState;

        //private constructor
        private HonoursState() {

            //set the inherited auto-implemented properties
            this.LowerLimit = 3.70;
            this.UpperLimit = 4.50;
            this.TuitionRateFactor = 0.9;
        }

        //public method to get the only instance of the class
        public static HonoursState GetInstance()
        {
            if (honoursState == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    honoursState = db.HonoursState.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (honoursState == null)
                    {
                        honoursState = new HonoursState();
                        db.HonoursState.Add(honoursState);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return honoursState;
        }

        //Tuition RateAdjustment method
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRateAdjustment = this.TuitionRateFactor;

            int completedCourseCount = student.Registrations.Count(registration => registration.Grade != null);
            
            if (completedCourseCount >= 5)
            {
                tuitionRateAdjustment -= 0.05;    
            }

            if (student.GradePointAverage > 4.25)
            {
                tuitionRateAdjustment -= 0.02;
            }

            return tuitionRateAdjustment;
        }

        //StateChangeCheck(student:Student) : void method
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage < LowerLimit)
            {
                //change the student's state to RegularState
                GradePointState nextState = RegularState.GetInstance();
                student.GradePointStateId = nextState.GradePointStateId;
                db.SaveChanges();
            }
        }
        
        
    }

    /// <summary>
    /// NextUniqueNumber Model. represents the NextUniqueNumber table in database
    /// </summary>
    public abstract class NextUniqueNumber 
    {
        //Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextUniqueNumberId { get; set; }

        //property
        [Required]
        public long NextAvailableNumber { get; set; }

        //Create a protected static variable of data context object 
        protected static BITCollege_JYContext db = new BITCollege_JYContext();
    }

    /// <summary>
    /// NextStudent Model. represents the NextUniqueNumber table in database, subclass of NextUniqueNumber
    /// </summary>
    public class NextStudent : NextUniqueNumber
    {
        //field
        private static NextStudent nextStudent;

        //private constructor
        private NextStudent()
        {
            //set the inherited auto-implemented properties
            this.NextAvailableNumber = 20000000;
        }

        //public method to get the only instance of the class
        public static NextStudent GetInstance()
        {
            if (nextStudent == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    nextStudent = db.NextStudents.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (nextStudent == null)
                    {
                        nextStudent = new NextStudent();
                        db.NextStudents.Add(nextStudent);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return nextStudent;
        }
    }

    /// <summary>
    /// NextRegistration model. represent a table in database, is subclass of NextUniqueNumber
    /// </summary>
    public class NextRegistration : NextUniqueNumber
    {
        //field
        private static NextRegistration nextRegistration;

        //private constructor
        private NextRegistration()
        {
            //set the inherited auto-implemented properties
            this.NextAvailableNumber = 700;
        }

        //public method to get the only instance of the class
        public static NextRegistration GetInstance()
        {
            if (nextRegistration == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    nextRegistration = db.NextRegistrations.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (nextRegistration == null)
                    {
                        nextRegistration = new NextRegistration();
                        db.NextRegistrations.Add(nextRegistration);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return nextRegistration;
        }

    }

    /// <summary>
    /// NextGradedCourse model represent a table in database, is subclass of NextUniqueNumber
    /// </summary>
    public class NextGradedCourse : NextUniqueNumber
    {
        //field
        private static NextGradedCourse nextGradedCourse;

        //private constructor
        private NextGradedCourse()
        {
            //set the inherited auto-implemented properties
            this.NextAvailableNumber = 200000;
        }

        //public method to get the only instance of the class
        public static NextGradedCourse GetInstance()
        {
            if (nextGradedCourse == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    nextGradedCourse = db.NextGradedCourses.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (nextGradedCourse == null)
                    {
                        nextGradedCourse = new NextGradedCourse();
                        db.NextGradedCourses.Add(nextGradedCourse);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return nextGradedCourse;
        }
    }

    /// <summary>
    /// NextAuditCourse model represent a table in database, is subclass of NextUniqueNumber
    /// </summary>
    public class NextAuditCourse : NextUniqueNumber
    {
        //field
        private static NextAuditCourse nextAuditCourse;

        //private constructor
        private NextAuditCourse()
        {
            //set the inherited auto-implemented properties
            this.NextAvailableNumber = 2000;
        }

        //public method to get the only instance of the class
        public static NextAuditCourse GetInstance()
        {
            if (nextAuditCourse == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    nextAuditCourse = db.NextAuditCourses.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (nextAuditCourse == null)
                    {
                        nextAuditCourse = new NextAuditCourse();
                        db.NextAuditCourses.Add(nextAuditCourse);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return nextAuditCourse;
        }


    }


    public class NextMasteryCourse : NextUniqueNumber
    {
        //field
        private static NextMasteryCourse nextMasteryCourse;

        //private constructor
        private NextMasteryCourse()
        {
            //set the inherited auto-implemented properties
            this.NextAvailableNumber = 20000;
        }

        //public method to get the only instance of the class
        public static NextMasteryCourse GetInstance()
        {
            if (nextMasteryCourse == null)
            {
                try
                {
                    // Use the data context to check if a record of this type exists in the database
                    // If so populate the static variable with that record
                    nextMasteryCourse = db.NextMasteryCourses.SingleOrDefault();
                    // If not, create a new record and save it to the database
                    if (nextMasteryCourse == null)
                    {
                        nextMasteryCourse = new NextMasteryCourse();
                        db.NextMasteryCourses.Add(nextMasteryCourse);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return nextMasteryCourse;
        }
    }

    /// <summary>
    /// StoreProcedure model represent a store procedure in database
    /// </summary>
    public static class StoredProcedures
    {
        //method for a stored procedure NextNumber to search the next available number by inputing the discriminator
        public static long? NextNumber(String discriminator)
        {
            try
            {
                //create a variable to store the next number
                long? nextNumber;

                //create the connection to database
                using (SqlConnection connection = new SqlConnection("Data Source=JOCELYN; " +
                        "Initial Catalog=BITCollege_JYContext; Integrated Security=True"))
                {
                    //create the command to execute the stored procedure
                    SqlCommand storedProcedure = new SqlCommand("next_number", connection);
                    storedProcedure.CommandType = CommandType.StoredProcedure;

                    //add the parameter to the command
                    storedProcedure.Parameters.AddWithValue("@Discriminator", discriminator);
                    
                    //get the output
                    SqlParameter outputParameter = new SqlParameter("@NewVal", SqlDbType.BigInt)
                    {
                        Direction = ParameterDirection.Output
                    };

                    storedProcedure.Parameters.Add(outputParameter);
                    
                    //open the connection
                    connection.Open();

                    //execute the command
                    storedProcedure.ExecuteNonQuery();

                    //close the connection
                    connection.Close();

                    //get the next number
                    nextNumber = (long?)outputParameter.Value;
                    
                }

                ////assign the next number to the variable converted to long
                //nextNumber = Convert.ToInt64(nextNumber);

                //return the next number
                return nextNumber;
                
                
            }
            catch (Exception ex)
            {
                return null;
                
            }

        }
    }




}