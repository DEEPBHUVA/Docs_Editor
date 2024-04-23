using Docs_Editor.DAL;
using Docs_Editor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Docs_Editor.Controllers
{
    public class User_LoginController : Controller
    {
        public IConfiguration Configuration;
        SEC_User_DAL dal = new SEC_User_DAL();
        public User_LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        public IActionResult Login(SEC_UserModel modelSEC_User)
        {
            string ErrorMsg = string.Empty;
            if (string.IsNullOrEmpty(modelSEC_User.Email))
            {
                ErrorMsg += "User Name is Required";
            }
            if (string.IsNullOrEmpty(modelSEC_User.Password))
            {
                ErrorMsg += "<br/>Password is Required";
            }
            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                TempData["Error"] = ErrorMsg;
                return RedirectToAction("Index");
            }
            else
            {
                DataTable dt = dal.PR_SEC_UserByUN_PS_EM(modelSEC_User.Email, modelSEC_User.Password);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                        HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        HttpContext.Session.SetString("Email", dr["Email"].ToString());
                        HttpContext.Session.SetString("Password", dr["Password"].ToString());
                        break;
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid User Name or Password";
                    return RedirectToAction("Index");
                }

                if (HttpContext.Session.GetString("Email") != null && HttpContext.Session.GetString("Password") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        //Logout action to clear current session and redirect user to login page
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SignIN(SEC_UserModel modelSEC_User)
        {
            string connectiongstring = this.Configuration.GetConnectionString("ConStr");
            SqlConnection con = new SqlConnection(connectiongstring);
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_SEC_User_SignIn";
            sqlCommand.Parameters.AddWithValue("@UserName", modelSEC_User.UserName);
            sqlCommand.Parameters.AddWithValue("@Password", modelSEC_User.Password);
            sqlCommand.Parameters.AddWithValue("@Email", modelSEC_User.Email);
            if (Convert.ToBoolean(sqlCommand.ExecuteNonQuery()))
            {
                TempData["SignMEssage"] = "Sign Successfully!!";
            }
            con.Close();
            return RedirectToAction("Index");
        }
    }
}