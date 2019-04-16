using Orbit.Domain.Game.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class ServiceStatusViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "A name for the service has to be provided.")]
        public string Service { get; set; }
        public DateTime TimeStamp { get; set; }
        public EServiceState State { get; set; }
    }
}
