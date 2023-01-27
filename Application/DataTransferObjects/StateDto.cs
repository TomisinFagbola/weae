using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransferObjects
{
    public record StateDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Motto { get; set; }


    }

    public record StateCreateDto
    {
        public string Name { get; set; }

        public string MMotto { get; set; }
    }

    public record StateUpdateDto : StateCreateDto
    {
    }
}
