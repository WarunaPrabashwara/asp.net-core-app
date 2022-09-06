using Braintree;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using mvcweb.Models;
using mvcweb.services;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace mvcweb.Controllers
{
    public class landpagesController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ItemList = "Computer Shop Item List Page";
            return View();
        }

        public IActionResult adminhome()
        {
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            //var uniqueId = Query<users>.EQ(p => p.uniqueId, new ObjectId(User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value));
            var collection = DB.GetCollection<users>("users").Find(x => x.isapproved == "notapproved").ToList();

            ViewBag.collectionlist = collection;
            return View();
        }

       
        public IActionResult approval(string uniqueId, string status)
        {


            Debug.Write("My error message. " + uniqueId + status );

            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<users>("users");
            if (status == "approved")
            {
                var update = collection.FindOneAndUpdateAsync(Builders<users>.Filter.Eq("uniqueId", uniqueId), Builders<users>.Update.Set("isapproved", status));
            }
            else if (status == "delete")
            {
                var DeleteRecored = collection.DeleteOneAsync( Builders<users>.Filter.Eq("uniqueId", uniqueId ));
            }
            return RedirectToAction("adminhome", "landpages");

        }

        public IActionResult jobdonebutton(string isJobdone )
        {


            Debug.Write("My error message. " + isJobdone );

            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("driverid", useremail) & builderr.Type("isJobdone", BsonType.Null);
            var update = collection.FindOneAndUpdateAsync(filter, Builders<jobstatus>.Update.Set("isJobdone", "yes"));
            return RedirectToAction("driverhome", "landpages");

        }


        public IActionResult selectthejob(string customerid )
        {


            Debug.Write("My error message. " + customerid);

            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("customerid", customerid) & builderr.Type("isJobdone", BsonType.Null);
            var update = collection.FindOneAndUpdateAsync(filter, Builders<jobstatus>.Update.Set("driverid", useremail ));
            return RedirectToAction("driverhome", "landpages");

        }



        public IActionResult addratingbycustomerr(jobstatus model  )
        {
            Debug.Write("My error message. " + model.feedbackfordriver + model.feedbackRatefordriver);
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("customerid", useremail) & builderr.Type("feedbackRatefordriver" , BsonType.Null);

            var updatestring = Builders<jobstatus>.Update.Set("feedbackRatefordriver", model.feedbackRatefordriver).Set("feedbackfordriver", model.feedbackfordriver);
            var update = collection.FindOneAndUpdateAsync(filter , updatestring);
            return RedirectToAction("customerhome", "landpages");

        }


        public IActionResult addratingbydriver(jobstatus model)
        {
            Debug.Write("My error message. " + model.feedbackforcustomer + model.feedbackrateforcustomer);
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("driverid", useremail) & builderr.Type("feedbackrateforcustomer", BsonType.Null);

            var updatestring = Builders<jobstatus>.Update.Set("feedbackrateforcustomer", model.feedbackrateforcustomer).Set("feedbackforcustomer", model.feedbackforcustomer);
            var update = collection.FindOneAndUpdateAsync(filter, updatestring);
            return RedirectToAction("driverhome", "landpages");

        }




        [HttpPost] 
        public IActionResult postjob(jobstatus model)
        {
            model.customerid = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            model.dateTime = DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            collection.InsertOne(model);
            return RedirectToAction("customerhome", "landpages");

        }



        public IActionResult cancelpostedjob( )
        {
            var customerid = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");

            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("customerid", customerid) & builderr.Type("driverid", BsonType.Null);

            collection.DeleteOneAsync(filter);
            return RedirectToAction("customerhome", "landpages");

        }





        private readonly IBraintreeService _braintreeService;

        public landpagesController(IBraintreeService braintreeService)
        {
            _braintreeService = braintreeService;
        }


        public IActionResult paywithcard( )
        {
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("customerid", useremail) & builderr.Type("paywith", BsonType.Null);

            var updatestring = Builders<jobstatus>.Update.Set("paywith", "cash");
            var update = collection.FindOneAndUpdateAsync(filter, updatestring);
            return RedirectToAction("customerhome", "landpages");

        }


        [HttpPost]
        public IActionResult cardpayclick( jobstatus objectwithamounttopay )
        {
            var gateway = _braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(objectwithamounttopay.amountPayable),
                PaymentMethodNonce = objectwithamounttopay.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Debug.Write("error message. " + request.Amount);

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Debug.Write("My error m. " + result );


                string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
                var Client = new MongoClient(constr);
                string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
                var DB = Client.GetDatabase(DBname);
                var collection = DB.GetCollection<jobstatus>("jobstatus");
                string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                var builderr = Builders<jobstatus>.Filter;
                var filter = builderr.Eq("customerid", useremail) & builderr.Type("paywith", BsonType.Null);
                var updatestring = Builders<jobstatus>.Update.Set("paywith", objectwithamounttopay.amountPayable );
                var update = collection.FindOneAndUpdateAsync(filter, updatestring);


                return RedirectToAction("customerhome", "landpages");
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    Debug.Write("My error message. " + (int)error.Code + " - " + error.Message);
                }

                
                return View("Failure");
            }


        }


            public IActionResult customerhome()
        {
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            //var uniqueId = Query<users>.EQ(p => p.uniqueId, new ObjectId(User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value));
            var collection = DB.GetCollection<jobstatus>("jobstatus").Find(x => x.customerid == useremail).ToList();
            if(collection.Count == 0) // job ekak wath nathi awasthawa
            {
                ViewBag.situation = "normalpage";
                ViewBag.resultobject = null;
            }
            else // job ekak hari daala tyena awasthawaw
            {
                jobstatus result = collection.Find(x => x.feedbackRatefordriver == null);
                if ( result  == null ) 
                {
                    ViewBag.situation = "normalpage";
                    ViewBag.resultobject = null;
                }
                else
                {
                    if( result.isJobdone == null)
                    {
                        if (result.driverid == null)
                        {
                            ViewBag.situation = "waitforadriverselectREFRESHALWAYS";
                            ViewBag.resultobject = result;
                        }
                        else
                        {
                            ViewBag.situation = "youareonhetrip";
                            ViewBag.resultobject = result;

                            
                            //var uniqueId = Query<users>.EQ(p => p.uniqueId, new ObjectId(User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value));
                            var collection2 = DB.GetCollection<users>("users").Find(x => x.uniqueId == result.driverid ).ToList();

                            ViewBag.driver = collection2[0];
                        }
                    }
                    else
                    {
                        if (result.paywith == null)
                        {
                            ViewBag.situation = "pageforpayment";
                            ViewBag.resultobject = result;
                        }
                        else
                        {
                            ViewBag.situation = "pageforratingdriver";
                            ViewBag.resultobject = result;
                            ViewBag.paidmethod = result.paywith;
                            ViewBag.amount = result.amountPayable ;
                        }
                            
                    }
                }
                
            }



            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            //Genarate a token
            ViewBag.ClientToken = clientToken;
            var builderr = Builders<jobstatus>.Filter;
            var filter = builderr.Eq("customerid", useremail) & builderr.Type("paywith", BsonType.Null);
            var collection6 = DB.GetCollection<jobstatus>("jobstatus").Find(filter).ToList();
            if(collection6.Count == 0)
            {

            }
            else
            {
                ViewBag.objectwithamounttopay = collection6[0];
            }
            

            return View();
        }

        public class temporyclas
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public async Task<IActionResult> driverhomeAsync()
        {
            string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            //var uniqueId = Query<users>.EQ(p => p.uniqueId, new ObjectId(User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value));
            var builderr = Builders<jobstatus>.Filter; 
            var filter = builderr.Eq("driverid", useremail) & builderr.Eq("isJobdone", "yes") &  builderr.Type("feedbackrateforcustomer", BsonType.Null);
            var collection = DB.GetCollection<jobstatus>("jobstatus").Find(filter).ToList();
            if (collection.Count == 0) //  ekak wath nathi awasthawa
            {

                var filter2 = builderr.Eq("driverid", useremail) & builderr.Type("isJobdone", BsonType.Null);
                collection = DB.GetCollection<jobstatus>("jobstatus").Find(filter2).ToList();
                if (collection.Count != 0)
                {
                    ViewBag.situation = "youareonatrip";
                    ViewBag.resultobject = collection;
                    
                    var collection2 = DB.GetCollection<users>("users").Find(x => x.uniqueId == collection[0].customerid ).ToList();
                    ViewBag.customer = collection2[0];
                }
                else
                {
                    ViewBag.situation = "joblistekapenwannameeke";
                    ViewBag.resultobject = null;
                }
                   
            }
            else // job ekak hari daala tyena awasthawaw
            {
                ViewBag.situation = "ratingpage";
                ViewBag.resultobject = collection ;
                                                             

            }




            int minimumjobsnottogotensor = 200 ;
            var filter9 =  builderr.Eq("driverid", useremail ); // ee driver kalin kala job 5 kwath tyewna nm tensorfow ekata yanna . nattham siyalu jobs penwanna kiyala meken kiyanne 
            var collection19 = DB.GetCollection<jobstatus>("jobstatus").Find(filter9).ToList();
            List<jobstatus> collection9 = new List<jobstatus>();
            if (collection19.Count > minimumjobsnottogotensor)
            {
                // tensorflow api eken ganna oni job list eka 
                // meekata aawoth ehema collection2 kiyanja uda variable eka aluth list eken replace karanna . api eken anna list eken collection2 replace karanna 

                ViewBag.graphqlalow = "yes";
                ViewBag.driverid = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;


            }
            else
            {
                filter9 = builderr.Type("driverid", BsonType.Null );
                collection9 = DB.GetCollection<jobstatus>("jobstatus").Find(filter9).ToList();

            }
            List<users> customersset = new List<users>(); 
            foreach(jobstatus jobss in collection9)
            {
                var builderr2 = Builders<users>.Filter;
                var filter3 = builderr2.Eq("uniqueId", jobss.customerid )   ;
                var useroforder = DB.GetCollection<users>("users").Find(filter3).ToList()[0] ;
                customersset.Add(useroforder); 
            }

            ViewBag.joblist = collection9;
            ViewBag.customerlistofjobs = customersset;

            return View();
        }



    }
}
