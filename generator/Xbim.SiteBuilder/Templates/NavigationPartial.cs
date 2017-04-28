using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.SiteBuilder.Structure;

namespace Xbim.SiteBuilder.Templates
{
    public partial class Navigation
    {
        private bool _isRoot;
        private ContentNode _node;
        private ContentNode _activeNode;
        private IEnumerable<ContentNode> Children => GetChildren(_node);
        IEnumerable<IGrouping<string, ContentNode>> ChildrenGroups => 
            Children.GroupBy(n => n.Settings?.MenuGroup)
            .OrderBy(g => g.FirstOrDefault().Settings?.MenuGroupOrder)
            .ThenBy(g => g.Key);

        public Navigation(ContentNode node, bool isRoot, ContentNode activeNode)
        {
            _node = node;
            _activeNode = activeNode;
            _isRoot = isRoot;
        }

        /// <summary>
        /// Returns only content nodes and directory nodes which should appear in menu, ordered by the order setting and by the name
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private IEnumerable<ContentNode> GetChildren(ContentNode node)
        {
            return node.Children
                    .Where(c => c.Settings != null && c.Settings.Order >= 0)
                    .OrderBy(c => c.Settings.Order)
                    .ThenBy(c => c.UrlName);
        }

        private IEnumerable<ContentNode> GetChildren(IGrouping<string, ContentNode> group)
        {
            return group
                    .Where(c => c.Settings != null && c.Settings.Order >= 0)
                    .OrderBy(c => c.Settings.Order)
                    .ThenBy(c => c.UrlName);
        }


    }
}
