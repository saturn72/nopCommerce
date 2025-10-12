namespace Saturn72.BlogPostExtensions.Views;
internal class ViewUtil
{

    private const string VIEW_PATH = "~/Plugins/Saturn72.BlogPostExtensions/Views/";

    /// <summary>
    /// generate cshtml full path
    /// </summary>
    /// <param name="viewNameWithoutExtension">the view name, no cshtml extension is required!!!</param>
    /// <returns>relative path to the view</returns>
    public static string GetViewPath(string viewNameWithoutExtension)
    {
        // Use string.Concat for better performance with known string count
        return string.Concat(VIEW_PATH, viewNameWithoutExtension, ".cshtml");
    }
}
