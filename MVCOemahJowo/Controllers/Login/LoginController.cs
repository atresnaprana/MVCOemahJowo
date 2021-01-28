using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace MVCOemahJowo.Controllers.Login
{
    public class LoginController : Controller
    {
        // GET: Login
        string hash = "%4h&bn9873*7^><?:'";
        private oemahjowodbEntities db = new oemahjowodbEntities();

        public ActionResult Index()
        {
            return View();
        }
        protected string Decrypt(string pass)
        {
            string decrpyted = "";
            byte[] data = Convert.FromBase64String(pass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    decrpyted = UTF8Encoding.UTF8.GetString(result);
                }
            }
            return decrpyted;
        }
        protected string Encrypt(string pass)
        {
            string encrypted = "";
            byte[] data = UTF8Encoding.UTF8.GetBytes(pass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    encrypted = Convert.ToBase64String(result, 0, result.Length);
                }
            }
            return encrypted;
        }
        [HttpPost]
        public ActionResult SendDt(me_user loginDt)
        {
            var username = loginDt.USER_NAME;
            var pass = Encrypt(loginDt.USER_PASS);
            var userdt = db.me_user.Where(y => y.USER_NAME == username).FirstOrDefault();
            if(userdt.USER_PASS == pass)
            {
                Session["id"] = username;
                
                return RedirectToAction("index", "Home");
            }
            else
            {
                return RedirectToAction("index");
            }

        }
        public ActionResult Logout()
        {
            var id = Session["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                Session.Remove(Session["id"].ToString());
                return RedirectToAction("index");
            }
           
           
        }
    }
}