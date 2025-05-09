using ds_rca.bot;
using ds_rca.config;
using ds_rca.data.db.modules;
using Google.Cloud.Firestore;

namespace ds_rca.data.db;

public class Database
{
    private static FirestoreDb? _db;
    private static DataModule? _dataModule;
    private static ConfigModule? _configModule;

    public static void CreateInstance()
    {
        try
        {
            _db = new FirestoreDbBuilder
            {
                ProjectId = Config.FIREBASE_PROJECT_ID,
                JsonCredentials = Config.FIREBASE_CREDENTIALS
            }.Build();

            _dataModule = new DataModule(_db);
            _configModule = new ConfigModule(_db);
        }
        catch (Exception e)
        {
            Bot.Log($"Error initializing db: {e.Message}");
        }
    }

    public static async Task SetLastStorefrontIdAsync(string id)
    {
        _dataModule.SetLastStorefrontIdAsync(id);
    }

    public static async Task<string> GetLastStorefrontIdAsync()
    {
        var rcaId = await _dataModule.GetLastStorefrontIdAsync();
        return rcaId;
    }

    public static async Task SetLastEntityIdAsync(string id)
    {
        _dataModule.SetLastEntityIdAsync(id);
    }

    public static async Task<string> GetLastEntityIdAsync()
    {
        var contractId = await _dataModule.GetLastEntityIdAsync();
        return contractId;
    }

    public static async Task SetServerConfigAsync(ulong serverId, string rcaChannelId, string contractChannelId)
    {
        _configModule.ConfigAsync(serverId, rcaChannelId, contractChannelId);
    }

    public static async Task<List<(ulong Rca, ulong Contract, ulong Server)>> GetServerConfigsAsync()
    {
        var idsList = await _configModule.GetServerConfigsAsync();
        return idsList;
    }
}