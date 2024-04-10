using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GA_TEST.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FileController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        //[HttpGet("{fileId}")]
        //public ActionResult GetFile(string fileId)
        //{
        //    var pathToFile = "pasta.jpeg";
        //    if (!System.IO.File.Exists(pathToFile))
        //    {
        //        return NotFound();
        //    }
        //    if (!_fileExtensionContentTypeProvider.TryGetContentType(
        //        pathToFile, out var contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }

        //    var bytes = System.IO.File.ReadAllBytes(pathToFile);

        //    return File(bytes, contentType, Path.GetFileName(pathToFile));
        //}

        //[HttpPost]
        //public async Task<ActionResult> CreateFile(IFormFile file)
        //{
        //    if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
        //    {
        //        return BadRequest("No File");
        //    }
        //    var path = Path.Combine(Directory.GetCurrentDirectory(), $"uploaded_file_{Guid.NewGuid()}.pdf");

        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    return Ok("Your file was uploaded");
        //}
    }
}

