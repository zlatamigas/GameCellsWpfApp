using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.DB.DTOs
{
    public class DbGeneratedGameResult
    {
        [Key]
        public int Size { get; set; }

        public int? Duration { get; set; }
    }
}
