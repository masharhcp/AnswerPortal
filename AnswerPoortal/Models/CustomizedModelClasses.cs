using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnswerPoortal.Models
{
    [MetadataType(typeof(QuestionMetaData))]
    public partial class Question
    {
        [NotMapped, Display(Name = "Upload Image")]
        public HttpPostedFileBase ImageToUpload { get; set; }
    }

    public class QuestionMetaData
    {
        [Required,  Display(Name = "Title")]
        public string title { get; set; }

        // [MaxLength(100, ErrorMessage="Image must contain less than 100 bytes.")]
        public byte[] pic_path { get; set; }
    }
}