using curso.api.Business.Entities;
using curso.api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Configurations
{
    public interface IAuthenticationService
    {
        public string GetToken(User user);
    }
}
