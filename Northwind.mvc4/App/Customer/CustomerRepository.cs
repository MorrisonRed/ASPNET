using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Customer
{
    public class CustomerRepository<TCustomer> where TCustomer : ICustomer
    {
        private readonly string _connectionString;

        #region Constructors and Destructors
        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD Methods
        public bool Add(TCustomer customer)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TCustomer);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var value = (info.GetValue(customer, null) == null) ? DBNull.Value : info.GetValue(customer, null);
                    parameters.Add("@" + info.Name, value);        
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                        @"INSERT INTO Customers(CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax) 
                        VALUES(@CustomerID,@CompanyName,@ContactName,@ContactTitle,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax)",
                            parameters);

                return true;
            }
        }
        public bool Update(TCustomer customer)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TCustomer);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    var value = (info.GetValue(customer, null) == null) ? DBNull.Value : info.GetValue(customer, null);
                    parameters.Add("@" + info.Name, value);
                }

                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"UPDATE Customers SET CompanyName=@CompanyName,ContactName=@ContactName,ContactTitle=@ContactTitle,
                            Address=@Address,City=@City,Region=@Region,PostalCode=@PostalCode,Country=@Country,Phone=@Phone,
                            Fax=@Fax WHERE CustomerID=@CustomerID",
                            parameters);

                return true;
            }
        }
        public bool Delete(TCustomer customer)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "@ID", customer.CustomerID }
                };
            
                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"DELETE FROM Customers WHERE CustomerID = @ID",
                            parameters);

                return true;
            }
        }
        #endregion

        #region Queries
        public TCustomer FindById(string id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id }
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, 
                        Region, PostalCode, Country, Phone, Fax FROM Customers 
                        WHERE CustomerID=@ID",
                    parameters);

                var customer = (TCustomer)Activator.CreateInstance(typeof(TCustomer));
                while (reader.Read())
                {
                    customer.CustomerID = reader["CustomerID"].ToString();
                    customer.CompanyName = reader["CompanyName"].ToString();
                    customer.ContactName = reader["ContactName"].ToString();
                    customer.ContactTitle = reader["ContactTitle"].ToString();
                    customer.Address = reader["Address"].ToString();
                    customer.City = reader["City"].ToString();
                    customer.Region = reader["Region"].ToString();
                    customer.PostalCode = reader["PostalCode"].ToString();
                    customer.Country = reader["Country"].ToString();
                    customer.Phone = reader["Phone"].ToString();
                    customer.Fax = reader["Fax"].ToString();
                }
                return customer;
            }
        }
        public IQueryable<TCustomer> GetCustomers()
        {
            var customers = new List<TCustomer>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, 
                        Region, PostalCode, Country, Phone, Fax FROM Customers"
                    , null);

                while (reader.Read())
                {
                    var customer = (TCustomer)Activator.CreateInstance(typeof(TCustomer));
                    customer.CustomerID = reader["CustomerID"].ToString();
                    customer.CompanyName = reader["CompanyName"].ToString();
                    customer.ContactName = reader["ContactName"].ToString();
                    customer.ContactTitle = reader["ContactTitle"].ToString();
                    customer.Address = reader["Address"].ToString();
                    customer.City = reader["City"].ToString();
                    customer.Region = reader["Region"].ToString();
                    customer.PostalCode = reader["PostalCode"].ToString();
                    customer.Country = reader["Country"].ToString();
                    customer.Phone = reader["Phone"].ToString();
                    customer.Fax = reader["Fax"].ToString();
                    customers.Add(customer);
                }
            }
            return customers.AsQueryable();
        }
        #endregion
    }
}