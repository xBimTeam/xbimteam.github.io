using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.SiteBuilder.Structure
{
    public class DirectoryNode: ContentNode
    {
        public DirectoryNode(DirectoryInfo dir, DirectoryInfo root, DirectoryNode parent)
        {
            Parent = parent;
            var name = dir.Name;
            _settings = GetSettingsFromName(ref name) ?? new PageSettings() ;

            Directory = dir;
            RootDirectory = root;
            SourcePath = dir.FullName;
            Title = name;
            UrlName = name.URLFriendly();
            RelativePath = parent == null ? "" : parent.RelativePath + UrlName + Path.DirectorySeparatorChar;

            //get all MarkDown and HTML content files and create content nodes
            var files = dir.EnumerateFiles();
            foreach (var file in files)
            {
                var node = CreateContentNode(file);
                Children.Add(node );
            }

            foreach (var subDir in dir.GetDirectories())
            {
                Children.Add(new DirectoryNode(subDir, root, this));
            }
        }

        private PageSettings _settings;

        public override PageSettings Settings => _settings;

        public override void Render(DirectoryInfo webRoot)
        {
            if (Directory != RootDirectory)
            {
                var dir = Path.Combine(webRoot.FullName, RelativePath);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
            }
            
            foreach (var child in Children)
                child.Render(webRoot);
        }

        private ContentNode CreateContentNode(FileInfo file)
        {
            var ext = file.Extension.ToLowerInvariant();
            //spell checking workaround
            if (file.Name.EndsWith(".md.txt", StringComparison.InvariantCultureIgnoreCase))
                ext = ".md.txt";

            switch (ext)
            {
                case ".htm":
                case ".html":
                    return new HtmlNode(file, this);
                case ".md":
                case ".md.txt":
                    return new MarkdownNode(file, this);
                default:
                    return new StaticNode(file, this);
            }
        }
    }
}
