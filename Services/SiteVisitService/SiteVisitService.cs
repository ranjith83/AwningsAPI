using AwningsAPI.Database;
using AwningsAPI.Dto.SiteVisit;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.SiteVisit;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.SiteVisitService
{
    public class SiteVisitService : ISiteVisitService
    {
        private readonly AppDbContext _context;

        public SiteVisitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Model.SiteVisit.SiteVisit>> GetSiteVisitsByWorkflowIdAsync(int workflowId)
        {
            return await _context.SiteVisits
                .Where(sv => sv.WorkflowId == workflowId)
                .OrderByDescending(sv => sv.DateCreated)
                .ToListAsync();
        }

        public async Task<Model.SiteVisit.SiteVisit?> GetSiteVisitByIdAsync(int id)
        {
            return await _context.SiteVisits
                .Include(sv => sv.Workflow)
                .FirstOrDefaultAsync(sv => sv.SiteVisitId == id);
        }

        public async Task<Model.SiteVisit.SiteVisit> CreateSiteVisitAsync(CreateSiteVisitDto dto, string currentUser)
        {
            var siteVisit = new Model.SiteVisit.SiteVisit
            {
                WorkflowId = dto.WorkflowId,
                ProductModelType = dto.ProductModelType,
                Model = dto.Model,
                OtherPleaseSpecify = dto.OtherPleaseSpecify,

                // Product Model Section
                SiteLayout = dto.SiteLayout,
                Structure = dto.Structure,
                PassageHeight = dto.PassageHeight,
                Width = dto.Width,
                Projection = dto.Projection,
                HeightAvailable = dto.HeightAvailable,
                WallType = dto.WallType,
                ExternalInsulation = dto.ExternalInsulation,
                WallFinish = dto.WallFinish,
                WallThickness = dto.WallThickness,
                SpecialBrackets = dto.SpecialBrackets,
                SideInfills = dto.SideInfills,
                FlashingRequired = dto.FlashingRequired,
                FlashingDimensions = dto.FlashingDimensions,
                StandOfBrackets = dto.StandOfBrackets,
                StandOfBracketDimension = dto.StandOfBracketDimension,
                Electrician = dto.Electrician,
                ElectricalConnection = dto.ElectricalConnection,
                Location = dto.Location,
                OtherSiteSurveyNotes = dto.OtherSiteSurveyNotes,

                // Model Details Section
                FixtureType = dto.FixtureType,
                Operation = dto.Operation,
                CrankLength = dto.CrankLength,
                OperationSide = dto.OperationSide,
                Fabric = dto.Fabric,
                RAL = dto.RAL,
                ValanceChoice = dto.ValanceChoice,
                Valance = dto.Valance,
                WindSensor = dto.WindSensor,

                // ShadePlus Section
                ShadePlusRequired = dto.ShadePlusRequired,
                ShadeType = dto.ShadeType,
                ShadeplusFabric = dto.ShadeplusFabric,
                ShadePlusAnyOtherDetail = dto.ShadePlusAnyOtherDetail,

                // Lights Section
                Lights = dto.Lights,
                LightsType = dto.LightsType,
                LightsAnyOtherDetails = dto.LightsAnyOtherDetails,

                // Heater Section
                Heater = dto.Heater,
                HeaterManufacturer = dto.HeaterManufacturer,
                NumberRequired = dto.NumberRequired,
                HeaterOutput = dto.HeaterOutput,
                HeaterColour = dto.HeaterColour,
                RemoteControl = dto.RemoteControl,
                ControllerBox = dto.ControllerBox,
                HeaterAnyOtherDetails = dto.HeaterAnyOtherDetails,

                // Audit
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            _context.SiteVisits.Add(siteVisit);
            await _context.SaveChangesAsync();

            return siteVisit;
        }

        public async Task<Model.SiteVisit.SiteVisit> UpdateSiteVisitAsync(int id, SiteVisitDto dto, string currentUser)
        {
            var existingSiteVisit = await _context.SiteVisits.FindAsync(id);

            if (existingSiteVisit == null)
            {
                throw new Exception($"Site visit with ID {id} not found");
            }

            // Update all fields
            existingSiteVisit.ProductModelType = dto.ProductModelType;
            existingSiteVisit.Model = dto.Model;
            existingSiteVisit.OtherPleaseSpecify = dto.OtherPleaseSpecify;

            // Product Model Section
            existingSiteVisit.SiteLayout = dto.SiteLayout;
            existingSiteVisit.Structure = dto.Structure;
            existingSiteVisit.PassageHeight = dto.PassageHeight;
            existingSiteVisit.Width = dto.Width;
            existingSiteVisit.Projection = dto.Projection;
            existingSiteVisit.HeightAvailable = dto.HeightAvailable;
            existingSiteVisit.WallType = dto.WallType;
            existingSiteVisit.ExternalInsulation = dto.ExternalInsulation;
            existingSiteVisit.WallFinish = dto.WallFinish;
            existingSiteVisit.WallThickness = dto.WallThickness;
            existingSiteVisit.SpecialBrackets = dto.SpecialBrackets;
            existingSiteVisit.SideInfills = dto.SideInfills;
            existingSiteVisit.FlashingRequired = dto.FlashingRequired;
            existingSiteVisit.FlashingDimensions = dto.FlashingDimensions;
            existingSiteVisit.StandOfBrackets = dto.StandOfBrackets;
            existingSiteVisit.StandOfBracketDimension = dto.StandOfBracketDimension;
            existingSiteVisit.Electrician = dto.Electrician;
            existingSiteVisit.ElectricalConnection = dto.ElectricalConnection;
            existingSiteVisit.Location = dto.Location;
            existingSiteVisit.OtherSiteSurveyNotes = dto.OtherSiteSurveyNotes;

            // Model Details Section
            existingSiteVisit.FixtureType = dto.FixtureType;
            existingSiteVisit.Operation = dto.Operation;
            existingSiteVisit.CrankLength = dto.CrankLength;
            existingSiteVisit.OperationSide = dto.OperationSide;
            existingSiteVisit.Fabric = dto.Fabric;
            existingSiteVisit.RAL = dto.RAL;
            existingSiteVisit.ValanceChoice = dto.ValanceChoice;
            existingSiteVisit.Valance = dto.Valance;
            existingSiteVisit.WindSensor = dto.WindSensor;

            // ShadePlus Section
            existingSiteVisit.ShadePlusRequired = dto.ShadePlusRequired;
            existingSiteVisit.ShadeType = dto.ShadeType;
            existingSiteVisit.ShadeplusFabric = dto.ShadeplusFabric;
            existingSiteVisit.ShadePlusAnyOtherDetail = dto.ShadePlusAnyOtherDetail;

            // Lights Section
            existingSiteVisit.Lights = dto.Lights;
            existingSiteVisit.LightsType = dto.LightsType;
            existingSiteVisit.LightsAnyOtherDetails = dto.LightsAnyOtherDetails;

            // Heater Section
            existingSiteVisit.Heater = dto.Heater;
            existingSiteVisit.HeaterManufacturer = dto.HeaterManufacturer;
            existingSiteVisit.NumberRequired = dto.NumberRequired;
            existingSiteVisit.HeaterOutput = dto.HeaterOutput;
            existingSiteVisit.HeaterColour = dto.HeaterColour;
            existingSiteVisit.RemoteControl = dto.RemoteControl;
            existingSiteVisit.ControllerBox = dto.ControllerBox;
            existingSiteVisit.HeaterAnyOtherDetails = dto.HeaterAnyOtherDetails;

            // Update audit fields
            existingSiteVisit.DateUpdated = DateTime.UtcNow;
            existingSiteVisit.UpdatedBy = currentUser;

            _context.SiteVisits.Update(existingSiteVisit);
            await _context.SaveChangesAsync();

            return existingSiteVisit;
        }

        public async Task<bool> DeleteSiteVisitAsync(int id)
        {
            var siteVisit = await _context.SiteVisits.FindAsync(id);

            if (siteVisit == null)
            {
                return false;
            }

            _context.SiteVisits.Remove(siteVisit);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SiteVisitValues>> GetAllValuesAsync()
        {
            return await _context.SiteVisitValues
                .Where(v => v.IsActive)
                .OrderBy(v => v.Category)
                .ThenBy(v => v.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<SiteVisitValues>> GetValuesByCategoryAsync(string category)
        {
            return await _context.SiteVisitValues
                .Where(v => v.IsActive && v.Category == category)
                .OrderBy(v => v.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Dictionary<string, List<string>>> GetAllValuesDictionaryAsync()
        {
            var values = await GetAllValuesAsync();

            return values
                .GroupBy(v => v.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(v => v.Value).ToList()
                );
        }
    }
}