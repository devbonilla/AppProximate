using PruebaXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruebaXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        int maxLength = 30;
        private LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginViewModel();
            viewModel.Navigation = this.Navigation;

            ////restriccion de largo de cadena
            UserEntry.TextChanged += EntryUser_TextChanged;
            PasswordEntry.TextChanged += EntryPass_TextChanged;

            btnEntraUsuario.Clicked += BtnEntraUsuario_Clicked;
        }

        private void BtnEntraUsuario_Clicked(object sender, EventArgs e)
        {
            //Temporal para autenticacion rapida
            UserEntry.Text = "prueba@proximateapps.com";
            PasswordEntry.Text = "12digo16digo18#$";
        }

        #region Eventos

        private void EntryPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(sender, e);
        }

        private void EntryUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(sender, e);
        }

        #endregion

        #region "Metodos"

        /// <summary>
        /// Metodo para validar el maximo de caracteres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTextChanged(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            String val = entry.Text;

            if (val.Length > maxLength)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }

        #endregion
    
    }
}