using Google.Cloud.Firestore;

namespace ds_rca.data.db.firestore;

public class DatabaseActions(FirestoreDb db)
{
    public async Task SetLastRcaIdAsync(string id)
    {
        var docRef = db
            .Collection("config")
            .Document("rca");

        var data = new Dictionary<string, string>
        {
            { "last", id }
        };

        await docRef.SetAsync(data);
    }

    public async Task<string> GetLastRcaIdAsync()
    {
        var docRef = db
            .Collection("config")
            .Document("rca");

        var snapshot = await docRef.GetSnapshotAsync();

        string? value;
        snapshot.TryGetValue<string>("last", out value);
        return value ?? "";
    }

    public async Task SetLastContractIdAsync(string id)
    {
        var docRef = db
            .Collection("config")
            .Document("contract");

        var data = new Dictionary<string, string>
        {
            { "last", id }
        };

        await docRef.SetAsync(data);
    }

    public async Task<string> GetLastContractIdAsync()
    {
        var docRef = db
            .Collection("config")
            .Document("contract");

        var snapshot = await docRef.GetSnapshotAsync();

        string? value;
        snapshot.TryGetValue<string>("last", out value);
        return value ?? "";
    }

    public async Task AddRcaAsync(string storefrontId)
    {
        var docRef = db
            .Collection("rca")
            .Document(storefrontId);
        await docRef.SetAsync(new { });
    }

    public async Task DeleteRcaAsync(string storefrontId)
    {
        var docRef = db
            .Collection("rca")
            .Document(storefrontId);
        await docRef.DeleteAsync();
    }

    public async Task AddWishlisterAsync(string storefrontId, ulong serverId, ulong wishlisterId)
    {
        var docRef = db
            .Collection("rca")
            .Document(storefrontId)
            .Collection(serverId.ToString())
            .Document(wishlisterId.ToString());
        await docRef.SetAsync(new { });
    }

    public async Task<List<string>> GetWishlistersAsync(string storefrontId, ulong serverId)
    {
        IAsyncEnumerable<DocumentReference> docRef = db
            .Collection("rca")
            .Document(storefrontId)
            .Collection(serverId.ToString())
            .ListDocumentsAsync();

        var wishlisterRefs = docRef.GetAsyncEnumerator();
        var wishlisterIds = new List<string>();
        while (await wishlisterRefs.MoveNextAsync()) wishlisterIds.Add(wishlisterRefs.Current.Id);

        return wishlisterIds;
    }

    public async Task SetServerConfigAsync(ulong serverId, string rcaChannelId, string contractChannelId)
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
        var idsList = new List<(ulong Rca, ulong Contract, ulong Server)>();

        while (await docsRefs.MoveNextAsync())
        {
            var snapshot = await docsRefs.Current.GetSnapshotAsync();
            var rcaId = snapshot.GetValue<string>("rca");
            var contractId = snapshot.GetValue<string>("contract");

            idsList.Add((ulong.Parse(rcaId), ulong.Parse(contractId), ulong.Parse(docsRefs.Current.Id)));
        }

        return idsList;
    }
}