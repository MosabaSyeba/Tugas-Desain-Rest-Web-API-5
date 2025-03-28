using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRestAPI.Models;

namespace SimpleRestAPI.Data
{
    public interface ICours
    {
        IEnumerable<ViewCourseWithCategory> GetCours();
        ViewCourseWithCategory GetCoursById(int coursId);
        Course AddCours(Course course);
        Course UpdateCours(Course course);
        void DeleteCours(int categoryId);

    }
}