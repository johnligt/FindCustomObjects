using EPiServer.Find.Framework;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using FindCustomObjects.Models.ViewModels;

namespace FindCustomObjects.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class FindInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var client = SearchClient.Instance;

            client.Conventions.UnifiedSearchRegistry
                .Add<WidgetSearchResultItem>();            
        }

        public void Uninitialize(InitializationEngine context)
        {
        }        
    }
}
