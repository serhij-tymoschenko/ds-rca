using ds_rca.bot;
using ds_rca.data.db;
using ds_rca.data.entities;
using ds_rca.data.remote.api;
using ds_rca.utils;
using Microsoft.Extensions.Logging;

namespace ds_rca.services;

public class RcaService(RedditApi api, RedditGqlApi gqlApi)
{
    public async Task StartAsync()
    {
        var token = await gqlApi.GetTokenAsync();
        while (true)
        {
            try
            {
                Bot.Log("Rca service started");
                var lastId = await Database.GetLastStorefrontIdAsync();
                Bot.Log(lastId);
                var storefrontIds = await api.GetStorefrontIdsAsync();
                Bot.Log(storefrontIds.Count.ToString());
                if (storefrontIds == null) throw new Exception("No storefrontIds fetched");

                if (lastId.Length > 0)
                {
                    if (token == null) throw new Exception("Token not generated");

                    var lastIdIndex = storefrontIds.IndexOf(lastId);
                    var localStorefrontIds = new List<string>();
                    if (lastIdIndex != -1)
                    {
                        for (var i = 0; i < lastIdIndex; i++)
                        {
                            localStorefrontIds.Add(storefrontIds[i]);
                        }
                    }
                    
                    
                    localStorefrontIds.Reverse();
                    var rcas = new List<Rca>();
                    foreach (var id in localStorefrontIds)
                    {
                        var rca = await gqlApi.GetRcaAsync(token, id);
                        if (rca != null) rcas.Add((Rca)rca);
                    }

                    rcas.ForEach(rca =>
                    {
                        Bot.PostRcaAsync(rca, MessageType.RCA);
                    });

                    
                    if (localStorefrontIds.Count > 0)
                    {
                        await Database.SetLastStorefrontIdAsync(localStorefrontIds[^1]);
                    }
                }
                else if (lastId != storefrontIds[0])
                {
                    await Database.SetLastStorefrontIdAsync(storefrontIds[0]);
                }
            }
            catch (Exception e)
            {
                if (e is AuthException) token = await gqlApi.GetTokenAsync();
                Bot.Log($"Error getting rcas: {e.Message}");
            }

            Thread.Sleep(1200);
        }
    }
}