namespace TarimTakip.API.Models.DTOs.Advert
{
    public class AdvertResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }

        public string SellerName { get; set; } // Satıcının adı (User tablosundan gelecek)
        public int SellerId { get; set; }      // Profiline gitmek istersek diye
        public DateTime CreatedAt { get; set; }
    }
}