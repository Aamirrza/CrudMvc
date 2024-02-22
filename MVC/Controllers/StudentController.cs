using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
   // [Route("[controller]")]
    public class StudentController : Controller
    {
               private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepositories;
        private readonly ICourseRepository _courseRepositories;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepositories, ICourseRepository courseRepositories, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _courseRepositories = courseRepositories;
            _studentRepositories = studentRepositories;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(int id)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.Username = username;
            List<tblStudent> stuList = _studentRepositories.FetchAllStudents(id);
            return View(stuList);
        }

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            tblStudent stu = _studentRepositories.FetchStudentDetails(id);
            return View(stu);
        }

        [HttpGet]
        public IActionResult Add()
        {
            string username = HttpContext.Session.GetString("username");
            int userId = HttpContext.Session.GetInt32("userid") ?? 0;

            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
              var userid = new tblStudent
    {
       
       
        userId = userId, 
        
    };

            ViewBag.Username = username;
            ViewBag.Userid = userId;
            ViewBag.Course = _courseRepositories.GetAllCourses();
            return View(userid);
        }

        [HttpPost]
        public IActionResult Add(tblStudent stu, IFormFile studentImage, IFormFile studentDocument)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            _studentRepositories.AddStudent(stu, studentImage, studentDocument);

            ViewBag.Userid = stu.userId;   
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.Course = _courseRepositories.GetAllCourses();
            return RedirectToAction("Index", "Student");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.Course = _courseRepositories.GetAllCourses();
            var stu = _studentRepositories.FetchStudentDetails(id);
            return View(stu);
        }

        [HttpPost]
        public IActionResult Edit(tblStudent stu, IFormFile studentImage, IFormFile studentDocument)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

          
                _studentRepositories.UpdateExistingStudent(stu, studentImage, studentDocument);
               
            

            ViewBag.Course = _courseRepositories.GetAllCourses();
            return RedirectToAction("Index", "Student");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            var stu = _studentRepositories.FetchStudentDetails(id);
            return View(stu);
        }

        [HttpPost]
        public IActionResult DeleteStudent(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            _studentRepositories.DeleteExistingStudent(id);
            return RedirectToAction("Index", "Student");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}