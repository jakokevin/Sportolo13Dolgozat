namespace Sportolo13B.Models.DTOs
{
    public class UpdateEredmenyDto
    {
        public string Competition { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime ResultTime { get; set; }

        public int SportoloId { get; set; }
    }
}