using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace AppCore.Country
{
    public class CountryRepository<TCountry> where TCountry : ICountry
    {
        private readonly string _connectionString;

        #region Contructors and Destructors
        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD methods
        public void Insert(TCountry country)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Code", country.Code},
                    {"@Name", country.Name},
                    {"@Continent", country.Continent},
                    {"@Region", country.Region},
                    {"@SurfaceArea", country.SurfaceArea},
                    {"@IndepYear", country.IndepYear},
                    {"@Population", country.Population},
                    {"@LifeExpectancy", country.LifeExpectancy},
                    {"@GNP", country.GNP},
                    {"@GNPOld", country.GNPOld},
                    {"@LocalName", country.LocalName},
                    {"@GovernmentForm", country.GovernmentForm},
                    {"@HeadOfState", country.HeadOfState},
                    {"@Capital", country.Capital},
                    {"@Code2", country.Code2 }
                };

                SqlHelper.ExecuteNonQuery(conn,
                    @"INSERT INTO languages (Code,Name,Continent,Region,SurfaceArea,IndepYear,Population,LifeExpectancy,
                        GNP,GNPOld,LocalName,GovernmentForm,HeadOfState,Capital,Code2) 
                        VALUES (@Code,@Name,@Continent,@Region,@SurfaceArea,@IndepYear,@Population,
                        @LifeExpectancy,@GNP,@GNPOld,@LocalName,@GovernmentForm,@HeadOfState,@Code2)",
                    parameters);

            }
        }

        #endregion

        #region Queries
        public IQueryable<TCountry> GetCountries()
        {
            var countries = new List<TCountry>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT Code,Name,Continent,Region,SurfaceArea,IndepYear,Population,LifeExpectancy,
                        GNP,GNPOld,LocalName,GovernmentForm,HeadOfState,Capital,Code2
                        FROM country"
                    , null);

                while (reader.Read())
                {
                    var country = (TCountry)Activator.CreateInstance(typeof(TCountry));
                    country.Code = reader["code"].ToString();
                    country.Code = reader["Code"].ToString();
                    country.Name = reader["Name"].ToString();
                    country.Continent = reader["Continent"].ToString();
                    country.Region = reader["Region"].ToString();
                    country.SurfaceArea =Convert.ToDecimal(reader["SurfaceArea"]);
                    if (reader["IndepYear"] != System.DBNull.Value) country.IndepYear = Convert.ToInt16(reader["IndepYear"]); 
                    country.Population = (int)reader["Population"];
                    if (reader["LifeExpectancy"] != System.DBNull.Value) country.LifeExpectancy = Convert.ToDecimal(reader["LifeExpectancy"]);
                    if (reader["GNP"] != System.DBNull.Value) country.GNP = Convert.ToDecimal(reader["GNP"]);
                    if (reader["GNPOld"] != System.DBNull.Value) country.GNPOld = Convert.ToDecimal(reader["GNPOld"]);
                    country.LocalName = reader["LocalName"].ToString();
                    country.GovernmentForm = reader["GovernmentForm"].ToString();
                    country.HeadOfState = reader["HeadOfState"].ToString();
                    if (reader["Capital"] != System.DBNull.Value) country.Capital = (int)reader["Capital"];
                    country.Code2 = reader["Code2"].ToString();
                    countries.Add(country);
                }

            }
            return countries.AsQueryable();
        }
        #endregion
    }
}