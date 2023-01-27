using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{

    public class ResourceParameter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; }
        public string Sort { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class StateParameter : ResourceParameter
    {
    }
    public class UserParameters : ResourceParameter 
    { 
    }
    public class CountryParameters : ResourceParameter
    {
    }
}