namespace AwningsAPI.Dto.Outlook
{
    public class ShowroomInviteDto
    {
        public int WorkflowId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimeSlot { get; set; }
        public bool EmailClient { get; set; }
    }

    public class OutlookEventDto
    {
        public string Subject { get; set; }
        public EventBody Body { get; set; }
        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }
        public EventLocation Location { get; set; }
        public List<EventAttendee> Attendees { get; set; }
    }

    public class EventBody
    {
        public string ContentType { get; set; } = "HTML";
        public string Content { get; set; }
    }

    public class EventDateTime
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; } = "Europe/Dublin";
    }

    public class EventLocation
    {
        public string DisplayName { get; set; }
    }

    public class EventAttendee
    {
        public EmailAddress EmailAddress { get; set; }
        public string Type { get; set; } = "required";
    }

    public class EmailAddress
    {
        public string Address { get; set; }
        public string Name { get; set; }
    }
}