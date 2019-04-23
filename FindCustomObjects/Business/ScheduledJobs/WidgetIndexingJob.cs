using System;
using System.Configuration;
using System.Linq;
using System.Text;
using EPiServer.Find;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using FindCustomObjects.Business.Services;
using FindCustomObjects.Models.ViewModels;

namespace FindCustomObjects.Business.ScheduledJobs
{
    [ScheduledPlugIn(
        DisplayName = "Episerver Find Widget Indexing Job",
        Description = "Adds widgets to EPiFind",
        Restartable = true,
        GUID = "364e24a7-3c81-4bc2-9081-7c2733809cda",
        SortIndex = 100000)]
    public class WidgetIndexingJob : ScheduledJobBase 
    {
        private const string StopSignaledMessage = "Requested to stop, give us a second to save our work please...";

        private bool _stopSignaled;
        private readonly IWidgetRepository _widgetRepository;
        private readonly IWidgetSearchResultItemService _widgetSearchResultItemService;

        private readonly IClient _client;        

        public WidgetIndexingJob(IWidgetSearchResultItemService widgetSearchResultItemService, IWidgetRepository widgetRepository, IClient client)
        { 
            IsStoppable = true;
            _widgetSearchResultItemService = widgetSearchResultItemService;
            _widgetRepository = widgetRepository;
            _client = client;            
        }

        public override string Execute()
        {            
            const int NumberOfItemsInBulkIndexAction = 10;

            // Use this if you need to delete (only) Widgets from the index during development.
            //_client.Delete<WidgetSearchResultItem>(x => x.GetType().Name.Exists() | !x.GetType().Name.Exists());

            // Get our widgets from the datasource, through the WidgetRepository
            var widgets = _widgetRepository.GetWidgets(102);

            // Convert them to a format which will give the required 
            // results with a multilingual Unified Search.
            var listOfItemsToPush = _widgetSearchResultItemService.GetListToPushToTheIndex(widgets);

            var count = listOfItemsToPush.Count;
            var i = 0;

            while (i <= count)
            {
                var itemsToPush = listOfItemsToPush.Skip(i).Take(NumberOfItemsInBulkIndexAction);

                // Always push a list of items, never push them one by one, to
                // limit the number of calls to Episerver Find and improve performance.
                _client.Index(itemsToPush);

                i += NumberOfItemsInBulkIndexAction;

                if (_stopSignaled)
                {
                    OnStatusChanged(StopSignaledMessage);
                    break;
                }
            }

            // Clear old items from the index.
            var conversionSucceeded = int.TryParse(ConfigurationManager.AppSettings["MaxAgeIndexedWidgetSearchResultItemInDays"], out var maxAgeIndexedWidgetSearchResultItemInDays);

            var feedbackMessage = new StringBuilder();

            if (!conversionSucceeded) return "Error converting AppSetting to integer for MaxAgeIndexedWidgetSearchResultItemInDays";

            var deleteByQueryResult = _client.Delete<WidgetSearchResultItem>(x => x.DateIndexed.Before(DateTime.Today.AddDays(-maxAgeIndexedWidgetSearchResultItemInDays)));

            if (deleteByQueryResult.Ok)
            {
                feedbackMessage.Append($" Deleted items from the index which were not re-indexed for the last {maxAgeIndexedWidgetSearchResultItemInDays} days.");
            }

            return $"Pushed {count} widgets to the index.";
        }
       
        /// <inheritdoc />
        /// <summary>
        /// Called when a widget clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }
    }
}
