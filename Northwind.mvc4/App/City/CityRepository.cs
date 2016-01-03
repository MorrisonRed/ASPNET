using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace AppCore.City
{
    public class CityRepository<TCity> where TCity : ICity
    {
        private readonly string _connectionString;

        #region Contructors and Destructors
        public CityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD methods
        public void Insert(TCity city)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", city.ID},
                    {"@Name", city.Name},
                    {"@CountryCode", city.CountryCode},
                    {"@District", city.District},
                    {"@Population", city.Population}
                };

                SqlHelper.ExecuteNonQuery(conn,
                    @"INSERT INTO city (ID,Name,CountryCode,District,Population) 
                        VALUES (@ID,@Name,@CountryCode,@District,@Population)",
                    parameters);

            }
        }

        #endregion

        #region Queries
        public IQueryable<TCity> GetCities()
        {
            var cities = new List<TCity>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT ID,Name,CountryCode,District,Population
                        FROM city"
                    , null);

                while (reader.Read())
                {
                    var city = (TCity)Activator.CreateInstance(typeof(TCity));
                    city.ID = (int)reader["ID"];
                    city.Name = reader["Name"].ToString();
                    city.CountryCode = reader["CountryCode"].ToString();
                    city.District = reader["District"].ToString();
                    city.Population = (int)reader["Population"];
                    cities.Add(city);
                }

            }
            return cities.AsQueryable();
        }
        #endregion
    }
}