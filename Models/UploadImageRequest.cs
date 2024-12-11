namespace AoiCryptoAPI.Models
{
    public class UploadImageRequest
    {
        public string ImageBase64 { get; set; } // Base64-encoded image
        public string Name { get; set; }       // Optional file name
        public int? Expiration { get; set; }   // Optional expiration time in seconds
    }
}
