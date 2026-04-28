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
            var quoteNumber = await GenerateDraftQuoteNumberAsync();

            var discountType = string.IsNullOrWhiteSpace(createDto.DiscountType) ? string.Empty : createDto.DiscountType;
            var discountValue = string.IsNullOrWhiteSpace(createDto.DiscountType) ? 0 : createDto.DiscountValue;

            var quote = new Quote
            {
                WorkflowId = createDto.WorkflowId,
                QuoteNumber = quoteNumber,
                QuoteDate = createDto.QuoteDate,
                FollowUpDate = createDto.FollowUpDate,
                CustomerId = createDto.CustomerId,
                Notes = createDto.Notes ?? string.Empty,
                Terms = createDto.Terms ?? string.Empty,
                DiscountType = discountType,
                DiscountValue = discountValue,
                IsFinal = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            var sortOrder = 1;
            foreach (var itemDto in createDto.QuoteItems)
            {
                var item = CreateQuoteItem(itemDto, sortOrder++);
                quote.QuoteItems.Add(item);
            }

            CalculateQuoteTotals(quote);

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return await GetQuoteByIdAsync(quote.QuoteId);
        }

        public async Task<QuoteDto> CreateFinalQuoteAsync(CreateFinalQuoteDto createDto, string currentUser)
        {
            var draft = await _context.Quotes
                .Include(q => q.QuoteItems)
                .FirstOrDefaultAsync(q => q.QuoteId == createDto.DraftQuoteId);

            if (draft == null)
                throw new InvalidOperationException($"Draft quote with ID {createDto.DraftQuoteId} not found.");

            if (draft.IsFinal)
                throw new InvalidOperationException("Cannot finalize a quote that is already a final quote.");

            var hasFinal = await _context.Quotes.AnyAsync(q => q.DraftQuoteId == createDto.DraftQuoteId);
            if (hasFinal)
                throw new InvalidOperationException("This draft quote already has a final quote.");

            var quoteNumber = await GenerateFinalQuoteNumberAsync();
            var discountType = string.IsNullOrWhiteSpace(createDto.DiscountType) ? string.Empty : createDto.DiscountType;
            var discountValue = string.IsNullOrWhiteSpace(createDto.DiscountType) ? 0 : createDto.DiscountValue;

            var finalQuote = new Quote
            {
                WorkflowId = draft.WorkflowId,
                CustomerId = draft.CustomerId,
                QuoteNumber = quoteNumber,
                QuoteDate = createDto.QuoteDate,
                FollowUpDate = createDto.FollowUpDate,
                Notes = createDto.Notes ?? string.Empty,
                Terms = createDto.Terms ?? string.Empty,
                DiscountType = discountType,
                DiscountValue = discountValue,
                IsFinal = true,
                FinalizedAt = DateTime.UtcNow,
                DraftQuoteId = draft.QuoteId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            var sortOrder = 1;
            foreach (var itemDto in createDto.QuoteItems)
            {
                var item = CreateQuoteItem(itemDto, sortOrder++);
                finalQuote.QuoteItems.Add(item);
            }

            CalculateQuoteTotals(finalQuote);

            draft.IsFinal = true;

            _context.Quotes.Add(finalQuote);
            await _context.SaveChangesAsync();

            return await GetQuoteByIdAsync(finalQuote.QuoteId);
        }

        public async Task<QuoteDto> UpdateQuoteAsync(int quoteId, UpdateQuoteDto updateDto, string currentUser)
        {
            var quote = await _context.Quotes
                .Include(q => q.QuoteItems)
                .FirstOrDefaultAsync(i => i.QuoteId == quoteId);

            if (quote == null)
                return null;

            // Block editing a draft that already has a final quote
            if (!quote.IsFinal && quote.DraftQuoteId == null)
            {
                var hasFinal = await _context.Quotes.AnyAsync(q => q.DraftQuoteId == quoteId);
                if (hasFinal)
                    throw new InvalidOperationException("Cannot edit a draft quote that has a final quote. Edit or delete the final quote first.");
            }

            if (updateDto.QuoteDate.HasValue)
                quote.QuoteDate = updateDto.QuoteDate.Value;

            if (updateDto.FollowUpDate.HasValue)
                quote.FollowUpDate = updateDto.FollowUpDate.Value;

            if (updateDto.Notes != null)
                quote.Notes = updateDto.Notes;

            if (updateDto.Terms != null)
                quote.Terms = updateDto.Terms;

            if (updateDto.DiscountType != null)
                quote.DiscountType = string.IsNullOrWhiteSpace(updateDto.DiscountType)
                    ? string.Empty
                    : updateDto.DiscountType;

            if (updateDto.DiscountValue.HasValue)
                quote.DiscountValue = updateDto.DiscountValue.Value;

            if (updateDto.QuoteItems != null && updateDto.QuoteItems.Any())
            {
                _context.QuoteItems.RemoveRange(quote.QuoteItems);

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
                        ProductItemId = itemDto.ProductItemId,
                        SortOrder = sortOrder++
                    };

                    var subtotal = item.Quantity * item.UnitPrice;
                    var discount = subtotal * (item.DiscountPercentage / 100);
                    var taxableAmount = subtotal - discount;
                    var tax = taxableAmount * (item.TaxRate / 100);
                    item.TotalPrice = taxableAmount + tax;

                    quote.QuoteItems.Add(item);
                }

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

            // Block deleting a draft that has a final quote
            if (!quote.IsFinal && quote.DraftQuoteId == null)
            {
                var hasFinal = await _context.Quotes.AnyAsync(q => q.DraftQuoteId == quoteId);
                if (hasFinal)
                    throw new InvalidOperationException("Cannot delete a draft quote that has a final quote. Delete the final quote first.");
            }

            // When deleting a final quote, mark the draft as no longer finalized
            if (quote.IsFinal && quote.DraftQuoteId.HasValue)
            {
                var draft = await _context.Quotes.FindAsync(quote.DraftQuoteId.Value);
                if (draft != null)
                    draft.IsFinal = false;
            }

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return true;
        }

        private QuoteItem CreateQuoteItem(CreateQuoteItemDto itemDto, int sortOrder)
        {
            var item = new QuoteItem
            {
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TaxRate = itemDto.TaxRate,
                DiscountPercentage = itemDto.DiscountPercentage,
                ProductItemId = itemDto.ProductItemId,
                SortOrder = sortOrder
            };

            var subtotal = item.Quantity * item.UnitPrice;
            var discount = subtotal * (item.DiscountPercentage / 100);
            var taxableAmount = subtotal - discount;
            var tax = taxableAmount * (item.TaxRate / 100);
            item.TotalPrice = taxableAmount + tax;

            return item;
        }

        private void CalculateQuoteTotals(Quote quote)
        {
            quote.SubTotal = quote.QuoteItems.Sum(i => i.Quantity * i.UnitPrice);

            var itemLevelDiscount = quote.QuoteItems.Sum(i =>
                (i.Quantity * i.UnitPrice) * (i.DiscountPercentage / 100));

            decimal quoteLevelDiscount = 0;
            if (!string.IsNullOrEmpty(quote.DiscountType) && quote.DiscountValue > 0)
            {
                if (quote.DiscountType == "Percentage")
                    quoteLevelDiscount = quote.SubTotal * (quote.DiscountValue / 100);
                else if (quote.DiscountType == "Fixed")
                    quoteLevelDiscount = quote.DiscountValue;
            }

            quote.DiscountAmount = itemLevelDiscount + quoteLevelDiscount;

            var subtotalAfterItemDiscount = quote.SubTotal - itemLevelDiscount;
            var subtotalAfterAllDiscounts = subtotalAfterItemDiscount - quoteLevelDiscount;

            quote.TaxAmount = quote.QuoteItems.Sum(i =>
            {
                var itemSubtotal = i.Quantity * i.UnitPrice;
                var itemDiscount = itemSubtotal * (i.DiscountPercentage / 100);
                var itemTaxable = itemSubtotal - itemDiscount;
                return itemTaxable * (i.TaxRate / 100);
            });

            if (quoteLevelDiscount > 0 && subtotalAfterItemDiscount > 0)
            {
                var discountRatio = quoteLevelDiscount / subtotalAfterItemDiscount;
                quote.TaxAmount = quote.TaxAmount * (1 - discountRatio);
            }

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
                UpdatedAt = quote.UpdatedAt,
                CreatedBy = quote.CreatedBy,
                UpdatedBy = quote.UpdatedBy,
                IsFinal = quote.IsFinal,
                FinalizedAt = quote.FinalizedAt,
                DraftQuoteId = quote.DraftQuoteId,
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
                    SortOrder = q.SortOrder,
                    ProductItemId = q.ProductItemId
                }).ToList() ?? new List<QuoteItemDto>()
            };
        }

        private async Task<string> GenerateDraftQuoteNumberAsync()
        {
            var year = DateTime.Now.Year;
            var prefix = $"DRAFT-QUOTE-{year}";
            var lastQuote = await _context.Quotes
                .Where(q => q.QuoteNumber.StartsWith(prefix))
                .OrderByDescending(q => q.QuoteNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastQuote != null)
            {
                var lastNumber = lastQuote.QuoteNumber.Split('-').Last();
                if (int.TryParse(lastNumber, out int num))
                    nextNumber = num + 1;
            }

            return $"{prefix}-{nextNumber:D4}";
        }

        private async Task<string> GenerateFinalQuoteNumberAsync()
        {
            var year = DateTime.Now.Year;
            var prefix = $"FINAL-QUOTE-{year}";
            var lastQuote = await _context.Quotes
                .Where(q => q.QuoteNumber.StartsWith(prefix))
                .OrderByDescending(q => q.QuoteNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastQuote != null)
            {
                var lastNumber = lastQuote.QuoteNumber.Split('-').Last();
                if (int.TryParse(lastNumber, out int num))
                    nextNumber = num + 1;
            }

            return $"{prefix}-{nextNumber:D4}";
        }
    }
}
