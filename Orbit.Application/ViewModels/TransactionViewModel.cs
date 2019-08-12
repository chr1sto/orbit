using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Currency { get; set; }
        public string Target { get; set; }
        public string TargetInfo { get; set; }
        public string Status { get; set; }
    }
}
