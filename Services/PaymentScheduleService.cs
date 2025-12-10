using AwiningsIreland_WebAPI.Models;
using AwningsAPI.Database;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.WorkflowService
{
    public class PaymentScheduleService : IPaymentScheduleService
    {
        private readonly AppDbContext _context;

        public PaymentScheduleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentScheduleDto>> GetAllPaymentSchedulesAsync()
        {
            var schedules = await _context.PaymentSchedules
                .Include(ps => ps.Invoice)
                .OrderBy(ps => ps.InvoiceId)
                .ThenBy(ps => ps.SortOrder)
                .ToListAsync();

            return schedules.Select(MapToDto);
        }

        public async Task<IEnumerable<PaymentScheduleDto>> GetPaymentScheduleByInvoiceIdAsync(int invoiceId)
        {
            var schedules = await _context.PaymentSchedules
                .Include(ps => ps.Invoice)
                .Where(ps => ps.InvoiceId == invoiceId)
                .OrderBy(ps => ps.SortOrder)
                .ToListAsync();

            return schedules.Select(MapToDto);
        }

        public async Task<PaymentScheduleSummaryDto> GetPaymentScheduleSummaryAsync(int invoiceId)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return null;

            var schedules = await _context.PaymentSchedules
                .Include(ps => ps.Invoice)
                .Where(ps => ps.InvoiceId == invoiceId)
                .OrderBy(ps => ps.SortOrder)
                .ToListAsync();

            var totalPaid = schedules.Sum(s => s.AmountPaid);
            var totalDue = schedules.Sum(s => s.AmountDue);

            return new PaymentScheduleSummaryDto
            {
                InvoiceId = invoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                TotalAmount = invoice.TotalAmount,
                TotalPaid = totalPaid,
                TotalDue = totalDue,
                ScheduleItems = schedules.Select(MapToDto).ToList()
            };
        }

        public async Task<PaymentScheduleDto> GetPaymentScheduleItemByIdAsync(int id)
        {
            var schedule = await _context.PaymentSchedules
                .Include(ps => ps.Invoice)
                .FirstOrDefaultAsync(ps => ps.Id == id);

            return schedule != null ? MapToDto(schedule) : null;
        }

        public async Task<IEnumerable<PaymentScheduleDto>> CreatePaymentScheduleAsync(
            CreatePaymentScheduleDto createDto, string currentUser)
        {
            // Check if invoice exists
            var invoice = await _context.Invoices.FindAsync(createDto.InvoiceId);
            if (invoice == null)
                throw new ArgumentException($"Invoice with ID {createDto.InvoiceId} not found");

            // Check if payment schedule already exists for this invoice
            var existingSchedule = await _context.PaymentSchedules
                .Where(ps => ps.InvoiceId == createDto.InvoiceId)
                .ToListAsync();

            if (existingSchedule.Any())
            {
                _context.PaymentSchedules.RemoveRange(existingSchedule);
                await _context.SaveChangesAsync();
            }

            // Create new payment schedule items
            var schedules = new List<PaymentSchedule>();
            var sortOrder = 1;

            foreach (var itemDto in createDto.ScheduleItems)
            {
                var schedule = new PaymentSchedule
                {
                    InvoiceId = createDto.InvoiceId,
                    Description = itemDto.Description,
                    Percentage = itemDto.Percentage,
                    Amount = itemDto.Amount,
                    DueDate = itemDto.DueDate,
                    Status = "Pending",
                    AmountPaid = 0,
                    AmountDue = itemDto.Amount,
                    Reference = itemDto.Reference,
                    SortOrder = sortOrder++,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser,
                    UpdatedBy = currentUser
                };

                schedules.Add(schedule);
            }

            await _context.PaymentSchedules.AddRangeAsync(schedules);
            await _context.SaveChangesAsync();

            return await GetPaymentScheduleByInvoiceIdAsync(createDto.InvoiceId);
        }

        public async Task<PaymentScheduleDto> UpdatePaymentScheduleItemAsync(
            int id, UpdatePaymentScheduleItemDto updateDto, string currentUser)
        {
            var schedule = await _context.PaymentSchedules.FindAsync(id);
            if (schedule == null)
                return null;

            if (!string.IsNullOrEmpty(updateDto.Description))
                schedule.Description = updateDto.Description;

            if (updateDto.Percentage.HasValue)
                schedule.Percentage = updateDto.Percentage.Value;

            if (updateDto.Amount.HasValue)
            {
                schedule.Amount = updateDto.Amount.Value;
                schedule.AmountDue = updateDto.Amount.Value - schedule.AmountPaid;
            }

            if (updateDto.DueDate.HasValue)
                schedule.DueDate = updateDto.DueDate.Value;

            if (!string.IsNullOrEmpty(updateDto.Status))
                schedule.Status = updateDto.Status;

            if (!string.IsNullOrEmpty(updateDto.Reference))
                schedule.Reference = updateDto.Reference;

            schedule.UpdatedAt = DateTime.UtcNow;
            schedule.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return await GetPaymentScheduleItemByIdAsync(id);
        }

        public async Task<PaymentScheduleDto> RecordPaymentAsync(
            int id, RecordSchedulePaymentDto paymentDto, string currentUser)
        {
            var schedule = await _context.PaymentSchedules
                .Include(ps => ps.Invoice)
                .FirstOrDefaultAsync(ps => ps.Id == id);

            if (schedule == null)
                return null;

            // Validate payment amount
            if (paymentDto.Amount > schedule.AmountDue)
                throw new ArgumentException("Payment amount cannot exceed amount due");

            // Update schedule item
            schedule.AmountPaid += paymentDto.Amount;
            schedule.AmountDue -= paymentDto.Amount;
            schedule.PaymentDate = paymentDto.PaymentDate;

            // Update status
            if (schedule.AmountDue == 0)
                schedule.Status = "Paid";
            else if (schedule.AmountPaid > 0)
                schedule.Status = "Partially Paid";

            schedule.UpdatedAt = DateTime.UtcNow;
            schedule.UpdatedBy = currentUser;

            // Also record payment in InvoicePayments table
            var invoicePayment = new InvoicePayment
            {
                InvoiceId = schedule.InvoiceId,
                PaymentDate = paymentDto.PaymentDate,
                Amount = paymentDto.Amount,
                PaymentMethod = "Payment Schedule",
                TransactionReference = paymentDto.Reference,
                Notes = paymentDto.Notes ?? $"Payment for: {schedule.Description}",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            _context.InvoicePayments.Add(invoicePayment);

            // Update invoice status
            var allSchedules = await _context.PaymentSchedules
                .Where(ps => ps.InvoiceId == schedule.InvoiceId)
                .ToListAsync();

            var totalPaid = allSchedules.Sum(s => s.AmountPaid);
            var invoice = schedule.Invoice;

            if (totalPaid >= invoice.TotalAmount)
            {
                invoice.Status = "Paid";
            }
            else if (totalPaid > 0)
            {
                invoice.Status = "Partially Paid";
            }

            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return await GetPaymentScheduleItemByIdAsync(id);
        }

        public async Task<bool> DeletePaymentScheduleItemAsync(int id)
        {
            var schedule = await _context.PaymentSchedules.FindAsync(id);
            if (schedule == null)
                return false;

            _context.PaymentSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePaymentScheduleByInvoiceIdAsync(int invoiceId)
        {
            var schedules = await _context.PaymentSchedules
                .Where(ps => ps.InvoiceId == invoiceId)
                .ToListAsync();

            if (!schedules.Any())
                return false;

            _context.PaymentSchedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExportToXeroAsync(int invoiceId)
        {
            // TODO: Implement Xero integration
            // This would require Xero API credentials and SDK
            await Task.CompletedTask;
            throw new NotImplementedException("Xero export not yet implemented");
        }

        private PaymentScheduleDto MapToDto(PaymentSchedule schedule)
        {
            return new PaymentScheduleDto
            {
                Id = schedule.Id,
                InvoiceId = schedule.InvoiceId,
                InvoiceNumber = schedule.Invoice?.InvoiceNumber ?? string.Empty,
                Description = schedule.Description,
                Percentage = schedule.Percentage,
                Amount = schedule.Amount,
                DueDate = schedule.DueDate,
                Status = schedule.Status,
                AmountPaid = schedule.AmountPaid,
                AmountDue = schedule.AmountDue,
                Reference = schedule.Reference,
                PaymentDate = schedule.PaymentDate,
                CreatedAt = schedule.CreatedAt,
                CreatedBy = schedule.CreatedBy
            };
        }
    }
}