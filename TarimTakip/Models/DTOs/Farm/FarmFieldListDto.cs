namespace TarimTakip.API.Models.DTOs.Farm
{
    public class FarmFieldListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public string City { get; set; }    
        public string County { get; set; }
        public string PlantName { get; set; }
        public decimal Area { get; set; }
        public string RegionName { get; set; }
    }
}