using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IST_Submission_Form.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProposalID { get; set; }
        public Proposal Proposal { get; set; }
        public string From { get; set; }
        [Column(TypeName = "text")]
        public string Body { get; set; }
        public string CreatedByID { get; set; }
        public string CreatedByName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime CreatedAt { get; set; }

    }
}