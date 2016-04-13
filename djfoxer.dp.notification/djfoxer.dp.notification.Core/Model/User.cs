using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core.Model
{
    public class User
    {
        public string Login { get; set; }

        public List<string> NewNotyifications { get; set; }

        public List<string> OldNotyifications { get; set; }


    }
}
