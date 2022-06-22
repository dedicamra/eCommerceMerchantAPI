using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Requests
{
    public class SearchRequest
    {
        public string SearchTerm { get; set; }
        public string Adress { get; set; }
        public bool? Active { get; set; }

    }
}
