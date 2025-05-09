using ds_rca.bot;
using ds_rca.data.db;
using ds_rca.data.entities;
using ds_rca.data.remote.api;
using ds_rca.utils;

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
                var lastId = await Database.GetLastStorefrontIdAsync();
                var storefrontIds = await api.GetStorefrontIdsAsync();
                if (storefrontIds == null || storefrontIds.Count == 0) throw new Exception("No storefrontIds fetched");

                if (lastId.Length > 0)
                {
                    if (token == null) throw new Exception("Token not generated");

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
                    });

                    storefrontIds.Reverse();
                    if (storefrontIds.Count > 0) await Database.SetLastStorefrontIdAsync(storefrontIds[0]);
                }
                else if (lastId != storefrontIds[0])
                {
                    await Database.SetLastStorefrontIdAsync(storefrontIds[0]);
                }
            }
            catch (Exception e)
            {
                if (e is AuthException)
                {
                    token = await gqlApi.GetTokenAsync();
                }
                Bot.Log($"Error getting rcas: {e.Message}");
            }

            Thread.Sleep(1200);
        }
    }
}