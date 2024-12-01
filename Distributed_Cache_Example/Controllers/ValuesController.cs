using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed_Cache_Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        [HttpGet("set")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                SlidingExpiration = TimeSpan.FromSeconds(5),
                AbsoluteExpiration = DateTime.Now.AddSeconds(30)
            });
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                SlidingExpiration = TimeSpan.FromSeconds(5),
                AbsoluteExpiration = DateTime.Now.AddSeconds(30)
            });
            return Ok();
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surnameBynary = await _distributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameBynary);
            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
