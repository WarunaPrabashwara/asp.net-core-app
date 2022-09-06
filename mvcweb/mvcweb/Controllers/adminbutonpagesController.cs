using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using mvcweb.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace mvcweb.Controllers
{
    public class adminbutonpagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult viewCustomers()
        {

            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<users>.Filter;
            var filter = builderr.Eq("userType", "customer");
            var collection = DB.GetCollection<users>("users").Find(filter).ToList();


            ViewBag.customerlist = collection;
            return View();
        }
        public IActionResult viewdrivers()
        {
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<users>.Filter;
            var filter = builderr.Eq("userType", "driver");
            var collection = DB.GetCollection<users>("users").Find(filter).ToList();


            ViewBag.driverlist = collection;



            int accbalnceofeachone = 0;
            List<int> AccBalance = new List<int>();
            foreach (users item in collection)
            {
                var builder2 = Builders<jobstatus>.Filter;
                var filter2 = builder2.Eq("driverid", item.uniqueId );
                var collection3 = DB.GetCollection<jobstatus>("jobstatus").Find(filter2).ToList();

                foreach (jobstatus item2 in collection3)
                {
                    if (item2.paywith != null && item2.paywith != "cash")
                    {
                        accbalnceofeachone = accbalnceofeachone + Int16.Parse(item2.amountPayable) ;
                    }

                }
                AccBalance.Add(accbalnceofeachone);
                accbalnceofeachone = 0;
            }

            ViewBag.Accountbalance = AccBalance;
            return View();

        }
        public IActionResult Viewtrips()
        {

            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<jobstatus>.Filter;
           // var filter = builderr.Type("Nonce", useremail);
            var collection = DB.GetCollection<jobstatus>("jobstatus").Find(new BsonDocument()).ToList();




            ViewBag.triplist = collection;
            return View();
        }




        public IActionResult paythedriverfee(string driverid )
        {
            Debug.Write("My error message. " + driverid );

            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("driverid", driverid )  ;
            var collection = DB.GetCollection<jobstatus>("jobstatus") ;

            var updatestring = Builders<jobstatus>.Update.Set("paywith", "cash" );
            collection.UpdateMany(filter, updatestring);

             
            return RedirectToAction("viewdrivers", "adminbutonpages");




        }

    }
}
