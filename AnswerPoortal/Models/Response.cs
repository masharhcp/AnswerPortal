//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnswerPoortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Response
    {
        public int id { get; set; }
        public System.DateTime creationDate { get; set; }
        public int idChannel { get; set; }
        public int idQuestion { get; set; }
        public int idAccount { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Channel Channel { get; set; }
        public virtual QuestionCopy QuestionCopy { get; set; }
    }
}