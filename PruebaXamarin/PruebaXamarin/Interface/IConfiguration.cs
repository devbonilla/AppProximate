using SQLite.Net.Interop;

namespace PruebaXamarin.Interface
{
    public interface IConfiguration
    {
        string Directorio { get; }
        ISQLitePlatform Plataforma { get; }
    }
}
