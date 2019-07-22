using A.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index(string fromEmail,string toEmail)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-FRFLA73\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "select * from chat where mto='"+toEmail+"' and mfrom='"+fromEmail+"' or (mto='"+fromEmail+"' and mfrom='"+toEmail+"' )";
            SqlDataReader r = cmd.ExecuteReader();
            List<chat> chats = new List<chat>(); 
            while (r.Read())
            {
                chat c = new chat();
               c.mfrom= r["mfrom"].ToString();
               c.mto = r["mto"].ToString();
                c.msgcontent = r["msgcontent"].ToString();
               c.mdate= r["mdate"].ToString();
                chats.Add(c);

            }
            ViewBag.AllMsgFrom = chats;
            return View("index");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult insertMsg(string toEmail,string fromEmail,string msgContent)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-FRFLA73\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;
            
            cmd.CommandText = "insert into chat (msgcontent,mfrom,mto,mdate) values(N'"+msgContent+"','"+fromEmail+"','"+toEmail+"','"+ DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")+"')";
            int x = cmd.ExecuteNonQuery();
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
