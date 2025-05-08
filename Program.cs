using ds_rca.data;

namespace ds_rca;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var redditGqlApi = new RedditGqlApi(httpClient);

        var token = await redditGqlApi.GetTokenAsync();
        if (token != null)
        {
            var entityIds = new List<string> { "nft_eip155:137_df2b3ad0bd1f3a4464177f6ee6adf4fc87da70c8_0",  "nft_eip155:137_aacfe5bec0f13ebd1ee3ebecb5bb366368b54709_0" };
            var storefrontIds = await redditGqlApi.GetStorefrontIdsAsync(token, entityIds);

            if (storefrontIds != null)
            {
                redditGqlApi.GetNftDetailsAsync(token);
            }
        }

        Task.WaitAll(Task.Delay(-1));
    }
}