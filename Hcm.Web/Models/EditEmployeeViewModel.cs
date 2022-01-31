using Hcm.Api.Client;
using Hcm.Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Web.Models
{
    public class EditEmployeeViewModel
    {
        public EmployeeViewModel EmployeeToBeEdited { get; set; }
        public IFormFile AvatarChange { get; set; }


        public bool SuccessMessageVisible { get; set; }
        public bool ErrorMessageVisible { get; set; }
    }
}
