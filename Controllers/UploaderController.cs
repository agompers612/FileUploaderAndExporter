using FileUploaderAndExporter.Classes;
using FileUploaderAndExporter.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploaderAndExporter.Controllers
{
    public class UploaderController : Controller
    {
        // GET: Uploader
        public ActionResult Index()
        {
            var path = Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString());
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles();
            
            return View(Files);
        }

        public FileResult DownloadCSV(string name)
        {
            var path = Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString());
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles();

            var FileList = new List<FileDetails>();
            foreach (var item in Files)
            {
                FileList.Add(new FileDetails { Name = item.Name, Location = item.FullName, Extension = item.Extension, Size = item.Length.ToString() });
            }

            return File(DataTableToCSV.ConvertDataTableToCSV(DataTableToCSV.ToDataTable(FileList)), "text/csv", "ListOfFiles.csv");
        }

        public FileResult Download(string filePath, string name)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public JsonResult UploadJsonDocument()
        {
            var uploadPath = Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString());

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];

                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                var path = Path.Combine(uploadPath, fileName);
                Stream fileContent = file.InputStream;
                file.SaveAs(path);

            }
            return Json(true);
        }
    }
}
