using ColorCode;
using ColorCode.Formatting;
using HeyRed.MarkdownSharp;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xbim.SiteBuilder.Templates;

namespace Xbim.SiteBuilder.Structure
{
    public class MarkdownNode : ContentNode
    {
        private const string _htmlExtension = ".html";
        private static Markdown md = new Markdown();
        private static CodeColorizer cc = new CodeColorizer();

        public MarkdownNode(FileInfo file, DirectoryNode dir)
        {
            //fix for the case of multi-extension like .md.txt
            var name = Path.GetFileNameWithoutExtension(file.Name);
            name = Path.GetFileNameWithoutExtension(name);
            var nameSettings = GetSettingsFromName(ref name);


            var path = string.IsNullOrWhiteSpace(dir.RelativePath) ? "" : dir.RelativePath;

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

            //transform tables which are not supported in this Markdown processor
            content = TransformTables(content);

            //transform code blocks as in GFM, render HTML highlighting
            content = TransformCodeBlocks(content);

            //transform markdown syntax
            content = md.Transform(content);
            Content = content;
        }

        

        private PageSettings _settings;
        public override PageSettings Settings => _settings;

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
            var htmlFile = Path.Combine( webRoot.FullName, RelativePath);
            File.WriteAllText(htmlFile, content, Encoding.UTF8);
        }

        private static string TransformTables(string content)
        {
            var reader = new StringReader(content);
            var writer = new StringWriter();

            //find tables
            var line = reader.ReadLine();
            var inTable = false;
            while (line != null)
            {
                if (!line.StartsWith("|"))
                {
                    inTable = false;
                    writer.WriteLine(line);
                    line = reader.ReadLine();
                    continue;
                }

                //check next line for context
                var nextLine = reader.ReadLine();

                //start table
                if (!inTable)
                {
                    writer.WriteLine("<table class=\"table\">");
                    inTable = true;

                    //line is a header
                    if (nextLine.StartsWith("|---"))
                    {
                        line = line.Trim('|', ' ');
                        writer.WriteLine("<thead>");
                        line = "<tr><th>" + line.Replace("|", "</th><th>") + "</th></tr>";
                        writer.WriteLine(line);
                        writer.WriteLine("</thead>");
                        writer.WriteLine("<tbody>");
                        line = reader.ReadLine();
                        continue;
                    }
                    else
                    {
                        writer.WriteLine("<tbody>");
                    }
                }


                //end of table
                if (nextLine == null || !nextLine.StartsWith("|"))
                {
                    writer.WriteLine("</tbody>");
                    writer.WriteLine("</table>");
                    line = nextLine;
                    continue;
                }

                //write normal table row
                line = line.Trim('|', ' ');
                line = "<tr><td>" + 
                    string.Join("</td><td>", line.Split('|').Select(col => md.Transform(col))) + 
                    "</td></tr>";
                writer.WriteLine(line);
                line = nextLine;
            }

            return writer.ToString();

        }

        private static string TransformCodeBlocks(string content)
        {
            var reader = new StringReader(content);
            var writer = new StringWriter();
            var code = new StringWriter();
            var line = reader.ReadLine();
            var lang = "";
            var inCodeBlock = false;
            while (line != null)
            {
                if (!line.StartsWith("```"))
                {
                    if (inCodeBlock)
                        code.WriteLine(line);
                    else
                        writer.WriteLine(line);
                    line = reader.ReadLine();
                    continue;
                }

                if (!inCodeBlock)
                {
                    lang = line.Trim('`', ' ');
                    inCodeBlock = true;
                }
                else
                {
                    var codeContent = code.ToString();
                    if (lang.Equals("cs", StringComparison.InvariantCultureIgnoreCase) || lang.Equals("c#", StringComparison.InvariantCultureIgnoreCase))
                        cc.Colorize(codeContent, Languages.CSharp, new HtmlClassFormatter(), StyleSheets.Default, writer);
                    else if (lang.Equals("js", StringComparison.InvariantCultureIgnoreCase))
                        cc.Colorize(codeContent, Languages.JavaScript, new HtmlClassFormatter(), StyleSheets.Default, writer);
                    else if (lang.Equals("step", StringComparison.InvariantCultureIgnoreCase))
                        cc.Colorize(codeContent, Languages.Step, new HtmlClassFormatter(), StyleSheets.Default, writer);
                    else if (lang.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                        cc.Colorize(codeContent, Languages.Xml, new HtmlClassFormatter(), StyleSheets.Default, writer);
                    else
                    {
                        codeContent = $"<pre><code>{codeContent}</code></pre>";
                        writer.Write(codeContent);
                    }


                    //clear
                    code = new StringWriter();
                    inCodeBlock = false;
                    lang = "";
                }

                line = reader.ReadLine();
            }
            return writer.ToString();
        }
    }
}
