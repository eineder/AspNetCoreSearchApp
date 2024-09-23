using AspNetCoreSearchApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MyApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        public class Operator
        {
            public string Name { get; set; }
            public Func<string, string> SqlOperator { get; set; }
        }

        readonly Dictionary<string, Dictionary<string, Operator>> searchTypes = new Dictionary<string, Dictionary<string, Operator>>
        {
            { "string", new Dictionary<string, Operator>
                {
                    { "equals", new Operator { Name = "equals", SqlOperator = (value) => $"= '{value}'" } },
                    { "not-equals", new Operator { Name = "not-equals", SqlOperator = (value) => $"<> '{value}'" } },
                    { "startsWith", new Operator { Name = "startsWith", SqlOperator = (value) => $"LIKE '{value}%'" } },
                    { "endsWith", new Operator { Name = "endsWith", SqlOperator = (value) => $"LIKE '%{value}'" } },
                    { "contains", new Operator { Name = "contains", SqlOperator = (value) => $"LIKE '%{value}%'" } }
                }
            },
            { "number", new Dictionary<string, Operator>
                {
                    { "equals", new Operator { Name = "equals", SqlOperator = (value) => $"= {value}" } },
                    { "not-equals", new Operator { Name = "not-equals", SqlOperator = (value) => $"<> {value}" } },
                    { "greater-than", new Operator { Name = "greater-than", SqlOperator = (value) => $"> {value}" } },
                    { "less-than", new Operator { Name = "less-than", SqlOperator = (value) => $"< {value}" } },
                    { "greater-equal", new Operator { Name = "greater-equal", SqlOperator = (value) => $">= {value}" } },
                    { "less-equal", new Operator { Name = "less-equal", SqlOperator = (value) => $"<= {value}" } }
                }
            },
            { "enum", new Dictionary<string, Operator>
                {
                    { "equals", new Operator { Name = "equals", SqlOperator = (value) => $"= '{value}'" } },
                    { "not-equals", new Operator { Name = "not-equals", SqlOperator = (value) => $"<> '{value}'" } },
                }
            }
        };


        private readonly SearchContext _context;

        public SearchController(SearchContext context)
        {
            _context = context;
        }

        [HttpGet("types")]
        public ActionResult<IEnumerable<string>> GetJsonSchemas()
        {
            var schemas = _context.JsonSchemas.Select(p => p.SchemaName).ToList();
            return Ok(schemas);
        }


        // POST: api/JsonDocuments/filter
        [HttpPost("filter")]
        public ActionResult<IEnumerable<object>> FilterJsonDocuments([FromBody] Filter filter)
        {
            var query = BuildSqlQuery(filter);

            var results = _context.JsonDocuments
                .FromSqlRaw(query)
                .Select(p => new
                {
                    p.Id,
                    p.DocumentName,
                    p.JsonData
                })
                .ToList();

            return results;
        }

        private string BuildSqlQuery(Filter filter)
        {
            var schema = _context.JsonSchemas.Single(js => js.SchemaName == filter.Type);
            var schemaJson = JObject.Parse(schema.SchemaData);
            var properties = schemaJson["properties"];

            if (properties == null)
                throw new ArgumentException("Invalid schema: 'properties' not found");

            var whereClauses = new List<string>();

            foreach (var criteria in filter.Criteria)
            {
                var sqlOperation = GetSqlOperation(criteria.Field, criteria.Operator, properties, criteria.Value);

                var clause = $"json_extract(JsonData, '$.{criteria.Field}') {sqlOperation}";
                whereClauses.Add(clause);
            }

            var whereSql = string.Join(" AND ", whereClauses.Prepend("1 = 1"));
            var query = $"SELECT * FROM JsonDocuments where {whereSql}";

            return query;
        }

        private string GetSqlOperation(string fieldName, string operatorName, JToken properties, string value)
        {
            var type = GetType(fieldName, properties);

            if (!searchTypes.TryGetValue(type, out var operators))
                throw new ArgumentException($"Unknown type: {type}");

            if (!operators.TryGetValue(operatorName, out var operation))
                throw new ArgumentException($"Unknown operator: {operatorName}");

            return operation.SqlOperator(value);
        }

        private static string GetType(string fieldName, JToken properties)
        {
            var field = properties[fieldName];

            if (field == null)
                throw new ArgumentException($"Field '{fieldName}' not found in schema");

            var fieldType = field["type"]?.ToString();

            if (string.IsNullOrEmpty(fieldType))
                throw new ArgumentException($"Type for field '{fieldName}' not found in schema");

            return fieldType;
        }

        public class Filter
        {
            /// <summary>
            /// The schema type to filter
            /// </summary>
            /// <example>Person</example>
            public string Type { get; set; }

            /// <summary>
            /// The filter criteria
            /// </summary>
            /// <example>[{"Field":"name","Operator":"startsWith","Value":"Jo"}]</example>
            public List<FilterCriteria> Criteria { get; set; }
        }

        public class FilterCriteria
        {
            public string Field { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
        }
    }
}
