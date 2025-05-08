using System.Text.RegularExpressions;
using ds_rca.bot;
using ds_rca.config;
using ds_rca.utils.constants;

namespace ds_rca.data.remote.api;

public class RedditApi(HttpClient client)
{
    public async Task<List<string>?> GetStorefrontIdsAsync()
    {
        var query = "shop-gallery-data-fetcher?" +
                    "sort=RELEASE_TIME_REVERSE" +
                    "&categoryName=Recently+released";
        var reqMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(ApiConstants.REDDIT_SHREDDIT_ENDPOINT + query),
            Headers =
            {
                { "Accept", "text/vnd.reddit.partial+html, text/html;q=0.9" },
                {
                    "Authorization",
                    $"bearer {Config.REDDIT_API_KEY}"
                },
                { "User-Agent", "Rca" }
            }
        };

        try
        {
            var resMessage = await client.SendAsync(reqMessage);
            Bot.Log(resMessage.ToString());
            resMessage.EnsureSuccessStatusCode();
            var resContent = await resMessage.Content.ReadAsStringAsync();
            
            var storefrontIds = Regex
                .Matches(resContent, ApiConstants.STOREFRONT)
                .ToList()
                .ConvertAll(match => match.Groups[1].Value)
                .ToHashSet()
                .ToList();

            if (storefrontIds.Count > 0)
            {
                return storefrontIds;
            }
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting storefront ids: {e.Message}");
        }
        
        return null;
    }
}