using System.Collections.Generic;
using FindCustomObjects.Models.CustomContent;

namespace FindCustomObjects.Business.Services
{
    public interface IWidgetRepository
    {
        /// <summary>
        /// Simulates obtaining the original widget objects from a database or other data source.
        /// </summary>
        /// <returns></returns>
        List<Widget> GetWidgets(int numberOfWidgetsRequired);

        Widget GetWidget(int id);
    }
}