using AwningsAPI.Database;
using AwningsAPI.Dto.Outlook;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Showroom;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.OutlookService
{
    public class OutlookService : IOutlookService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly GraphServiceClient _graphClient;
        private readonly ILogger<OutlookService> _logger;

        public OutlookService(
            AppDbContext context,
            IConfiguration configuration,
            ILogger<OutlookService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;

            var tenantId = _configuration["AzureAd:TenantId"];
            var clientId = _configuration["AzureAd:ClientId"];
            var clientSecret = _configuration["AzureAd:ClientSecret"];

            var options = new ClientSecretCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            _graphClient = new GraphServiceClient(clientSecretCredential);
        }

        public async Task<string> CreateShowroomInviteAsync(ShowroomInviteDto dto, string currentUser)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var startTime = ParseTimeSlot(dto.EventDate, dto.TimeSlot);
                var endTime = dto.EndDate ?? startTime.AddHours(1);

                var eventSubject = !string.IsNullOrEmpty(dto.EventName)
                    ? dto.EventName 
                    : $"Showroom Visit - {dto.CustomerName}";

                // Create the calendar event
                var newEvent = new Event
                {
                    Subject = eventSubject,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = GenerateInviteEmailBody(dto)
                    },
                    Start = new DateTimeTimeZone
                    {
                        DateTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TimeZone = "Europe/Dublin"
                    },
                    End = new DateTimeTimeZone
                    {
                        DateTime = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TimeZone = "Europe/Dublin"
                    },
                    Location = new Location
                    {
                        DisplayName = _configuration["Showroom:Address"] ?? "Awnings Ireland Showroom"
                    },
                    Attendees = new List<Attendee>
                    {
                        new Attendee
                        {
                            EmailAddress = new Microsoft.Graph.Models.EmailAddress
                            {
                                Address = dto.CustomerEmail,
                                Name = dto.CustomerName
                            },
                            Type = AttendeeType.Required
                        }
                    },
                    IsReminderOn = true,
                    ReminderMinutesBeforeStart = 1440
                };

                var organizerEmail = _configuration["AzureAd:OrganizerEmail"];

                var createdEvent = await _graphClient.Users[organizerEmail]
                    .Events
                    .PostAsync(newEvent);

                // Save to database
                var showroomInvite = new ShowroomInvite
                {
                    WorkflowId = dto.WorkflowId,
                    CustomerId = dto.CustomerId,
                    CustomerName = dto.CustomerName,
                    CustomerEmail = dto.CustomerEmail,
                    EventName = eventSubject,
                    Description = dto.Description ?? string.Empty,
                    EventDate = startTime,
                    EndDate = endTime,
                    TimeSlot = dto.TimeSlot,
                    EmailClient = dto.EmailClient,
                    OutlookEventId = createdEvent.Id,
                    Status = "Scheduled",
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = currentUser
                };

                _context.ShowroomInvites.Add(showroomInvite);
                await _context.SaveChangesAsync();

                // Send email if requested
                if (dto.EmailClient)
                {
                    await SendShowroomInviteEmailAsync(dto);
                }

                await transaction.CommitAsync();

                _logger.LogInformation($"Created showroom invite '{eventSubject}' for {dto.CustomerName} on {startTime}");

                return createdEvent.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating showroom invite");
                throw;
            }
        }

        public async Task<object> GetCalendarEventsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var organizerEmail = _configuration["AzureAd:OrganizerEmail"];

                var events = await _graphClient.Users[organizerEmail]
                    .CalendarView
                    .GetAsync((requestConfiguration) =>
                    {
                        requestConfiguration.QueryParameters.StartDateTime = startDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        requestConfiguration.QueryParameters.EndDateTime = endDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        requestConfiguration.QueryParameters.Top = 50;
                        requestConfiguration.QueryParameters.Orderby = new[] { "start/dateTime" };
                    });

                return events;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving calendar events");
                throw;
            }
        }

        public async Task UpdateCalendarEventAsync(string eventId, OutlookEventDto eventDto)
        {
            try
            {
                var organizerEmail = _configuration["AzureAd:OrganizerEmail"];

                var eventUpdate = new Event
                {
                    Subject = eventDto.Subject,
                    Body = new ItemBody
                    {
                        ContentType = eventDto.Body.ContentType == "HTML" ? BodyType.Html : BodyType.Text,
                        Content = eventDto.Body.Content
                    },
                    Start = new DateTimeTimeZone
                    {
                        DateTime = eventDto.Start.DateTime,
                        TimeZone = eventDto.Start.TimeZone
                    },
                    End = new DateTimeTimeZone
                    {
                        DateTime = eventDto.End.DateTime,
                        TimeZone = eventDto.End.TimeZone
                    }
                };

                if (eventDto.Location != null)
                {
                    eventUpdate.Location = new Location
                    {
                        DisplayName = eventDto.Location.DisplayName
                    };
                }

                if (eventDto.Attendees != null && eventDto.Attendees.Any())
                {
                    eventUpdate.Attendees = eventDto.Attendees.Select(a => new Attendee
                    {
                        EmailAddress = new Microsoft.Graph.Models.EmailAddress
                        {
                            Address = a.EmailAddress.Address,
                            Name = a.EmailAddress.Name
                        },
                        Type = a.Type.ToLower() == "required" ? AttendeeType.Required : AttendeeType.Optional
                    }).ToList();
                }

                await _graphClient.Users[organizerEmail]
                    .Events[eventId]
                    .PatchAsync(eventUpdate);

                _logger.LogInformation($"Updated calendar event {eventId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating calendar event {eventId}");
                throw;
            }
        }

        public async Task DeleteCalendarEventAsync(string eventId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var organizerEmail = _configuration["AzureAd:OrganizerEmail"];

                // Delete from Outlook
                await _graphClient.Users[organizerEmail]
                    .Events[eventId]
                    .DeleteAsync();

                // Update database record
                var showroomInvite = await _context.ShowroomInvites
                    .FirstOrDefaultAsync(si => si.OutlookEventId == eventId);

                if (showroomInvite != null)
                {
                    showroomInvite.Status = "Cancelled";
                    showroomInvite.DateUpdated = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                _logger.LogInformation($"Deleted calendar event {eventId}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"Error deleting calendar event {eventId}");
                throw;
            }
        }

        public async Task SendShowroomInviteEmailAsync(ShowroomInviteDto dto)
        {
            try
            {
                var organizerEmail = _configuration["AzureAd:OrganizerEmail"];

                var message = new Message
                {
                    Subject = $"Showroom Visit Invitation - {dto.Description}",
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = GenerateInviteEmailBody(dto)
                    },
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new Microsoft.Graph.Models.EmailAddress
                            {
                                Address = dto.CustomerEmail,
                                Name = dto.CustomerName
                            }
                        }
                    }
                };

                await _graphClient.Users[organizerEmail]
                    .SendMail
                    .PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                    {
                        Message = message,
                        SaveToSentItems = true
                    });

                _logger.LogInformation($"Sent showroom invite email to {dto.CustomerEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending showroom invite email");
                throw;
            }
        }

        private DateTime ParseTimeSlot(DateTime eventDate, string timeSlot)
        {
            var timeParts = timeSlot.Split(' ');
            var time = timeParts[0].Split(':');
            var hour = int.Parse(time[0]);
            var minute = int.Parse(time[1]);
            var isPM = timeParts[1].ToUpper() == "PM";

            if (isPM && hour != 12)
                hour += 12;
            else if (!isPM && hour == 12)
                hour = 0;

            return new DateTime(
                eventDate.Year,
                eventDate.Month,
                eventDate.Day,
                hour,
                minute,
                0
            );
        }

        private string GenerateInviteEmailBody(ShowroomInviteDto dto)
        {
            var showroomAddress = _configuration["Showroom:Address"] ?? "Awnings Ireland Showroom";
            var showroomPhone = _configuration["Showroom:Phone"] ?? "+353 1 234 5678";
            var showroomEmail = _configuration["Showroom:Email"] ?? "info@awnings.ie";

            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #2c3e50; color: white; padding: 20px; text-align: center; }}
                        .content {{ background-color: #f9f9f9; padding: 30px; border: 1px solid #ddd; }}
                        .details {{ background-color: white; padding: 20px; margin: 20px 0; border-left: 4px solid #3498db; }}
                        .footer {{ text-align: center; padding: 20px; color: #777; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Showroom Visit Invitation</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {dto.CustomerName},</p>
                            
                            <p>Thank you for your interest in our products. We're delighted to invite you to visit our showroom.</p>
                            
                            <div class='details'>
                                <h3>Visit Details:</h3>
                                <p><strong>Date:</strong> {dto.EventDate:dddd, MMMM dd, yyyy}</p>
                                <p><strong>Time:</strong> {dto.TimeSlot}</p>
                                <p><strong>Product:</strong> {dto.Description}</p>
                                <p><strong>Location:</strong> {showroomAddress}</p>
                            </div>
                            
                            <p>During your visit, our expert team will:</p>
                            <ul>
                                <li>Showcase our premium awning products</li>
                                <li>Discuss your specific requirements</li>
                                <li>Provide professional recommendations</li>
                                <li>Answer any questions you may have</li>
                            </ul>
                            
                            <p>If you need to reschedule or have any questions, please don't hesitate to contact us.</p>
                            
                            <p><strong>Contact Information:</strong><br>
                            Phone: {showroomPhone}<br>
                            Email: {showroomEmail}</p>
                            
                            <p>We look forward to seeing you!</p>
                            
                            <p>Best regards,<br>
                            <strong>Awnings Ireland Team</strong></p>
                        </div>
                        <div class='footer'>
                            <p>This is an automated message. Please do not reply directly to this email.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }
    }
}