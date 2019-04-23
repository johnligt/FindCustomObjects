using System.Collections.Generic;
using FindCustomObjects.Models.CustomContent;
using FindCustomObjects.Models.ViewModels;

namespace FindCustomObjects.Business.Services
{
    public interface IWidgetSearchResultItemService
    {
        List<WidgetSearchResultItem> GetListToPushToTheIndex(List<Widget> widgets);
    }
}