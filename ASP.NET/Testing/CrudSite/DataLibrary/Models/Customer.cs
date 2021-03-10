using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
#nullable enable
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
#nullable disable
        public virtual ICollection<Order> Orders { get; set; }
    }
}
