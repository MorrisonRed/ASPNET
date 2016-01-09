using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace AppCore.Category
{
    public class CategoryRepository<TCategory> where TCategory : ICategory
    {
        private readonly string _connectionString;

        #region Constructor and Destructors
        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD Methods
        public bool Add(TCategory cat)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Name", cat.CategoryName},
                    {"@Desc", cat.Description }
                };
                if (cat.Picture != null) parameters.Add("@Picture", cat.Picture);

                String query;
                if (cat.Picture != null)
                    query = "INSERT INTO Categories(CategoryName,Description,Picture) VALUES (@Name,@Desc,@Picture)";
                else
                    query = "INSERT INTO Categories(CategoryName,Description) VALUES (@Name,@Desc)";

                var reader = SqlHelper.ExecuteNonQuery(conn, query, parameters);

                return true;
            }
        }
        public bool Update(TCategory cat)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", cat.CategoryId},
                    {"@Name", cat.CategoryName},
                    {"@Desc", cat.Description },
                    {"@Picture", cat.Picture }
                };

                String query;
                if(cat.Picture != null)
                    query = "UPDATE Categories SET CategoryName=@Name,Description=@Desc,Picture=@Picture WHERE CategoryID = @ID";
                else
                    query = "UPDATE Categories SET CategoryName=@Name,Description=@Desc,Picture = NULL WHERE CategoryID = @ID";

                var reader = SqlHelper.ExecuteNonQuery(conn, query, parameters);

                return true;
            }
        }
        public bool Delete(TCategory cat)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@ID", cat.CategoryId }
                };

                SqlHelper.ExecuteScalar(conn, CommandType.Text,
                    @"DELETE FROM Categories WHERE CategoryID = @ID", parameters);

                return true;
            }
        }
        #endregion

        #region Queries
        public TCategory FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id}
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT CategoryID,CategoryName,Description,Picture
                        FROM Categories WHERE CategoryID=@ID",
                    parameters);

                var cat = (TCategory)Activator.CreateInstance(typeof(TCategory));
                while (reader.Read())
                {                 
                    cat.CategoryId = (int)reader["CategoryID"];
                    cat.CategoryName = reader["CategoryName"].ToString();
                    cat.Description = reader["Description"].ToString();
                    cat.Picture = reader["Picture"] == System.DBNull.Value ? null : (byte[])reader["Picture"];
                }
                return cat;
            }            
        }

        public IQueryable<TCategory> GetCategories()
        {
            var cats = new List<TCategory>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT CategoryID,CategoryName,Description,Picture
                        FROM Categories"
                    , null);

                while (reader.Read())
                {
                    var cat = (TCategory)Activator.CreateInstance(typeof(TCategory));
                    cat.CategoryId = (int)reader["CategoryID"];
                    cat.CategoryName = reader["CategoryName"].ToString();
                    cat.Description = reader["Description"].ToString();
                    cat.Picture = reader["Picture"] == System.DBNull.Value ? null : (byte[])reader["Picture"]; 
                    cats.Add(cat);
                }
            }
            return cats.AsQueryable();
        }
        #endregion
    }
}