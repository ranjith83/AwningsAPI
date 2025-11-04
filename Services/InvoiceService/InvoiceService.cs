using AwiningsIreland_WebAPI.Models;
using AwningsAPI.Database;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AwningsAPI.Services.WorkflowService
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.InvoicePayments)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return invoices.Select(MapToDto);
        }

        public async Task<IEnumerable<InvoiceDto>> GetInvoicesByWorkflowIdAsync(int workflowId)
        {
            var invoices = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.InvoicePayments)
                .Where(i => i.WorkflowId == workflowId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return invoices.Select(MapToDto);
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.InvoicePayments)
                .FirstOrDefaultAsync(i => i.Id == id);

            return invoice != null ? MapToDto(invoice) : null;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto createDto, string currentUser)
        {
            // Generate invoice number
            var invoiceNumber = await GenerateInvoiceNumberAsync();

            // Get customer details (assuming you have a Customer table/service)
            // For now, using placeholder data
            var invoice = new Invoice
            {
                WorkflowId = createDto.WorkflowId,
                InvoiceNumber = invoiceNumber,
                InvoiceDate = createDto.InvoiceDate,
                DueDate = createDto.DueDate,
                CustomerId = createDto.CustomerId,
                Notes = createDto.Notes,
                Terms = createDto.Terms,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            // Add invoice items
            var sortOrder = 1;
            foreach (var itemDto in createDto.InvoiceItems)
            {
                var item = new InvoiceItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    TaxRate = itemDto.TaxRate,
                    DiscountPercentage = itemDto.DiscountPercentage,
                    Unit = itemDto.Unit,
                    SortOrder = sortOrder++
                };

                // Calculate total price
                var subtotal = item.Quantity * item.UnitPrice;
                var discount = subtotal * (item.DiscountPercentage / 100);
                var taxableAmount = subtotal - discount;
                var tax = taxableAmount * (item.TaxRate / 100);
                item.TotalPrice = taxableAmount + tax;

                invoice.InvoiceItems.Add(item);
            }

            // Calculate invoice totals
            CalculateInvoiceTotals(invoice);

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return await GetInvoiceByIdAsync(invoice.Id);
        }

        public async Task<InvoiceDto> UpdateInvoiceAsync(int id, UpdateInvoiceDto updateDto, string currentUser)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return null;

            // Update invoice fields
            if (updateDto.InvoiceDate.HasValue)
                invoice.InvoiceDate = updateDto.InvoiceDate.Value;

            if (updateDto.DueDate.HasValue)
                invoice.DueDate = updateDto.DueDate.Value;

            if (!string.IsNullOrEmpty(updateDto.Status))
                invoice.Status = updateDto.Status;

            if (updateDto.Notes != null)
                invoice.Notes = updateDto.Notes;

            if (updateDto.Terms != null)
                invoice.Terms = updateDto.Terms;

            // Update invoice items if provided
            if (updateDto.InvoiceItems != null && updateDto.InvoiceItems.Any())
            {
                // Remove existing items
                _context.InvoiceItems.RemoveRange(invoice.InvoiceItems);

                // Add updated items
                var sortOrder = 1;
                foreach (var itemDto in updateDto.InvoiceItems)
                {
                    var item = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        Description = itemDto.Description,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        TaxRate = itemDto.TaxRate,
                        DiscountPercentage = itemDto.DiscountPercentage,
                        Unit = itemDto.Unit,
                        SortOrder = sortOrder++
                    };

                    // Calculate total price
                    var subtotal = item.Quantity * item.UnitPrice;
                    var discount = subtotal * (item.DiscountPercentage / 100);
                    var taxableAmount = subtotal - discount;
                    var tax = taxableAmount * (item.TaxRate / 100);
                    item.TotalPrice = taxableAmount + tax;

                    invoice.InvoiceItems.Add(item);
                }

                // Recalculate totals
                CalculateInvoiceTotals(invoice);
            }

            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return await GetInvoiceByIdAsync(id);
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return false;

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<InvoiceDto> UpdateInvoiceStatusAsync(int id, string status, string currentUser)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return null;

            invoice.Status = status;
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return await GetInvoiceByIdAsync(id);
        }

        public async Task<InvoicePaymentDto> AddPaymentAsync(int invoiceId, CreatePaymentDto paymentDto, string currentUser)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoicePayments)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return null;

            var payment = new InvoicePayment
            {
                InvoiceId = invoiceId,
                PaymentDate = paymentDto.PaymentDate,
                Amount = paymentDto.Amount,
                PaymentMethod = paymentDto.PaymentMethod,
                TransactionReference = paymentDto.TransactionReference,
                Notes = paymentDto.Notes,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            _context.InvoicePayments.Add(payment);

            // Update invoice status if fully paid
            var totalPaid = invoice.InvoicePayments.Sum(p => p.Amount) + payment.Amount;
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

            return new InvoicePaymentDto
            {
                Id = payment.Id,
                InvoiceId = payment.InvoiceId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                TransactionReference = payment.TransactionReference,
                Notes = payment.Notes,
                CreatedAt = payment.CreatedAt,
                CreatedBy = payment.CreatedBy
            };
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(int id)
        {
            // Implement PDF generation logic
            // You can use libraries like iTextSharp or QuestPDF
            throw new NotImplementedException("PDF generation not implemented");
        }

        public async Task<bool> SendInvoiceEmailAsync(int id)
        {
            // Implement email sending logic
            throw new NotImplementedException("Email sending not implemented");
        }

        // Helper Methods

        private async Task<string> GenerateInvoiceNumberAsync()
        {
            var year = DateTime.Now.Year;
            var lastInvoice = await _context.Invoices
                .Where(i => i.InvoiceNumber.StartsWith($"INV-{year}"))
                .OrderByDescending(i => i.InvoiceNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastInvoice != null)
            {
                var lastNumber = lastInvoice.InvoiceNumber.Split('-').Last();
                if (int.TryParse(lastNumber, out int num))
                {
                    nextNumber = num + 1;
                }
            }

            return $"INV-{year}-{nextNumber:D4}";
        }

        private void CalculateInvoiceTotals(Invoice invoice)
        {
            invoice.SubTotal = invoice.InvoiceItems.Sum(i => i.Quantity * i.UnitPrice);

            var totalDiscount = invoice.InvoiceItems.Sum(i =>
                (i.Quantity * i.UnitPrice) * (i.DiscountPercentage / 100));
            invoice.DiscountAmount = totalDiscount;

            var taxableAmount = invoice.SubTotal - totalDiscount;
            invoice.TaxAmount = invoice.InvoiceItems.Sum(i =>
            {
                var itemSubtotal = i.Quantity * i.UnitPrice;
                var itemDiscount = itemSubtotal * (i.DiscountPercentage / 100);
                var itemTaxable = itemSubtotal - itemDiscount;
                return itemTaxable * (i.TaxRate / 100);
            });

            invoice.TotalAmount = invoice.InvoiceItems.Sum(i => i.TotalPrice);
        }

        private InvoiceDto MapToDto(Invoice invoice)
        {
            var amountPaid = invoice.InvoicePayments?.Sum(p => p.Amount) ?? 0;
            var amountDue = invoice.TotalAmount - amountPaid;

            return new InvoiceDto
            {
                Id = invoice.Id,
                WorkflowId = invoice.WorkflowId,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                CustomerId = invoice.CustomerId,
                SubTotal = invoice.SubTotal,
                TaxAmount = invoice.TaxAmount,
                DiscountAmount = invoice.DiscountAmount,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status,
                Notes = invoice.Notes,
                Terms = invoice.Terms,
                CreatedAt = invoice.CreatedAt,
                CreatedBy = invoice.CreatedBy,
                InvoiceItems = invoice.InvoiceItems?.Select(i => new InvoiceItemDto
                {
                    Id = i.Id,
                    InvoiceId = i.InvoiceId,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TaxRate = i.TaxRate,
                    DiscountPercentage = i.DiscountPercentage,
                    TotalPrice = i.TotalPrice,
                    Unit = i.Unit,
                    SortOrder = i.SortOrder
                }).ToList() ?? new List<InvoiceItemDto>(),
                InvoicePayments = invoice.InvoicePayments?.Select(p => new InvoicePaymentDto
                {
                    Id = p.Id,
                    InvoiceId = p.InvoiceId,
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    TransactionReference = p.TransactionReference,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy
                }).ToList() ?? new List<InvoicePaymentDto>(),
                AmountPaid = amountPaid,
                AmountDue = amountDue
            };
        }
    }
}