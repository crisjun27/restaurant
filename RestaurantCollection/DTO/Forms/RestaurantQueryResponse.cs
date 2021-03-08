using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantCollection.WebApi.DTO.Forms
{
    public class RestaurantQueryResponse
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; } 
        public string Rating { get; set; } 
        public int Cost { get; set; }
        public int Votes { get; set; }
    }
}
