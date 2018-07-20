using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Verify")]
    public class VerifyController : Controller
    {
        // GET: api/Verify
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Verify/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Verify
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Verify/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
