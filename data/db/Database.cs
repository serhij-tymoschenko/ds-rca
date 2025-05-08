using ds_rca.config;
using Google.Cloud.Firestore;

namespace ds_rca.data.db.firestore;

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
            Console.WriteLine($"Error initializing db: {e.Message}");
            throw;
        }
    }

    public static async Task AddRca(string storefrontId)
    {
        try
        {
            await _dbActions.AddRcaAsync(storefrontId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error adding rca: {e.Message}");
        }
    }

    public static async Task DeleteRca(string storefrontId)
    {
        try
        {
            await _dbActions.DeleteRcaAsync(storefrontId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting rca: {e.Message}");
        }
    }

    public static async Task AddWishlister(string storefrontId, ulong serverId, ulong wishlisterId)
    {
        try
        {
            await _dbActions.AddWishlisterAsync(storefrontId, serverId, wishlisterId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error adding wishlister: {e.Message}");
        }
    }

    public static async Task<List<string>> GetWishlisters(string storefrontId, ulong serverId)
    {
        try
        {
            var wishlister = await _dbActions.GetWishlistersAsync(storefrontId, serverId);
            return wishlister;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting wishlisters: {e.Message}");
        }

        return new List<string>();
    }

    public static async Task SetLastRcaId(string id)
    {
        try
        {
            await _dbActions.SetLastRcaIdAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error setting last rca: {e.Message}");
        }
    }

    public static async Task<string> GetLastRcaId()
    {
        try
        {
            var rcaId = await _dbActions.GetLastRcaIdAsync();
            return rcaId;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting last rca: {e.Message}");
        }

        return "";
    }

    public static async Task SetLastContractId(string id)
    {
        try
        {
            await _dbActions.SetLastContractIdAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error setting last rca: {e.Message}");
        }
    }

    public static async Task<string> GetLastContractId()
    {
        try
        {
            var contractId = await _dbActions.GetLastContractIdAsync();
            return contractId;
        }
        catch (Exception e)
        {
            Console.WriteLine($"error getting last rca: {e.Message}");
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
            Console.WriteLine($"Error seeding server config: {e.Message}");
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
            Console.WriteLine($"Error getting server configs: {e.Message}");
        }
        
        return new List<(ulong Rca, ulong Contract, ulong Server)>();
    }
}