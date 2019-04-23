using FindCustomObjects.Models.CustomContent;
using FindCustomObjects.Models.Pages;

namespace FindCustomObjects.Models.ViewModels
{
    public class WidgetPageViewModel : PageViewModel<WidgetPage>
    {
        public WidgetPageViewModel(WidgetPage currentPage)
            : base(currentPage)
        {
        }

        public Widget Widget { get; set; }

        public string WidgetDescription { get; set; }      
    }
}
