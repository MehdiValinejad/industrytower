using IndustryTower.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace IndustryTower.Controllers
{
    public class CanvasRec
    {
        public frame[] frames { get; set; }
        public string Token { get; set; }
    }

    public class frame
    {
        public string F { get; set; }
        public string name { get; set; }
    }

    public class WebpSave
    {
        public async Task writeWebp(CanvasRec value)
        {
            foreach (var item in value.frames)
            {
                var decompressed = LZString.decompressFromUTF16(item.F);
                byte[] data = Convert.FromBase64String(decompressed);
                var path = HttpContext.Current.Server.MapPath("~/Uploads/Webinar/" + value.Token + "/Frames/" + item.name + ".jpg");
                using (FileStream st = new FileStream(path, FileMode.Create))
                {
                    await st.WriteAsync(data, 0, data.Length);
                }
            }
        }
    }


    public class WimgRecController : ApiController
    {

        //[HttpPost]
        //async public Task Webp(CanvasRec value)
        //{
        //    WebpSave wps = new WebpSave();
        //    await wps.writeWebp(value);
        //}


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        async public Task Post(CanvasRec value)
        {
            WebpSave wps = new WebpSave();
            await wps.writeWebp(value);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}