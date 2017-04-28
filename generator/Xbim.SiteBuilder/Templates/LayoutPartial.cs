using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.SiteBuilder.Structure;

namespace Xbim.SiteBuilder.Templates
{
    public partial class Layout: IPageTemplate
    {
        protected string Content => ContentNode.Content;
        protected ContentNode NavigationRoot { get; private set; }
        protected ContentNode ContentNode { get; private set; }

        protected PageSettings Settings => ContentNode.Settings;


        public string Name
        {
            get
            {
                return "Layout";
            }
        }

        public string TransformText(ContentNode contentNode)
        {
            ContentNode = contentNode;
            NavigationRoot = contentNode.RootNode;
            return TransformText();
        }
    }
}
