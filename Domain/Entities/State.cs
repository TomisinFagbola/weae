using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class State
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Motto { get; set; }
        
        //Navigation Property
        public Country Country { get; set; }

        //ForeignKey
        public Guid CountryId { get; set; }

    }
}
