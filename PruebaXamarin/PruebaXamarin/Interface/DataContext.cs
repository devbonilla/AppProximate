using PruebaXamarin.Interface;
using PruebaXamarin.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace PruebaXamarin
{
    class DataContext : IDisposable
    {
        private SQLiteConnection connection;

        public DataContext()
        {
            var config = DependencyService.Get<IConfiguration>();
            connection = new SQLiteConnection(config.Plataforma, Path.Combine(config.Directorio, "AppPrueb.db3"));
            connection.CreateTable<Data>();
        }

        /// <summary>
        /// Método para insertar Usuario
        /// </summary>
        /// <param name="user">Objeto User</param>
        public void insertUser(Data user)
        {
            connection.Insert(user);
        }

        /// <summary>
        /// Método para Actualizar un Usuario
        /// </summary>
        /// <param name="user">Objeto User</param>
        public void updateUser(Data user)
        {
            connection.Update(user);
        }

        /// <summary>
        /// Método para Eliminar un Usuario
        /// </summary>
        /// <param name="user">Objeto User</param>
        public void deleteUser(Data user)
        {
            connection.Delete(user);
        }

        /// <summary>
        /// Métódo para buscar un Usuario por Número de Identificación
        /// </summary>
        /// <param name="IdentificationNumber">IdentificationNumber</param>
        /// <returns>Users</returns>
        public Data GetUser(int IdentificationNumber)
        {
            return connection.Table<Data>().FirstOrDefault(c => c.IdUser == IdentificationNumber);
        }

        /// <summary>
        /// Método para Listar todos los usuarios
        /// </summary>
        /// <returns>List<Users></returns>
        public List<Data> ListUser()
        {
            return connection.Table<Data>().ToList();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
