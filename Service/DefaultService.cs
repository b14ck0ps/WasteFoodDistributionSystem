using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WasteFoodDistributionSystem.Service
{
    public class DefaultService
    {
        public static string UploadImage(string Name, HttpPostedFileBase File, string OldImgUrl, string directoryPath)
        {

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (File != null && File.ContentLength > 0)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(OldImgUrl) && OldImgUrl != "Default.jpg")
                {
                    var oldImagePath = Path.Combine(directoryPath, OldImgUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new image
                var fileExtension = Path.GetExtension(File.FileName);
                var fileName = $"{Name}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                var path = Path.Combine(directoryPath, fileName);
                File.SaveAs(path);
                return fileName;
            }
            return null;
        }
    }
}