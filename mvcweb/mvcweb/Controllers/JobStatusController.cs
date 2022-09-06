using Microsoft.AspNetCore.Mvc;

using mvcweb.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace mvcweb.Controllers
{
    public class JobStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(jobstatus model)
        {
            string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
            var Client = new MongoClient(constr);
            string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
            var DB = Client.GetDatabase(DBname);
            var collection = DB.GetCollection<jobstatus>("jobstatus");
            collection.InsertOne(model);
            return View();
        }

    }
}
