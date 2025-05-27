namespace EryfitProxy.Kernel.Tools.DocGen
{
    public static class FilterDescriptionCategory
    {
        public static string GetCategory(this Type filterType)
        {
            var fullNamespace = filterType.Namespace!;

            if (fullNamespace.Contains("request", StringComparison.OrdinalIgnoreCase)) {
                return "Request";
            }
            
            if (fullNamespace.Contains("response", StringComparison.OrdinalIgnoreCase)) {
                return "Response";
            }

            return "General"; 
        }
    }
}
