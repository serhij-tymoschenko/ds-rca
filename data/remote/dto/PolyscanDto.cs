using Newtonsoft.Json;

namespace ds_rca.data.remote.dto;

public class PolyscanDto
{
    [JsonProperty("result")] public List<TransactionResult> Result { get; set; }

    public class TransactionResult
    {
        [JsonProperty("methodId")] public string MethodId { get; set; }

        [JsonProperty("contractAddress")] public string ContractAddress { get; set; }
    }
}