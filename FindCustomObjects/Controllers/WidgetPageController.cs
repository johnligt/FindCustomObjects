using System.Web.Mvc;
using FindCustomObjects.Business.Services;
using FindCustomObjects.Models.Pages;
using FindCustomObjects.Models.ViewModels;

namespace FindCustomObjects.Controllers
{
    public class WidgetPageController : PageControllerBase<WidgetPage>
    {
        private readonly IWidgetRepository _widgetRepository;

        public WidgetPageController(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        public ActionResult Index(WidgetPage currentPage)
        {
            var model = new WidgetPageViewModel(currentPage);

            PageViewModel.SetOtherLanguage(model, currentPage);

            var widgetId = Request["WidgetId"];

            var (language, id) = GetId(widgetId);

            if (string.IsNullOrEmpty(language) || id < 0)
            {
                return View(model);
            }

            model.Widget = _widgetRepository.GetWidget(id);

            switch (language)
            {
                case "en":
                    model.WidgetDescription = model.Widget.WidgetDescriptionEn;
                    break;
                case "nl":
                    model.WidgetDescription = model.Widget.WidgetDescriptionNl;
                    break;
            }

            return View(model);
        }

        private static (string, int) GetId(string widgetId)
        {
            if (string.IsNullOrEmpty(widgetId))
            {
                return (string.Empty, int.MinValue);
            }

            if (!widgetId.StartsWith("en") && !widgetId.StartsWith("nl"))
            {
                return (string.Empty, int.MinValue);
            }

            var elements = widgetId.Split('-');

            if (elements.Length != 2)
            {
                return (string.Empty, int.MinValue);
            }

            if (!int.TryParse(elements[1], out var id))
            {
                return (string.Empty, int.MinValue);
            }

            return (elements[0], id);
        }

    }
}