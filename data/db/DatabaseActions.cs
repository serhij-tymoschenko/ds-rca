using Google.Cloud.Firestore;

namespace ds_rca.data.db;

public class DatabaseActions(FirestoreDb db)
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

    public async Task AddStorefrontAsync(string storefrontId)
    {
        var docRef = db
            .Collection("storefronts")
            .Document(storefrontId);
        await docRef.SetAsync(new { });
    }

    public async Task DeleteStorefrontAsync(string storefrontId)
    {
        var docRef = db
            .Collection("storefronts")
            .Document(storefrontId);
        await docRef.DeleteAsync();
    }

    public async Task AddUserToNotifyAsync(string storefrontId, ulong serverId, ulong userId)
    {
        var docRef = db
            .Collection("rca")
            .Document(storefrontId)
            .Collection(serverId.ToString())
            .Document(userId.ToString());
        await docRef.SetAsync(new { });
    }

    public async Task<List<ulong>> GetUsersToNotifyAsync(string storefrontId, ulong serverId)
    {
        IAsyncEnumerable<DocumentReference> docRef = db
            .Collection("rca")
            .Document(storefrontId)
            .Collection(serverId.ToString())
            .ListDocumentsAsync();

        var userRefs = docRef.GetAsyncEnumerator();
        var userIds = new List<ulong>();
        while (await userRefs.MoveNextAsync()) userIds.Add(UInt64.Parse(userRefs.Current.Id));

        return userIds;
    }

    public async Task ConfigAsync(ulong serverId, string rcaChannelId, string contractChannelId)
    {
        var docRef = db
            .Collection("servers")
            .Document(serverId.ToString());

        var data = new Dictionary<string, string>
        {
            { "rca", rcaChannelId },
            { "contract", contractChannelId }
        };

        await docRef.SetAsync(data);
    }

    public async Task<List<(ulong Rca, ulong Contract, ulong Server)>> GetServerConfigsAsync()
    {
        IAsyncEnumerable<DocumentReference> docRef = db
            .Collection("servers")
            .ListDocumentsAsync();

        var docsRefs = docRef.GetAsyncEnumerator();
        var ids = new List<(ulong Rca, ulong Contract, ulong Server)>();

        while (await docsRefs.MoveNextAsync())
        {
            var snapshot = await docsRefs.Current.GetSnapshotAsync();
            var rcaId = snapshot.GetValue<string>("rca_channel_id");
            var contractId = snapshot.GetValue<string>("contract_channel_id");

            ids.Add((ulong.Parse(rcaId), ulong.Parse(contractId), ulong.Parse(docsRefs.Current.Id)));
        }

        return ids;
    }
}