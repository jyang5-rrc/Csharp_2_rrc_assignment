using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BITCollege_JY.Data
{
    public class BITCollege_JYContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BITCollege_JYContext() : base("name=BITCollege_JYContext")
        {
        }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.AcademicProgram> AcademicPrograms { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.GradedCourse> GradedCourse { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.AuditCourse> AuditCourse { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.MasteryCourse> MasteryCourse { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.GradePointState> GradePointStates { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.SuspendedState> SuspendedState { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.ProbationState> ProbationState { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.RegularState> RegularState { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.HonoursState> HonoursState { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.Registration> Registrations { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.NextUniqueNumber> NextUniqueNumbers { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.NextStudent> NextStudents { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.NextAuditCourse> NextAuditCourses { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.NextGradedCourse> NextGradedCourses { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.NextMasteryCourse> NextMasteryCourses { get; set; }

        public System.Data.Entity.DbSet<BITCollege_JY.Models.NextRegistration> NextRegistrations { get; set; }
    }
}
