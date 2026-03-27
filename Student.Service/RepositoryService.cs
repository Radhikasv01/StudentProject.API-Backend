using Dapper;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Npgsql;
using Student.Interface;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Student.Service
{

    public class RepositoryService<T> : IRepository<T>
    {
        private string _db;
        public RepositoryService()
        {
            _db = Settings.ConnectionString;
        }
        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_db);
            }
        }

        public bool Delete(string Deletequery, object param)
        {
            using IDbConnection dbConnection = Connection;
            Connection.Open();
            dbConnection.QueryFirstOrDefault(Deletequery, param);
            return true;

        }

        public List<T> GetAllID(string query)
        {
            List<T> obj = new List<T>();
            using (IDbConnection connection = Connection)
            {
                connection.Open();
                obj = connection.Query<T>(query).ToList();
            }
            return obj;
        }

        public IEnumerable<T> GetByFilter(string query, object param)
        {
            IEnumerable<T> obj = new List<T>();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                obj = dbConnection.Query<T>(query, param);
            }
            return obj;
        }

        public T GetById(string getidquery, int Id)
        {
            T obj;
            using (IDbConnection dbConnection = Connection)
            {
                Connection.Open();
                obj = Connection.QueryFirstOrDefault<T>(getidquery, new { Id = Id });
            }
            return obj;
        }

    

        public bool Insert(string Insertquery, object param)
        {
            using IDbConnection connection = Connection;
            connection.Open();
            connection.QueryFirst<bool>(Insertquery, param);
            return true; 
        }

        public bool Update(string Updatequery, object param)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            dbConnection.Execute(Updatequery, param);
            return true;
        }
    }
}
