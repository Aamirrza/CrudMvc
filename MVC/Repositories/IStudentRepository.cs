using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
namespace MVC.Repositories
{
    public interface IStudentRepository
    {
        void AddStudent(tblStudent student, IFormFile profileImg, IFormFile document);

        void UpdateExistingStudent(tblStudent student, IFormFile profileImg, IFormFile document);
        tblStudent FetchStudentDetails(int id);

        public List<tblStudent> FetchAllStudents(int id);

        void DeleteExistingStudent(int id);

    }
}