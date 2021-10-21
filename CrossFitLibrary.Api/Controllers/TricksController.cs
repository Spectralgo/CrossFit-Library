using System;
using CrossFitLibrary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CrossFitLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/tricks")]
    public class TricksController : ControllerBase
    {
        private TrickyStore _store;

        public TricksController(TrickyStore store)
        {
            _store = store;
        }

        [HttpGet] 
        public IActionResult All() => Ok(_store.All);
        
        [HttpGet("{id}")] 
        public IActionResult Get(int id) => Ok(_store.All.FirstOrDefault(x => x.Id.Equals(id)));

        [HttpPost]
        public IActionResult Create([FromBody] Trick trick)
        {
          _store.Add(trick);
          return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Trick trick)
        {
            throw new NotImplementedException();
        }
        

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}