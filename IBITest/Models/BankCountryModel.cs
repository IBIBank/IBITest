using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBITest.Models
{
    public class BankCountryModel
    {
        public List<City> CityModel { get; set; }
        public SelectList FilteredBranch { get; set; }
        public List<Branch> BranchModel { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
    }

    public class Branch
    {
        public int Id { get; set; }
      //  public int CityId { get; set; }
        public string BranchName { get; set; }
    }
}