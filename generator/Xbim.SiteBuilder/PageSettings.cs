namespace Xbim.SiteBuilder
{
    public class PageSettings
    {
        public bool UseContainer { get; set; } = true;
        public int Order { get; set; } = 0;
        public string MenuGroup { get; set; } = null;
        public int MenuGroupOrder { get; set; } = 0;
        public bool ShowInMenu => Order < 0;
        public bool ShowBanner { get; set; } = false;
        public string BannerContent { get; set; } = "";
        public string Title { get; set; }
        public string Template { get; set; } = "Layout";
        public string ExternalLink { get; set; } = null;
    }
}
