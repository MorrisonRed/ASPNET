using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using AppCore;
using AppCore.Product;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASPNET.Models
{

    #region User Models
    public class UserAddViewModel
    {
        public string SelectedRoleID { get; set; }
        public System.Web.Mvc.SelectList Roles;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //custom properties added to identiy user
        [Required]
        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public virtual string Gender { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", NullDisplayText = "")]
        public virtual DateTime? DateOfBirth { get; set; }
        [Display(Name = "Language")]
        public virtual string Language { get; set; }
        [Display(Name = "Country")]
        public virtual string Country { get; set; }
        [Display(Name = "Postal Code")]
        public virtual string PostalCode { get; set; }
    }
    public class UserEditViewModel
    {
        public string SelectedRoleID { get; set; }
        public System.Web.Mvc.SelectList Roles;

        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //custom properties added to identiy user
        [Required]
        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public virtual string Gender { get; set; }
        [DataType(DataType.DateTime)]      
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", NullDisplayText = "")]
        public virtual DateTime? DateOfBirth { get; set; }
        [Display(Name = "Language")]
        public virtual string Language { get; set; }
        [Display(Name = "Country")]
        public virtual string Country { get; set; }
        [Display(Name = "Postal Code")]
        public virtual string PostalCode { get; set; }
    }
    #endregion

    #region Category Models
    public class CategoryAddViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }
    }
    public class CategoryEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }
    }
    public class CategoryDeleteViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }
    }
    #endregion

    #region Product Models
    public class ProductViewModel : IProduct 
    {
        [Display(Name = "Id")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Supplier Id")]
        public int SupplierID { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }
        [Display(Name = "Quanitity per Unit")]
        public string QuantityPerUnit { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Units in Stock")]
        public int UnitsInStock { get; set; }
        [Display(Name = "Units on Order")]
        public int UnitsOnOrder { get; set; }
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }
    }
    public class ProductAddViewModel
    {
        public int SelectedCategoryID { get; set; }
        public System.Web.Mvc.SelectList Categories;
        public int SelectedSupplierID { get; set; }
        public System.Web.Mvc.SelectList Suppliers;

        [Display(Name = "Id")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Supplier Id")]
        public int SupplierID { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }
        [Display(Name = "Quanitity per Unit")]
        public string QuantityPerUnit { get; set; }
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Units in Stock")]
        [UIHint("NumberTemplate")]
        public int UnitsInStock { get; set; }
        [Display(Name = "Units on Order")]
        [UIHint("NumberTemplate")]
        public int UnitsOnOrder { get; set; }
        [Display(Name = "Reorder Level")]
        [UIHint("NumberTemplate")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }
    }
    public class ProductEditViewModel
    {
        public int SelectedCategoryID { get; set; }
        public System.Web.Mvc.SelectList Categories;
        public int SelectedSupplierID { get; set; }
        public System.Web.Mvc.SelectList Suppliers;

        [Display(Name = "Id")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Supplier Id")]
        public int SupplierID { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }
        [Display(Name = "Quanitity per Unit")]
        public string QuantityPerUnit { get; set; }
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Units in Stock")]
        [UIHint("NumberTemplate")]
        public int UnitsInStock { get; set; }
        [Display(Name = "Units on Order")]
        [UIHint("NumberTemplate")]
        public int UnitsOnOrder { get; set; }
        [Display(Name = "Reorder Level")]
        [UIHint("NumberTemplate")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }
    }
    public class ProductDeleteViewModel
    {
        [Display(Name = "Id")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Supplier Id")]
        public int SupplierID { get; set; }
        [Display(Name = "Supplier")]
        public string Supplier { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Quanitity per Unit")]
        public string QuantityPerUnit { get; set; }
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Units in Stock")]
        [UIHint("NumberTemplate")]
        public int UnitsInStock { get; set; }
        [Display(Name = "Units on Order")]
        [UIHint("NumberTemplate")]
        public int UnitsOnOrder { get; set; }
        [Display(Name = "Reorder Level")]
        [UIHint("NumberTemplate")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }
    }
    #endregion

    #region Supplier Models
    public class SupplierAddViewModel
    {
        public string SelectedCountryID { get; set; }
        public System.Web.Mvc.SelectList Countries;

        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Url")]
        public string HomePage { get; set; }
    }
    public class SupplierEditViewModel
    {
        public string SelectedCountryID { get; set; }
        public System.Web.Mvc.SelectList Countries;


        [Display(Name = "Id"), Key, Required]
        public int SupplierId { get; set; }
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Url")]
        public string HomePage { get; set; }
    }
    public class SupplierDeleteViewModel
    {
        [Required, Key, Display(Name = "Id")]
        public int SupplierId { get; set; }
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Url")]
        public string HomePage { get; set; }
    }
    #endregion

    #region Shipper Models
    public class ShipperAddViewModel
    {
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
    public class ShipperEditViewModel
    {
        [Display(Name = "Id"), Key, Required]
        public int ShipperID { get; set; }
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
    public class ShipperDeleteViewModel
    {
        [Display(Name = "Id"), Key, Required]
        public int ShipperID { get; set; }
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
    #endregion

    #region Customer Models
    public class CustomerAddViewModel
    {
        public string SelectedCountryID { get; set; }
        public System.Web.Mvc.SelectList Countries;

        [Display(Name = "Customer Id"), Key, Required]
        public string CustomerID { get; set; }
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
    }
    public class CustomerEditViewModel
    {
        public string SelectedCountryID { get; set;}
        public System.Web.Mvc.SelectList Countries;

        [Display(Name = "Customer Id"), Key, Required]
        public string CustomerID { get; set; }
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
    }
    public class CustomerDeleteViewModel
    {
        [Display(Name = "Customer Id"), Key, Required]
        public string CustomerID { get; set; }
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
    }
    #endregion

    #region Employee Models
    public class EmployeeAddViewModel
    {
        public string SelectedCountryID { get; set; }
        public System.Web.Mvc.SelectList Countries;
        public int SelectedEmployeeID { get; set; }
        public System.Web.Mvc.SelectList Employees;

        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Title of Courtesy")]
        public string TitleOfCourtesy { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Extension")]
        public string Extension { get; set; }
        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }
        [Display(Name = "Notes"), System.Web.Mvc.AllowHtml]
        public string Notes { get; set; }
        [Display(Name = "Reports To")]
        public int? ReportsTo { get; set; }
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }
        [Display(Name = "Salary")]
        public decimal? Salary { get; set; }
    }
    public class EmployeeEditViewModel
    {
        public string SelectedCountryID { get; set; }
        public System.Web.Mvc.SelectList Countries;
        public int SelectedEmployeeID { get; set; }
        public System.Web.Mvc.SelectList Employees;

        [Display(Name = "Employee Id"), Key]
        public int EmployeeID { get; set; }
        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Title of Courtesy")]
        public string TitleOfCourtesy { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Extension")]
        public string Extension { get; set; }
        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }

        private string _notes; 
        [Display(Name = "Notes"), System.Web.Mvc.AllowHtml]
        public string Notes
        {
            get
            {
                return HttpUtility.HtmlDecode(_notes);
            }
            set
            {
                _notes = HttpUtility.HtmlEncode(value);
            }
        }

        [Display(Name = "Reports To")]
        public int? ReportsTo { get; set; }
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }
        [Display(Name = "Salary")]
        public decimal? Salary { get; set; }
    }
    public class EmployeeDeleteViewModel
    {
        [Display(Name = "Employee Id"), Key]
        public int EmployeeID { get; set; }
        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Title of Courtesy")]
        public string TitleOfCourtesy { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Extension")]
        public string Extension { get; set; }
        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }
        [Display(Name = "Notes"), System.Web.Mvc.AllowHtml]
        public string Notes { get; set; }
        [Display(Name = "Reports To")]
        public int? ReportsTo { get; set; }
        [Display(Name = "Reports To")]
        public string ReportsToName { get; set; }
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }
        [Display(Name = "Salary")]
        public decimal? Salary { get; set; }
    }
    #endregion
}