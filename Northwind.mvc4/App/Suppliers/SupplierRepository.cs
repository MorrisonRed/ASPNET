using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Suppliers
{
    public class SupplierRepository<TSupplier> where TSupplier : ISupplier
    {
        private readonly string _connectionString;

        #region Constructors and Destructors
        public SupplierRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD Methods
        public bool Add(TSupplier supplier)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TSupplier);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var attribute = Attribute.GetCustomAttribute(info, typeof(KeyAttribute)) as KeyAttribute;
                    //do not add [Key] property to insert statement as it is the Primary Key
                    if (attribute == null) 
                    {
                        var value = (info.GetValue(supplier, null) == null) ? DBNull.Value : info.GetValue(supplier, null);
                        parameters.Add("@" + info.Name, value);
                    }            
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"INSERT INTO SUPPLIERS(CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage) 
                        VALUES(@CompanyName,@ContactName,@ContactTitle,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax,@HomePage)",
                            parameters);

                return true;
            }
        }
        public bool Update(TSupplier supplier)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TSupplier);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var value = (info.GetValue(supplier, null) == null) ? DBNull.Value : info.GetValue(supplier, null);
                    parameters.Add("@" + info.Name, value);
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"UPDATE Suppliers SET CompanyName=@CompanyName,ContactName=@ContactName,ContactTitle=@ContactTitle,
                            Address=@Address,City=@City,Region=@Region,PostalCode=@PostalCode,Country=@Country,Phone=@Phone,
                            Fax=@Fax,HomePage=@HomePage WHERE SupplierID=@SupplierID",
                            parameters);

                return true;
            }
        }
        public bool Delete(TSupplier supplier)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "@ID", supplier.SupplierId }
                };
            
                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"DELETE FROM Suppliers WHERE SupplierID = @ID",
                            parameters);

                return true;
            }
        }
        #endregion

        #region Queries
        public TSupplier FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id }
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT SupplierId, CompanyName, ContactName, ContactTitle, Address, City, 
                        Region, PostalCode, Country, Phone, Fax, HomePage FROM Suppliers 
                        WHERE SupplierID=@ID",
                    parameters);

                var supplier = (TSupplier)Activator.CreateInstance(typeof(TSupplier));
                while (reader.Read())
                {
                    supplier.SupplierId = (int)reader["SupplierID"];
                    supplier.CompanyName = reader["CompanyName"].ToString();
                    supplier.ContactName = reader["ContactName"].ToString();
                    supplier.ContactTitle = reader["ContactTitle"].ToString();
                    supplier.Address = reader["Address"].ToString();
                    supplier.City = reader["City"].ToString();
                    supplier.Region = reader["Region"].ToString();
                    supplier.PostalCode = reader["PostalCode"].ToString();
                    supplier.Country = reader["Country"].ToString();
                    supplier.Phone = reader["Phone"].ToString();
                    supplier.Fax = reader["Fax"].ToString();
                }
                return supplier;
            }
        }
        public IQueryable<TSupplier> GetSuppliers()
        {
            var suppliers = new List<TSupplier>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT SupplierId, CompanyName, ContactName, ContactTitle, Address, City, 
                        Region, PostalCode, Country, Phone, Fax, HomePage FROM Suppliers"
                    , null);

                while (reader.Read())
                {
                    var supplier = (TSupplier)Activator.CreateInstance(typeof(TSupplier));
                    supplier.SupplierId = (int)reader["SupplierID"];
                    supplier.CompanyName = reader["CompanyName"].ToString();
                    supplier.ContactName = reader["ContactName"].ToString();
                    supplier.ContactTitle = reader["ContactTitle"].ToString();
                    supplier.Address = reader["Address"].ToString();
                    supplier.City = reader["City"].ToString();
                    supplier.Region = reader["Region"].ToString();
                    supplier.PostalCode = reader["PostalCode"].ToString();
                    supplier.Country = reader["Country"].ToString();
                    supplier.Phone = reader["Phone"].ToString();
                    supplier.Fax = reader["Fax"].ToString();
                    supplier.HomePage = reader["HomePage"].ToString();
                    suppliers.Add(supplier);
                }
            }
            return suppliers.AsQueryable();
        }
        #endregion
    }
}