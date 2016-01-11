using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Country
{
    public class Country : ICountry
    {
        [Key]
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Continet")]
        public string Continent { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "Surface Area")]
        public decimal SurfaceArea { get; set; }
        [Display(Name = "Independence Year")]
        public int? IndepYear { get; set; }
        [Display(Name = "Population")]
        public int Population { get; set; }
        [Display(Name = "Life Expectancy")]
        public decimal? LifeExpectancy { get; set; }
        [Display(Name = "Gross National Product")]
        public decimal? GNP { get; set; }
        [Display(Name = "Gross National Product (previous)")]
        public decimal? GNPOld { get; set; }
        [Display(Name = "Local Name")]
        public string LocalName { get; set; }
        [Display(Name = "Government")]
        public string GovernmentForm { get; set; }
        [Display(Name = "Head of State")]
        public string HeadOfState { get; set; }
        [Display(Name = "Capital")]
        public int? Capital { get; set; }
        [Display(Name = "Alternate Code")]
        public string Code2 { get; set; }       
    }
}
