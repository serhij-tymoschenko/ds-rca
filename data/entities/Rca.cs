namespace ds_rca.data.entities;

public struct Rca
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string AuthorName { get; init; }
    public required string Count { get; init; }
    public required string Price { get; init; }
    public required string ShopUrl { get; init; }
    public required string ImageUrl { get; init; }
    public required RcaTraits Traits { get; set; }
}

public struct RcaTraits
{
    public required string Face { get; init; }
    public required string Eyes { get; init; }
    public required string Tops { get; init; }
    public required string Bottoms { get; init; }
    public required string Background { get; init; }
    public required string Hair { get; init; }
    public required string HairBack { get; init; }
    public required string Hats { get; init; }
    public required string Left { get; init; }
    public required string Right { get; init; }
}