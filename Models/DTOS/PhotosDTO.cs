using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOS
{
    public class PhotosDTO
    {
        public bool MainPic { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
    }
}