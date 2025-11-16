using Microsoft.AspNetCore.Http;

namespace walkeasyfinal.Models
{
    public class FileUploadViewModel
    {
        public IFormFile CsvFile { get; set; }
    }
}
