using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Saturn72.BlogPostExtensions.Models;

public class BlogPostExtensionsConfigurationModel
{
    [NopResourceDisplayName("Plugins.BlogPostAttributes.Fields.MaxSubtitleLength")]
    [Range(1, 2056, ErrorMessage = "Max subtitle length must be between 1 and 2056.")]
    public int MaxSubtitleLength { get; set; }

    [NopResourceDisplayName("Plugins.BlogPostAttributes.Fields.MinSubtitleLength")]
    [Range(0, 2056, ErrorMessage = "Min subtitle length must be between 0 and 2056.")]
    public int MinSubtitleLength { get; set; }
}
