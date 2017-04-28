using Xbim.SiteBuilder.Structure;

namespace Xbim.SiteBuilder.Templates
{
    public interface IPageTemplate
    {
        string TransformText(ContentNode contentNode);
         
        string Name { get; }

    }
}
