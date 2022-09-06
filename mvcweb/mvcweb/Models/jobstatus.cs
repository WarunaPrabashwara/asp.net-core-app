
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcweb.Models
{
    public class jobstatus
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string customerid { get; set; }
        public string driverid { get; set; }

        [Display(Name = "Starting place in words")]
        public string customerFrom { get; set; }
        [Display(Name = "Drag pointer to your ending location")]
        public string customerTo { get; set; } 
        public string isJobdone { get; set; }
        [Display(Name = "How much you like to offer(in rupees)")]
        public string amountPayable { get; set; }
        [Display(Name = "What is the vehicle type")]
        public string vehicleType { get; set; }
        [Display(Name = "Feedback rate for driver")]
        public string feedbackRatefordriver { get; set; }
        [Display(Name = "Customer's feedbadck rate")]
        public string feedbackrateforcustomer { get; set; }
        [Display(Name = "Customer's feedback")]
        public string feedbackforcustomer { get; set; }
        [Display(Name = "Feedback for driver")]
        public string feedbackfordriver { get; set; }
        public string dateTime { get; set; }
        [Display(Name = "Pay with cash or card ?")]
        public string paywith { get; set; }

        public string Nonce { get; set; }


    }
}
