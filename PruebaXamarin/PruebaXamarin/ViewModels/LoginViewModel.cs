using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PruebaXamarin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruebaXamarin.ViewModels
{
    public class LoginViewModel : EstadosModel
    {
     
        #region Commands
        public INavigation Navigation { get; set; }
        public ICommand LoginCommand { get; set; }

        #endregion

        #region Propiedades
        private Data _user = new Data();
        private LoginModel _loginUser = new LoginModel();

        public Data User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(nameof(User)); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }
        #endregion

        public LoginViewModel()
        {
            LoginCommand = new Command(Login);
        }

        public async void Login()
        {
            IsBusy = true;
            try
            {

                if (User.UserName == null)
                {
                    IsBusy = false;
                    Message = "Debe ingresar un usuario";
                    return;
                }

                if (User.Password == null)
                {
                    IsBusy = false;
                    Message = "Debe ingresar una clave";
                    return;
                }

                using (HttpClient clientHttp = new HttpClient())
                {
                    Uri requestUri = new Uri("https://serveless.proximateapps-services.com.mx/catalog/dev/webadmin/authentication/login");

                    //creacion del json
                    JObject dynamicJson = new JObject
                      {
                          { "correo", User.UserName },
                          { "contrasenia",  User.Password }
                      };

                    //consumo del web service
                    HttpResponseMessage respon = await clientHttp.PostAsync(requestUri, new StringContent(dynamicJson.ToString(), Encoding.UTF8, "application/json"));
                    var responJsonText = await respon.Content.ReadAsStringAsync();

                    if (respon.IsSuccessStatusCode)
                    {
                        _loginUser = JsonConvert.DeserializeObject<LoginModel>(responJsonText);

                        if (_loginUser.Success == false)
                        {
                            IsBusy = false;
                            await App.Current.MainPage.DisplayAlert("Alerta", _loginUser.Message, "Ok");
                            return;
                        }

                    }
                    else
                    {
                        IsBusy = false;
                        await App.Current.MainPage.DisplayAlert("Error", "Error en la Conexión con el Servidor", "Ok");
                        return;
                    }

                }

                await Navigation.PushAsync(new PerfilPage(_loginUser.Token));

            }
            catch (Exception e)
            {
                IsBusy = false;
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
        }
    }
}
