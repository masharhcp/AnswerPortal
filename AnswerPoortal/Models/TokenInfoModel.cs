using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnswerPoortal.Models
{
    public class TokenInfoModel
    {
        [Display(Name = "Silver Box")]
        public int silver { get; set; }
        [Display(Name = "Gold Box")]
        public int gold { get; set; }
        [Display(Name = "Platinum Box")]
        public int platinum { get; set; }
        [Display(Name = "Number of tokens")]
        [Key]
        public int tokenNum { get; set; }
    }
}