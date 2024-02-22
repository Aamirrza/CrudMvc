using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class StudentRepository : CommonRepository, IStudentRepository
    {
        public void AddStudent(tblStudent student, IFormFile profileImg, IFormFile document)
        {
            try
            {
                conn.Open();

                if (profileImg != null && profileImg.Length > 0)
                {
                    student.studProfile = SaveFile(profileImg, "images");
                }

                if (document != null && document.Length > 0)
                {
                    student.studDocument = SaveFile(document, "documents");
                }

                using var command = new NpgsqlCommand("INSERT INTO t_studentdetails(c_name, c_dob, c_address, c_gender, c_language, c_profileimg, c_document, c_mobileno, c_courseid, c_userid) VALUES(@name, @dob, @address, @gender, @language, @profile, @document, @mobile, @course, @userid)", conn);
                command.Parameters.AddWithValue("@name", student.studName);
                command.Parameters.AddWithValue("@dob", student.studDob.Date);
                command.Parameters.AddWithValue("@address", student.studAddress);
                command.Parameters.AddWithValue("@gender", student.studGender);
                command.Parameters.AddWithValue("@language", string.Join(",", student.studLanguage));
                command.Parameters.AddWithValue("@profile", student.studProfile);
                command.Parameters.AddWithValue("@document", student.studDocument);
                command.Parameters.AddWithValue("@mobile", student.studMobile);
                command.Parameters.AddWithValue("@course", student.courseId);
                command.Parameters.AddWithValue("@userid", student.userId);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateExistingStudent(tblStudent student, IFormFile profileImg, IFormFile document)
        {
            try
            {
                conn.Open();

                if (profileImg != null && profileImg.Length > 0)
                {
                    student.studProfile = SaveFile(profileImg, "images");
                }
                else
                {
                    student.studProfile = LoadExistingFilePathFromDatabase(student.studId, "c_profileimg");
                }

                if (document != null && document.Length > 0)
                {
                    student.studDocument = SaveFile(document, "documents");
                }
                else
                {
                    student.studDocument = LoadExistingFilePathFromDatabase(student.studId, "c_document");
                }

                using var command = new NpgsqlCommand("UPDATE t_studentdetails SET c_name=@name, c_dob=@dob, c_address=@address, c_gender=@gender, c_profileimg=@profile, c_document=@document, c_mobileno=@mobile,c_language=@language, c_courseid=@course WHERE c_studentid=@id", conn);
                command.Parameters.AddWithValue("@id", student.studId);
                command.Parameters.AddWithValue("@name", student.studName);
                command.Parameters.AddWithValue("@dob", student.studDob);
                command.Parameters.AddWithValue("@address", student.studAddress);
                command.Parameters.AddWithValue("@gender", student.studGender);
                command.Parameters.AddWithValue("@language", string.Join(",", student.studLanguage));
                command.Parameters.AddWithValue("@profile", student.studProfile);
                command.Parameters.AddWithValue("@document", student.studDocument);
                command.Parameters.AddWithValue("@mobile", student.studMobile);
                command.Parameters.AddWithValue("@course", student.courseId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"{rowsAffected} row(s) updated successfully.");
                }
                else
                {
                    Console.WriteLine("No rows updated. Check if the record with the specified ID exists.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public List<tblStudent> FetchAllStudents(int id)
        {
            try
            {
                conn.Open();
                var students = new List<tblStudent>();

                using var command = new NpgsqlCommand("SELECT kc.c_studentid, kc.c_name, kc.c_mobileno, kc.c_dob, kc.c_address, kc.c_gender, kc.c_language, kc.c_profileimg, kc.c_document, kc.c_userid, kc.c_courseid, s.c_coursename FROM t_studentdetails kc INNER JOIN t_course s ON s.c_courseid = kc.c_courseid WHERE kc.c_userid =@userid;", conn);
                command.Parameters.AddWithValue("@userid", id);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var student = new tblStudent
                    {
                        studId = Convert.ToInt32(reader["c_studentid"]),
                        userId = Convert.ToInt32(reader["c_userid"]),
                        courseId = Convert.ToInt32(reader["c_courseid"]),
                        studName = reader["c_name"].ToString(),
                        studDob = Convert.ToDateTime(reader["c_dob"]),
                        studAddress = reader["c_address"].ToString(),
                        studGender = reader["c_gender"].ToString(),
                        studLanguage = reader["c_language"].ToString().Split(',').ToList(),
                        studProfile = reader["c_profileimg"].ToString(),
                        studDocument = reader["c_document"].ToString(),
                        studMobile = Convert.ToInt64(reader["c_mobileno"]),
                        coursename = reader["c_coursename"].ToString()
                    };
                    students.Add(student);
                }

                return students;
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or perform any necessary actions
                Console.WriteLine("An error occurred while fetching students: " + ex.Message);
                return null; // Or any other appropriate action, like throwing the exception again
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close(); // Ensure connection is always closed, even if an exception occurs
            }
        }


        public tblStudent FetchStudentDetails(int id)
        {
            var student = new tblStudent();
            conn.Open();

            using var command = new NpgsqlCommand("SELECT * FROM t_studentdetails WHERE c_studentid=@id", conn);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                student.studId = Convert.ToInt32(reader["c_studentid"]);
                student.studName = reader["c_name"].ToString();
                student.studDob = Convert.ToDateTime(reader["c_dob"]);
                student.studAddress = reader["c_address"].ToString();
                student.studGender = reader["c_gender"].ToString();
                student.studLanguage = reader["c_language"].ToString().Split(',').ToList();
                student.studProfile = reader["c_profileimg"].ToString();
                student.studDocument = reader["c_document"].ToString();
                student.studMobile = Convert.ToInt64(reader["c_mobileno"]);
                student.courseId = Convert.ToInt32(reader["c_courseid"]);
            }

            conn.Close();
            return student;
        }

        public void DeleteExistingStudent(int id)
        {
            try
            {
                conn.Open();

                using var command = new NpgsqlCommand("DELETE FROM t_studentdetails WHERE c_studentid=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private string SaveFile(IFormFile file, string folderName)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{folderName}", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/{folderName}/" + uniqueFileName;
        }

        private string LoadExistingFilePathFromDatabase(int studentId, string columnName)
        {
            string filePath = null;

            using var command = new NpgsqlCommand($"SELECT {columnName} FROM t_studentdetails WHERE c_studentid=@id", conn);
            command.Parameters.AddWithValue("@id", studentId);
            filePath = command.ExecuteScalar() as string;
            return filePath;
        }

    }
}