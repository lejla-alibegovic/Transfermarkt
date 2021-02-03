using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MimeKit;

namespace Transfermarkt.Web.Controllers
{
    public class EmailController : Controller
    {
        IHostingEnvironment _env;
        public EmailController(IHostingEnvironment e)
        {
            _env = e;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string from,string mess,string subject,IFormFile filee)
        {
            var file="";
            string SendData="";
            if(filee!=null)
            {
                var textpath = @"\TextFiles";
                var uploadpath = _env.WebRootPath + textpath;
                //var filepath = Path.Combine(Directory.GetCurrentDirectory(), uploadpath+filee.FileName);
                string fullpath = uploadpath+"\\" + filee.FileName;

                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await filee.CopyToAsync(stream);
                }
                file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TextFiles", filee.FileName);

                SendData = System.IO.File.ReadAllText(file);
            }


            var mes = new MimeMessage();
            mes.From.Add(new MailboxAddress("", from));
            mes.To.Add(new MailboxAddress("Abedin", "abedinhalilovic12345@gmail.com"));
            mes.Subject = subject;
          
            if(filee != null)
            {
                mes.Body = new TextPart("plain")
                {
                    Text = SendData
                };
            }
            else
            {
                mes.Body = new TextPart("plain")
                {
                    Text = mess
                };
            }
            
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("abedinhalilovic12345@gmail.com", "Civolilah1999");
                client.Send(mes);
                client.Disconnect(true);
                return View("Report");
            }
        }
    }
}