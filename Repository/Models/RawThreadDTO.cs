using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class RawThreadDTO
    {
        public int Id { get; set; }
        public int ThreadWeight { get; set; }
        public int BillAmount { get; set; }
        public int BillNo { get; set; }

        public DateTime BillDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CompanyName { get; set; }
        public string Quality { get; set; }
    }
}
