using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace AppCore.Product
{
    public class ProductRepository<TProduct> where TProduct : IProduct
    {
        private readonly string _connectionString;

        #region Contructors and Destructors
        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD methods
        public void Insert(IProduct  product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@productId", product.ProductID},
                    {"@productName", product.ProductName}
                };

                SqlHelper.ExecuteNonQuery(conn, 
                    @"INSERT INTO Products (ProductId,ProductName) VALUES (@productId,@productName)",
                    parameters);

            }
        }

        #endregion

        #region Queries
        public IQueryable<TProduct> GetProducts()
        {
            var products = new List<TProduct>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text, 
                    @"SELECT ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,
                        UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued 
                        FROM products"
                    , null);

                while (reader.Read())
                {
                    var product = (TProduct)Activator.CreateInstance(typeof(TProduct));
                    product.ProductID = (int)reader["ProductID"];
                    product.ProductName = reader["ProductName"].ToString();
                    product.SupplierID = (int)reader["SupplierID"];
                    product.CategoryID = (int)reader["CategoryID"];
                    product.QuantityPerUnit = reader["QuantityPerUnit"].ToString();
                    product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    product.UnitsInStock = Convert.ToInt16(reader["UnitsInStock"]);
                    product.UnitsOnOrder = Convert.ToInt16(reader["UnitsOnOrder"]);
                    product.ReorderLevel = Convert.ToInt16(reader["ReorderLevel"]);
                    product.Discontinued = Convert.ToBoolean(reader["Discontinued"]);
                    products.Add(product);
                }

            }
            return products.AsQueryable();
        }
        #endregion
    }
}