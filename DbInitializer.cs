using AspNetCoreSearchApp.Data;
using AspNetCoreSearchApp.Models;

public static class DbInitializer
{
    public static void Seed(SearchContext context)
    {
        if (!context.JsonDocuments.Any())
        {
            context.JsonDocuments.AddRange(
                new JsonDoc
                {
                    DocumentName = "Sample1",
                    JsonData = "{\"name\": \"John Doe\", \"age\": 30, \"status\": \"Active\"}"
                },
                new JsonDoc
                {
                    DocumentName = "Sample2",
                    JsonData = "{\"name\": \"Jane Smith\", \"age\": 25, \"status\": \"Inactive\"}"
                }
            );
            context.SaveChanges();
        }
    }
}
