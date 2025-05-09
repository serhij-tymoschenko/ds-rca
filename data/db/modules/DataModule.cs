using Google.Cloud.Firestore;

namespace ds_rca.data.db;

public class DataModule(FirestoreDb db)
{
    public async Task SetLastStorefrontIdAsync(string id)
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

    public async Task<string> GetLastStorefrontIdAsync()
    {
        var docRef = db
            .Collection("config")
            .Document("rca");

        var snapshot = await docRef.GetSnapshotAsync();

        string? value;
        snapshot.TryGetValue<string>("last_storefront_id", out value);
        return value ?? "";
    }

    public async Task SetLastEntityIdAsync(string id)
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

    public async Task<string> GetLastEntityIdAsync()
    {
        var docRef = db
            .Collection("config")
            .Document("contract");

        var snapshot = await docRef.GetSnapshotAsync();

        string? value;
        snapshot.TryGetValue<string>("last_entity_id", out value);
        return value ?? "";
    }
}