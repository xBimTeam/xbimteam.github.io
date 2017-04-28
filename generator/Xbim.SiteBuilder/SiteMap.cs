using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xbim.SiteBuilder.Structure;

namespace Xbim.SiteBuilder
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SiteMap
    {

        [XmlElement("url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public List<Url> Urls { get; set; } = new List<Url>();

        public void Save(Stream stream)
        {
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stream, this);
        }

        public static SiteMap Load(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(SiteMap));
            return serializer.Deserialize(stream) as SiteMap;
        }

        public void Build(ContentNode rootNode, string rootUrl)
        {
            if (!Urls.Any())
            {
                Urls.Add(new Url { Location = rootUrl, Priority = 1.0F, ChangeFrequency = ChangeFrequency.monthly });
            }

            var dir = rootNode as DirectoryNode;
            if (dir == null && (
                rootNode.RelativeUrl.EndsWith(".html", StringComparison.InvariantCultureIgnoreCase) ||
                rootNode.RelativeUrl.EndsWith(".htm", StringComparison.InvariantCultureIgnoreCase)
                ))
            {

                Urls.Add(new Url { Location = rootUrl + rootNode.RelativeUrl });
            }
            else
            {
                if (rootNode.Children?.Any() == true)
                {
                    foreach (var child in rootNode.Children)
                    {
                        Build(child, rootUrl);
                    }
                }
            }

        }
    }

    [XmlRoot("url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Url
    {
        public Url()
        {
            LastModified = DateTime.Now;
        }

        [XmlElement("loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", Order = 0)]
        public string Location { get; set; }

        [XmlElement("lastmod", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", Order = 1)]
        public string _lastModified { get; set; }

        [XmlElement("changefreq", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", Order = 2)]
        public ChangeFrequency ChangeFrequency { get; set; } = ChangeFrequency.weekly;

        [XmlElement("priority", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", Order = 3)]
        public float Priority { get; set; } = 0.5f;

        [XmlIgnore]
        public DateTime LastModified {
            get
            {
                return DateTime.Parse(_lastModified, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);
            }
            set
            {
                _lastModified = value.ToString("O").Substring(0, 10);
            }
        }

    }

    public enum ChangeFrequency
    {
        always,
        hourly,
        daily,
        weekly,
        monthly,
        yearly,
        never
    }


}
