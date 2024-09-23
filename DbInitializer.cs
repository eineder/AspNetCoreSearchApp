using AspNetCoreSearchApp.Data;
using AspNetCoreSearchApp.Models;

public static class DbInitializer
{
    public static void Seed(SearchContext context)
    {
        context.JsonDocuments.RemoveRange(context.JsonDocuments);
        context.JsonSchemas.RemoveRange(context.JsonSchemas);

        // Add sample schemas
        var schema1 = new JsonSchema
        {
            SchemaName = "Person",
            SchemaData = @"{
                    'type': 'object',
                    'properties': {
                        'name': { 'type': 'string' },
                        'age': { 'type': 'number' },
                        'status': { 'type': 'string', 'enum': ['Active', 'Inactive', 'Pending'] }
                    },
                    'required': ['name', 'age']
                }"
        };

        var schema2 = new JsonSchema
        {
            SchemaName = "Product",
            SchemaData = @"{
                    'type': 'object',
                    'properties': {
                        'productName': { 'type': 'string' },
                        'price': { 'type': 'number' },
                        'inStock': { 'type': 'boolean' }
                    },
                    'required': ['productName', 'price']
                }"
        };

        context.JsonSchemas.AddRange(schema1, schema2);
        context.SaveChanges();

        // Add sample documents and link them to schemas
        context.JsonDocuments.AddRange(
            new JsonDoc
            {
                DocumentName = "Sample1",
                JsonData = "{\"name\": \"John Doe\", \"age\": 30, \"status\": \"Active\"}",
                JsonSchemaId = schema1.Id // Link to PersonSchema
            },
            new JsonDoc
            {
                DocumentName = "Sample2",
                JsonData = "{\"productName\": \"Widget\", \"price\": 19.99, \"inStock\": true}",
                JsonSchemaId = schema2.Id // Link to ProductSchema
            }
        );
        context.SaveChanges();
    }

}
