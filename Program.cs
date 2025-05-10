using System.Net;
using ds_rca.bot;
using ds_rca.data.db;
using ds_rca.data.remote.api;
using ds_rca.services;

namespace ds_rca;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Bot.CreateInstance();
        Database.CreateInstance();

        var cookieContainer = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true
        };
        var client = new HttpClient(handler);
        
        var redditApi = new RedditApi(client, cookieContainer);
        var redditGqlApi = new RedditGqlApi(client);
        var polyscanApi = new PolyscanApi(client);

        var rcaService = new RcaService(redditApi, redditGqlApi);
        var contractService = new ContractService(polyscanApi, redditGqlApi);

        Bot.StartBotAsync();
        rcaService.StartAsync();
        contractService.StartAsync();

        Task.WaitAll(Task.Delay(-1));
    }
}