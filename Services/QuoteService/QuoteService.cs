using AwiningsIreland_WebAPI.Models;
using AwningsAPI.Database;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.QuoteService
{
    public class QuoteService : IQuoteService
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
                DiscountType = createDto.DiscountType,
                DiscountValue = createDto.DiscountValue,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            // Add quote items
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

            if (updateDto.DiscountType != null)
                quote.DiscountType = updateDto.DiscountType;

            if (updateDto.DiscountValue.HasValue)
                quote.DiscountValue = updateDto.DiscountValue.Value;

            // Update quote items if provided
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
            // Calculate subtotal from all items
            quote.SubTotal = quote.QuoteItems.Sum(i => i.Quantity * i.UnitPrice);

            // Calculate item-level discounts
            var itemLevelDiscount = quote.QuoteItems.Sum(i =>
                (i.Quantity * i.UnitPrice) * (i.DiscountPercentage / 100));

            // Calculate quote-level discount
            decimal quoteLevelDiscount = 0;
            if (!string.IsNullOrEmpty(quote.DiscountType) && quote.DiscountValue > 0)
            {
                if (quote.DiscountType == "Percentage")
                {
                    quoteLevelDiscount = quote.SubTotal * (quote.DiscountValue / 100);
                }
                else if (quote.DiscountType == "Fixed")
                {
                    quoteLevelDiscount = quote.DiscountValue;
                }
            }

            // Total discount amount
            quote.DiscountAmount = itemLevelDiscount + quoteLevelDiscount;

            // Calculate tax on discounted amount
            var subtotalAfterItemDiscount = quote.SubTotal - itemLevelDiscount;
            var subtotalAfterAllDiscounts = subtotalAfterItemDiscount - quoteLevelDiscount;

            quote.TaxAmount = quote.QuoteItems.Sum(i =>
            {
                var itemSubtotal = i.Quantity * i.UnitPrice;
                var itemDiscount = itemSubtotal * (i.DiscountPercentage / 100);
                var itemTaxable = itemSubtotal - itemDiscount;
                return itemTaxable * (i.TaxRate / 100);
            });

            // Apply quote-level discount proportion to tax
            if (quoteLevelDiscount > 0 && subtotalAfterItemDiscount > 0)
            {
                var discountRatio = quoteLevelDiscount / subtotalAfterItemDiscount;
                quote.TaxAmount = quote.TaxAmount * (1 - discountRatio);
            }

            // Calculate total
            quote.TotalAmount = subtotalAfterAllDiscounts + quote.TaxAmount;
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
                DiscountType = quote.DiscountType,
                DiscountValue = quote.DiscountValue,
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