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

        [Required]
        [Range(10000000, 99999999, ErrorMessage = "Student Number must be between 10000000 and 99999999")]
        [Display(Name = "Student\nNumber")]
        [DisplayFormat(DataFormatString = "{0}\n", ApplyFormatInEditMode = true, NullDisplayText = "")]
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
        [DisplayFormat(DataFormatString = "${0:F2}", ApplyFormatInEditMode = true, NullDisplayText = "")]
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

        [Required]
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

    }

    /// <summary>
    /// MasteryCourse Model. represents the MasteryCourse table in database
    /// </summary>
    public class MasteryCourse : Course
    {
        [Required]
        [Display(Name = "Maximum\nAttempts")]
        public int MaximumAttempts { get; set; }

    }

    /// <summary>
    /// AuditCourse Model. represents the AuditCourse table in database
    /// </summary>
    public class AuditCourse : Course { }


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

        [Required]
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



    }

    /// <summary>
    /// SuspendedState Model. represents the SuspendedState table in database
    /// </summary>
    public class SuspendedState : GradePointState
    {
        private static SuspendedState suspendedState;// It is a filed to store the only instance of the class
    }

    /// <summary>
    /// probationState Model. represents the probationState table in database
    /// </summary>
    public class ProbationState : GradePointState
    {
        private static ProbationState probationState;
    }

    /// <summary>
    /// RegularState Model. represents the RegularState table in database
    /// </summary>
    public class RegularState : GradePointState
    {
        private static RegularState regularState;
    }

    /// <summary>
    /// HonoursState Model. represents the HonoursState table in database
    /// </summary>
    public class HonoursState : GradePointState
    {
        private static HonoursState honoursState;
    }


    
    
}