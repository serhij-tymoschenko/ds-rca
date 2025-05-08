namespace ds_rca.config;

public class Config
{
    public const string REDDIT_DEPLOYER_ADDRESS = "0x36FB3886CF3Fc4E44d8b99D9a8520425239618C2";
    public const int RCA_SERVICE_SCAN_TIMEOUT_MIN = 1;
    public const int CONTRACT_SERVICE_SCAN_TIMEOUT_MIN = 5;
    public static string DS_API_KEY = Environment.GetEnvironmentVariable("DS_API_KEY");
    public static string POLYSCAN_API_KEY = Environment.GetEnvironmentVariable("POLYSCAN_API_KEY");
    public static string FIREBASE_PROJECT_ID = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
    public static string FIREBASE_CREDENTIALS = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
    public static string REDDIT_API_KEY = Environment.GetEnvironmentVariable("REDDIT_API_KEY");
}