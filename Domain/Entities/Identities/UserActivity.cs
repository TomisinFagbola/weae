using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identities
{
    public class UserActivity
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string ObjectClass { get; set; }
        public Guid ObjectId { get; set; }
        public string Details { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
