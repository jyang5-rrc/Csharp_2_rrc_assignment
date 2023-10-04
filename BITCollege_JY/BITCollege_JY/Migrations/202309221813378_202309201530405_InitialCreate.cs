namespace BITCollege_JY.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202309201530405_InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Registrations", "CourseId", "dbo.Courses");
            DropPrimaryKey("dbo.Courses");
            //AddColumn("dbo.Courses", "CourseId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Courses", "CourseId");
            AddForeignKey("dbo.Registrations", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
            //DropColumn("dbo.Courses", "courseId");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Courses", "courseId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Registrations", "CourseId", "dbo.Courses");
            DropPrimaryKey("dbo.Courses");
            //DropColumn("dbo.Courses", "CourseId");
            AddPrimaryKey("dbo.Courses", "CourseId");
            AddForeignKey("dbo.Registrations", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
