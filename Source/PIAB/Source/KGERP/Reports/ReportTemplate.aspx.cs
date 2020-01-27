using KGERP.Utility;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KGERP.Reports
{
    public partial class ReportTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                   // rvSiteMapping.ServerReport.ReportServerCredentials = new ReportCredentials("Administrator", "kgerp@123", "Domain");

                    string reportFolder = ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();
                    rvSiteMapping.Height = Unit.Pixel(Convert.ToInt32(Request["Height"]) - 58);
                    rvSiteMapping.ProcessingMode = ProcessingMode.Local;


                    //rvSiteMapping.ServerReport.ReportServerUrl = new Uri("http://192.168.0.7/ReportServer_SQLEXPRESS"); // Add the Reporting Server URL
                    //rvSiteMapping.ServerReport.ReportPath = String.Format("/{0}/{1}", reportFolder, Request["ReportName"].ToString());
                    //rvSiteMapping.ServerReport.Refresh(); 

                    rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/RDLC/ClientClearanceCertificate.rdlc");
                    rvSiteMapping.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource("AllClient", "DataSetClientClearanceCertificate");
                    rvSiteMapping.LocalReport.DataSources.Add(rdc);
                    rvSiteMapping.LocalReport.Refresh();
                    rvSiteMapping.DataBind();

                    //string CS = ConfigurationManager.ConnectionStrings["DBCS2"].ConnectionString;
                    //string searchtext = string.Empty;
                    //using (SqlConnection con = new SqlConnection(CS))
                    //{

                    //    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Northwind WHERE([Country]='" + searchtext + "')", con);
                    //    DataSet ds = new DataSet();
                    //    da.Fill(ds);
                    //    GridView gv = new GridView();
                    //    gv.DataSource = ds;
                    //    gv.DataBind(); 

                    //    ReportDataSource datasource = new ReportDataSource("DataSet1_Northwind", ds.Tables[0]);
                    //    rvSiteMapping.LocalReport.DataSources.Clear();
                    //    rvSiteMapping.LocalReport.DataSources.Add(datasource);
                    //    rvSiteMapping.LocalReport.Refresh();

                    //}

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}