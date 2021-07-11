using Orbit.Domain.Game.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class ServiceStatusViewModel
    {
        public ServiceStatusViewModel(Guid id, string service, DateTime timeStamp, int state)
        {
            Id = id;
            Service = service;
            TimeStamp = timeStamp;
            State = state;
        }

        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "A name for the service has to be provided.")]
        public string Service { get; set; }
        public DateTime TimeStamp { get; set; }
        public int State { get; set; }
    }
}
