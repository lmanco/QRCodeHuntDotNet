using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.DAL.Models
{
    public class UserGame
    {
        private const char CodesFoundDelimiter = ';';

        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string GameName { get; set; }
        [Required]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CodesFoundRaw { get; set; }
        [NotMapped]
        public bool[] CodesFound {
            get { return Array.ConvertAll(CodesFoundRaw.Split(CodesFoundDelimiter), bool.Parse); }
            set { CodesFoundRaw = string.Join(CodesFoundDelimiter, value); }
        }
    }

    public class UserGameResponseDTO
    {
        public string GameName { get; set; }
        public bool[] CodesFound { get; set; }
    }
}
