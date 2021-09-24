using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS
{
    public class ConfirmEmailModelDTO
    {
        public string token { get; set; }
        public string userId { get; set; }
    }
}