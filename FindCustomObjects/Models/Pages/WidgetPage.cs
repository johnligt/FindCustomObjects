using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace FindCustomObjects.Models.Pages
{
    [ContentType(DisplayName = "WidgetPage", GUID = "21d29826-6098-45bc-b1cb-c0fc21fb0777", Description = "")]
    public class WidgetPage : SitePageData
    {
        
                [CultureSpecific]
                [Display(
                    Name = "Title",
                    Description = "Title of the page",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual string Title { get; set; }
         
    }
}