using PruebaXamarin.Droid;
using PruebaXamarin.Interface;
using SQLite.Net.Interop;
using Xamarin.Forms;

[assembly: Dependency(typeof(PruebaXamarin.Droid.Config))]
namespace PruebaXamarin.Droid
{
    class Config : IConfiguration
    {
        private string directorio;
        private ISQLitePlatform plataforma;
        public string Directorio
        {
            get
            {
                if (string.IsNullOrEmpty(directorio))
                {
                    directorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                }
                return directorio;
            }
        }

        public ISQLitePlatform Plataforma
        {
            get
            {
                if (plataforma==null)
                {
                    plataforma = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                }
                return plataforma;
            }

        }
    }
}