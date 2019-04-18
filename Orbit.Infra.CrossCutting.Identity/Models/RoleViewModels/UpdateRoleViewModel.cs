using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.CrossCutting.Identity.Models.RoleViewModels
{
    public class UpdateRolesViewModel
    {
        public Guid UserId { get; set; }
        public string[] Roles { get; set; }
    }
}
