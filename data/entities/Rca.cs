namespace ds_rca.data.entities;

public struct Rca
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string AuthorName { get; init; }
    public required int Count { get; init; }
    public required string Price { get; init; }
    public required string ShopUrl { get; init; }
    public required string ImageUrl { get; init; }
    public required RcaTraits Traits { get; set; }
}

public struct RcaTraits
{
    public required string FaceUrl { get; init; }
    public required string EyesUrl { get; init; }
    public required string TopsUrl { get; init; }
    public required string BottomsUrl { get; init; }
    public required string BackgroundUrl { get; init; }
    public required string? HairUrl { get; init; }
    public required string? HairBackUrl { get; init; }
    public required string? HatsUrl { get; init; }
    public required string? LeftUrl { get; init; }
    public required string? RightUrl { get; init; }
}