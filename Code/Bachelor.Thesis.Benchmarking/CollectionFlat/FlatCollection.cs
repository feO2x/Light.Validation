using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionFlat.Validators;

namespace Bachelor.Thesis.Benchmarking.CollectionFlat;

public class FlatCollection
{
    public static FlatCollection ValidDto = new ()
    {
        Names = new ()
        {
            "John Doe",
            "Max Muster",
            "Foo Foo"
        },
        Availability = new ()
        {
            { 1234, true },
            { 4567, false },
            { 9876, true }
        }
    };

    public static FlatCollection InvalidDto = new ()
    {
        Names = new ()
        {
            "John Doe 0",
            "John Doe 0123456789 0123456789",
            "John Doe 2",
            "John Doe 3",
            "John Doe 4",
            "John Doe 5",
            "John Doe 6",
            "John Doe 7",
            "John Doe 8",
            "John Doe 9"
        },
        Availability = new ()
        {
            {10001, false},
            {10002, true}
        }
    };

    [Required]
    [MinLength(1), MaxLength(10)]
    [NoStringInCollectionBiggerThan(20)]
    public List<string> Names { get; set; } = new ();

    [Required]
    [NoLongInDictionaryBiggerThan(10000)]
    public Dictionary<long, bool> Availability { get; set; } = new ();
}