using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnswerPoortal.Models
{
    using System;
    using System.Collections.Generic;

    public partial class OrderVO
    {
        public int id { get; set; }
        public int tokenNum { get; set; }
        public int tokenPrice { get; set; }
        public string status { get; set; }
        public int idAccount { get; set; }
        public String email { get; set; }

        public virtual Account Account { get; set; }
    }
}