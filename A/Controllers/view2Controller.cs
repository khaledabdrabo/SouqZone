using A.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A.Controllers
{
    public class view2Controller : Controller
    {
        //
        // GET: /view2/

        //public ActionResult Index2()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult xxx(shopOwner s)
        {
            if (s.shopImg == null)
            {

                s.shopImg = "user-image.jpg";

            }
            if (s.shopName == null) s.shopName = "User";
            Session["shopOwner"] = s.personalInfoID;
            Session["shopOwnerID"] = s.shopOwnerID;
           Session["email"]=  s.email;
            return RedirectToAction("Index2", new { shopOnwerId = s.shopOwnerID });
        }

        public ActionResult Index2(int shopOnwerId)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "getshopOwner";
            cmd.Parameters.AddWithValue("@hisPersonalinfoID", shopOnwerId);
            SqlDataReader r = cmd.ExecuteReader();
            shopOwner lt = new shopOwner();
            while (r.Read())
            {
                lt.shopName = r["shopName"].ToString();
                lt.shopImg = r["shopImg"].ToString();
                lt.shopOwnerID = int.Parse(r["shopOwnerID"].ToString());
                lt.personalInfoID = int.Parse(r["personalInfoID"].ToString());
                lt.email = Session["email"].ToString();
                lt.categoryID = r["categoryID"].ToString() == "" ? 1 : int.Parse(r["categoryID"].ToString());

            }

            // ViewBag.supplierData = lt;
            r.Close();
            //-----------------------------------------------------
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.Connection = con;//بربط ال command على الداتابيز
            cmd2.CommandText = "select * from product where product.shopOwnerID=" + shopOnwerId;
            SqlDataReader r2 = cmd2.ExecuteReader();
            List<product> lt1 = new List<product>();
            while (r2.Read())
            {
                product i = new product();
                i.name = r2["name"].ToString();
                i.picture = r2["picture"].ToString();
                i.shopOwnerID = int.Parse(r2["shopOwnerID"].ToString());
                i.categoryID = int.Parse(r2["categoryID"].ToString());
                i.price = float.Parse(r2["price"].ToString());
                i.productID = int.Parse(r2["productID"].ToString());
                lt1.Add(i);
            }

            r2.Close();
            //-------------------------------------------------------
            /* SqlCommand cmd3 = new SqlCommand();
             cmd3.CommandType = System.Data.CommandType.StoredProcedure;
             cmd3.Connection = con;
             cmd3.CommandText = "getSupplierPackage";
             cmd3.Parameters.AddWithValue("@hisPersonalinfoID", id);
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
             r3.Close();*/
            r2.Close();

            //ViewBag.shopOwnerItems = lt1;
            ViewBag.package = lt1;
            //  ViewBag.suppackage = lt3;

            SqlCommand cmd4 = new SqlCommand();
            cmd4.CommandType = System.Data.CommandType.Text;
            cmd4.Connection = con;
            cmd4.CommandText = "select name,picture,price,ShopOwnerOffers.description,ShopOwnerOffers.location ,offerID,ShopOwnerOffers.productID,ShopOwnerOffers.shopOwnerID from ShopOwnerOffers , product where product.productID=ShopOwnerOffers.productID";
            SqlDataReader r5 = cmd4.ExecuteReader();
            List<shopOffersProducts> lt5 = new List<shopOffersProducts>();
            while (r5.Read())
            {
                shopOffersProducts sp1 = new shopOffersProducts();
                sp1.picture = r5["picture"].ToString();
                sp1.name = r5["name"].ToString();
                sp1.price = float.Parse(r5["price"].ToString());
                sp1.description = r5["description"].ToString();
                sp1.location = r5["location"].ToString();
                sp1.offerID = int.Parse(r5["offerID"].ToString());
                sp1.productID = int.Parse(r5["productID"].ToString());
                sp1.shopOwnerID = int.Parse(r5["shopOwnerID"].ToString());

                lt5.Add(sp1);
            }

            ViewBag.supoffers = lt5;
            r5.Close();
            SqlCommand getMsg = new SqlCommand();
            getMsg.CommandType = System.Data.CommandType.Text;
            getMsg.Connection = con;
            getMsg.CommandText = "	select distinct * from chat where mto = '"+lt.email+"'  order by msgID desc";
            SqlDataReader r6 = getMsg.ExecuteReader();
            if (!r6.HasRows)
                ViewBag.MyMsg = null;
            else
            {
                List<chat> allChat = new List<chat>();
                while (r6.Read())
                {
                    if(allChat.FirstOrDefault(c=>c.mfrom== r6["mfrom"].ToString())==null)
                    {
                        chat chat = new chat();
                        //chat.msgID = int.Parse(r6["msgID"].ToString());
                        chat.mdate = r6["mdate"].ToString();
                        chat.mfrom = r6["mfrom"].ToString();
                        chat.mto = r6["mto"].ToString();
                        chat.msgcontent = r6["msgcontent"].ToString();
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


            Session["shopOwner"] = shopOnwerId;

            r6.Close();

            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand cmd6 = new SqlCommand();
            cmd6.CommandType = System.Data.CommandType.StoredProcedure;
            cmd6.Connection = con;//بربط ال command على الداتابيز
            cmd6.CommandText = "getshopsstatistaics";
            cmd6.Parameters.AddWithValue("@shopOwnerID", 1);
            SqlDataReader r7 = cmd6.ExecuteReader();
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            while (r7.Read())
            {
                DataPoint d = new DataPoint(r7["month"].ToString(), int.Parse(r7["procount"].ToString()));

                dataPoints1.Add(d);
            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            // ViewBag.supplierData = lt;
            r7.Close();
            SqlCommand cmd5 = new SqlCommand();
            cmd5.CommandType = System.Data.CommandType.Text;
            cmd5.Connection = con;
            cmd5.CommandText = "select distinct su.supImg,su.supplierID,su.phone,su.city,su.companyName,su.country,su.lat,su.lng,su.name from supplier su,shopOwner sp  where su.categoryID=(select categoryID from shopOwner where shopOwnerID="+ shopOnwerId+") and su.city=(select city from shopOwner where shopOwnerID=" + shopOnwerId + ")";
            SqlDataReader r8 = cmd5.ExecuteReader();
            List<supplier> lt7 = new List<supplier>();
             
            while (r8.Read())
            {
                supplier sp1 = new supplier();
                sp1.supplierID = int.Parse(r8["supplierID"].ToString());
                sp1.companyName = r8["companyName"].ToString();
                sp1.supImg = r8["supImg"].ToString();
                sp1.name = r8["name"].ToString();
                sp1.country = r8["country"].ToString();
                sp1.city = (r8["city"].ToString());
                sp1.phone = r8["phone"].ToString();
                sp1.lat =float.Parse( r8["lat"].ToString());
                sp1.lng = float.Parse(r8["lng"].ToString());
                lt7.Add(sp1);
            }
            ViewBag.Suppliers = lt7;
            r8.Close();
            con.Close();

            return View(lt);
        }
        [HttpPost]
        public ActionResult addPakage(product p)
        {

            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "insert into product values('" + p.name + "'," + p.quantity + "," + p.discount + ",'" + p.picture + "'," + p.price + ",'" + p.description + "'," + p.shopOwnerID + "," + p.categoryID + "," + p.categoryID + ",'" + p.month + "')";

            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = p.shopOwnerID });
        }
        [HttpPost]
        public ActionResult updatePackage(product p)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            if (p.picture != null)
            {
                cmd.CommandText = "update  product set picture='" + p.picture + "',name='" + p.name + "',price=" + p.price + ", categoryID=" + p.categoryID + ",shopOwnerID=" + p.shopOwnerID + " where productID=" + p.productID;
            }
            else
            {
                cmd.CommandText = "update  product set name='" + p.name + "',price=" + p.price + ", categoryID=" + p.categoryID + ",shopOwnerID=" + p.shopOwnerID + " where productID=" + p.productID;


            }
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = p.shopOwnerID });
        }
        public ActionResult deletePackage(int id, int shopID)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "delete from product where productID=" + id;
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = shopID });
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

                sp.packageID = int.Parse(r3["packageID"].ToString());
                sp.itemID = int.Parse(r3["itemID"].ToString());

                lt3.Add(sp);
            }

            r3.Close();
            con.Close();

            ViewBag.packageItems = lt3;

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
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2");
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
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2");
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

            cmd.CommandText = "insert into  item values('" + p.img + "','" + p.name + "'," + p.packageID + "," + p.supplierID + "," + p.price + ")";

            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2");
        }
        [HttpPost]
        public ActionResult addoffer(ShopOwnerOffers o)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "insert into ShopOwnerOffers values('" + o.description + "','" + o.location + "'," + o.productID + "," + o.shopOwnerID + ")";
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = o.shopOwnerID });
            //return View(o);
        }
        public ActionResult Updateoffer(ShopOwnerOffers o)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "update  ShopOwnerOffers  set description='" + o.description + "' , location='" + o.location + "' where offerID=" + o.offerID;
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = o.shopOwnerID });
            //return View(o);
        }
        public ActionResult deleteOffer(int id, int shopID)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "delete from  ShopOwnerOffers  where offerID=" + id;
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = shopID });
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
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index2", new { shopOnwerId = int.Parse(Session["shopOwnerID"].ToString()) });
            //return View(o);
        }
        // [ChildActionOnly]
        //public ActionResult getAllMsgFrom(int mfrom,int mto)
        // {

        //     SqlConnection con = new SqlConnection();
        //     //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //    con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

        //     con.Open();
        //     SqlCommand cmd = new SqlCommand();
        //     cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //     cmd.Connection = con;//بربط ال command على الداتابيز
        //     cmd.CommandText = "getMsg";
        //     cmd.Parameters.AddWithValue("@from", mfrom);
        //     cmd.Parameters.AddWithValue("@to", mto);


        //     SqlDataReader r3 = cmd.ExecuteReader();
        //     List<chat> lt3 = new List<chat>();
        //     while (r3.Read())
        //     {
        //         chat sp = new chat();
        //         sp.msgcontent = r3["msgcontent"].ToString();
        //         sp.mdate = r3["mdate"].ToString();


        //         lt3.Add(sp);
        //     }

        //     ViewBag.allmsg = lt3;
        //     r3.Close();
        //     con.Close();
        //     return PartialView("_chat");

        // }
        public ActionResult drowssharts()
        {
            return View();
        }


        public ActionResult drowsshart1s(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "getshopsstatistaics";
            cmd.Parameters.AddWithValue("@shopOwnerID", id);
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

        public ActionResult Profile2(int id)
        {


            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
                                 //  cmd.CommandText = "select i.itemID, i.name,i.img,i.price,s.city,s.country,s.lat,s.lng,s.name as suName,s.supImg ,s.phone,s.companyName,s.supplierID from item i ,supplier s where s.supplierID=i.supplierID and s.supplierID=" + id;
            cmd.CommandText = "select p.packageID,p.name,p.price,p.details,p.supplierID,p.img,s.companyName,s.city,s.country,s.lat,s.lng,s.phone,s.supImg from package p , supplier s where p.supplierID=s.supplierID and  s.supplierID = " + id;
            SqlDataReader r3 = cmd.ExecuteReader();
            List<shopOwnerWithProcduct> lt3 = new List<shopOwnerWithProcduct>();
            while (r3.Read())
            {
                shopOwnerWithProcduct sp = new shopOwnerWithProcduct();
                //sp.productID = int.Parse(r3["itemID"].ToString());
                sp.name = r3["name"].ToString();
                sp.city = r3["city"].ToString();
                sp.picture = r3["img"].ToString();
                sp.price = float.Parse(r3["price"].ToString());
                sp.shopName = r3["companyName"].ToString();
                sp.description = r3["details"].ToString();
                sp.phone = r3["phone"].ToString();
                sp.shopImg = r3["supImg"].ToString();
                sp.country = r3["country"].ToString();
                //sp.discount
                sp.lat = float.Parse(r3["lat"].ToString());
                sp.lng = float.Parse(r3["lng"].ToString());
                sp.supplierID = int.Parse(r3["supplierID"].ToString());
                sp.categoryID = int.Parse(r3["packageID"].ToString());
                lt3.Add(sp);
            }

            ViewBag.shproduct = lt3;
            r3.Close();
            con.Close();




            return View();
        }


        public ActionResult items(int id)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;//بربط ال command على الداتابيز
                                 //  cmd.CommandText = "select i.itemID, i.name,i.img,i.price,s.city,s.country,s.lat,s.lng,s.name as suName,s.supImg ,s.phone,s.companyName,s.supplierID from item i ,supplier s where s.supplierID=i.supplierID and s.supplierID=" + id;
            cmd.CommandText = "select i.img,i.name,i.packageID,i.supplierID,i.price,s.companyName,s.phone,s.country,s.city,s.lat,s.lng,s.supImg from item i, supplier s where i.supplierID = s.supplierID and packageID =" + id;
            SqlDataReader r3 = cmd.ExecuteReader();
            List<shopOwnerWithProcduct> lt3 = new List<shopOwnerWithProcduct>();
            while (r3.Read())
            {
                shopOwnerWithProcduct sp = new shopOwnerWithProcduct();
                //sp.productID = int.Parse(r3["itemID"].ToString());
                sp.name = r3["name"].ToString();
                sp.city = r3["city"].ToString();
                sp.picture = r3["img"].ToString();
                sp.price = float.Parse(r3["price"].ToString());
                sp.shopName = r3["companyName"].ToString();
               // sp.description = r3["details"].ToString();
                sp.phone = r3["phone"].ToString();
                sp.shopImg = r3["supImg"].ToString();
                sp.country = r3["country"].ToString();
                //sp.discount
                sp.lat = float.Parse(r3["lat"].ToString());
                sp.lng = float.Parse(r3["lng"].ToString());
                sp.supplierID = int.Parse(r3["supplierID"].ToString());
                sp.categoryID = int.Parse(r3["packageID"].ToString());
                lt3.Add(sp);
            }

            ViewBag.shproduct = lt3;
            r3.Close();
            con.Close();
            return View("profile2");
        }

    }
}
