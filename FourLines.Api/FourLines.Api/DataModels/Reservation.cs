//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace FourLines.Api.DataModels;

//[Table("Reservations")]
//public class Reservation
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    public int Status { get; set; } = default!;

//    [Required]
//    public int SlotId { get; set; }

//    [ForeignKey(nameof(SlotId))]
//    public TimeSlot TimeSlot { get; set; } = default!;

//    [Required]
//    public int CreatedByUserId { get; set; }

//    [ForeignKey(nameof(CreatedByUserId))]
//    public User User { get; set; } = default!;

//    [Required]
//    public int CourtId { get; set; }

//    [ForeignKey(nameof(CourtId))]
//    public Court Court { get; set; } = default!;

//}
