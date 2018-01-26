using PruebaXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruebaXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {
        private PerfilViewModel viewModel;
        public PerfilPage(string TokenUser)
        {
            InitializeComponent();
            BindingContext = viewModel = new PerfilViewModel(TokenUser);
            viewModel.Navigation = this.Navigation;
        }

    }
}
