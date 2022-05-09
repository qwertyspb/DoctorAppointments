using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Tags;

public class PageLinkTagHelper : TagHelper
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }
    public PageViewModel PageModel { get; set; }
    public string PageAction { get; set; }

    [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
    public Dictionary<string, object> PageUrlValues { get; set; } = new();

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
        output.TagName = "nav";
        output.Attributes.Add("aria-label", "Page navigation");

        var tag = new TagBuilder("ul");
        tag.AddCssClass("pagination");
        tag.AddCssClass("justify-content-center");

        tag.InnerHtml.AppendHtml(CreatePrevNextTag(PageModel.PageNumber, urlHelper, true));

        foreach (var page in PageModel.Pages)
            tag.InnerHtml.AppendHtml(page == -1 ? CreateDotsTag() : CreateTag(page, urlHelper));

        tag.InnerHtml.AppendHtml(CreatePrevNextTag(PageModel.PageNumber, urlHelper, false));

        output.Content.AppendHtml(tag);
    }

    private TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
    {
        var item = new TagBuilder("li");
        var link = new TagBuilder("a");
        if (pageNumber == PageModel.PageNumber)
            item.AddCssClass("active");
        else
        {
            PageUrlValues["page"] = pageNumber;
            link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
        }
        item.AddCssClass("page-item");
        link.AddCssClass("page-link");
        link.InnerHtml.Append(pageNumber.ToString());
        item.InnerHtml.AppendHtml(link);
        return item;
    }

    private TagBuilder CreateDotsTag()
    {
        var item = new TagBuilder("li");
        item.AddCssClass("page-item");
        item.AddCssClass("disabled");

        var span = new TagBuilder("span");
        span.AddCssClass("page-link");
        span.Attributes["aria-hidden"] = "true";

        span.InnerHtml.Append("...");
        item.InnerHtml.AppendHtml(span);

        return item;
    }

    private TagBuilder CreatePrevNextTag(int pageNumber, IUrlHelper urlHelper, bool prev)
    {
        var item = new TagBuilder("li");
        var link = new TagBuilder("a");
        var span = new TagBuilder("span");

        string spanText;
        if (prev)
        {
            spanText = "&laquo;";
            link.Attributes["aria-label"] = "Previuos";
            if (PageModel.HasPreviousPage)
            {
                PageUrlValues["page"] = pageNumber - 1;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }
            else
            {
                item.AddCssClass("disabled");
                link.Attributes["href"] = "#";
            }
        }
        else
        {
            spanText = "&raquo;";
            link.Attributes["aria-label"] = "Next";
            if (PageModel.HasNextPage)
            {
                PageUrlValues["page"] = pageNumber + 1;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }
            else
            {
                item.AddCssClass("disabled");
                link.Attributes["href"] = "#";
            }
        }

        span.Attributes["aria-hidden"] = "true";
        span.InnerHtml.AppendHtml(spanText);
        item.AddCssClass("page-item");
        link.AddCssClass("page-link");
        
        link.InnerHtml.AppendHtml(span);
        item.InnerHtml.AppendHtml(link);
        
        return item;
    }
}