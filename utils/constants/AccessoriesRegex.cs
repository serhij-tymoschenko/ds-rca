namespace ds_rca.utils.constants;

public class AccessoriesRegex
{
    public static string Right = "accessory_([^_]+)$";
    public static string Left = "accessory_back_([^_]+)$";
    public static string Bottoms = "body_bottom_([^_]+)$";
    public static string Tops = "body_([^_]+)$";
    public static string Face = "face_lower_([^_]+)$";
    public static string Eyes = "face_upper_([^_]+)$";
    public static string Hats = "head_accessory_([^_]+)$";
    public static string Hair = "hair_([^_]+)(?:_([^_]+))?";
}