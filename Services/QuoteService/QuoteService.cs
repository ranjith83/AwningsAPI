using AwiningsIreland_WebAPI.Models;
using AwningsAPI.Database;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.QuoteService
{
    public class QuoteService: IQuoteService
    {
        private readonly AppDbContext _context;

        public QuoteService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<QuoteDto>> GetAllQuotesAsync()
        {
            var quotes = await _context.Quotes
                .Include(q => q.QuoteItems)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return quotes.Select(MapToDto);
        }
        public async Task<IEnumerable<QuoteDto>> GetQuotesByWorkflowIdAsync(int workflowId)
        {
            var quotes = await _context.Quotes
                .Include(q => q.QuoteItems)
                .Where(i => i.WorkflowId == workflowId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return quotes.Select(MapToDto);
        }

        public async Task<QuoteDto> GetQuoteByIdAsync(int quoteId)
        {
            var quote = await _context.Quotes
                .Include(q => q.QuoteItems)
                .FirstOrDefaultAsync(i => i.QuoteId == quoteId);

            return quote != null ? MapToDto(quote) : null;
        }

        public async Task<QuoteDto> CreateQuoteAsync(CreateQuoteDto createDto, string currentUser)
        {
            var quoteNumber = await GenerateQuoteNumberAsync();

            var quote = new Quote
            {
                WorkflowId = createDto.WorkflowId,
                QuoteNumber = quoteNumber,
                QuoteDate = createDto.QuoteDate,
                FollowUpDate = createDto.FollowUpDate,
                CustomerId = createDto.CustomerId,
                Notes = createDto.Notes,
                Terms = createDto.Terms,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            // Add invoice items
            var sortOrder = 1;
            foreach (var itemDto in createDto.QuoteItems)
            {
                var item = new QuoteItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    TaxRate = itemDto.TaxRate,
                    DiscountPercentage = itemDto.DiscountPercentage,
                    SortOrder = sortOrder++
                };

                // Calculate total price
                var subtotal = item.Quantity * item.UnitPrice;
                var discount = subtotal * (item.DiscountPercentage / 100);
                var taxableAmount = subtotal - discount;
                var tax = taxableAmount * (item.TaxRate / 100);
                item.TotalPrice = taxableAmount + tax;

                quote.QuoteItems.Add(item);
            }

            CalculateQuoteTotals(quote);

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return await GetQuoteByIdAsync(quote.QuoteId);
        }

        public async Task<QuoteDto> UpdateQuoteAsync(int quoteId, UpdateQuoteDto updateDto, string currentUser)
        {
            var quote = await _context.Quotes
                .Include(q => q.QuoteItems)
                .FirstOrDefaultAsync(i => i.QuoteId == quoteId);

            if (quote == null)
                return null;

            // Update quote fields
            if (updateDto.QuoteDate.HasValue)
                quote.QuoteDate = updateDto.QuoteDate.Value;

            if (updateDto.FollowUpDate.HasValue)
                quote.FollowUpDate = updateDto.FollowUpDate.Value;

            if (updateDto.Notes != null)
                quote.Notes = updateDto.Notes;

            if (updateDto.Terms != null)
                quote.Terms = updateDto.Terms;

            // Update invoice items if provided
            if (updateDto.QuoteItems != null && updateDto.QuoteItems.Any())
            {
                // Remove existing items
                _context.QuoteItems.RemoveRange(quote.QuoteItems);

                // Add updated items
                var sortOrder = 1;
                foreach (var itemDto in updateDto.QuoteItems)
                {
                    var item = new QuoteItem
                    {
                        QuoteId = quote.QuoteId,
                        Description = itemDto.Description,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        TaxRate = itemDto.TaxRate,
                        DiscountPercentage = itemDto.DiscountPercentage,
                        SortOrder = sortOrder++
                    };

                    // Calculate total price
                    var subtotal = item.Quantity * item.UnitPrice;
                    var discount = subtotal * (item.DiscountPercentage / 100);
                    var taxableAmount = subtotal - discount;
                    var tax = taxableAmount * (item.TaxRate / 100);
                    item.TotalPrice = taxableAmount + tax;

                    quote.QuoteItems.Add(item);
                }

                // Recalculate totals
                CalculateQuoteTotals(quote);
            }

            quote.UpdatedAt = DateTime.UtcNow;
            quote.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return await GetQuoteByIdAsync(quoteId);
        }

        public async Task<bool> DeleteQuoteAsync(int quoteId)
        {
            var quote = await _context.Quotes.FindAsync(quoteId);
            if (quote == null)
                return false;

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return true;
        }
        private void CalculateQuoteTotals(Quote quote)
        {
            quote.SubTotal = quote.QuoteItems.Sum(i => i.Quantity * i.UnitPrice);

            var totalDiscount = quote.QuoteItems.Sum(i =>
                (i.Quantity * i.UnitPrice) * (i.DiscountPercentage / 100));
            quote.DiscountAmount = totalDiscount;

            var taxableAmount = quote.SubTotal - totalDiscount;
            quote.TaxAmount = quote.QuoteItems.Sum(i =>
            {
                var itemSubtotal = i.Quantity * i.UnitPrice;
                var itemDiscount = itemSubtotal * (i.DiscountPercentage / 100);
                var itemTaxable = itemSubtotal - itemDiscount;
                return itemTaxable * (i.TaxRate / 100);
            });

            quote.TotalAmount = quote.QuoteItems.Sum(i => i.TotalPrice);
        }
        private QuoteDto MapToDto(Quote quote)
        {
            return new QuoteDto
            {
                QuoteId = quote.QuoteId,
                WorkflowId = quote.WorkflowId,
                QuoteNumber = quote.QuoteNumber,
                QuoteDate = quote.QuoteDate,
                FollowUpDate = quote.FollowUpDate,
                CustomerId = quote.CustomerId,
                SubTotal = quote.SubTotal,
                TaxAmount = quote.TaxAmount,
                DiscountAmount = quote.DiscountAmount,
                TotalAmount = quote.TotalAmount,
                CreatedAt = quote.CreatedAt,
                CreatedBy = quote.CreatedBy,
                QuoteItems = quote.QuoteItems?.Select(q => new QuoteItemDto
                {
                    QuoteItemId = q.QuoteItemId,
                    QuoteId = q.QuoteId,
                    Description = q.Description,
                    Quantity = q.Quantity,
                    UnitPrice = q.UnitPrice,
                    TaxRate = q.TaxRate,
                    DiscountPercentage = q.DiscountPercentage,
                    TotalPrice = q.TotalPrice,
                    SortOrder = q.SortOrder
                }).ToList() ?? new List<QuoteItemDto>()
            };
        }

        private async Task<string> GenerateQuoteNumberAsync()
        {
            var year = DateTime.Now.Year;
            var lastQuote = await _context.Quotes
                .Where(i => i.QuoteNumber.StartsWith($"QUOTE-{year}"))
                .OrderByDescending(q => q.QuoteNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastQuote != null)
            {
                var lastNumber = lastQuote.QuoteNumber.Split('-').Last();
                if (int.TryParse(lastNumber, out int num))
                {
                    nextNumber = num + 1;
                }
            }

            return $"QUOTE-{year}-{nextNumber:D4}";
        }
    }
}
