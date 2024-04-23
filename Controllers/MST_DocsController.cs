using Addresh_Book5th.BAL;
using Docs_Editor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;



namespace Docs_Editor.Controllers
{
    [CheckAccess]
    public class MST_DocsController : Controller
    {
       
        public IConfiguration Configuration;
        public MST_DocsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            // Assuming CV is an object of a class containing UserID() method
            int UserID = (int)CV.UserID();

            string connectiongstring = this.Configuration.GetConnectionString("ConStr");
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectiongstring);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_Docs_SelectDocs";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            dt.Load(sqlDataReader);
            return View("MST_Docs_List", dt);
        }


        public IActionResult ViewDocs(int DocsID) 
        {
            int UserID = (int)CV.UserID();
            string connectiongstring = this.Configuration.GetConnectionString("ConStr");
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectiongstring);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_Docs_SelectByID";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@DocsID", DocsID);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            dt.Load(sqlDataReader);
            return View("MST_Docs_ViewDetail",dt);
        }

        public IActionResult Delete(int DocID)
        {
            int UserID = (int)CV.UserID();
            string connectiongstring = this.Configuration.GetConnectionString("ConStr");
            SqlConnection con = new SqlConnection(connectiongstring);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_Docs_Delete";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@DocsID", DocID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        public IActionResult Create(int DocID)
        {
            int UserID = (int)CV.UserID();
            if (DocID != null)
            {
                string connectiongstring = this.Configuration.GetConnectionString("ConStr");
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connectiongstring);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_MST_Docs_SelectByID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@DocsID", DocID);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                dt.Load(sqlDataReader);

                MST_DocsModel modelMST_Docs= new MST_DocsModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelMST_Docs.DocID= Convert.ToInt32(dr["DocID"]);
                    modelMST_Docs.Title = dr["Title"].ToString();
                    modelMST_Docs.Content = dr["Content"].ToString();
                }
                return View("MST_Docs_CreateUpdate", modelMST_Docs);

            }
            return View("MST_Docs_CreateUpdate");
        }

        public IActionResult Save(MST_DocsModel modelMST_Docs) 
        {
            int UserID = (int)CV.UserID();
            string connectiongstring = this.Configuration.GetConnectionString("ConStr");
            SqlConnection con = new SqlConnection(connectiongstring);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelMST_Docs.DocID == null)
            {
                cmd.CommandText = "PR_MST_Docs_Create";
            }
            else
            {
                cmd.CommandText = "PR_MST_Docs_Update";
                cmd.Parameters.AddWithValue("@DocsID", modelMST_Docs.DocID);
            }
            cmd.Parameters.AddWithValue("@Title", modelMST_Docs.Title);
            cmd.Parameters.AddWithValue("@Content", modelMST_Docs.Content);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelMST_Docs.DocID == null)
                {
                    TempData["CreateDocument"] = "Document Created Succesfully";
                }
                else
                {
                    TempData["UpdateDocument"] = "Document Updated Succesfully";
                }
            }
            con.Close();
            return RedirectToAction("Index");
        }

        public IActionResult Cancle()
        {
            return RedirectToAction("Index");
        }
    }
}
