namespace ds_rca.config;

public class Config
{
    public const int POLYSCAN_OFFSET = 40;

    public static ulong LOG_CHANNEL_ID = ulong.Parse(Environment.GetEnvironmentVariable("LOG_CHANNEL_ID"));
    public static string DS_API_KEY = Environment.GetEnvironmentVariable("DS_API_KEY");
    public static string POLYSCAN_API_KEY = Environment.GetEnvironmentVariable("POLYSCAN_API_KEY");
    public static string FIREBASE_PROJECT_ID = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
    public static string FIREBASE_CREDENTIALS = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
    public static string REDDIT_API_KEY = Environment.GetEnvironmentVariable("REDDIT_API_KEY");
}