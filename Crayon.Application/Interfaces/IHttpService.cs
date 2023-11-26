namespace Crayon.Application.Interfaces
{
    public interface IHttpService
    {
        public Task<T> GetAsync<T>(string url);
    }
}