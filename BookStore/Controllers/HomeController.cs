using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Util;
using System.Data;
using System.Data.Entity;

namespace BookStore.Controllers
{
	public class HomeController : Controller
	{
		// создаем контекст данных
		BookContext db = new BookContext();

		public ActionResult Index()
		{
			// получаем из бд все объекты Book
			IEnumerable<Book> books = db.Books;
			ViewBag.Message = "тестовая фигня";
			ViewBag.Books = books;
			// возвращаем представление
			return View();
		}

		[HttpGet]
		public ActionResult Buy(int id)
		{
			ViewBag.BookId = id;
			return View();
		}
		[HttpPost]
		public string Buy(Purchase purchase)
		{
			purchase.Date = DateTime.Now;
			// добавляем информацию о покупке в базу данных
			db.Purchases.Add(purchase);
			// сохраняем в бд все изменения
			db.SaveChanges();
			return "Спасибо," + purchase.Person + ", за покупку!";
		}

		public string Square(int a = 10, int h = 3)
		{
			double s = a * h / 2.0;
			return "<h2>Площадь треугольника с основанием " + a +
					" и высотой " + h + " равна " + s + "</h2>";
		}

		public ActionResult GetHtml()
		{
			return new HtmlResult("<h2>Привет мир!</h2>");
		}

		public ActionResult GetImage()
		{
			string path = "../Images/visualstudio.png";
			return new ImageResult(path);
		}

		public ActionResult GetList()
		{
			string[] states = { "Russia", "USA", "Canada", "France" };
			return PartialView(states);
		}

		[HttpPost]
		public string GetForm(string text)
		{
			return text; 
		}

		[HttpPost]
		public string GetSurvey(FormCollection frm)
		{
			string voteradio = frm["Vote"].ToString();
			return "вы выбрали " + voteradio;
		}

		[HttpGet]
		public ActionResult EditBook(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			Book book = db.Books.Find(id);
			if (book != null)
			{
				return View(book);
			}
			return HttpNotFound();
		}

		[HttpPost]
		public ActionResult EditBook(Book book)
		{
			db.Entry(book).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Create(Book book)
		{
			db.Books.Add(book);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		//Тоже самое
		/*
		[HttpPost]
		public ActionResult Create(Book book)
		{
			db.Entry(book).State = EntityState.Added;
			db.SaveChanges();

			return RedirectToAction("Index");
		}
		*/

		[HttpGet]
		public ActionResult Delete(int id)
		{
			Book b = db.Books.Find(id);
			if (b == null)
			{
				return HttpNotFound();
			}
			return View(b);
		}
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Book b = db.Books.Find(id);
			if (b == null)
			{
				return HttpNotFound();
			}
			db.Books.Remove(b);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}

}