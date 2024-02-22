using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MVC.Models;
using Npgsql;

namespace MVC.Repositories
{
    public class UserRepository : CommonRepository, IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void UserRegister(tblUser user)
        {

            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand("INSERT into t_userregistration(c_uname, c_email, c_password) VALUES(@username, @email, @password)", conn);
                cmd.Parameters.AddWithValue("@username", user.userName);
                cmd.Parameters.AddWithValue("@email", user.userEmail);
                cmd.Parameters.AddWithValue("@password", user.userPassword);

                cmd.ExecuteNonQuery();
                conn.Close();

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
        public bool Login(tblUser user)
        {
            bool isUserAuthenticated = false;

            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand("select * from t_userregistration where c_email=@email and c_password=@password;", conn);
                cmd.Parameters.AddWithValue("@email", user.userEmail);
                cmd.Parameters.AddWithValue("@password", user.userPassword);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        isUserAuthenticated = true;
                        var session = _httpContextAccessor.HttpContext.Session;
                        session.SetString("username", dr["c_uname"].ToString());
                        session.SetInt32("userid", (int)dr["c_regid"]);
                    }
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
            return isUserAuthenticated;
        }

        public bool EmailExists(string email)
        {
            bool isExists = false;

            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand("select * from t_userregistration where c_email=@email", conn);
                cmd.Parameters.AddWithValue("@email", email);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        isExists = true;

                    }
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
            return isExists;

        }

        public bool ValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            return passwordRegex.IsMatch(password);

        }
    }
}