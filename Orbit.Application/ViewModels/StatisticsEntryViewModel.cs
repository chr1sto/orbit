using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class StatisticsEntryViewModel
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StatGroup { get; set; }
        public string StatName { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
    }
}
