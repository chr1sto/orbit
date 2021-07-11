using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class GenericObjectViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required(ErrorMessage = "A Type is Required")]
        [MinLength(5)]
        [MaxLength(120)]
        [DisplayName("Type")]
        public string Type { get; set; }
        public string ValueType { get; set; }
        [Required(ErrorMessage = "A Value is Required")]
        [DisplayName("Value")]
        public string Value { get; set; }
    }
}
