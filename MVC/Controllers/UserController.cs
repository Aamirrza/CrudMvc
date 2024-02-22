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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepositories;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(ILogger<UserController> logger,IUserRepository userRepositories,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userRepositories=userRepositories;
            _httpContextAccessor=httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register(){
            return View();
        }
        [HttpPost]

        public IActionResult Register(tblUser user){
            if(ModelState.IsValid){
            _userRepositories.UserRegister(user);
            return RedirectToAction("Login","User");
            }
            return View("Register");
        }

       

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

        
        [HttpPost]
        public IActionResult Login(tblUser c_user)
        {
              var session=_httpContextAccessor.HttpContext.Session;
          
           
               

                if (_userRepositories.Login(c_user))
                {
                      int userId = session.GetInt32("userid") ?? 0;
                    return RedirectToAction("Index", "Student",new { id =userId });
                }
               
           
            
            return View("Login", c_user);
        }


        public IActionResult EmailExists(string email){
            bool emailexists=_userRepositories.EmailExists(email);

            return Json(emailexists);
        }
         public IActionResult ValidPassword(string password){
            bool validpass=_userRepositories.ValidPassword(password);

            return Json(validpass);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}