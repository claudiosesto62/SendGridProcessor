using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendGridProcessor.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System;
using Cloudmersive.APIClient.NETCore.VirusScan.Api;
using Cloudmersive.APIClient.NETCore.VirusScan.Client;
using Cloudmersive.APIClient.NETCore.VirusScan.Model;

namespace SendGridProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncomingEmailController : ControllerBase
    {
        private const string FilesRootPath = "C:\\dev\\SendGridFiles";
        private List<EmailRule> _rules = new List<EmailRule>(); // cs con los parametros a incluir

        private bool VirusCheck(System.IO.Stream inputFile)
        {
            var apiInstance = new ScanApi();
            // Configure API key authorization: Apikey
            //Configuration.AddApiKey = "33b4df6a-6c51-48f6-bdfd-63925e3dd8c0";
            apiInstance.Configuration.AddApiKey("Apikey", "33b4df6a-6c51-48f6-bdfd-63925e3dd8c0");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
           // apiInstance.Configuration.AddApiKeyPrefix("Apikey", "Bearer");




            // var inputFile = new System.IO.Stream(); // System.IO.Stream | Input file to perform the operation on.

            try
            {
                // Scan a file for viruses
                VirusScanResult result = apiInstance.ScanFile(inputFile);
                //Debug.WriteLine(result);
                Console.WriteLine(result);
                return true;


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when calling ScanApi.ScanFile: " + e.Message);
                return false;
            }
        }

        private static async Task MailToSG(string mensaje, string paraStr)
        {
            var arrFrom = paraStr.Split("<");
            var apiKey = "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";// Environment.GetEnvironmentVariable("SENDGRID_API_KEY"); // "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"; //Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("csesto@planexware.com", "Canal de Email"); // tiene que ser un sender declarado en el dashboard
            var subject = "Planexware te Responde";
            var to = new EmailAddress(arrFrom[1].Replace(">",""), "Usuario Originante");
            var plainTextContent = "";
            var htmlContent = "<strong>" + mensaje +"</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public IncomingEmailController()
        {
            _rules.Add(new EmailRule // la cs conserva la colección de atributos para control
            {
                AttachmentName = "FacturaCoto.pdf",
                Subject = "Demo Coto",
                To = "coto@em533.krikos360.com",
                FolderName = "COTO",
            });
            _rules.Add(new EmailRule
            {
                AttachmentName = "FacturaAdidas.pdf",
                Subject = "Demo Adidas",
                To = "adidas@em533.krikos360.com",
                FolderName = "Adidas",
            });
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string[] emailToAddresses = Request.Form["to"]; // el post es form-multipart
            string emailSubject = Request.Form["Subject"];
            var files = Request.Form.Files;
            bool existSubjectInRules = _rules.Any(x => x.Subject == emailSubject); // compara si es valido para la policy
             // compara si es valido para la policy

            if (existSubjectInRules)
            {
                foreach (string emailTo in emailToAddresses) // por cada direccion      
                {
                    bool existUser = _rules.Any(x => x.To == emailTo);
                    if (!existUser) { 
                        _ = MailToSG("Mail rechazado por: Usuario no identificado", Request.Form["from"]);
                       // return BadRequest();
                    }
                    if (files.Count == 0)
                    {
                        _ = MailToSG("Mail rechazado por: Sin Adjuntos", Request.Form["from"]);
                       // return BadRequest();
                    }
                    else 
                    { 
                    foreach (var file in files) // por cada attach en e mail 
                    {
                        IEnumerable<EmailRule> rulesAvailables = _rules.Where(x => x.Subject == emailSubject &&
                                                                                                        x.To == emailTo && x.AttachmentName == file.FileName);
                        foreach (EmailRule ruleAvailable in rulesAvailables) //lo guarda en el directorio de la policy
                        {
                            string filePath = Path.Combine(FilesRootPath, ruleAvailable.FolderName, file.FileName);
                                var iStream = file.OpenReadStream(); //creo el stream para chek de virus en cloudmersive
                             
                                if ( VirusCheck(iStream))
                                {
                                    using var inputStream = new FileStream(filePath, FileMode.Create); 
                                    await file.CopyToAsync(inputStream);
                                }
                            //; // se guarda en el directorio de la policy con el nombre del attach (sobreescribe)
                        }
                    }
                    }
                }
            }
            else
            {
                _ = MailToSG("Mail rechazado por: Asunto no esperado", Request.Form["from"]);
              //  return BadRequest();

            } 
            

            return Ok();
        }

    }
}

