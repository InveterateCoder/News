using System;
using System.Xml.Linq;
using System.Net;

namespace News
{
    class RSSItem
    {
        private string title, link, description;
        public string Title
        {
            get => title;
            set => title = value;
        }
        public string Link
        {
            get => link;
            set => link = value;
        }
        public string Description
        {
            get => description;
            set => description = value;
        }
        public RSSItem(XElement element)
        {
            Title = WebUtility.HtmlDecode(element.Element(XName.Get("title")).Value);
            Link = WebUtility.HtmlDecode(element.Element(XName.Get("link")).Value);
            Description = WebUtility.HtmlDecode(element.Element(XName.Get("description")).Value);
            
        }
        public RSSItem(string tit, string desc, string addr)
        {
            Title = tit;
            Description = desc;
            Link = addr;
        }
    }
}
