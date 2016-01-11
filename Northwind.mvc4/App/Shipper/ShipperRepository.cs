using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;


namespace AppCore.Shipper
{
    public class ShipperRepository<TShipper> where TShipper : IShipper
    {
        public readonly string _connectionString;

        #region Constructors and Destructors
        public ShipperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD Methods
        public bool Add(TShipper shipper)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(Shipper);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var attribute = Attribute.GetCustomAttribute(info, typeof(KeyAttribute)) as KeyAttribute;
                    //do not add [Key] property to insert statement as it is the Primary Key
                    if (attribute == null)
                    {
                        var value = (info.GetValue(shipper, null) == null) ? DBNull.Value : info.GetValue(shipper, null);
                        parameters.Add("@" + info.Name, value);
                    }
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"INSERT INTO Shippers(CompanyName, Phone) VALUES (@CompanyName, @Phone)",
                            parameters);

                return true;
            }
        }
        public bool Update(TShipper shipper)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(Shipper);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var value = (info.GetValue(shipper, null) == null) ? DBNull.Value : info.GetValue(shipper, null);
                    parameters.Add("@" + info.Name, value);
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"UPDATE Shippers SET CompanyName=@CompanyName, Phone=@Phone WHERE ShipperID=@ShipperID",
                            parameters);

                return true;
            }
        }
        public bool Delete(TShipper shipper)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "@ID", shipper.ShipperID }
                };

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"DELETE FROM Shippers WHERE ShipperID = @ID",
                            parameters);

                return true;
            }
        }
        #endregion

        #region Queries
        public TShipper FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id }
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT ShipperID, CompanyName, Phone FROM Shippers 
                        WHERE ShipperID=@ID",
                    parameters);

                var shipper = (TShipper)Activator.CreateInstance(typeof(TShipper));
                while (reader.Read())
                {
                    shipper.ShipperID = (int)reader["ShipperID"];
                    shipper.CompanyName = reader["CompanyName"].ToString();
                    shipper.Phone = reader["Phone"].ToString();
                }
                return shipper;
            }
        }
        public IQueryable<TShipper> GetShippers()
        {
            var shippers = new List<TShipper>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT ShipperID, CompanyName, Phone FROM Shippers"
                    , null);

                while (reader.Read())
                {
                    var shipper = (TShipper)Activator.CreateInstance(typeof(TShipper));
                    shipper.ShipperID = (int)reader["ShipperID"];
                    shipper.CompanyName = reader["CompanyName"].ToString();
                    shipper.Phone = reader["Phone"].ToString();       
                    shippers.Add(shipper);
                }
            }
            return shippers.AsQueryable();
        }
        #endregion
    }
}