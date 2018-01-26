using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaXamarin.Models
{
    public class Data
    {
        [PrimaryKey, AutoIncrement]
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime LastSession { get; set; }
        public string Latitude { get; set; }
        public string Longitud { get; set; }
        public string StorageUbication { get; set; }
    }
}
