using ds_rca.bot;
using Google.Cloud.Firestore;

namespace ds_rca.data.db.modules;

public class ConfigModule(FirestoreDb db)
{
    public async Task ConfigAsync(ulong serverId, string rcaChannelId, string contractChannelId)
    {
        try
        {
            var docRef = db
                .Collection("servers")
                .Document(serverId.ToString());

            var data = new Dictionary<string, string>
            {
                { "rca_channel_id", rcaChannelId },
                { "contract_channel_id", contractChannelId }
            };

            await docRef.SetAsync(data);
        }
        catch (Exception e)
        {
            Bot.Log($"Error configuring {serverId}: {e.Message}");
        }
    }

    public async Task<List<(ulong Rca, ulong Contract, ulong Server)>> GetServerConfigsAsync()
    {
        try
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
        catch (Exception e)
        {
            Bot.Log($"Error getting server ids: {e.Message}");
        }

        return new List<(ulong Rca, ulong Contract, ulong Server)>();
    }
}