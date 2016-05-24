using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core.Model
{
   public class SelectedOrder
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SelectedOrder(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
