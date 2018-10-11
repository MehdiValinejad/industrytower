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
    

    public class Voice
    {
        public string base64 { get; set; }
        public string Token { get; set; }
    }

    

    public class Mp3Save
    {
        public async Task writeMp3(Voice value)
        {
            var decompressed = LZString.decompressFromUTF16(value.base64);
            byte[] data = Convert.FromBase64String(decompressed);
            var path = HttpContext.Current.Server.MapPath("~/uploads/Webinar/" + value.Token + "/Voice.mp3");
            using (FileStream st = new FileStream(path, FileMode.Append))
            {
                await st.WriteAsync(data, 0, data.Length);
            }
        }
    }

    public class WRecController : ApiController
    {

        //[HttpPost]
        //public void Ogg(Voice value)
        //{
        //    //var decompressed = LZString.decompressFromUTF16(value.base64);
        //    byte[] data = Convert.FromBase64String(value.base64);
        //    var path = HttpContext.Current.Server.MapPath("~/uploads/Webinar/" + value.Token + "/Voice.ogg");
        //    using (FileStream st = new FileStream(path, FileMode.Append))
        //    {
        //        st.Write(data, 0, data.Length);
        //    }
        //}

        //[HttpPost]
        //public void mp3(Voice value)
        //{
        //    //Mp3Save wps = new Mp3Save();
        //    //await Task.WhenAll(wps.writeMp3(value));
        //    //await wps.writeMp3(value);
        //    //var decompressed = LZString.decompressFromUTF16(value.base64);
        //    byte[] data = Convert.FromBase64String(value.base64);
        //    var path = HttpContext.Current.Server.MapPath("~/uploads/Webinar/" + value.Token + "/" + DateTime.Now.ToString("MMddhhmmssffff") + ".mp3");
        //    using (FileStream st = new FileStream(path, FileMode.Create))
        //    {
        //        st.Write(data, 0, data.Length);
        //    }
        //}

        //[HttpPost]
        //async public Task WebP(CanvasRec value)
        //{
        //    foreach (var item in value.frames)
        //    {
        //        var decompressed = LZString.decompressFromUTF16(item.F);
        //        byte[] data = Convert.FromBase64String(decompressed);
        //        var path = HttpContext.Current.Server.MapPath("~/Uploads/Webinar/"+ value.Token +"/Frames/" + item.name + ".jpg");
        //        using (FileStream st = new FileStream(path, FileMode.Create))
        //        {
        //            st.Write(data, 0, data.Length);
        //        }
        //    }
        //}


        //public void Post(Voice value)
        //{
        //    var decompressed = LZString.decompressFromUTF16(value.base64);
        //    byte[] data = Convert.FromBase64String(decompressed);
        //    var path = HttpContext.Current.Server.MapPath("~/uploads/Webinar" + value.Token + "/Voice.mp3");
        //    using (FileStream st = new FileStream(path, FileMode.Append))
        //    {
        //        st.Write(data, 0, data.Length);
        //    }
        //}


        //[HttpPost]
        //async public Task Mp3(Voice value)
        //{
        //    var decompressed = LZString.decompressFromUTF16(value.base64);
        //    byte[] data = Convert.FromBase64String(decompressed);
        //    var path = HttpContext.Current.Server.MapPath("~/uploads/Webinar" + value.Token + "/Voice.mp3");
        //    using (FileStream st = new FileStream(path, FileMode.Append))
        //    {
        //        st.Write(data, 0, data.Length);
        //    }
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
        [HttpPost]
        public void Post(Voice value)
        {
            var decompressed = LZString.decompressFromUTF16(value.base64);
            byte[] data = Convert.FromBase64String(decompressed);
            var path = HttpContext.Current.Server.MapPath("~/uploads/Webinar/" + value.Token + "/Voice.ogg");
            using (FileStream st = new FileStream(path, FileMode.Append))
            {
                st.Write(data, 0, data.Length);
            }
        }

        // PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}