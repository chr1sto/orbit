using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class GameAccountViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "You need to provide a valid Alias")]
        public string Alias { get; set; }
        public string Account { get; set; }
    }
}
