namespace AwningsAPI.Dto.Product
{
    public class FrameColourOptionDto
    {
        public int FrameColourOptionId { get; set; }
        public string Description { get; set; }
        /// <summary>true = price from NonStandardRALColours, false = included (no extra charge)</summary>
        public bool IsNonStandardRAL { get; set; }
    }
}
