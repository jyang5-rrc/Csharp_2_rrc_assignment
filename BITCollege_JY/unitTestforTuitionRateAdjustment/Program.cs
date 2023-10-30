using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITCollege_JY;
using BITCollege_JY.Data;
using BITCollege_JY.Models;
using Utility;


namespace unitTestforTuitionRateAdjustment
{
    internal class Program
    {
        private static BITCollege_JYContext db = new BITCollege_JYContext();

        static void Main(string[] args)
        {
            int testNumber = 0;
            // Testing SuspendedState TuitionRateAdjustment(Student student)
            Console.WriteLine("\nTesting SuspendedState TuitionRateAdjustment(Student student)");
            Console.WriteLine("Test {0}:SuspendedState_TuitionRateAdjustment_0_44", ++testNumber);
            SuspendedState_TuitionRateAdjustment_0_44();

            Console.WriteLine("Test {0}:SuspendedState_TuitionRateAdjustment_0_60()", ++testNumber);
            SuspendedState_TuitionRateAdjustment_0_60();

            Console.WriteLine("Test {0}:SuspendedState_TuitionRateAdjustment_0_80()", ++testNumber);
            SuspendedState_TuitionRateAdjustment_0_80();

            // Testing ProbationState TuitionRateAdjustment(Student student)
            Console.WriteLine("\nTesting ProbationState TuitionRateAdjustment(Student student)");
            Console.WriteLine("Test {0}:ProbationState_TuitionRateAdjustment_3_1_15", ++testNumber);
            ProbationState_TuitionRateAdjustment_3_1_15();

            Console.WriteLine("Test {0}:ProbationState_TuitionRateAdjustment_7_1_15()", ++testNumber);
            ProbationState_TuitionRateAdjustment_7_1_15();


            // Testing RegularState TuitionRateAdjustment(Student student)
            Console.WriteLine("\nTesting RegularState TuitionRateAdjustment(Student student)");
            Console.WriteLine("Test {0}:RegularState_TuitionRateAdjustment_2_5()", ++testNumber);
            RegularState_TuitionRateAdjustment_2_5();


            Console.ReadLine();
        }

        static void SuspendedState_TuitionRateAdjustment_0_44()
        {
            // Set up test Student
            Student student = db.Students.Find(11);
            student.GradePointAverage = 0.44;
            //student.GradePointStateId = 2;
            db.SaveChanges();

            student.ChangeState();
            //Get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment method
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected:1150");
            Console.WriteLine("Actual Tuition Rate: {0}", tuitionRate);

        }

        static void SuspendedState_TuitionRateAdjustment_0_60()
        {
            // Set up test Student
            // 是对数据库里的数据进行测试，是一个完整的测试，不只是一个方法的测试
            Student student = db.Students.Find(11);// 找到id为6的学生用于测试
            student.GradePointAverage = 0.6;//modify the student's GPA
            db.SaveChanges();

            //Get an instance of the student's state， if use this ,must change student.GradePointStateId = 2;before save changes.
            //GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the student changeState method to make sure the student's GPAId is correct
            student.ChangeState();
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment method
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected:1120");
            Console.WriteLine("Actual Tuition Rate: {0}", tuitionRate);

        }


        static void SuspendedState_TuitionRateAdjustment_0_80()
        {
            // Set up test Student
            Student student = db.Students.Find(11);
            student.GradePointAverage = 0.8;
            //student.GradePointStateId = 2;
            db.SaveChanges();

            student.ChangeState();
            //Get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment method
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected:1100");
            Console.WriteLine("Actual Tuition Rate: {0}", tuitionRate);

        }

        static void ProbationState_TuitionRateAdjustment_3_1_15()
        {
            // Set up test Student
            Student student = db.Students.Find(5);
            student.GradePointAverage = 1.15;
            //student.GradePointStateId = 2;
            db.SaveChanges();

            student.ChangeState();
            //Get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment method
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected:1075");
            Console.WriteLine("Actual Tuition Rate: {0}", tuitionRate);
        }

        static void ProbationState_TuitionRateAdjustment_7_1_15()
        {
            // Set up test Student
            Student student = db.Students.Find(11);
            student.GradePointAverage = 1.15;
            //student.GradePointStateId = 2;
            db.SaveChanges();

            student.ChangeState();
            //Get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment method
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected:1035");
            Console.WriteLine("Actual Tuition Rate: {0}", tuitionRate);
        }

        static void RegularState_TuitionRateAdjustment_2_5()
        {
            // Set up test Student
            Student student = db.Students.Find(5);
            student.GradePointAverage = 2.5;
            //student.GradePointStateId = 2;
            db.SaveChanges();

            student.ChangeState();
            //Get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment method
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected:1000");
            Console.WriteLine("Actual Tuition Rate: {0}", tuitionRate);
        }
    }


}
    

