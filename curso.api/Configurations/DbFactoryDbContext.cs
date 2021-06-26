using curso.api.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Configurations
{
    public class DbFactoryDbContext : IDesignTimeDbContextFactory<CourseDbContext>
    {
        public CourseDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<CourseDbContext>();
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Curso;Integrated Security=True");
            CourseDbContext context = new CourseDbContext(options.Options);

            return context;
        }
    }
}
