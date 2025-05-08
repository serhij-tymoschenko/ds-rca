namespace ds_rca.utils.constants;

public class RegexConstants
{
    public const string STOREFRONT_REGEX = "storefront_nft_[A-Za-z0-9]{26}$";
    public const string TITLE_REGEX = "nft-title=\"([^\"]+)\"";
    public const string DESCRIPTION_REGEX = "nft-description=\"([^\"]+)\"";
    public const string IMAGE_URL_REGEX = "nft-avatar-image-url=\"([^\"]+)\"";
    public const string AUTHOR_NAME_REGEX = "artist-name=\"([^\"]+)\"";
    public const string COUNT_REGEX = "drop-size=\"([^\"]+)\"";
    public const string SHOP_URL_REGEX = "cta-url=\"([^\"]+)\"";
    public const string PRICE_REGEX = "price=\"([^\"]+)\"";
}