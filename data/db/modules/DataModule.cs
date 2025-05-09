using ds_rca.bot;
using Google.Cloud.Firestore;

namespace ds_rca.data.db.modules;

public class DataModule(FirestoreDb db)
{
    public async Task SetLastStorefrontIdAsync(string id)
    {
        try
        {
            var docRef = db
                .Collection("config")
                .Document("rca");

            var data = new Dictionary<string, string>
            {
                { "last_storefront_id", id }
            };

            await docRef.SetAsync(data);
        }
        catch (Exception e)
        {
            Bot.Log($"Error setting storefront id: {e.Message}");
        }
    }

    public async Task<string> GetLastStorefrontIdAsync()
    {
        try
        {
            var docRef = db
                .Collection("config")
                .Document("rca");

            var snapshot = await docRef.GetSnapshotAsync();

            string? value;
            snapshot.TryGetValue<string>("last_storefront_id", out value);
            return value ?? "";
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting storefront id: {e.Message}");
        }
        
        return "";
    }

    public async Task SetLastEntityIdAsync(string id)
    {
        try
        {
            var docRef = db
                .Collection("config")
                .Document("contract");

            var data = new Dictionary<string, string>
            {
                { "last_entity_id", id }
            };

            await docRef.SetAsync(data);
        }
        catch (Exception e)
        {
            Bot.Log($"Error setting entity id: {e.Message}");
        }
    }

    public async Task<string> GetLastEntityIdAsync()
    {
        try
        {
            var docRef = db
                .Collection("config")
                .Document("contract");

            var snapshot = await docRef.GetSnapshotAsync();

            string? value;
            snapshot.TryGetValue<string>("last_entity_id", out value);
            return value ?? "";
        }
        catch (Exception e)
        {
            Bot.Log($"error getting entity id: {e.Message}");
        }
        
        return "";
    }
}