using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using A.Models;
using Newtonsoft.Json;

namespace A.Controllers
{
    public class viewController : Controller
    {
        public int x ;
        public int y;
        //////////////// Supllier //////////////////
        [HttpGet]
        public ActionResult xxx(supplier s)
        {
            if (s.supImg == null) {

                s.supImg = "user-image.jpg";
                
            }
            x = s.supplierID;
            y = s.supplierID;
           Session["supplierID"]  = s.supplierID;
            Session["email"] = s.email;
            if (s.name == null) s.name = "User";
           
            return RedirectToAction("Index", new  {idd=s.supplierID });
        }
       
        
        public ActionResult Index(int idd)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "getSupplier";
            cmd.Parameters.AddWithValue("@hisPersonalinfoID",idd);
            SqlDataReader r = cmd.ExecuteReader();
            supplier lt = new supplier();
            while(r.Read())
            {
                
                lt.name = r["name"].ToString();
                if (r["companyName"].ToString() != null) { 
                lt.companyName = r["companyName"].ToString();
                }
                lt.supImg = r["supImg"].ToString();
                lt.categoryID = r["categoryID"].ToString() == "" ? 1 : int.Parse(r["categoryID"].ToString());
                lt.supplierID = int.Parse(r["supplierID"].ToString());
                lt.email = Session["email"].ToString();


            }
            ViewBag.supplierData = lt;
            r.Close();
            //-----------------------------------------------------
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Connection = con;//بربط ال command على الداتابيز
            cmd2.CommandText = "getSupplierCategories";
            cmd2.Parameters.AddWithValue("@hisPersonalinfoID", idd);
            SqlDataReader r2 = cmd2.ExecuteReader();
            List<supplierCategory> lt2 = new List<supplierCategory>();
            while (r2.Read())
            {
                supplierCategory p = new supplierCategory();
                p.type = r2["type"].ToString();
                p.CategoryImg = r2["CategoryImg"].ToString();
                lt2.Add(p);
            }
            r2.Close();
            //-------------------------------------------------------
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandType = System.Data.CommandType.StoredProcedure;
            cmd3.Connection = con;
            cmd3.CommandText = "getSupplierPackage";
            cmd3.Parameters.AddWithValue("@supplierID",idd);
            SqlDataReader r3 = cmd3.ExecuteReader();
            List<package> lt3 = new List<package>();

            while (r3.Read())
            {
                package sp = new package();
                sp.img = r3["img"].ToString();
                sp.name = r3["name"].ToString();
                sp.price = float.Parse(r3["price"].ToString());
                sp.details = r3["details"].ToString();
                sp.supplierID = int.Parse(r3["supplierID"].ToString());
                sp.categoryID = int.Parse(r3["categoryID"].ToString());
                sp.packageID = int.Parse(r3["packageID"].ToString());

                lt3.Add(sp);
            }
              r3.Close();
            r2.Close();
            
            ViewBag.package = lt2;
            ViewBag.suppackage = lt3;

            SqlCommand cmd4 = new SqlCommand();
            cmd4.CommandType = System.Data.CommandType.Text;
            cmd4.Connection = con;
            cmd4.CommandText = "select img,name,price,offers.description,location ,offerID,offers.packageID,package.supplierID from package , offers where package.packageID=offers.packageID";
            SqlDataReader r5 = cmd4.ExecuteReader();
            List<SupplierOffers> lt5 = new List<SupplierOffers>();
            while (r5.Read())
            {
                SupplierOffers sp1 = new SupplierOffers();
                sp1.img = r5["img"].ToString();
                sp1.name = r5["name"].ToString();
                sp1.price = float.Parse(r5["price"].ToString());
                sp1.description = r5["description"].ToString();
                sp1.location = r5["location"].ToString();
                sp1.offerID = int.Parse(r5["offerID"].ToString());
                sp1.packageID = int.Parse(r5["packageID"].ToString());
                sp1.supplierID= int.Parse(r5["supplierID"].ToString());
                lt5.Add(sp1);
            }

            ViewBag.supoffers = lt5;
            r5.Close();

            SqlCommand getMsg = new SqlCommand();
            getMsg.CommandType = System.Data.CommandType.Text;
            getMsg.Connection = con;
            getMsg.CommandText = "	select distinct * from chat where mto = '" + lt.email + "'  order by msgID desc";
            SqlDataReader r7 = getMsg.ExecuteReader();
            if (!r7.HasRows)
                ViewBag.MyMsg = null;
            else
            {
                List<chat> allChat = new List<chat>();
                while (r7.Read())
                {
                    if (allChat.FirstOrDefault(c => c.mfrom == r7["mfrom"].ToString()) == null)
                    {
                        chat chat = new chat();
                        //chat.msgID = int.Parse(r6["msgID"].ToString());
                        chat.mdate = r7["mdate"].ToString();
                        chat.mfrom = r7["mfrom"].ToString();
                        chat.mto = r7["mto"].ToString();
                        chat.msgcontent = r7["msgcontent"].ToString();
                        allChat.Add(chat);
                    }
                    else
                    {
                        break;
                    }
                    //if (allChat.FirstOrDefault(c => c.mfrom == int.Parse(r6["mfrom"].ToString())) == null)
                    //{

                    // }
                }
                ViewBag.MyMsg = allChat;
            }
            r7.Close();

