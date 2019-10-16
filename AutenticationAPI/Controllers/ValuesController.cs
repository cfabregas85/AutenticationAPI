using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace AutenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("AllowAPIRequest")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ValuesController : ControllerBase
    {
        //Use this var to protect the test
        private readonly IDataProtector _protector;


        public ValuesController(IDataProtectionProvider protectionProvider)
        {
            _protector = protectionProvider.CreateProtector("Unique_Value");
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {           
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //This is an example how we can use IDataProtectionProvider and IDataProtector classes

            //this var allow to create a time scope
            var protectorLimit = _protector.ToTimeLimitedDataProtector();


            string text = "Carlos Fabregas";
            string textEnc = protectorLimit.Protect(text, TimeSpan.FromSeconds(5));
            //Stop the Process for o seconds
           // Thread.Sleep(6000);
            string text1 = protectorLimit.Unprotect(textEnc);
            //string textEnc = _protector.Protect(text);
            //string text1 = _protector.Unprotect(textEnc);
            return Ok(new { text,textEnc,text1});
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
