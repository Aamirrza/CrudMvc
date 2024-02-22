using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Repositories
{
    public interface ICourseRepository
    {
        List<tblCourse> GetAllCourses();
        
    }
}