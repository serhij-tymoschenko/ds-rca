using ds_rca.bot;
using ds_rca.data.db;
using ds_rca.data.entities;
using ds_rca.data.remote.api;

namespace ds_rca.services;

public class ContractService(PolyscanApi api, RedditGqlApi gqlApi)
{
    public async Task StartAsync()
    {
        var previousEntityIds = new List<string>();
        while (true)
        {
            try
            {
                var lastId = await Database.GetLastEntityIdAsync();
                var entityIds = await api.GetEntityIdsAsync();
                entityIds = previousEntityIds
                    .Concat(entityIds)
                    .ToList();
                if (entityIds == null) throw new Exception("No entityIds fetched");

                if (lastId.Length > 0)
                {
                    var token = await gqlApi.GetTokenAsync();
                    if (token == null) throw new Exception("Token not generated");

                    var isContainsLastId = entityIds.Contains(lastId);
                    if (isContainsLastId)
                    {
                        var lastIdIndex = entityIds.IndexOf(lastId);
                        entityIds = entityIds.Slice(0, lastIdIndex);
                    }

                    entityIds.Reverse();
                    var storefrontIds = await gqlApi.GetStorefrontIdsAsync(token, entityIds);
                    if (storefrontIds == null) throw new Exception("No storefrontIds fetched");

                    previousEntityIds = new List<string>();
                    var rcas = new List<Rca>();
                    foreach (var idIndexed in storefrontIds.Select((id, index) => new {id, index}))
                    {
                        var rca = await gqlApi.GetRcaAsync(token, idIndexed.id);
                        if (rca == null) previousEntityIds.Add(entityIds[idIndexed.index]);
                        if (rca != null) rcas.Add((Rca)rca);
                    }
                    
                    rcas.ForEach(rca =>
                    {
                        Bot.PostRcaAsync(rca, MessageType.CONTRACT);
                        var storefrontId = rca.ShopUrl.Substring(rca.ShopUrl.LastIndexOf('/') + 1);
                        Database.AddStorefrontAsync(storefrontId);
                    });

                    entityIds.Reverse();
                    if (rcas.Count > 0) Database.SetLastEntityIdAsync(entityIds[0]);
                }
                else if (lastId != entityIds[0])
                {
                    await Database.SetLastEntityIdAsync(entityIds[0]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting contracts: {e.Message}");
            }
            
            Thread.Sleep(2400);
        }
    }
}