namespace ds_rca.data.entities;

public record Session
{
    public required string PostData;
    public required string CsrfToken;
    public required string SessionTracker;
}