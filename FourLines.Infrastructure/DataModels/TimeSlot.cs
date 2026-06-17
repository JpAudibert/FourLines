//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace FourLines.Api.DataModels;

//[Table("TimeSlots")]
//public class TimeSlot
//{
//    [Key]
//    public int Id { get; init; }

//    [Required]
//    public int CourtId { get; init; }

//    [Required]
//    public DateOnly SlotDate { get; init; } = default!;

//    [Required]
//    public TimeOnly StartTime { get; init; } = default!;

//    [Required]
//    public TimeOnly EndTime { get; init; } = default!;

//    [Required]
//    public decimal Price { get; init; } = default!;

//    [Required]
//    public int IsAvailable { get; init; } = default!;

//    [Required]
//    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

//    [Required]
//    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;

//    [ForeignKey(nameof(CourtId))]
//    public Court Court { get; init; } = default!;

//}
