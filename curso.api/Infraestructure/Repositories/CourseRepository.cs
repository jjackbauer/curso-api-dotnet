using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infraestructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseDbContext _context;

        public CourseRepository(CourseDbContext context)
        {
            _context = context;
        }

        public void Add(Course course)
        {
            _context.Course.Add(course);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IList<Course> GetUserCourses(int userCode)
        {
            return _context.Course.Include(i => i.User).Where(w => w.UserCode == userCode).ToList();
        }
    }
}
