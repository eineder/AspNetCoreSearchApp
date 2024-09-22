using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AspNetCoreSearchApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        // Simulated JSON data
        private static readonly List<string> JsonData = new List<string>
        {
            "{\"name\": \"John Doe\", \"age\": 30, \"city\": \"New York\"}",
            "{\"name\": \"Jane Smith\", \"age\": 40, \"city\": \"Los Angeles\"}",
            "{\"name\": \"Mike Johnson\", \"age\": 25, \"city\": \"New York\"}"
        };

        [HttpPost]
        public IActionResult Search([FromBody] dynamic model)
        {
            var data = JsonData.Select(JObject.Parse).ToList();

            // Apply dynamic filtering
            var results = data.Where(item =>
            {
                bool match = true;
                if (model.name != null)
                    match &= item["name"]?.ToString().Contains(model.name.ToString());
                if (model.age != null)
                    match &= item["age"]?.ToObject<int>() == (int)model.age;
                if (model.city != null)
                    match &= item["city"]?.ToString().Contains(model.city.ToString());

                return match;
            }).ToList();

            return Ok(results);
        }
    }
}
