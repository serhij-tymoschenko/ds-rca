namespace ds_rca.config;

public class Config
{
    public const int POLYSCAN_OFFSET = 40;
    public const ulong LOG_CHANNEL_ID = 1370139109477060753;

    public static string DS_API_KEY = Environment.GetEnvironmentVariable("DS_API_KEY");
    public static string POLYSCAN_API_KEY = Environment.GetEnvironmentVariable("POLYSCAN_API_KEY");
    public static string FIREBASE_PROJECT_ID = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
    public static string FIREBASE_CREDENTIALS = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
    public static string REDDIT_API_KEY = Environment.GetEnvironmentVariable("REDDIT_API_KEY");
}