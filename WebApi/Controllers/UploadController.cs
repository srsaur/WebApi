using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    public class UploadController : BaseController
    {
        [HttpPost,DisableRequestSizeLimit]
        public async Task<string> UploadFile()
        {
            var file= Request.Form.Files[0];
            var folderName = Path.Combine("wwwroot", "Images");
            var pathTosave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = file.FileName.Trim();
                var fullPath = Path.Combine(pathTosave, fileName);
               
                using(var stream=new FileStream(fullPath, FileMode.Create))
                {
                   await file.CopyToAsync(stream); 
                }
                return Path.Combine("Images",fileName);
            }
            else
            {
                throw new Exception("Invalid File");
            }
        }
    }
}