using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestAPI.Data
{
    public interface IInstructor
    {
        IEnumerable<Instructor> GetInstructors();
        Instructor GetInstructorById(int instructorId);
        Instructor AddInstructor(Instructor instructor);
        Instructor UpdateInstructor(Instructor instructor);
        void DeleteInstructor(int instructorId);
    }
}