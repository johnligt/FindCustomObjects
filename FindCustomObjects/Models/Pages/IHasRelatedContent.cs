using EPiServer.Core;

namespace FindCustomObjects.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
