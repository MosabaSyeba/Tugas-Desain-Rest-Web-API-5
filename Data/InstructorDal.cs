using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestAPI.Data
{
    public class InstructorDal : IInstructor
    {
        private List<Instructor> _instructors = new List<Instructor>();

        public InstructorDal()
        {
            _instructors = new List<Instructor>
            {
                new Instructor { InstructorId = 1, InstructorName = "Mosaba", InstructorEmail = "mosabasyebaa@gmail.com", InstructorPhone = "082135817093", InstructorAddress = "Jalan Aru", InstructorCity = "Merauke" },
                new Instructor { InstructorId = 2, InstructorName = "Axel", InstructorEmail = "axel@gmail.com", InstructorPhone = "082256994711", InstructorAddress = "Jalan Moh Toha", InstructorCity = "Serui" },
                new Instructor { InstructorId = 3, InstructorName = "Veronafa", InstructorEmail = "veronafa@gmail.com", InstructorPhone = "08556974412", InstructorAddress = "Jalan Moh Toha", InstructorCity = "Serui" },
                new Instructor { InstructorId = 4, InstructorName = "Rudolf", InstructorEmail = "rudolf@gmail.com", InstructorPhone = "082254166974", InstructorAddress = "Jalan Moh Toha", InstructorCity = "Serui" },
                new Instructor { InstructorId = 5, InstructorName = "Kerenhapuk", InstructorEmail = "kerenhapuk@gmail.com", InstructorPhone = "082255471366", InstructorAddress = "Jalan Moh Toha", InstructorCity = "Serui" },
                new Instructor { InstructorId = 6, InstructorName = "Liontin", InstructorEmail = "liontin@gmail.com", InstructorPhone = "081477885596", InstructorAddress = "Jalan Moh Toha", InstructorCity = "Serui" }
            };
        }

        public Instructor AddInstructor(Instructor instructor)
        {
            _instructors.Add(instructor);
            return instructor;
        }

        public void DeleteInstructor(int instructorId)
        {
            var instructor = GetInstructorById(instructorId);
            if (instructor != null)
            {
                _instructors.Remove(instructor);
            }
        }

        public IEnumerable<Instructor> GetInstructors()
        {
            return _instructors;
        }

        public Instructor GetInstructorById(int instructorId)
        {
            var instructor = _instructors.FirstOrDefault(i => i.InstructorId == instructorId);
            if (instructor == null)
            {
                throw new Exception("Instructor not found");
            }
            return instructor;
        }

        public Instructor UpdateInstructor(Instructor instructor)
        {
            var existingInstructor = GetInstructorById(instructor.InstructorId);
            if (existingInstructor != null)
            {
                existingInstructor.InstructorName = instructor.InstructorName;
                existingInstructor.InstructorEmail = instructor.InstructorEmail;
                existingInstructor.InstructorPhone = instructor.InstructorPhone;
                existingInstructor.InstructorAddress = instructor.InstructorAddress;
                existingInstructor.InstructorCity = instructor.InstructorCity;
            }
            return existingInstructor;
        }
    }
}