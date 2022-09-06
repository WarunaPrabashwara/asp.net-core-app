
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using mvcweb.ViewModels;

using Microsoft.AspNetCore.Authorization; // 👈 new code
using Microsoft.AspNetCore.Authentication.Cookies; // 👈 new code
using mvcweb.Models;
using MongoDB.Driver;
using MongoDB.Bson;

public class AccountController : Controller
{
    public async Task Login(string returnUrl = "/")
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [Authorize]
    public async Task Logout()
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(Url.Action("Index", "Home"))
            .Build();

        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }



    [Authorize]
    public IActionResult Profile()
    {
        string constr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["ConnectionString"];
        var Client = new MongoClient(constr);
        string DBname = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("db")["DatabaseName"];
        var DB = Client.GetDatabase(DBname);
        string useremail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        var builderr = Builders<users>.Filter;
        var filter = builderr.Eq("uniqueId", useremail);
        var collection = DB.GetCollection<users>("users").Find(filter).ToList();


        


        var builder2 = Builders<jobstatus>.Filter;
        var filter2 = builder2.Eq("driverid", useremail)  ; 
        var collection3 = DB.GetCollection<jobstatus>("jobstatus").Find(filter2).ToList();

        List<jobstatus> collection2     = new List<jobstatus>();
        var AccBalance = 0;

        foreach (jobstatus item in collection3)
        {
            if(item.paywith != null && item.paywith != "cash")
            {
                collection2.Add(item);
            }
           
        }


        

        foreach( jobstatus item in collection2)
        {
            AccBalance = AccBalance + Int16.Parse(item.amountPayable) ;
        }

        ViewBag.AccBalance = AccBalance;

        return View(new UserProfileViewModel()
        {
            Name = User.Identity.Name,
            EmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
            ProfileImage = User.FindFirst(c => c.Type == "picture")?.Value,
            collectionofuserdeatail = collection[0]
        });
    }

}