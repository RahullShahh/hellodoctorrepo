using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class CreateUpdateVendorViewModel
    {
        public string? UserName { get; set; }
        public List<Healthprofessionaltype> types { get; set; }
        public List<Region> regions { get; set; }

        public string? BusinessName { get; set; }
        public int? type { get; set; }
        public string? Fax { get; set; }
        public string? code { get; set; }
        public string? phone { get; set; }
        public string? Email { get; set; }
        public string? code1 { get; set; }
        public string? phone1 { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public int? state { get; set; }
        public string? zip { get; set; }

        public int? id { get; set; }
    }
}
