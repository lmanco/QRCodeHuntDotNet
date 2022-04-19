using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.DAL.Models
{
    public class Code
    {
        [Required]
        public Guid Key { get; set; }
        [Required]
        public int Num { get; set; }
    }
}
