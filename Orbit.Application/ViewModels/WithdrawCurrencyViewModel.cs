using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class WithdrawCurrencyViewModel
    {
        [Required(ErrorMessage = "You need to select a Character.")]
        public string Character { get; set; }
        [Required(ErrorMessage = "You need to provide a Currency.")]
        public string Currency { get; set; }
        [Range(1,Int32.MaxValue, ErrorMessage = "The Amount has to be greater than zero!")]
        public int Amount { get; set; }
    }
}
