namespace AwningsAPI.Dto.Product
{
    public class FrameColourOptionDto
    {
        public int FrameColourId { get; set; }
        public string Description { get; set; }
        /// <summary>0 = white/light (extra charge), 1 = black/dark (no charge)</summary>
        public int ColorValue { get; set; }
        public decimal Price { get; set; }
        public int SortOrder { get; set; }
    }
}
