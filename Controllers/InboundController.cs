using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGridProcessor.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace SendGridProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboundController : ControllerBase
    {
        // POST api/inbound
       // [HttpPost]
      /*
        public async Task<HttpResponseMessage> Post()
        {
            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            await Request.Content.ReadAsMultipartAsync(provider);

            var email = new Email
            {
                Dkim = provider.FormData.GetValues("dkim").FirstOrDefault(),
                To = provider.FormData.GetValues("to").FirstOrDefault(),
                Html = provider.FormData.GetValues("html").FirstOrDefault(),
                From = provider.FormData.GetValues("from").FirstOrDefault(),
                Text = provider.FormData.GetValues("text").FirstOrDefault(),
                SenderIp = provider.FormData.GetValues("sender_ip").FirstOrDefault(),
                Envelope = provider.FormData.GetValues("envelope").FirstOrDefault(),
                Attachments = int.Parse(provider.FormData.GetValues("attachments").FirstOrDefault()),
                Subject = provider.FormData.GetValues("subject").FirstOrDefault(),
                Charsets = provider.FormData.GetValues("charsets").FirstOrDefault(),
                Spf = provider.FormData.GetValues("spf").FirstOrDefault()
            };

            // The email is now stored in the email variable

            return new HttpResponseMessage(HttpStatusCode.OK);
        }*/

    }
}
