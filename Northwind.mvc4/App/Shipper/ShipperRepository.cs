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

        #endregion

        #region Queries
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