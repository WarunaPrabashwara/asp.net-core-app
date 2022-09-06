using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using mvcweb.Models;
using System.Security.Claims;

namespace mvcweb.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/UsersController/Insert")]
        public IActionResult Insert(users model)
        {
            model.uniqueId = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            
                string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
                var Client = new MongoClient(constr);
                string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
                var DB = Client.GetDatabase(DBname);
                var collection = DB.GetCollection<users>("users");
                collection.InsertOne(model);
                return RedirectToAction("Index", "Home");
            
        }

        [HttpGet]
        [Route("/UsersController/ulevNemail")]
        public IActionResult getuserlevelNemanil()
        {
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            //var uniqueId = Query<users>.EQ(p => p.uniqueId, new ObjectId(User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value));
            var collection = DB.GetCollection<users>("users").Find(new BsonDocument()).ToList();
            var uniqueifromauth = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            
            if (collection.Any(x => x.uniqueId == uniqueifromauth ))
            {
                var item = collection.FirstOrDefault(o => o.uniqueId == uniqueifromauth );
                if (item.userType == "admin")
                {
                    return RedirectToAction("adminhome", "landpages");
                }
                else if (item.userType == "driver")
                {
                    return RedirectToAction("driverhome", "landpages");
                }
                else if (item.userType == "customer")
                {
                    return RedirectToAction("customerhome", "landpages");
                }
                else
                {
                    return RedirectToAction("Index", "Registration");
                }



            }
            else
            {
                return RedirectToAction("Index", "Registration");
            }

            

        }


    }
}
