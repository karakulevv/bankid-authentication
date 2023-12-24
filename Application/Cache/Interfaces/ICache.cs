namespace Application.Cache.Interfaces;

public interface ICache
{
    Task Set<T>(string key, T value, TimeSpan time);
    Task Remove(string key);
    Task<T> GetAsync<T>(string key);
}