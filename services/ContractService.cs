using ds_rca.bot;
using ds_rca.config;
using ds_rca.data.db;
using ds_rca.data.entities;
using ds_rca.data.remote.api;

namespace ds_rca.services;

public class ContractService(PolyscanApi api, RedditGqlApi gqlApi)
{
    public async Task StartAsync()
    {
        while (true)
        {
            try
            {
                var lastId = await Database.GetLastEntityIdAsync();
                var entityIds = await api.GetEntityIdsAsync();

                if (lastId.Length > 0)
                {
                    if (entityIds != null)
                    {
                        var token = await gqlApi.GetTokenAsync();

                        if (token != null)
                        {
                            var isContainsLastId = entityIds.Contains(lastId);

                            if (isContainsLastId)
                            {
                                var lastIdIndex = entityIds.IndexOf(lastId);
                                entityIds = entityIds.Slice(0, lastIdIndex);
                            }

                            entityIds.Reverse();

                            var storefrontIds = await gqlApi.GetStorefrontIdsAsync(token, entityIds);
                            var rcas = new List<Rca>();

                            foreach (var id in storefrontIds)
                            {
                                var rca = await gqlApi.GetRcaAsync(token, id);
                                
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
                    }
                }
                else
                {
                    if (entityIds != null)
                    {
                        if (lastId != entityIds[0])
                        {
                            await Database.SetLastEntityIdAsync(entityIds[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting contracts: {e.Message}");
            }
            
            Thread.Sleep(1000);
        }
    }
}