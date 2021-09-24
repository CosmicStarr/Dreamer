using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ConfirmEmailModel
    {
        public string token { get; set; }
        public string userId { get; set; }
    }
}