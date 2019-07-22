using A.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace A.Controllers
{
    public class HomeController : Controller
    {
        List<string> l = new List<string>();
        

        
        public ActionResult home(consumer consumer)
        {
            SqlConnection con = new SqlConnection();
           con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
          
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=SouqZone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //con.ConnectionString = "Data Source=DESKTOPcon.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";J0TTRBH\\SQLEXPRESS;Initial Catalog=souqzone;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandText = "getConsumerShops";
            cmd.Parameters.AddWithValue("@id", consumer.consumerID);
            SqlDataReader r = cmd.ExecuteReader();


            List<shopOwner> l = new List<shopOwner>();


            while (r.Read())
            {
                shopOwner s = new shopOwner();
                s.shopName = r["shopName"].ToString();
                s.shopImg = r["shopImg"].ToString();
                s.consumerID = int.Parse(r["consumerID"].ToString());
                s.shopOwnerID = int.Parse(r["shopOwnerID"].ToString());
                s.categoryID = int.Parse(r["categoryID"].ToString());
                //s.rate = float.Parse(r["rate"].ToString());
                Session["consumerID"]= int.Parse(r["consumerID"].ToString());
                l.Add(s);
            }
            ViewBag.st1 = l;
            r.Close();


            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Connection = con;
            cmd2.CommandText = "getShopsOffers";
            cmd2.Parameters.AddWithValue("@id", consumer.consumerID);
            SqlDataReader r2 = cmd2.ExecuteReader();


            List<shopOwner> l2 = new List<shopOwner>();


            while (r2.Read())
            {
                shopOwner s = new shopOwner();
                s.shopName = r2["shopName"].ToString();
                s.shopImg = r2["shopImg"].ToString();
                s.discount = float.Parse(r2["discount"].ToString());
                s.city= r2["city"].ToString();
                s.categoryID =int.Parse( r2["categoryID"].ToString());
                s.shopOwnerID = int.Parse(r2["shopOwnerID"].ToString());
                s.categoryType = r2["categoryType"].ToString();

                // s.discount = r2["discount"] != null ? double.Parse(r2["discount"].ToString()) : 0;
                l2.Add(s);
            }
            ViewBag.st2 = l2;

            con.Close();








            return View(l);
        }
        public ActionResult getshopsbylocation(string select1, int select2)
        {
            ViewData["location"] = select1;
            ViewData["category"] = select2;
            SqlConnection con = new SqlConnection();
          con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con; 
            cmd.CommandText = "getShopsIn";
            cmd.Parameters.AddWithValue("@city", select1);
            cmd.Parameters.AddWithValue("@categoryID", select2);
            SqlDataReader r = cmd.ExecuteReader();
            List<shopOwner> l = new List<shopOwner>();

            if (!r.HasRows)
            {
                ViewBag.Message = "no shops found";
            }
            while (r.Read())
            {
                shopOwner s = new shopOwner();
                s.shopName = r["shopName"].ToString();
                s.shopImg = r["shopImg"].ToString();
                s.shopOwnerID =int.Parse( r["shopOwnerID"].ToString());

                l.Add(s);
            }
            con.Close();

            return View(l);
        }

        public ActionResult getshopsdiscount()
        {
            SqlConnection con = new SqlConnection();
           con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandText = "getShopsOffers";
            cmd.Parameters.AddWithValue("@id", ViewBag.consumer.consumerID);
            SqlDataReader r = cmd.ExecuteReader();
            List<shopOwner> l2 = new List<shopOwner>();

            while (r.Read())
            {
                shopOwner s = new shopOwner();
                s.shopName = r["shopName"].ToString();
                s.shopImg = r["shopImg"].ToString();
                s.discount = double.Parse(r["discount"].ToString());
                l2.Add(s);
            }
            con.Close();

            return View(l);
        }
        [HttpPost]
        public ActionResult home(HttpPostedFileBase file)
        {
            var fileName = "";
            if (file != null && file.ContentLength > 0)
            {

                fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/content/images"), fileName);
                file.SaveAs(path);
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/todo/api/v1.0/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")

                );
            HttpResponseMessage response = client.GetAsync("get_images/" + fileName).Result;

            if (response.IsSuccessStatusCode)
            {

                ViewBag.result = response.Content.ReadAsAsync<IEnumerable<product>>().Result;
            }
            else
            {
                ViewBag.r = "error";
            }
            i = ViewBag.result;
            ViewBag.image = fileName;
            List<product> unique = i.GroupBy(x => x.picture).Select(x => x.First()).ToList<product>();

            unique = unique.OrderByDescending(x => x.ratio).ToList();
            ViewBag.MyList = unique;
            return View("matched");
            //return View("x",unique);
        }
        public ActionResult matched(List<string> l)
        {
            return View(l);
        }
        public List<product> i = new List<product>();

        public ActionResult GetMembers()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/todo/api/v1.0/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")

                );
            HttpResponseMessage response = client.GetAsync("get_images/10.jpg").Result;

            if (response.IsSuccessStatusCode)
            {

                ViewBag.result = response.Content.ReadAsAsync<IEnumerable<product>>().Result;
            }
            else
            {
                ViewBag.r = "error";
            }
            i = ViewBag.result;

            List<product> unique = i.GroupBy(x => x.picture).Select(x => x.First()).ToList<product>();

            return View("GetMembers2", unique);
        }

        public ActionResult viewImage(string name, float price, int id)
        {
            product im = new product();
            im.picture = name;
            im.price = price;
            im.shopOwnerID = id;
            return View("cv", im);
        }

        public ActionResult Products()
        {
            return View();
        }
        [HttpGet]
        public ActionResult test()
        {
            SqlConnection con = new SqlConnection();
           con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            con.Open();


            SqlCommand cmd2 = new SqlCommand();
            //cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Connection = con;
            cmd2.CommandText = "select supplierID,  name ,companyName from supplier  where supplierID=" + 1;
            //cmd2.Parameters.AddWithValue("@id", 6);
            SqlDataReader r2 = cmd2.ExecuteReader();


            //List<shopOwner> l2 = new List<shopOwner>();

            supplier s = new supplier();
            while (r2.Read())
            {
                s.supplierID = int.Parse(r2["supplierID"].ToString());
                s.name = r2["name"].ToString();
                s.companyName = r2["companyName"].ToString();

                //l2.Add(s);
            }
            //ViewBag.st2 = s;

            con.Close();


            return View(s);
        }
        [HttpPost]
        public ActionResult test(supplier s)
        {
            SqlConnection con = new SqlConnection();
           con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            TempData["name"] = s.name;

            SqlCommand cmd2 = new SqlCommand();
            //cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Connection = con;
            con.Open();

            cmd2.CommandText = "update supplier set name='" + s.name + "',companyName='" + s.companyName + "' where supplierID=" + s.supplierID;
            cmd2.Parameters.AddWithValue("@name", s.name);

            cmd2.Parameters.AddWithValue("@companyName", s.companyName);
            cmd2.Parameters.AddWithValue("@supplierID", s.supplierID);
            //SqlDataReader r2 = cmd2.ExecuteReader();


            //List<shopOwner> l2 = new List<shopOwner>();


            int x = cmd2.ExecuteNonQuery();
            if (x == 1)
            {
                TempData["MS"] = "supplier updated";
            }


            con.Close();


            return View();
        }


        public ActionResult ProductDetails()
        {
            return View();
        }

        //////////////// ShopOwner //////////////////

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult ProductsMale()
        {
            return View();
        }
        public ActionResult ProductsFemale()
        {
            return View();
        }

        public ActionResult Profile(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "select productID,p.categoryID , name , p.discount,picture,price ,p.description,shopName,phone,categoryType,shopImg,country,city,lat,lng,s.shopOwnerID from product p , shopOwner s where p.shopOwnerID=s.shopOwnerID and p.shopOwnerID=" + id;
            SqlDataReader r3 = cmd.ExecuteReader();
            List<shopOwnerWithProcduct> lt3 = new List<shopOwnerWithProcduct>();
            while (r3.Read())
            {
                shopOwnerWithProcduct sp = new shopOwnerWithProcduct();
                sp.productID = int.Parse(r3["productID"].ToString());
                sp.name = r3["name"].ToString();
                sp.city = r3["city"].ToString();
                sp.discount = float.Parse(r3["discount"].ToString());
                sp.picture = r3["picture"].ToString();
                sp.price = float.Parse(r3["price"].ToString());
                sp.description = r3["description"].ToString();
                sp.shopName = r3["shopName"].ToString();
                sp.phone = r3["phone"].ToString();
                sp.categoryType = r3["categoryType"].ToString();
                sp.shopImg = r3["shopImg"].ToString();
                sp.country = r3["country"].ToString();
                sp.city = r3["city"].ToString();
                sp.lat = float.Parse(r3["lat"].ToString());
                sp.lng = float.Parse(r3["lng"].ToString());
                sp.categoryID = int.Parse(r3["categoryID"].ToString());
                sp.shopOwnerID = int.Parse(r3["shopOwnerID"].ToString());
                lt3.Add(sp);
            }

            ViewBag.shproduct = lt3;
            r3.Close();
            con.Close();


            return View();
        }

        public ActionResult ProductDetails2()
        {
            return View();
        }

        public ActionResult addtocart(int consumerID,int productID ,int shopOwnerID,string picture,float price)
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            

            SqlCommand cmd2 = new SqlCommand();
            //cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Connection = con;
            con.Open();

            cmd2.CommandText = "insert into cart (consumerID,shopOwnerID,productID,picture,price) values("+ consumerID + ","+ shopOwnerID + ","+ productID + ",'"+picture + "',"+ price + ")";
            
            //SqlDataReader r2 = cmd2.ExecuteReader();


            //List<shopOwner> l2 = new List<shopOwner>();


            int x = cmd2.ExecuteNonQuery();
            if (x == 1)
            {
                TempData["MS"] = "supplier updated";
            }


            return RedirectToAction("Profile", new { id = shopOwnerID });
           
            
        }
        public  ActionResult cart( )
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;
            cmd.CommandText = "select * from cart where consumerID=" + int.Parse(Session["consumerID"].ToString());

            SqlDataReader r = cmd.ExecuteReader();


            List<cart> l = new List<cart>();


            while (r.Read())
            {
                cart s = new cart();
                s.picture = r["picture"].ToString();
                s.cartID= int.Parse(r["cartID"].ToString());
                s.consumerID = int.Parse(r["consumerID"].ToString());
                s.shopOwnerID = int.Parse(r["shopOwnerID"].ToString());
                s.productID = int.Parse(r["productID"].ToString());
                s.price = float.Parse(r["price"].ToString());

                l.Add(s);
            }
            ViewBag.cartItems = l;
            r.Close();




            con.Close();
            return View();
        }
        public ActionResult delete(int id)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString =@"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";


            SqlCommand cmd2 = new SqlCommand();
            //cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Connection = con;
            con.Open();

            cmd2.CommandText = "delete from cart where cartID="+id;

            //SqlDataReader r2 = cmd2.ExecuteReader();


            //List<shopOwner> l2 = new List<shopOwner>();


            int x = cmd2.ExecuteNonQuery();
            


            return RedirectToAction("cart");
        }

    }
}
