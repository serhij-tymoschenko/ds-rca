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
        try
        {
            await _dataModule.SetLastStorefrontIdAsync(id);
        }
        catch (Exception e)
        {
           Bot.Log($"Error setting storefront id: {e.Message}");
        }
    }

    public static async Task<string> GetLastStorefrontIdAsync()
    {
        try
        {
            var rcaId = await _dataModule.GetLastStorefrontIdAsync();
            return rcaId;
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting storefront id: {e.Message}");
        }

        return "";
    }

    public static async Task SetLastEntityIdAsync(string id)
    {
        try
        {
            await _dataModule.SetLastEntityIdAsync(id);
        }
        catch (Exception e)
        {
            Bot.Log($"Error setting entity id: {e.Message}");
        }
    }

    public static async Task<string> GetLastEntityIdAsync()
    {
        try
        {
            var contractId = await _dataModule.GetLastEntityIdAsync();
            return contractId;
        }
        catch (Exception e)
        {
            Bot.Log($"error getting entity id: {e.Message}");
        }

        return "";
    }

    public static async Task SetServerConfigAsync(ulong serverId, string rcaChannelId, string contractChannelId)
    {
        try
        {
            await _configModule.ConfigAsync(serverId, rcaChannelId, contractChannelId);
        }
        catch (Exception e)
        {
            Bot.Log($"Error configuring server: {e.Message}");
        }
    }
    
    public static async Task<List<(ulong Rca, ulong Contract, ulong Server)>> GetServerConfigsAsync()
    {
        try
        {
            var idsList = await _configModule.GetServerConfigsAsync();
            return idsList;
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting server ids: {e.Message}");
        }
        
        return new List<(ulong Rca, ulong Contract, ulong Server)>();
    }
}