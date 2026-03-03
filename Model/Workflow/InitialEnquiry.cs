using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Workflow
{
    public class InitialEnquiry
    {
        [Key]
        public int EnquiryId { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }
        public string? Images { get; set; }

        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }

        //Navigation property to link to Workflow
        public int WorkflowId { get; set; }

        /// <summary>
        /// The IncomingEmail.Id that was processed by the email processor and
        /// automatically generated this enquiry record. NULL for manually entered enquiries.
        /// </summary>
        public int? IncomingEmailId { get; set; }

        /// <summary>
        /// The EmailTask.TaskId that was created when the email was categorised as
        /// initial_enquiry. NULL for manually entered enquiries.
        /// Allows navigating from the enquiry back to the task inbox.
        /// </summary>
        public int? TaskId { get; set; }
    }
}