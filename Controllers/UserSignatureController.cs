using AwningsAPI.Database;
using AwningsAPI.Dto.Auth;
using AwningsAPI.Model.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Controllers
{
    /// <summary>
    /// CRUD for the authenticated user's email signatures.
    /// GET    /api/signatures
    /// POST   /api/signatures
    /// PUT    /api/signatures/{id}
    /// DELETE /api/signatures/{id}
    /// PUT    /api/signatures/{id}/default
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/signatures")]
    public class UserSignatureController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserSignatureController(AppDbContext context) => _context = context;

        private string CurrentUser => User?.Identity?.Name ?? string.Empty;

        // ── GET /api/signatures ───────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSignatureDto>>> GetSignatures()
        {
            var sigs = await _context.UserSignatures
                .Where(s => s.Username == CurrentUser)
                .OrderByDescending(s => s.IsDefault).ThenBy(s => s.Label)
                .Select(s => ToDto(s))
                .ToListAsync();

            return Ok(sigs);
        }

        // ── POST /api/signatures ──────────────────────────────────────────────
        [HttpPost]
        public async Task<ActionResult<UserSignatureDto>> CreateSignature([FromBody] UserSignatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Label))
                return BadRequest("Label is required.");
            if (string.IsNullOrWhiteSpace(dto.SignatureText))
                return BadRequest("SignatureText is required.");

            if (dto.IsDefault) await ClearDefaultAsync();

            var entity = Map(new UserSignature(), dto);
            entity.Username = CurrentUser;
            entity.DateCreated = DateTime.UtcNow;

            _context.UserSignatures.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSignatures),
                new { id = entity.SignatureId }, ToDto(entity));
        }

        // ── PUT /api/signatures/{id} ──────────────────────────────────────────
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserSignatureDto>> UpdateSignature(
            int id, [FromBody] UserSignatureDto dto)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == id && s.Username == CurrentUser);

            if (entity == null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Label))
                return BadRequest("Label is required.");
            if (string.IsNullOrWhiteSpace(dto.SignatureText))
                return BadRequest("SignatureText is required.");

            if (dto.IsDefault && !entity.IsDefault) await ClearDefaultAsync();

            Map(entity, dto);
            entity.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(ToDto(entity));
        }

        // ── PUT /api/signatures/{id}/default ─────────────────────────────────
        [HttpPut("{id:int}/default")]
        public async Task<IActionResult> SetDefault(int id)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == id && s.Username == CurrentUser);

            if (entity == null) return NotFound();

            await ClearDefaultAsync();
            entity.IsDefault = true;
            entity.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(ToDto(entity));
        }

        // ── DELETE /api/signatures/{id} ───────────────────────────────────────
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSignature(int id)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == id && s.Username == CurrentUser);

            if (entity == null) return NotFound();

            _context.UserSignatures.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Signature deleted." });
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task ClearDefaultAsync()
        {
            var existing = await _context.UserSignatures
                .Where(s => s.Username == CurrentUser && s.IsDefault)
                .ToListAsync();

            foreach (var s in existing)
            { s.IsDefault = false; s.DateUpdated = DateTime.UtcNow; }
        }

        private static UserSignature Map(UserSignature e, UserSignatureDto d)
        {
            e.Label = d.Label.Trim();
            e.FullName = d.FullName?.Trim();
            e.JobTitle = d.JobTitle?.Trim();
            e.Company = d.Company?.Trim();
            e.Phone = d.Phone?.Trim();
            e.Mobile = d.Mobile?.Trim();
            e.Email = d.Email?.Trim();
            e.Website = d.Website?.Trim();
            e.GreetingText = (d.GreetingText ?? "Kindest regards,").Trim();
            e.SeparatorStyle = (d.SeparatorStyle ?? "blank_line").Trim();
            e.LayoutOrder = (d.LayoutOrder ?? "name_first").Trim();
            e.SignatureText = d.SignatureText.Trim();
            e.IsDefault = d.IsDefault;
            return e;
        }

        private static UserSignatureDto ToDto(UserSignature s) => new()
        {
            SignatureId = s.SignatureId,
            Label = s.Label,
            FullName = s.FullName,
            JobTitle = s.JobTitle,
            Company = s.Company,
            Phone = s.Phone,
            Mobile = s.Mobile,
            Email = s.Email,
            Website = s.Website,
            GreetingText = s.GreetingText,
            SeparatorStyle = s.SeparatorStyle,
            LayoutOrder = s.LayoutOrder,
            SignatureText = s.SignatureText,
            IsDefault = s.IsDefault,
            DateCreated = s.DateCreated,
            DateUpdated = s.DateUpdated
        };
    }
}