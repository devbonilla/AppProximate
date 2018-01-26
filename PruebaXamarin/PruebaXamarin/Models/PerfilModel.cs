using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaXamarin.Models
{
    public class PerfilModel: IResponse
    {

        private List<UsuarioData> data;
        public List<UsuarioData> Data
        {
            get { return data; }
            set { data = value; }
        }

    }
}
