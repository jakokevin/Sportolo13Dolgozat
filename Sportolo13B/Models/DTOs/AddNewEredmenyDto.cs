namespace Sportolo13B.Models.DTOs
{
    public class AddNewEredmenyDto
    {
        public string Competition { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int SportoloId { get; set; }
    }
}