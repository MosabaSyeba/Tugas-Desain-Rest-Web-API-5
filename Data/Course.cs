using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace SimpleRestAPI.Data
{
    public class Course
    {

        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public string? Duration { get; set; }
        public int CategoryId { get; set; }
    }
}
