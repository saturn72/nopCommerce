using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Saturn72.BlogPostExtensions.Models;
public partial record BlogPostAttributesModel
{
    public int BlogPostId { get; set; }

    [NopResourceDisplayName("Plugins.BlogPostAttributes.Fields.Subtitle")]
    public string? Subtitle { get; set; }

    [NopResourceDisplayName("Plugins.BlogPostAttributes.Fields.Picture")]
    [UIHint("Picture")]
    public int PictureId { get; set; }

    [NopResourceDisplayName("Plugins.BlogPostAttributes.Fields.Authors")]
    public List<SelectListItem> SelectedAuthorIds { get; set; } = new();

    public List<SelectListItem> AvailableAuthors { get; set; } = new();
    public int MaxSubtitleLength { get; set; }
    public int MinSubtitleLength { get; set; }
}