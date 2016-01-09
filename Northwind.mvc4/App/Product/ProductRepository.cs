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
        public bool Add(IProduct product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ProductName", product.ProductName},
                    {"@SupplierID", product.SupplierID },
                    {"@CategoryID", product.CategoryID },
                    {"@QuantityPerUnit", product.QuantityPerUnit },
                    {"@UnitPrice", product.UnitPrice },
                    {"@UnitsInStock", product.UnitsInStock },
                    {"@UnitsOnOrder", product.UnitsOnOrder },
                    {"@ReorderLevel", product.ReorderLevel },
                    {"@Discontinued", product.Discontinued }
                };

                SqlHelper.ExecuteNonQuery(conn,
                    @"INSERT INTO PRODUCTS(ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,
                        UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued) VALUES (@ProductName, 
                        @SupplierID,@CategoryID,@QuantityPerUnit,@UnitPrice,@UnitsInStock,@UnitsOnOrder,@ReorderLevel, 
                        @Discontinued)",
                    parameters);

                return true;
            }
        }
        public bool Update(IProduct product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ProductId", product.ProductID},
                    {"@ProductName", product.ProductName},
                    {"@SupplierID", product.SupplierID },
                    {"@CategoryID", product.CategoryID },
                    {"@QuantityPerUnit", product.QuantityPerUnit },
                    {"@UnitPrice", product.UnitPrice },
                    {"@UnitsInStock", product.UnitsInStock },
                    {"@UnitsOnOrder", product.UnitsOnOrder },
                    {"@ReorderLevel", product.ReorderLevel },
                    {"@Discontinued", product.Discontinued }
                };

                SqlHelper.ExecuteNonQuery(conn,
                    @"UPDATE PRODUCTS SET ProductName=@ProductName,SupplierID=@SupplierID,CategoryID=@CategoryID
                        ,QuantityPerUnit=@QuantityPerUnit,UnitPrice=@UnitPrice,UnitsInStock=@UnitsInStock
                        ,UnitsOnOrder=@UnitsOnOrder,ReorderLevel=@ReorderLevel,Discontinued=@Discontinued 
                        WHERE ProductId=@ProductId",
                    parameters);

                return true;
            }
        }
        public bool Delete(IProduct product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", product.ProductID}
                };

                SqlHelper.ExecuteNonQuery(conn,
                    @"DELETE FROM PRODUCTS WHERE ProductId=@ID",
                    parameters);

                return true;
            }
        }
        #endregion

        #region Queries
        internal TProduct FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id}
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,
                        UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued 
                        FROM products WHERE ProductID=@ID",
                    parameters);

                var product = (TProduct)Activator.CreateInstance(typeof(TProduct));
                while (reader.Read())
                {
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
                }
                return product;
            }
        }
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