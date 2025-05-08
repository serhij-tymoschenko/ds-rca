namespace ds_rca.utils.constants;

public class AccessoriesRegex
{
    public static string Right = "accessory_(.{11})$";
    public static string Left = "accessory_back_(.{11})$";
    public static string Bottoms = "body_bottom_(.{11})$";
    public static string Tops = "body_(.{11})$";
    public static string Face = "face_lower_(.{11})$";
    public static string Eyes = "face_upper_(.{11})$";
    public static string Hats = "head_accessory_(.{11})$";
    public static string Hair = "hair_(.{11})(?:_(.{11}))?";
}