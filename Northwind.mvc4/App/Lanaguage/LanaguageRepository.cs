using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace AppCore.Lanaguage
{
    public class LanguageRepository<TLanguage> where TLanguage : ILanguage
    {
        private readonly string _connectionString;

        #region Contructors and Destructors
        public LanguageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD methods
        public void Insert(ILanguage lang)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@code", lang.Code},
                    {"@name_en", lang.Name_EN},
                    {"@name_native", lang.Name_Native},
                    {"@iso6392", lang.ISO6391},
                    {"@iso6391", lang.ISO6391},
                    {"@comments", lang.Comments}
                };

                SqlHelper.ExecuteNonQuery(conn,
                    @"INSERT INTO languages (code,name_en,name_native,iso6392,iso6391,comments) 
                        VALUES (@code,@name_en,@name_native,@iso6392,@iso6391,@comments)",
                    parameters);

            }
        }

        #endregion

        #region Queries
        public IQueryable<TLanguage> GetLanguages()
        {
            var langs = new List<TLanguage>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT code,name_en,name_native,iso6391,iso6392,comments
                        FROM languages"
                    , null);

                while (reader.Read())
                {
                    var lang = (TLanguage)Activator.CreateInstance(typeof(TLanguage));
                    lang.Code = reader["code"].ToString();
                    lang.Name_EN = reader["name_en"].ToString();
                    lang.Name_Native = reader["name_native"].ToString();
                    lang.ISO6392 = reader["iso6391"].ToString();
                    lang.ISO6391 = reader["iso6392"].ToString();
                    lang.Comments = reader["comments"].ToString();
                    langs.Add(lang);
                }

            }
            return langs.AsQueryable();
        }
        #endregion
    }
}