using AspNetCoreSearchApp.Models;
using AspNetCoreSearchApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                    { "startsWith", new Operator { Name = "startsWith", SqlOperator = (value) => $"LIKE {value}%" } },
                    { "endsWith", new Operator { Name = "endsWith", SqlOperator = (value) => $"LIKE %{value}" } },
                    { "contains", new Operator { Name = "contains", SqlOperator = (value) => $"LIKE %{value}%" } }
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

        // POST: api/JsonDocuments/filter
        [HttpPost("filter")]
        public ActionResult<IEnumerable<JsonDoc>> FilterJsonDocuments([FromBody] List<FilterCriteria> filters)
        {
            var query = BuildSqlQuery(filters);

            var results = _context.JsonDocuments
                .FromSqlRaw(query)
                .ToList();

            return results;
        }

        private string BuildSqlQuery(List<FilterCriteria> filters)
        {
            var whereClauses = new List<string>();

            foreach (var filter in filters)
            {
                var sqlOperator = GetSqlOperator(filter.Operator);
                var value = filter.Value;

                if (filter.Operator == "startsWith")
                {
                    value = $"{value}%";
                }
                else if (filter.Operator == "endsWith")
                {
                    value = $"%{value}";
                }
                else if (filter.Operator == "contains")
                {
                    value = $"%{value}%";
                }

                var clause = $"json_extract(JsonData, '$.{filter.Field}') {sqlOperator} '{value}'";
                whereClauses.Add(clause);
            }

            var whereSql = string.Join(" AND ", whereClauses);
            var query = $"SELECT * FROM JsonDocuments WHERE {whereSql}";

            return query;
        }

        private string GetSqlOperator(string operatorString)
        {
            return operatorString switch
            {
                "equals" => "=",
                "not-equals" => "!=",
                "greater-than" => ">",
                "less-than" => "<",
                "greater-equal" => ">=",
                "less-equal" => "<=",
                "startsWith" => "LIKE",
                "endsWith" => "LIKE",
                "contains" => "LIKE",
                _ => throw new ArgumentException($"Unknown operator: {operatorString}")
            };
        }
    }

    public class FilterCriteria
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
