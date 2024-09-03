using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tc.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public ICollection<string> Roles { get; set; } = [];
    }
}
