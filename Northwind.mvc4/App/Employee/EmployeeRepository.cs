using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Employee
{
    public class EmployeeRepository<TEmployee> where TEmployee : IEmployee
    {
        private readonly string _connectionString;

        #region Constructors and Destructors
        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region CRUD Methods
        public bool Add(TEmployee employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@LastName", employee.LastName},
                    {"@FirstName", employee.FirstName },
                    {"@Title", employee.Title },
                    {"@TitleOfCourtesy", employee.TitleOfCourtesy },
                    {"@Address", employee.Address },
                    {"@City", employee.City },
                    {"@Region", employee.Region },
                    {"@PostalCode", employee.PostalCode },
                    {"@Country", employee.Country },
                    {"@HomePhone", employee.HomePhone },
                    {"@Extension", employee.Extension },
                    {"@Notes", employee.Notes },
                    {"@ReportsTo", employee.ReportsTo },
                    {"@PhotoPath", employee.PhotoPath },
                    {"@Salary", employee.Salary }
                };
                if (employee.Photo == null)
                    parameters.Add("@Photo", DBNull.Value);
                else
                    parameters.Add("@Photo", employee.Photo);
                if (employee.BirthDate.HasValue)
                    parameters.Add("@BirthDate", employee.BirthDate.Value);
                else
                    parameters.Add("@BirthDate", DBNull.Value);
                if (employee.HireDate.HasValue)
                    parameters.Add("@HireDate", employee.HireDate.Value);
                else
                    parameters.Add("@HireDate", DBNull.Value);

                var result = SqlHelper.ExecuteNonQuery(conn,
                        @"INSERT INTO Employees(LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,
                            Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath,Salary) 
                        VALUES(@LastName,@FirstName,@Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,@PostalCode,
                            @Country,@HomePhone,@Extension,@Photo,@Notes,@ReportsTo,@PhotoPath,@Salary)",
                            parameters);

                return true;
            }
        }
        public bool Update(TEmployee employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>();
                //auto load all paramters for each public property of object
                Type type = typeof(TEmployee);
                var properties = type.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    if (info.GetValue(employee, null) == null)
                        parameters.Add("@" + info.Name, DBNull.Value);
                    else
                        parameters.Add("@" + info.Name, info.GetValue(employee, null));
                }

                String query = "";
                if(employee.Photo == null)
                {
                    query = @"UPDATE Employees SET LastName=@LastName,FirstName=@FirstName,Title=@Title,TitleOfCourtesy=@TitleOfCourtesy,
                                BirthDate=@BirthDate,HireDate=@HireDate,Address=@Address,City=@City,Region=@Region,PostalCode=@PostalCode,
                                Country=@Country,HomePhone=@HomePhone,Extension=@Extension,Notes=@Notes,ReportsTo=@ReportsTo,
                                PhotoPath=@PhotoPath,Salary=@Salary WHERE EmployeeID=@EmployeeID";
                }
                else
                {
                    query = @"UPDATE Employees SET LastName=@LastName,FirstName=@FirstName,Title=@Title,TitleOfCourtesy=@TitleOfCourtesy,
                                BirthDate=@BirthDate,HireDate=@HireDate,Address=@Address,City=@City,Region=@Region,PostalCode=@PostalCode,
                                Country=@Country,HomePhone=@HomePhone,Extension=@Extension,Photo=@Photo,Notes=@Notes,ReportsTo=@ReportsTo,
                                PhotoPath=@PhotoPath,Salary=@Salary WHERE EmployeeID=@EmployeeID";
                }
                  
                var result = SqlHelper.ExecuteNonQuery(conn, query, parameters);

                return true;
            }
        }
        public bool Delete(TEmployee employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "@ID", employee.EmployeeID }
                };
            
                var result = SqlHelper.ExecuteNonQuery(conn,
                            @"DELETE FROM Employees WHERE EmployeeID = @ID",
                            parameters);

                return true;
            }
        }
        #endregion

        #region Queries
        public TEmployee FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@ID", id }
                };

                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,
                        City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath,
                        Salary FROM Employees WHERE EmployeeID=@ID",
                    parameters);

                var employee = (TEmployee)Activator.CreateInstance(typeof(TEmployee));
                while (reader.Read())
                {
                    employee.EmployeeID = (int)reader["EmployeeID"];
                    employee.LastName = reader["LastName"].ToString();
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.Title = reader["Title"].ToString();
                    employee.TitleOfCourtesy = reader["TitleOfCourtesy"].ToString();
                    employee.BirthDate = (reader["BirthDate"] == DBNull.Value) ? (DateTime?)null : (DateTime)reader["BirthDate"];
                    employee.HireDate = (reader["HireDate"] == DBNull.Value) ? (DateTime?)null : (DateTime)reader["HireDate"];
                    employee.Address = reader["Address"].ToString();
                    employee.City = reader["City"].ToString();
                    employee.Region = reader["Region"].ToString();
                    employee.PostalCode = reader["PostalCode"].ToString();
                    employee.Country = reader["Country"].ToString();
                    employee.HomePhone = reader["HomePhone"].ToString();
                    employee.Extension = reader["Extension"].ToString();
                    employee.Photo = (reader["Photo"] == DBNull.Value) ? null : (byte[])reader["Photo"];
                    employee.Notes = reader["Notes"].ToString();
                    employee.ReportsTo = (reader["ReportsTo"] == DBNull.Value) ? (int?)null : (int)reader["ReportsTo"];
                    employee.PhotoPath = reader["PhotoPath"].ToString();
                    employee.Salary = (reader["Salary"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(reader["Salary"]);
                }
                return employee;
            }
        }
        public IQueryable<TEmployee> GetEmployees()
        {
            var employees = new List<TEmployee>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,
                        City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath,
                        Salary FROM Employees"
                    , null);

                while (reader.Read())
                {
                    var employee = (TEmployee)Activator.CreateInstance(typeof(TEmployee));
                    employee.EmployeeID = (int)reader["EmployeeID"];
                    employee.LastName = reader["LastName"].ToString();
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.Title = reader["Title"].ToString();
                    employee.TitleOfCourtesy = reader["TitleOfCourtesy"].ToString();
                    employee.BirthDate = (reader["BirthDate"] == DBNull.Value) ? (DateTime?)null : (DateTime)reader["BirthDate"];
                    employee.HireDate = (reader["HireDate"] == DBNull.Value) ? (DateTime?)null : (DateTime)reader["HireDate"];
                    employee.Address = reader["Address"].ToString();
                    employee.City = reader["City"].ToString();
                    employee.Region = reader["Region"].ToString();
                    employee.PostalCode = reader["PostalCode"].ToString();
                    employee.Country = reader["Country"].ToString();
                    employee.HomePhone = reader["HomePhone"].ToString();
                    employee.Extension = reader["Extension"].ToString();
                    employee.Photo = (reader["Photo"] == DBNull.Value) ? null : (byte[])reader["Photo"];
                    employee.Notes = reader["Notes"].ToString();
                    employee.ReportsTo = (reader["ReportsTo"] == DBNull.Value) ? (int?)null : (int)reader["ReportsTo"];
                    employee.PhotoPath = reader["PhotoPath"].ToString();
                    employee.Salary = (reader["Salary"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(reader["Salary"]);
                    employees.Add(employee);
                }
            }
            return employees.AsQueryable();
        }
        #endregion
    }
}