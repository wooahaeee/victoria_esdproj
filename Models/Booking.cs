using System.ComponentModel.DataAnnotations;

namespace _27_victoria_esdproj.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public string FacilityDescription { get; set; }
        public DateTime BookingDateFrom { get; set; }
        public DateTime BookingDateTo { get; set; }
        public string BookedBy { get; set; }
        public string BookingStatus { get; set; }

    }
}
