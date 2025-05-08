using ds_rca.bot;
using ds_rca.config;
using Google.Cloud.Firestore;

namespace ds_rca.data.db;

public class Database
{
    private static FirestoreDb? _db;
    private static DatabaseActions? _dbActions;

    public static void CreateInstance()
    {
        try
        {
            _db = new FirestoreDbBuilder
            {
                ProjectId = Config.FIREBASE_PROJECT_ID,
                JsonCredentials = Config.FIREBASE_CREDENTIALS
            }.Build();

            _dbActions = new DatabaseActions(_db);
        }
        catch (Exception e)
        {
            Bot.Log($"Error initializing db: {e.Message}");
        }
    }

    public static async Task AddStorefrontAsync(string storefrontId)
    {
        try
        {
            await _dbActions.AddStorefrontAsync(storefrontId);
        }
        catch (Exception e)
        {
            Bot.Log($"Error adding rca: {e.Message}");
        }
    }

    public static async Task DeleteStorefrontAsync(string storefrontId)
    {
        try
        {
            await _dbActions.DeleteStorefrontAsync(storefrontId);
        }
        catch (Exception e)
        {
            Bot.Log($"Error deleting rca: {e.Message}");
        }
    }

    public static async Task AddUserToNotifyAsync(string storefrontId, ulong serverId, ulong wishlisterId)
    {
        try
        {
            await _dbActions.AddUserToNotifyAsync(storefrontId, serverId, wishlisterId);
        }
        catch (Exception e)
        {
            Bot.Log($"Error adding wishlister: {e.Message}");
        }
    }

    public static async Task<List<ulong>> GetUsersToNotifyAsync(string storefrontId, ulong serverId)
    {
        try
        {
            var wishlister = await _dbActions.GetUsersToNotifyAsync(storefrontId, serverId);
            return wishlister;
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting wishlisters: {e.Message}");
        }

        return new List<ulong>();
    }

    public static async Task SetLastStorefrontIdAsync(string id)
    {
        try
        {
            await _dbActions.SetLastStorefrontIdAsync(id);
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
            var rcaId = await _dbActions.GetLastStorefrontIdAsync();
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
            await _dbActions.SetLastEntityIdAsync(id);
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
            var contractId = await _dbActions.GetLastEntityIdAsync();
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
            await _dbActions.ConfigAsync(serverId, rcaChannelId, contractChannelId);
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
            var idsList = await _dbActions.GetServerConfigsAsync();
            return idsList;
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting server ids: {e.Message}");
        }
        
        return new List<(ulong Rca, ulong Contract, ulong Server)>();
    }
}