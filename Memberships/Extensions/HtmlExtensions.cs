using System.Web.Mvc;

namespace Memberships.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString GlyphLink(this HtmlHelper helper,
            string controller, string action, string text,
            string glyphicon, string cssClasses = "", string id = "")
        {
            // declare a span for glyphicon
            var glyph = string.Format("<span class='glyphicon glyphicon-{0}'></span>",
                glyphicon);

            // declare anchor tag
            var anchor = new TagBuilder("a");
            anchor.MergeAttribute("href", string.Format("/{0}/{1}", controller, action));
            anchor.AddCssClass(cssClasses);
            anchor.GenerateId(id);
            anchor.InnerHtml = glyph + " " + text;

            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
        }
    }
}