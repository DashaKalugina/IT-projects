using DataCollection;
using Microsoft.AspNet.Identity;
using MVC.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace MVC.Controllers
{
    public class ProfilesController : Controller
    {
        // GET: Profiles
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var x = (PersonData)new XmlSerializer(typeof(PersonData)).Deserialize(file.InputStream);

            using (var context = new ApplicationDbContext())
            {
                context.Profiles.Add(
                    new UserProfile
                    {
                        UserId = User.Identity.GetUserId(),
                        LibraryCard = x.LibraryCard,
                        Name = x.Name,
                        DateOfBirth = x.DateOfBirth,
                        Address = x.Address,
                        Group = x.Group,
                        PhoneNumber = x.PhoneNumber,
                        Books = x.Books.Select(y => new UserBooks
                        {
                            Title = y.Title,
                            Author = y.Author,
                            YearOfIssue = y.YearOfIssue,
                            Reissue = y.Reissue,
                            Genre = y.Genre,
                            Isbn = y.Isbn,
                        }).ToList()
                    }
                );

                context.SaveChanges();
            }

            return View(x);
        }

        public ActionResult Download(int resourceId)
        {
            UserProfile g;
            using (var ctx = new ApplicationDbContext())
            {
                g = ctx.Profiles.Find(resourceId);

                ExcelPackage pkg;
                using (
                    var stream = System.IO.File.OpenRead(HostingEnvironment.ApplicationPhysicalPath + "template.xlsx"))
                {
                    pkg = new ExcelPackage(stream);
                    stream.Dispose();
                }

                var worksheet = pkg.Workbook.Worksheets[1];
                worksheet.Name = g.Name;
                worksheet.Cells[3, 2].Value = g.LibraryCard;
                worksheet.Cells[4, 2].Value = g.Name;
                worksheet.Cells[5, 2].Value = g.DateOfBirth;
                worksheet.Cells[6, 2].Value = g.Address;
                worksheet.Cells[7, 2].Value = g.Group;
                worksheet.Cells[8, 2].Value = g.PhoneNumber;
                worksheet.Cells[9, 2].Value = g.User.UserName;

                int rowNum = 15;
                foreach (var book in g.Books)
                {
                    worksheet.Cells[rowNum, 1].Value = book.Isbn;
                    worksheet.Cells[rowNum, 2].Value = book.Title;
                    worksheet.Cells[rowNum, 3].Value = book.Author;
                    worksheet.Cells[rowNum, 4].Value = book.YearOfIssue;
                    worksheet.Cells[rowNum, 5].Value = book.Reissue? "Да" : "Нет";
                    worksheet.Cells[rowNum, 6].Value = book.Genre;
                    
                    rowNum++;
                }

                // Заполнение файла Excel данными
                var ms = new MemoryStream();
                pkg.SaveAs(ms);

                return File(ms.ToArray(), "application/ooxml", (g.Name ?? "Без Названия").Replace(" ", "") + ".xlsx");
            }
        }
    }
}