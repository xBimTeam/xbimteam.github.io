using System;
using System.IO;
using System.Linq;
using System.Text;
using Xbim.SiteBuilder.Templates;

namespace Xbim.SiteBuilder.Structure
{
    public class HtmlNode: ContentNode
    {
        private const string _htmlExtension = ".html";

        private PageSettings _settings;
        public override PageSettings Settings => _settings;

        public HtmlNode(FileInfo file, DirectoryNode dir)
        {
            //fix for the case of multi-extension like .md.txt
            var name = Path.GetFileNameWithoutExtension(file.Name);
            name = Path.GetFileNameWithoutExtension(name);
            var nameSettings = GetSettingsFromName(ref name);

            var path = string.IsNullOrWhiteSpace(dir.RelativePath) ? "" : dir.RelativePath + Path.DirectorySeparatorChar;

            Parent = dir;
            Directory = file.Directory;
            RootDirectory = dir.RootDirectory;
            SourcePath = file.FullName;
            UrlName = name.URLFriendly() + _htmlExtension;
            RelativePath = path + UrlName;
            Title = name;

            var content = File.ReadAllText(SourcePath, Encoding.UTF8);
            _settings = GetSettings(ref content) ?? new PageSettings();
            MergeNameSettings(_settings, nameSettings);
            Content = content;
        }

        public override void Render(DirectoryInfo webRoot)
        {
            //choose the template
            var template = Templates.FirstOrDefault(t => t.Name == Settings.Template);
            if (template == null)
            {
                Console.WriteLine($"Template {Settings.Template} required in {UrlName} doesn't exist. Default layout will be used.");
                template = new Layout();
            }

            //render template and content
            var content = template.TransformText(this);

            //fix root absolute paths to relative path - levels up to get to root
            content = MakeRelativePaths(content, Depth, RootNode, this, webRoot);
            content = MakeExternalLinksOpenBlank(content);

            //save the result relative to root directory
            var htmlFile = Path.Combine(webRoot.FullName, RelativePath);
            File.WriteAllText(htmlFile, content, Encoding.UTF8);
        }
    }
}
