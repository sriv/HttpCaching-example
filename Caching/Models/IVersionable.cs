namespace Caching.Models
{
    public interface IVersionable
    {
        long Version { get; set; }
    }
}