using System.Collections.Generic;
using FindCustomObjects.Models.CustomContent;

namespace FindCustomObjects.Business.Services
{
    public class WidgetRepository : IWidgetRepository
    {
        /// <summary>
        /// Simulates obtaining the original widget object from a database or other data source.
        /// </summary>
        /// <returns></returns>
        public List<Widget> GetWidgets(int numberOfWidgetsRequired)
        {
            var widgets = new List<Widget>();

            for (var i = 0; i < numberOfWidgetsRequired; i++)
            {
                var widget = GetWidget(i);

                widgets.Add(widget);
            }

            return widgets;
        }


        public Widget GetWidget(int id)
        {
            var widget = new Widget
            {
                Id = id,
                WidgetName = $"Widget {id}",
                WidgetDescriptionNl = $"Dutch description of widget {id}",
                WidgetDescriptionEn = $"English description of widget {id}"
            };

            return widget;
        }
    }
}