using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Continent { get; set; }
        public ICollection<State> States { get; set; }
    }
}