           Session["supplierID"]  = idd;

            SqlCommand cmd5 = new SqlCommand();
            cmd5.CommandType = System.Data.CommandType.Text;
            cmd5.Connection = con;
            cmd5.CommandText = "select DISTINCT shopOwnerID,shopName,ownerName,shopOwner.phone,shopImg,shopOwner.country,shopOwner.city, shopOwner.lat,shopOwner.lng from shopOwner ,supplier where shopOwner.categoryID = (select categoryID from supplier where supplierID ="+idd+")and shopOwner.city=(select supplier.city from supplier where supplierID = "+idd+")";
            SqlDataReader r6 = cmd5.ExecuteReader();
            List<shopOwner> lt6 = new List<shopOwner>();
            while (r6.Read())
            {
                shopOwner sp1 = new shopOwner();
                sp1.shopOwnerID= int.Parse(r6["shopOwnerID"].ToString());
                sp1.shopName = r6["shopName"].ToString();
                sp1.shopImg = r6["shopImg"].ToString();
                sp1.ownerName = r6["ownerName"].ToString();
                sp1.country = r6["country"].ToString();
                sp1.city= (r6["city"].ToString());
                sp1.phone = r6["phone"].ToString();
               
                lt6.Add(sp1);
            }

            ViewBag.shops = lt6;






            con.Close();
            return View(lt);
        }

        [HttpPost]
        public ActionResult updateupplierData(supplier s)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "update supplier set supImg='" + s.supImg + "',companyName='" + s.companyName + "',phone=" + s.phone + " where supplierID="+s.supplierID;

