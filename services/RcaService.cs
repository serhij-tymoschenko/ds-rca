using ds_rca.bot;
using ds_rca.config;
using ds_rca.data.db;
using ds_rca.data.entities;
using ds_rca.data.remote.api;

namespace ds_rca.services;

public class RcaService(RedditApi api, RedditGqlApi gqlApi)
{
    public async Task StartAsync()
    {
        while (true)
        {
            try
            {
                var lastId = await Database.GetLastStorefrontIdAsync();
                var storefrontIds = await api.GetStorefrontIdsAsync();
                
                if (lastId.Length > 0)
                {
                    if (storefrontIds != null)
                    {
                        var token = await gqlApi.GetTokenAsync();

                        if (token != null)
                        {
                            var isContainsLastId = storefrontIds.Contains(lastId);

                            if (isContainsLastId)
                            {
                                var lastIdIndex = storefrontIds.IndexOf(lastId);
                                storefrontIds = storefrontIds.Slice(0, lastIdIndex);
                            }

                            storefrontIds.Reverse();

                            var rcas = new List<Rca>();
                            
                            foreach (var id in storefrontIds)
                            {
                                var rca = await gqlApi.GetRcaAsync(token, id);

                                if (rca != null) rcas.Add((Rca)rca);
                            }

                            rcas.ForEach(rca =>
                            {
                                Bot.PostRcaAsync(rca, MessageType.RCA);
                                var storefrontId = rca.ShopUrl.Substring(rca.ShopUrl.LastIndexOf('/') + 1);
                                Database.DeleteStorefrontAsync(storefrontId);
                            });

                            storefrontIds.Reverse();
                            if (rcas.Count > 0) await Database.SetLastStorefrontIdAsync(storefrontIds[0]);
                        }
                    }
                }
                else
                {
                    if (storefrontIds != null)
                        if (lastId != storefrontIds[0])
                            await Database.SetLastStorefrontIdAsync(storefrontIds[0]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting rcas: {e.Message}");
            }
        }
    }
}