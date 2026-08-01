namespace POS_API.DTO
{
    public class ImageUpdateDTO
    {
        public string ID { get; set; } = null!;

        public string CompanyId { get; set; } = null!;

        public List<string> ImageBase64 { get; set; } = new List<string>();
    }
}