            int xc = cmd.ExecuteNonQuery();
            return RedirectToAction("Index", new { idd = s.supplierID });

        }
        public ActionResult addPakage(package p)
        {

            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "insert into package values('"+p.img+"','"+p.name+"',"+p.price+",'"+p.details+"',"+p.categoryID+","+p.supplierID+")";
           
            int xc = cmd.ExecuteNonQuery();
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.Connection = con;//بربط ال command على الداتابيز
            cmd2.CommandText = "select packageID ,supplierID from package where supplierID=" + p.supplierID ;
            SqlDataReader r5 = cmd2.ExecuteReader();
            
            while (r5.Read())
            {
                
                
                Session["packageID"] = int.Parse(r5["packageID"].ToString());
                Session["supplierID"] = int.Parse(r5["supplierID"].ToString());

            }


            con.Close();
            return RedirectToAction("Index", new { idd = p.supplierID });
        }
        [HttpPost]
        public ActionResult updatePackage(package p)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            if (p.img != null) { 
            cmd.CommandText = "update  package set img='" + p.img + "',name='" + p.name + "',price=" + p.price + ",details='" + p.details + "',categoryID=" + p.categoryID + ",supplierID=" + p.supplierID + " where packageID="+p.packageID;
            }
            else
            {
                cmd.CommandText = "update  package set  name='" + p.name + "',price=" + p.price + ",details='" + p.details + "',categoryID=" + p.categoryID + ",supplierID=" + p.supplierID + " where packageID=" + p.packageID;


            }
             int f=cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index", new { idd=p.supplierID});
        }
        public ActionResult deletePackage(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "delete from package where packageID=" + id;
            int f = cmd.ExecuteNonQuery();
            con.Close();
            
            return RedirectToAction("Index", new { idd = int.Parse(Session["supplier"].ToString() )});
        }
        public ActionResult getPackageitem(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "select * from item where packageID=" + id;
            SqlDataReader r3 = cmd.ExecuteReader();
            List<packageItem> lt3 = new List<packageItem>();
            while (r3.Read())
            {
                packageItem sp = new packageItem();
                sp.img = r3["img"].ToString();
                sp.name = r3["name"].ToString();
                sp.price = float.Parse(r3["price"].ToString());
                sp.supplierID = int.Parse(r3["supplierID"].ToString());
                Session["supplierID"]= int.Parse(r3["supplierID"].ToString());
                sp.packageID = int.Parse(r3["packageID"].ToString());

                sp.itemID = int.Parse(r3["itemID"].ToString());

                lt3.Add(sp);
            }
            Session["packageID"] = id;
            //Session["supplierID"] = sid;
            ViewBag.packageItems = lt3;
            r3.Close();
            con.Close();
           
            

            return View("products");
            //return View("jk");
        }

        public ActionResult DleleteItem(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "delete from item where itemID=" + id;
            int f = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index", new { idd = x });
        }
        [HttpPost]
        public ActionResult Index(package pk)
        {
            //SqlConnection con = new SqlConnection();
            ////con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //con.ConnectionString = "Data Source=DELL-PC\\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            //con.Open();
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Connection = con;
            //cmd.CommandText = "updatePackage";
            //cmd.Parameters.AddWithValue("@img",pk.img);
            //cmd.Parameters.AddWithValue("@name", pk.name);
            //cmd.Parameters.AddWithValue("@price", pk.price);
            //cmd.Parameters.AddWithValue("@details", pk.details);
            return View();
        }
        public ActionResult updatePackageItem(packageItem p)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            if (p.img != null)
            {
                cmd.CommandText = "update  item set img='" + p.img + "',name='" + p.name + "',price=" + p.price + ",packageID=" + p.packageID + ",supplierID=" + p.supplierID + " where itemID=" + p.itemID;
            }
            else
            {
                cmd.CommandText = "update  item set  name='" + p.name + "',price=" + p.price + ",packageID=" + p.packageID + ",supplierID=" + p.supplierID + " where itemID=" + p.itemID;


            }
            int f = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index",new {idd=p.supplierID});
        }
        public ActionResult insertPackageItem(packageItem p)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            
            cmd.CommandText = "insert into  item values('" + p.img + "','" + p.name + "'," + p.packageID + "," + p.supplierID  + "," + p.price + ")";
            
            int f = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index", new { idd = p.supplierID });
        }
        [HttpPost]
        public ActionResult addoffer(offers o)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "insert into offers values('" + o.description + "','" + o.location + "'," + o.packageID + ","+o.supplierID+")";
            int ff= cmd.ExecuteNonQuery();
            con.Close();
            Session["supplierID"] = o.supplierID;
            return RedirectToAction("Index", new { idd = o.supplierID });
            //return View(o);
        }
        public ActionResult Updateoffer(offers o)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "update  offers  set description='" + o.description + "' , location='" + o.location + "' where offerID=" + o.offerID ;
            int xc = cmd.ExecuteNonQuery();
            con.Close();
            


            return RedirectToAction("Index", new { idd=o.supplierID});
            //return View(o);
        }
        public ActionResult deleteOffer(int id,int sid)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "delete from  offers  where offerID=" +id;
            int xs = cmd.ExecuteNonQuery();
            con.Close();
            int c=int.Parse(Session["supplier"].ToString());
            return RedirectToAction("Index", new { idd = sid });
            //return View(o);
        }
        public ActionResult insertoffer(offers o)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "insert into offers values('" + o.description + "','" + o.location + "'," + o.packageID + ")";
            int xc= cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index", new { idd = o.supplierID });
            //return View(o);
        }

        //public ActionResult updatePackage(package pk)
        //{
        //    return View();
        //}
        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Profile(int id)
        {


            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "select productID,p.categoryID , name , p.discount,picture,price ,p.description,shopName,phone,categoryType,shopImg,country,city,lat,lng from product p , shopOwner s where p.shopOwnerID=s.shopOwnerID and p.shopOwnerID=" + id;
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
                sp.shopName= r3["shopName"].ToString();
                sp.phone= r3["phone"].ToString();
                sp.categoryType= r3["categoryType"].ToString();
                sp.shopImg = r3["shopImg"].ToString();
                sp.country = r3["country"].ToString();
                sp.city = r3["city"].ToString();
                sp.lat =float.Parse( r3["lat"].ToString());
                sp.lng = float.Parse(r3["lng"].ToString());
                sp.categoryID = int.Parse(r3["categoryID"].ToString());
                lt3.Add(sp);
            }
            
            ViewBag.shproduct = lt3;
            r3.Close();
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

        public ActionResult Profile2()
        {
            return View();
        }

        public ActionResult ProductDetails2()
        {
            return View();
        }


        public ActionResult drowsshart2s(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "getSypplierStats";
            cmd.Parameters.AddWithValue("@supplierID", id);
            SqlDataReader r = cmd.ExecuteReader();
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            while (r.Read())
            {
                DataPoint d = new DataPoint(r["month"].ToString(), int.Parse(r["procount"].ToString()));

                dataPoints1.Add(d);
            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            // ViewBag.supplierData = lt;
            r.Close();
            con.Close();

            //ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            //ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            //ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);

            return View();
        }

    }
}
