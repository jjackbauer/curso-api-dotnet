using curso.api.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Business.Entities
{
    public class User
    {
        public int Code { get; set; }
        public string  Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
