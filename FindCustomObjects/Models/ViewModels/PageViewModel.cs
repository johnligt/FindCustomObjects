using System.Linq;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using FindCustomObjects.Models.Pages;

namespace FindCustomObjects.Models.ViewModels
{
    public class PageViewModel<T> : IPageViewModel<T> where T : SitePageData
    {
        public PageViewModel(T currentPage)
        {
            CurrentPage = currentPage;

            PageViewModel.SetOtherLanguage(this, currentPage);
        }

        public T CurrentPage { get; private set; }
        public LayoutModel Layout { get; set; }
        public IContent Section { get; set; }
        public string OtherLanguageUrl { get; set; }
        public string OtherLanguageAbbreviation { get; set; } 
    }

    public static class PageViewModel
    {
        /// <summary>
        /// Returns a PageViewModel of type <typeparam name="T"/>.
        /// </summary>
        /// <remarks>
        /// Convenience method for creating PageViewModels without having to specify the type as methods can use type inference while constructors cannot.
        /// </remarks>
        public static PageViewModel<T> Create<T>(T page) where T : SitePageData
        {
            var model = new PageViewModel<T>(page);

            return model;
        }

        public static void SetOtherLanguage<T>(PageViewModel<T> model, T page) where T : SitePageData
        {
            var languageRepository = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>();
            var urlResolver = ServiceLocator.Current.GetInstance<IUrlResolver>();

            var language = languageRepository.ListEnabled()
                .Where(l => !Equals(l.Culture, ContentLanguage.PreferredCulture))
                .Select(l => new LanguageSelector(l.LanguageID))
                .FirstOrDefault();

            if (language != null)
            {
                model.OtherLanguageUrl = urlResolver.GetUrl(page.ContentLink, language.Language.Name);
                model.OtherLanguageAbbreviation = language.Language.TwoLetterISOLanguageName;
            }

        }
    }
}
