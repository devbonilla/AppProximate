using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaXamarin.Models
{
    public class IResponse
    {
        private Boolean success;
        private Boolean error;
        private String message;

        public Boolean Success
        {
            get { return success; }
            set { success = value; }
        }

        public Boolean Error
        {
            get { return error; }
            set { error = value; }
        }

        public String Message
        {
            get { return message; }
            set { message = value;  }
        }

    }
}
