using System;
using System.Collections.Generic;
using System.Globalization;
using EPiServer.Web;
using FindCustomObjects.Models.CustomContent;
using FindCustomObjects.Models.ViewModels;

namespace FindCustomObjects.Business.Services
{
    public class WidgetSearchResultItemService : IWidgetSearchResultItemService
    {
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;

        public WidgetSearchResultItemService(ISiteDefinitionRepository siteDefinitionRepository)
        {
            _siteDefinitionRepository = siteDefinitionRepository;
        }

        public List<WidgetSearchResultItem> GetListToPushToTheIndex(List<Widget> widgets)
        {
            const string EnglishSectionName = "Widgets";
            const string DutchSectionName = "Dingetjes";

            // Get the site id for the site, otherwise the items will not be found in the search.
            var site = _siteDefinitionRepository.Get("FindCustomObjects");
            var siteId = site?.Id.ToString();

            var cultureInfoNl = new CultureInfo("nl");
            var cultureInfoEn = new CultureInfo("en");

            var cultureInfoArrayNl = new[] { cultureInfoNl };
            var cultureInfoArrayEn = new[] { cultureInfoEn };

            var listOfItemsToPush = new List<WidgetSearchResultItem>();

            foreach (var widget in widgets)
            {
                var widgetSearchResultItemNl = GetWidgetSearchResultItem(widget, ref cultureInfoNl, ref cultureInfoArrayNl, ref siteId, DutchSectionName, widget.WidgetDescriptionNl );

                listOfItemsToPush.Add(widgetSearchResultItemNl);

                // Workaround for finding the items when searching from the English search page:
                // push the same item again, with MasterLanguage English
                // and a different ID!!!

                var employeeSearchResultItemEn = GetWidgetSearchResultItem(widget, ref cultureInfoEn, ref cultureInfoArrayEn, ref siteId, EnglishSectionName, widget.WidgetDescriptionEn);

                listOfItemsToPush.Add(employeeSearchResultItemEn);               
            }

            return listOfItemsToPush;
        }


        private static WidgetSearchResultItem GetWidgetSearchResultItem(
            Widget widget,
            ref CultureInfo cultureInfo,
            ref CultureInfo[] cultureInfoArray,
            ref string siteId,
            string sectionName,
            string description)
        {
            var widgetSearchResultItem = new WidgetSearchResultItem();

            widgetSearchResultItem.Id = $"{cultureInfo.TwoLetterISOLanguageName}-{widget.Id}";
            widgetSearchResultItem.SearchSection = sectionName;
            widgetSearchResultItem.SiteId = siteId;
            widgetSearchResultItem.DateIndexed = DateTime.Today;
            widgetSearchResultItem.ExistingLanguages = cultureInfoArray;
            widgetSearchResultItem.MasterLanguage = cultureInfo;
            widgetSearchResultItem.SearchTitle = $"{widget.WidgetName} - {cultureInfo.TwoLetterISOLanguageName}";
            widgetSearchResultItem.WidgetDescription = description;
            widgetSearchResultItem.WidgetName = widget.WidgetName;

            return widgetSearchResultItem;
        }

    }
}