﻿using SalesAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesAdminPortal.Controllers
{
    public class DocumentsController : Controller
    {
        [HttpGet]
        public ActionResult UploadDocument()
        {
            var model = new DocumentModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadDocument()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(DocumentModel doc)
        {
            var document = new Document
            {
                DocumentId = doc.DocumentId,
                DocumentName = doc.Name,
                DocumentDesc = doc.DocumentDesc,
                DocType = doc.DocType
            };

            if (doc.DocType == "Content")
            {
                document.Content = doc.Content;
            }
            else
            {
                if (doc.File.ContentLength > 0)
                {
                    string FileName = Path.GetFileName(doc.File.FileName);
                    string SavePath = Path.Combine(Server.MapPath("~/UploadedDocuments"), FileName);
                    doc.File.SaveAs(SavePath);
                    document.Path = FileName;
                }
            }

            //Saving the Content
            using(var context = new ApplicationDbContext())
            {
                if (document.DocumentId == 0)
                {
                    context.Documents.Add(document);
                }
                else
                {
                    var entity = context.Documents.Where(r => r.DocumentId.Equals(document.DocumentId)).FirstOrDefault();
                    entity.DocumentName = document.DocumentName;

                    if (doc.DocType == "File")
                        entity.Path = document.Path;
                    else
                        entity.Content = document.Content;
                }
                context.SaveChanges();
            }

            return View("UploadDocument");
        }

        [HttpPost]
        public ActionResult DownloadFile(string fileName)
        {
            using(var context = new ApplicationDbContext())
            {
                var dbFileName = context.Documents.Where(r => r.DocumentName.Equals(fileName)).Select(r => r.Path).FirstOrDefault();
                byte[] fileData = System.IO.File.ReadAllBytes(Server.MapPath("~/UploadedDocuments/" + dbFileName));

                return File(fileData, System.Net.Mime.MediaTypeNames.Application.Octet, dbFileName);
            }
        }

        [HttpGet]
        public ActionResult DocumentsList()
        {
            using (var context = new ApplicationDbContext())
            {
                var documents = context.Documents.ToList();
                return Json(documents,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteFile(int documentId)
        {
            using(var context = new ApplicationDbContext())
            {
                var documents = context.Documents.Where(x => x.DocumentId.Equals(documentId)).FirstOrDefault();
                context.Documents.Remove(documents);
                return Json(context.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}