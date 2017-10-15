using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnswerPoortal.Models
{
    public class OrderStatus
    {
        [Key]
        [Display(Name = "Order id")]
        public int ordId { get; set; }
        [Display(Name = "Order status")]
        public String status { get; set; }
        [Display(Name = "Email")]
        public String email { get; set; }

    }
}