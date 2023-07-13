using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Models
{
    public class OrderNoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatorName { get; set; }
        public string CreatorSurname { get; set; }
        public DateTime Created { get; set; }
    }
}