using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IKEA.BLL.Common.Services
{
    public interface IAttachmentService
    {
        string UploadFile(IFormFile file, string FolderName);
        bool Delete(string filePath);
    }
}
