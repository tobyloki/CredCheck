using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderPlaced { get; set; }
        public DateTime? OrderFulfilled { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
