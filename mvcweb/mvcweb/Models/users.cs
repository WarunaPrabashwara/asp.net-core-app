using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcweb.Models
{
    public class users
    {
        [BsonId]

        public ObjectId _id { get; set; }
        public String uniqueId { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Birthday")]
        public string birthday { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Language")]
        public string race { get; set; }
        [Display(Name = "User Type")]
        public string userType { get; set; }
        
        [Display(Name = "Civil Status")]
        public string civilState { get; set; }
        [Display(Name = "Driving vehicles")]
        public string managablevehicles { get; set; }
        
        [Display(Name = "Phone No")]
        public string phoneNo { get; set; }

        [Display(Name = "2 Wheelers")]
        public bool twowheel { get; set; }
        [Display(Name = "3 wheelers")]
        public bool threewheel { get; set; }
        [Display(Name = "4 wheelers")]
        public bool fourwheel { get; set; }

        public string isapproved { get; set; }
        public string AccBalance { get; set; }

    }
}
