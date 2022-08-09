namespace RC.CA.Application.Settings;

public class CacheSettings
{
    public bool Enabled { get; set; } = true;
    public int SlidingExpiration { get; set; } = 2;
}
