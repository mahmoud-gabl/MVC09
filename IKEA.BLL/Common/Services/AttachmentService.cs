using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IKEA.BLL.Common.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private const int FileMaxSize = 2_097_152;//2Mbs;
        public string UploadFile(IFormFile file, string FolderName)
        {
            #region Validations
            var fileExtension = Path.GetExtension(file.FileName);
             if (!AllowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invalid File Extemstion Please Try Again");
            }
              if (file.Length>FileMaxSize)
            {
                throw new Exception("Invalid File Size");
            }
            #endregion
            //1- Get Located Folder Path;
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            if (!Directory.Exists(FolderPath))
                {
                Directory.CreateDirectory(FolderPath);
               }
            //2- Get File Name And Make It Unique

            var FileName = $"{Guid.NewGuid()}{fileExtension}";
            //3- Get File Path
            var FilePath = Path.Combine(FolderPath, FileName);

            //4-  Save File Use Streaming [Data Per Time]
            using var FileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FileStream);
            // 5- Return File
            return FileName;

        }
        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

      
    }
}
