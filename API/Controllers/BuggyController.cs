using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Infastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("NotFound")]
        public ActionResult  GetNotFoundRequest(){
           var thing = _context.Products.Find(70);
           if(thing == null){
               return NotFound(new ApiResponse(404));
           }
            return Ok();
        }

        [HttpGet("ServerError")]
        public ActionResult  GetServerError(){
           var thing = _context.Products.Find(70);
           
           // this line throw a server error because thing is null
           var thingToReturn = thing.ToString();
         
            return Ok();
        }

         [HttpGet("BadRequest")]
        public ActionResult  GetBadRequest(){
        
            return BadRequest(new ApiResponse(400));
        }

         [HttpGet("BadRequest/{id}")]
        public ActionResult  GetBadRequest(int id){
        
            return BadRequest();
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText(){
            return "secret stuff";
        }
    }
}