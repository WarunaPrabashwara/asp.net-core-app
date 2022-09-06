using mvcweb.Models;

namespace mvcweb.ViewModels; 

public class UserProfileViewModel
{
    public string EmailAddress { get; set; }

    public string Name { get; set; }

    public string ProfileImage { get; set; }
    public users collectionofuserdeatail { get; set; }
}
