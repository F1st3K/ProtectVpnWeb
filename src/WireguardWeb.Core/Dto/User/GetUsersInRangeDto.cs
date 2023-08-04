namespace WireguardWeb.Core.Dto.User;

public sealed class GetUsersInRangeDto : IDto
{
    public int StartIndex { get; set; }
    
    public int Count { get; set; }
    
    public bool IsValid()
    {
        if (StartIndex < 0 || Count <= 0)
            return false;
        return true;
    }
}