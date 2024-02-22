using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class CourseRepository: CourseRepository, ICourseRepository
    {
         public List<tblCourse> GetAllCourses()
        {
            var courses = new List<tblCourse>();

            try
            {
                conn.Open();
                string str = " SELECT c_courseid, c_coursename FROM public.t_course; ";
                NpgsqlCommand cmd = new NpgsqlCommand(str, conn);
                cmd.CommandType = CommandType.Text;
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var course = new tblCourse
                    {
                        courseId = Convert.ToInt32(reader["c_courseid"]),
                        courseName = reader["c_coursename"].ToString()
                    };
                    courses.Add(course);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return courses;
        }
        
        
    }
}