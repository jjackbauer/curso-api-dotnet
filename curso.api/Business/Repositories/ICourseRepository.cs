using curso.api.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Business.Repositories
{
	public interface ICourseRepository
	{
		void Add(Course course);
		void Commit();
		IList<Course> GetUserCourses(int userCode);
	}
}
