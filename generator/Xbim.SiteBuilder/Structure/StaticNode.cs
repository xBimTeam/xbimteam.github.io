using System;
using System.IO;

namespace Xbim.SiteBuilder.Structure
{
    public class StaticNode: ContentNode
    {
        public StaticNode(FileInfo file, DirectoryNode dir)
        {
            var path = string.IsNullOrWhiteSpace(dir.RelativePath) ? "" : dir.RelativePath;
            var name = Path.GetFileNameWithoutExtension(file.Name).URLFriendly();

            Directory = file.Directory;
            RootDirectory = dir.RootDirectory;
            SourcePath = file.FullName;
            UrlName = name;
            RelativePath = path + name + file.Extension;
            Title = name;
        }

        public override PageSettings Settings => null;

        public override void Render(DirectoryInfo webRoot)
        {
            var file = Path.Combine( webRoot.FullName, RelativePath);
            //just copy the file
            File.Copy(SourcePath, file, true);
        }
    }
}
