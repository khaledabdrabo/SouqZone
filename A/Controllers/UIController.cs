using A.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A.Controllers
{
    public class UIController : Controller
    {
        public ActionResult SignUp()
        {
            return View();
        }
        //
        // GET: /UI/
        [HttpPost]
        public ActionResult SignUp(uiSignConsumer u)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            if (u.role == 1 && u.password == u.repassword)
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;//بربط ال command على الداتابيز
                cmd.CommandText = "addConsumer";
                cmd.Parameters.AddWithValue("@consumerName", u.firstName);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@country", u.country);
                cmd.Parameters.AddWithValue("@city", u.city);
                cmd.Parameters.AddWithValue("@role", u.role);


                int x = cmd.ExecuteNonQuery();
                    if (x == 2)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Connection = con;//بربط ال command على الداتابيز
                    cmd2.CommandText = "getConsumerAll";
                    cmd2.Parameters.AddWithValue("@email", u.email);


                    SqlDataReader r = cmd2.ExecuteReader();
                    consumer s = new consumer();
                    while (r.Read())
                    {
                        
                        s.personalInfoID = int.Parse(r["personalInfoID"].ToString());
                        
                        s.email = r["email"].ToString();
                        s.consumerID = int.Parse(r["consumerID"].ToString());
                        s.password = r["password"].ToString();
                        s.firstName = r["firstName"].ToString();
                        s.country = r["country"].ToString();
                        s.city = r["city"].ToString();
                        ViewBag.consumer = s;
                        con.Close();
                        return RedirectToAction("home", "Home", s);

                    }
                }
                
            }
            else if (u.role == 2 && u.password == u.repassword)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;//بربط ال command على الداتابيز
                cmd.CommandText = "addSupplier";
                cmd.Parameters.AddWithValue("@supplierName", u.firstName);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@country", u.country);
                cmd.Parameters.AddWithValue("@city", u.city);
                cmd.Parameters.AddWithValue("@role", u.role);
                cmd.Parameters.AddWithValue("@categoryID", u.categoryID);
                cmd.Parameters.AddWithValue("@lat", u.lat);
                cmd.Parameters.AddWithValue("@lng", u.lng);

                int x = cmd.ExecuteNonQuery();
                if (x == 2)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Connection = con;//بربط ال command على الداتابيز
                    cmd2.CommandText = "getSupplierAll";
                    cmd2.Parameters.AddWithValue("@email", u.email);


                    SqlDataReader r = cmd2.ExecuteReader();
                    supplier s = new supplier();
                    while (r.Read())
                    {
                        
                        s.personalInfoID = int.Parse(r["personalInfoID"].ToString());
                        
                        s.email = r["email"].ToString();
                        s.supplierID = int.Parse(r["supplierID"].ToString());
                        s.password = r["password"].ToString();
                        s.name = r["name"].ToString();
                        s.country = r["country"].ToString();
                        s.city = r["city"].ToString();
                        s.supImg = r["supImg"].ToString();
                        s.categoryID= int.Parse(r["categoryID"].ToString());
                        //  ViewBag.supplier = s;
                        return RedirectToAction("xxx", "view",s);

                    }
                }
                
            }
            else if (u.role == 3 && u.password == u.repassword)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;//بربط ال command على الداتابيز
                cmd.CommandText = "addShopOwner";
                cmd.Parameters.AddWithValue("@shopName", u.firstName);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@country", u.country);
                cmd.Parameters.AddWithValue("@city", u.city);
                cmd.Parameters.AddWithValue("@role", u.role);
                cmd.Parameters.AddWithValue("@categoryID", u.categoryID);
                cmd.Parameters.AddWithValue("@lat", u.lat);
                cmd.Parameters.AddWithValue("@lng", u.lng);

                int x = cmd.ExecuteNonQuery();
                if (x == 2)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Connection = con;//بربط ال command على الداتابيز
                    cmd2.CommandText = "getShopOwnerAll";
                    cmd2.Parameters.AddWithValue("@email", u.email);


                    SqlDataReader r1 = cmd2.ExecuteReader();
                    shopOwner s = new shopOwner();
                    while (r1.Read())
                    {

                        s.personalInfoID = int.Parse(r1["personalInfoID"].ToString());

                        s.email = r1["email"].ToString();
                        s.shopOwnerID = int.Parse(r1["shopOwnerID"].ToString());
                        s.password = r1["password"].ToString();
                        s.ownerName = r1["ownerName"].ToString();
                        s.country = r1["country"].ToString();
                        s.city = r1["city"].ToString();
                        s.shopImg = r1["shopImg"].ToString();
                        s.categoryID = int.Parse(r1["categoryID"].ToString());
                        con.Close();
                        return RedirectToAction("xxx", "view2", s);

                    }
                }
                
            }
            con.Close();
            return View("errorSignUP"); 

            
            
        }
        public ActionResult login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult login(string email,string password)
        {
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=souqzone.database.windows.net;Initial Catalog=souqzone;Integrated Security=False;User ID=souqzone_admin;Password=Az@00000;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
con.ConnectionString = @"Data Source=DESKTOP-J0TTRBH\SQLEXPRESS;Initial Catalog=test99;Integrated Security=True";

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;//بربط ال command على الداتابيز
            cmd.CommandText = "checkLogin";
            cmd.Parameters.AddWithValue("@email ", email);

            cmd.Parameters.AddWithValue("@password ", password);
            SqlDataReader r1 = cmd.ExecuteReader();
            int personalInfoID,role;
            if (!r1.HasRows)
            {
                ModelState.AddModelError("invalid", "Email or Password is Wrong");
                con.Close();
                return View(); 
            }
            while (r1.Read())
            {
                personalInfoID = int.Parse(r1["personalInfoID"].ToString());
                role =  int.Parse(r1["role"].ToString());
                if (role == 1)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandText = "select * from consumer where personalInfoID=" + personalInfoID;
                    cmd2.Connection = con;
                    r1.Close();
                    SqlDataReader r2 = cmd2.ExecuteReader();
                    consumer s = new consumer();
                    while (r2.Read())
                    {
                        s.email = email;
                        s.consumerID = int.Parse(r2["consumerID"].ToString());
                        s.password = password;
                        s.firstName = r2["firstName"].ToString();
                        s.country = r2["country"].ToString();
                        s.city = r2["city"].ToString();
                        
                    }
                    ViewBag.consumer = s;
                    con.Close();
                    return RedirectToAction("home", "Home", s);

                }
                else if (role == 2)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandText = "select * from supplier where personalInfoID=" + personalInfoID;
                    cmd2.Connection = con;
                    r1.Close();
                    SqlDataReader r3 = cmd2.ExecuteReader();
                    supplier s = new supplier();
                    while (r3.Read())
                    {

                        s.personalInfoID = int.Parse(r3["personalInfoID"].ToString());

                        s.email = email;
                        s.supplierID = int.Parse(r3["supplierID"].ToString());
                        s.password = password;
                        s.name = r3["name"].ToString();
                        s.country = r3["country"].ToString();
                        s.city = r3["city"].ToString();
                        s.supImg = r3["supImg"].ToString();
                        return RedirectToAction("xxx", "view", s);

                    }
                } 
                else if (role == 3)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandText = "select * from shopOwner where personalInfoID=" + personalInfoID;
                    cmd2.Connection = con;
                    r1.Close();
                    SqlDataReader r2 = cmd2.ExecuteReader();
                    
                    shopOwner s = new shopOwner();
                    while (r2.Read())
                    {

                        s.personalInfoID = int.Parse(r2["personalInfoID"].ToString());

                        s.email = email;
                        s.shopOwnerID = int.Parse(r2["shopOwnerID"].ToString());
                        s.password = password;
                        s.ownerName = r2["ownerName"].ToString();
                        s.country = r2["country"].ToString();
                        s.city = r2["city"].ToString();
                        s.shopImg = r2["shopImg"].ToString();
                        s.categoryID = int.Parse(r2["categoryID"].ToString());
                        con.Close();
                        return RedirectToAction("xxx", "view2", s);

                    }

                }
                con.Close();
                //return RedirectToAction("Index2", "view2", s);

            }
           
            return View();
        }
    }
}
