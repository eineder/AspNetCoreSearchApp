using AspNetCoreSearchApp.Data;
using AspNetCoreSearchApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSearchApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonDocsController : ControllerBase
    {
        private readonly SearchContext _context;

        public JsonDocsController(SearchContext context)
        {
            _context = context;
        }

        // GET: api/JsonDocs
        [HttpGet]
        public ActionResult<IEnumerable<JsonDoc>> GetJsonDocs()
        {
            return _context.JsonDocuments.ToList();
        }

        // GET: api/JsonDocs/5
        [HttpGet("{id}")]
        public ActionResult<JsonDoc> GetJsonDoc(int id)
        {
            var JsonDoc = _context.JsonDocuments.Find(id);

            if (JsonDoc == null)
            {
                return NotFound();
            }

            return JsonDoc;
        }

        // POST: api/JsonDocs
        [HttpPost]
        public ActionResult<JsonDoc> PostJsonDoc(JsonDoc JsonDoc)
        {
            _context.JsonDocuments.Add(JsonDoc);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetJsonDoc), new { id = JsonDoc.Id }, JsonDoc);
        }
    }
}
