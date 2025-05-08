using ds_rca.bot;
using ds_rca.config;
using ds_rca.data.remote.dto;
using ds_rca.utils.constants;
using Newtonsoft.Json;

namespace ds_rca.data.remote.api;

public class PolyscanApi(HttpClient client)
{
    private readonly string _query =
        $"api?" +
        $"module=account" +
        $"&action=txlist" +
        $"&address={ApiConstants.REDDIT_DEPLOYER_ADDRESS}" +
        $"&page=1" +
        $"&offset={Config.POLYSCAN_OFFSET}" +
        $"&sort=desc" +
        $"&apikey={Config.POLYSCAN_API_KEY}";

    public async Task<List<string>?> GetEntityIdsAsync()
    {
        try
        {
            var reqMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ApiConstants.POLYSCAN_API_ENDPOINT + _query),
                Headers =
                {
                    { "User-Agent", ApiConstants.USER_AGENT },
                    { "Accept", "application/json" }
                }
            };

            var resMessage = await client.SendAsync(reqMessage);
            resMessage.EnsureSuccessStatusCode();

            var resContent = await resMessage.Content.ReadAsStringAsync();
            var polyscan = JsonConvert.DeserializeObject<PolyscanDto>(resContent);

            if (polyscan != null)
            {
                var entityIds = polyscan.Result
                    .Where(transaction => transaction.MethodId == ApiConstants.CREATE_METHOD_ID)
                    .ToList()
                    .ConvertAll(transaction =>
                    {
                        var entityId = $"nft_eip155:137_{transaction.ContractAddress.Substring(2)}_0";
                        return entityId;
                    });
                return entityIds;
            }
        }
        catch (Exception e)
        {
            Bot.Log($"Error getting entity ids: {e.Message}");
        }

        return null;
    }
}