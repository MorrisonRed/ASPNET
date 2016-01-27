using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AppCore.Region
{
    public class RegionRepository<TRegion> where TRegion : IRegion
    {
        private readonly string _connectionString;

        #region Constructors and Destructors
        public RegionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD Methods
        public bool Add(TRegion region)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TRegion);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var attribute = Attribute.GetCustomAttribute(info, typeof(KeyAttribute)) as KeyAttribute;
                    //do not add [Key] property to insert statement as it is the Primary Key
                    if (attribute == null)
                    {
                        var value = (info.GetValue(region, null) == null) ? DBNull.Value : info.GetValue(region, null);
                        parameters.Add("@" + info.Name, value);
                    }
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                        @"INSERT INTO Region(RegionDescription) 
                        VALUES(@RegionDescription)",
                            parameters);

                return true;
            }
        }
        public Task<bool> AddAsync(TRegion region)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TRegion);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var attribute = Attribute.GetCustomAttribute(info, typeof(KeyAttribute)) as KeyAttribute;
                    //do not add [Key] property to insert statement as it is the Primary Key
                    if (attribute == null)
                    {
                        var value = (info.GetValue(region, null) == null) ? DBNull.Value : info.GetValue(region, null);
                        parameters.Add("@" + info.Name, value);
                    }
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                        @"INSERT INTO Region(RegionDescription) 
                        VALUES(@RegionDescription)",
                            parameters);

                return Task.FromResult<bool>(true); ;
            }
        }
        public bool Update(TRegion region)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TRegion);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var value = (info.GetValue(region, null) == null) ? DBNull.Value : info.GetValue(region, null);
                    parameters.Add("@" + info.Name, value);
                }

                String query = "";
                query = @"UPDATE Region SET RegionDescription=@RegionDescription WHERE RegionID=@RegionID";

                var result = SqlHelper.ExecuteNonQuery(conn, query, parameters);

                return true;
            }
        }
        public Task<bool> UpdateAsync(TRegion region)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TRegion);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var value = (info.GetValue(region, null) == null) ? DBNull.Value : info.GetValue(region, null);
                    parameters.Add("@" + info.Name, value);
                }

                String query = "";
                query = @"UPDATE Region SET RegionDescription=@RegionDescription WHERE RegionID=@RegionID";

                var result = SqlHelper.ExecuteNonQuery(conn, query, parameters);

                return Task.FromResult<bool>(true);
            }
        }
        public bool Delete(TRegion region)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "@ID", region.RegionID }
                };

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"DELETE FROM Region WHERE RegionID = @ID",
                            parameters);

                return true;
            }
        }
        public Task<bool> DeleteAsync(TRegion region)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "@ID", region.RegionID }
                };

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"DELETE FROM Region WHERE RegionID = @ID",
                            parameters);

                return Task.FromResult<bool>(true);
            }
        }
        #endregion

        #region Queries
        public TRegion FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id }
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT RegionID,RegionDescription FROM Region WHERE RegionID=@ID",
                    parameters);

                var region = (TRegion)Activator.CreateInstance(typeof(TRegion));
                while (reader.Read())
                {
                    region.RegionID = (int)reader["RegionID"];
                    region.RegionDescription = reader["RegionDescription"].ToString();                  
                }
                return region;
            }
        }
        public IQueryable<TRegion> GetRegions()
        {
            var regions = new List<TRegion>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT RegionID,RegionDescription FROM Region"
                    , null);

                while (reader.Read())
                {
                    var region = (TRegion)Activator.CreateInstance(typeof(TRegion));
                    region.RegionID = (int)reader["RegionID"];
                    region.RegionDescription = reader["RegionDescription"].ToString();
                    regions.Add(region);
                }
            }
            return regions.AsQueryable();
        }
        #endregion
    }
}