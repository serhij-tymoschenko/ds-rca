using ds_rca.bot;
using ds_rca.data.db;
using ds_rca.data.entities;
using ds_rca.data.remote.api;
using ds_rca.utils;

namespace ds_rca.services;

public class RcaService(RedditApi api, RedditGqlApi gqlApi)
{
    private string lastId = "";
    private List<string> storefrontIds = new();

    private void SetLastId(string id)
    {
        lock (lastId)
        {
            lastId = id;
        }
    }

    private string GetLastId()
    {
        lock (lastId)
        {
            return lastId;
        }
    }

    private void SetStorefrontIds(List<string> newStorefrontIds)
    {
        lock (storefrontIds)
        {
            if (!storefrontIds.SequenceEqual(newStorefrontIds)) storefrontIds = newStorefrontIds;
        }
    }

    private List<string> GetStorefrontIds()
    {
        lock (storefrontIds)
        {
            return storefrontIds;
        }
    }

    private async Task StartMainPageFetching()
    {
        var session = await api.GetSession();
        while (true)
        {
            try
            {
                if (session == null) throw new Exception("Session is null");
                var storefrontIds = await api.GetMainPageStorefrontIds(session);
                if (storefrontIds == null) throw new Exception("No storefrontIds fetched");

                var lastIdIndex = storefrontIds.IndexOf(GetLastId());
                if (lastIdIndex != -1)
                {
                    var localStorefrontIds = new List<string>();
                    for (var i = 0; i < lastIdIndex; i++) localStorefrontIds.Add(storefrontIds[i]);
                    SetStorefrontIds(localStorefrontIds);
                }
            }
            catch (Exception e)
            {
                if (e is AuthException) session = await api.GetSession();
                Bot.Log($"Error fetching main page: {e.Message}");
            }

            Thread.Sleep(1200);
        }
    }

    private async Task StartCategoryFetching()
    {
        while (true)
        {
            try
            {
                var storefrontIds = await api.GetStorefrontIdsAsync();
                if (storefrontIds == null) throw new Exception("No storefrontIds fetched");

                var lastIdIndex = storefrontIds.IndexOf(GetLastId());
                if (lastIdIndex != -1)
                {
                    var localStorefrontIds = new List<string>();
                    for (var i = 0; i < lastIdIndex; i++) localStorefrontIds.Add(storefrontIds[i]);
                    SetStorefrontIds(localStorefrontIds);
                }
            }
            catch (Exception e)
            {
                Bot.Log($"Error fetching category: {e.Message}");
            }

            Thread.Sleep(2400);
        }
    }


    public async Task StartAsync()
    {
        var initialLastId = await Database.GetLastStorefrontIdAsync();
        SetLastId(initialLastId);

        StartMainPageFetching();
        StartCategoryFetching();

        var token = await gqlApi.GetTokenAsync();
        while (true)
        {
            try
            {
                var storefrontIds = GetStorefrontIds();
                if (token == null) throw new Exception("Token not generated");

                var rcas = new List<Rca>();
                foreach (var id in storefrontIds)
                {
                    var rca = await gqlApi.GetRcaAsync(token, id);
                    if (rca != null) rcas.Add((Rca)rca);
                }

                rcas.ForEach(rca => { Bot.PostRcaAsync(rca, MessageType.RCA); });

                if (storefrontIds.Count > 0)
                {
                    Database.SetLastStorefrontIdAsync(storefrontIds[^1]);
                    SetLastId(storefrontIds[^1]);
                }
            }
            catch (Exception e)
            {
                if (e is AuthException) token = await gqlApi.GetTokenAsync();
                Bot.Log($"Error getting rcas: {e.Message}");
            }

            Thread.Sleep(2400);
        }
    }
}