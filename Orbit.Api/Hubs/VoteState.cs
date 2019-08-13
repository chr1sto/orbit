using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Hubs
{
    public class VoteState
    {
        public VoteState(string state, string timeSpan)
        {
            State = state;
            TimeSpan = timeSpan;
        }

        public string State { get; set; }
        public string TimeSpan { get; set; }
    }
}
