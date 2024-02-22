using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class tblStudent
    {
         [Display(Name ="Student Id")]
        public int studId { get; set; }

         [Display(Name ="Course Id")]
        public int courseId { get; set; }

        [Display(Name ="User Id")]
        public int userId { get; set; }


        [Display(Name ="Student Name")]
        [Required(ErrorMessage ="Student name can't be blank")]
        public string studName { get; set; }

        [Display(Name ="Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime studDob { get; set; }

        [Display(Name ="Student Gender")]
        [Required(ErrorMessage ="Select gender")]
        public string studGender { get; set; }

        [Display(Name ="Student Address")]
        [MaxLength(255, ErrorMessage ="Address cannot exceed 255 characters")]
        public string studAddress { get; set; }

        [Display(Name ="Student Language")]
        [MaxLength(50, ErrorMessage ="Language cannot exceed 50 characters")]
        public List<string> studLanguage { get; set; }

        

        [Display(Name ="Student Profile")]
        [MaxLength(255, ErrorMessage ="Profile URL/path cannot exceed 255 characters")]
        public string studProfile { get; set; }

        [Display(Name ="Student Document")]
        [MaxLength(255, ErrorMessage ="Document URL/path cannot exceed 255 characters")]
        public string studDocument { get; set; }

        [Display(Name ="Student Mobile")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage ="Invalid mobile number")]
        public long studMobile { get; set;}

        public string coursename{get;set;}
    }
}