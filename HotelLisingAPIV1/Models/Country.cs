using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace HotelLisingAPIV1.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Collection<Hotel> Hotels { get; set;}
    }
}
