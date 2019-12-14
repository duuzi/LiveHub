using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LiveHub.ViewModel
{
    public class ResultViewModel
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
