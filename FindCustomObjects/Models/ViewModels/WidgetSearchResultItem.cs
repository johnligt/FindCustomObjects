using System;
using System.Collections.Generic;
using System.Globalization;
using EPiServer.Core;
using EPiServer.Find;

namespace FindCustomObjects.Models.ViewModels
{
    public class WidgetSearchResultItem : ILocalizable
    {
        [Id]
        public string Id { get; set; }

        public string WidgetName { get; set; }

        public string WidgetDescription { get; set; }

        public string SiteId { get; set; }

        public DateTime DateIndexed { get; set; }

        public string SearchTitle { get; set; }

        public string SearchHitUrl => $"/{MasterLanguage.TwoLetterISOLanguageName}/widget/?widgetid={Id}";

        public string SearchSection { get; set; }

        public IEnumerable<string> SearchCategories { get; set; }

        public string SearchText => $"{WidgetName} {WidgetDescription}";

        public string SearchTypeName => nameof(WidgetSearchResultItem);

        public string SearchHitTypeName => nameof(WidgetSearchResultItem);

        CultureInfo ILocale.Language
        {
            get => MasterLanguage;
            set => MasterLanguage = value;
        }

        public IEnumerable<CultureInfo> ExistingLanguages { get; set; }
        public CultureInfo MasterLanguage { get; set; }        
    }
}