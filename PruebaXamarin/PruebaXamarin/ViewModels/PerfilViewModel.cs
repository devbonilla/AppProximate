using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PruebaXamarin.Helpers;
using PruebaXamarin.Models;
using PruebaXamarin.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruebaXamarin.ViewModels
{
    public class PerfilViewModel : EstadosModel
    {
        #region "Variables"
        public INavigation Navigation { get; set; }
        public ICommand GrabarUsuarioCommand { get; set; }
        public ICommand CerrarSesionCommand { get; set; }
        public ICommand CapturarFotoCommand { get; set; }

        List<Data> _listUsers = new List<Data>();
        private PerfilModel _returnLogin = new PerfilModel();
        private Data objUser = new Data();

        private string _message;
        private string _latitud;
        private string _longitud;
        private ImageSource _photoUser;
        private string _storageDirectory;
        #endregion

        #region Propiedades
        
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        public string Latitud
        {
            get { return _latitud; }
            set { _latitud = value; OnPropertyChanged(nameof(Latitud)); }
        }

        public string Longitud
        {
            get { return _longitud; }
            set { _longitud = value; OnPropertyChanged(nameof(Longitud)); }
        }

        public List<Data> ListUsers
        {
            get { return _listUsers; }
            set { _listUsers = value; OnPropertyChanged(nameof(ListUsers)); }
        }

        public ImageSource PhotoUser
        {
            get { return _photoUser; }
            set { _photoUser = value; OnPropertyChanged(nameof(PhotoUser)); }
        }

        public string StorageDirectory
        {
            get { return _storageDirectory; }
            set { _storageDirectory = value;  OnPropertyChanged(nameof(StorageDirectory)); }
        }

         //Datos de Usuario
        private string _nameUser;
        public string NameUser
        {
            get { return _nameUser; }
            set { _nameUser = value; OnPropertyChanged(nameof(NameUser)); }
        }

        private string _lastNameUser;
        public string LastNameUser
        {
            get { return _lastNameUser; }
            set { _lastNameUser = value; OnPropertyChanged(nameof(LastNameUser)); }
        }

        private string _identificationType;
        public string IdentificationType
        {
            get { return _identificationType; }
            set { _identificationType = value; OnPropertyChanged(nameof(IdentificationType)); }
        }

        private string _identificationNumber;
        public string IdentificationNumber
        {
            get { return _identificationNumber; }
            set { _identificationNumber = value; OnPropertyChanged(nameof(IdentificationNumber )); }
        }

        private string _emailUser;
        public string EmailUser
        {
            get { return _emailUser; }
            set { _emailUser = value; OnPropertyChanged(nameof(EmailUser)); }
        }

        #endregion

        public PerfilViewModel(string TokenUser)
        {
            CerrarSesionCommand = new Command(CerrarSesion);
            CapturarFotoCommand = new Command(CapturarFoto);
            GrabarUsuarioCommand = new Command(GrabarUsuario);
            
            DatosUsuario(TokenUser);

        }


        #region MetodosPrivados

        /// <summary>
        /// Método para consultar los datos de usuario y ponerlos en el perfil
        /// </summary>
        /// <param name="TokenUser">Token del login</param>
        private async void DatosUsuario(string TokenUser)
        {
            using (HttpClient clientHttp = new HttpClient())
            {
                Uri requestUri = new Uri("https://serveless.proximateapps-services.com.mx/catalog/dev/webadmin/users/getdatausersession");

                //creacion del json
                JObject dynamicJson = new JObject
                     {
                            { "Authorization", TokenUser }
                     };
                var httpContent = new StringContent(dynamicJson.ToString(), Encoding.UTF8, "application/json");
                clientHttp.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TokenUser);

                HttpResponseMessage respon = await clientHttp.PostAsync(requestUri, httpContent);


                //Leemos el resultado tal como fue guardado.
                string data = await respon.Content.ReadAsStringAsync();

                if (respon.IsSuccessStatusCode)
                {
                    //Deserializamos el objeto data para obtener el id serializado guardado
                    _returnLogin = JsonConvert.DeserializeObject<PerfilModel>(data);

                    if (_returnLogin.Success == false)
                    {
                        IsBusy = false;
                        await App.Current.MainPage.DisplayAlert("Error", _returnLogin.Message, "Ok");
                        return;
                    }

                    //asigno los valores a grabar
                    IsBusy = true;
                    if (_returnLogin.Data != null && _returnLogin.Data.Count > 0)
                    {
                        foreach (UsuarioData objReturn in _returnLogin.Data)
                        {
                            NameUser = objReturn.nombres.ToString();
                            LastNameUser = objReturn.apellidos.ToString();
                            EmailUser = objReturn.correo.ToString();
                            IdentificationNumber = objReturn.numero_documento.ToString();
                            IdentificationType = objReturn.documentos_abrev.ToString();

                            objUser.Name = objReturn.nombres.ToString();
                            objUser.LastName = objReturn.apellidos.ToString();
                            objUser.IdentificationType = objReturn.documentos_abrev.ToString();
                            objUser.IdentificationNumber = objReturn.numero_documento.ToString();
                            objUser.LastSession = objReturn.ultima_sesion;

                        }

                        IsBusy = false;
                    }

                }
                else
                {
                    IsBusy = false;
                    await App.Current.MainPage.DisplayAlert("Error", "Error en la Conexión con el Servidor", "Ok");
                    return;
                }

            }
        }

        /// <summary>
        /// Método para tomar la foto. Se activa con el botón de cámara
        /// </summary>
        private async void CapturarFoto()
        {
            await CrossMedia.Current.Initialize();


            if (Plugin.Media.CrossMedia.Current.IsTakePhotoSupported && CrossMedia.Current.IsCameraAvailable)
            {
                try
                {

                    var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

                    if (photo != null)
                    {
                        StorageDirectory = photo.AlbumPath;
                        IsBusy = true;
                        PhotoUser = ImageSource.FromStream(() => { return photo.GetStream(); });
                        GrabarLocalizacion();
                    }
                }
                catch (Exception e)
                {
                    IsBusy = false;
                    await App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "Camara no disponible", "Ok");
            }

        }

        private async void GrabarLocalizacion()
        {
            try
            {
                TimeSpan TimeSearch = new TimeSpan(0, 0, 30);
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50; // Precisión deseada (en metros)
                var position = await locator.GetPositionAsync(TimeSearch);
                if (position != null)
                {
                    Latitud = position.Latitude.ToString();
                    Longitud = position.Longitude.ToString();

                    //Almaceno en las variables de Usuario los datos correspondientes a la imagen
                    objUser.Latitude = Latitud;
                    objUser.StorageUbication = StorageDirectory.ToString();
                    objUser.Longitud = Longitud;
                }

            }
            catch (Exception e)
            {
                IsBusy = false;
                await App.Current.MainPage.DisplayAlert("Error Localizacion", e.Message, "Ok");
            }

        }

        private async void GrabarUsuario()
        {
            try
            {
                IsBusy = true;
                using (var datos = new DataContext())
                {

                    Data tmpUser = new Data();
                    tmpUser = datos.GetUser(objUser.IdUser);

                    if (tmpUser != null && tmpUser.IdUser > 0)
                    {

                        //El usuario ya existe, entonces se actualiza
                        datos.updateUser(objUser);
                        await App.Current.MainPage.DisplayAlert("Actualizar", "El Usuario se actualizó correctamente", "Ok");
                        IsBusy = false;
                    }
                    else
                    {
                        // El usuario es nuevo, entonces lo inserto
                        datos.insertUser(objUser);
                        await App.Current.MainPage.DisplayAlert("Actualizar", "El Usuario se insertó correctamente", "Ok");
                        IsBusy = false;

                    }

                }

            }
            catch (Exception e)
            {
                IsBusy = false;
                await App.Current.MainPage.DisplayAlert("No se pudo guardar, se presentó un error", e.Message, "Ok");
            }


        }

        private async void CerrarSesion()
        {
            Settings.IsLoggedIn = false;
            await Navigation.PushAsync(new LoginPage());
        }

        #endregion
    }
}
