namespace AwningsAPI.Dto.Product
{
    public class FrameColourOptionDto
    {
        public int FrameColourOptionId { get; set; }
        public string Description { get; set; }
        public bool IsNonStandardRAL { get; set; }
    }

    /// <summary>Returned by the configuration endpoint — full junction-row view.</summary>
    public class FrameColourConfigDto
    {
        public int FrameColourId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int FrameColourOptionId { get; set; }
        public string Description { get; set; }
        public bool IsNonStandardRAL { get; set; }
    }

    /// <summary>Body for PUT /api/configuration/frame-colours/{id}.</summary>
    public class UpdateFrameColourDto
    {
        public bool IsNonStandardRAL { get; set; }
    }
}
