using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace ParkHouseSchool.SleepingDragons
{
    public partial class Index : System.Web.UI.Page
    {
        public string pageCount = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string ipaddress = IpAddress();
            string hostname;
            try
            {
                hostname = Dns.GetHostByAddress(ipaddress).HostName;
            }
            catch (Exception)
            {
                hostname = "(no dns)";
            } 
            StreamWriter sw = new StreamWriter(Server.MapPath("visitors.log"), true);
            sw.WriteLine(DateTime.Now.ToString() + "," 
                + ipaddress + "," + hostname + "," + Request.Url.ToString());
            sw.Close();

            if (ipaddress == "80.177.163.132" || ipaddress == "127.0.0.1")
                pageCount = "(dev)";
            else
                pageCount = GetCount().ToString();
        }

        private string IpAddress()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDRD_FOR"];
            if (ip != null) return ip;

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        int GetCount()
        {
            string connstr = "provider=sqloledb;server=(local)\\SQLExpress;database=web;Integrated Security=SSPI";
            OleDbConnection conn = new OleDbConnection(connstr);
            conn.Open();

            int retval = 0;
            string sql2 = "SELECT nSessions from AccessCount WHERE Site=?";
            OleDbCommand command2 = new OleDbCommand(sql2, conn);
            command2.Parameters.AddWithValue("Site", "ParkHouseSchool.SleepingDragons");
            OleDbDataReader reader = command2.ExecuteReader();
            if (reader.Read())
                retval = reader.GetOrdinal("nSessions");

            if (Session["ParkHouseSchool.SleepingDragons.counted"] == null)
            {
                retval++;
                string sql = "UPDATE AccessCount SET nSessions=? WHERE Site=?";
                OleDbCommand command = new OleDbCommand(sql, conn);
                command.Parameters.AddWithValue("nSessions", retval);
                command.Parameters.AddWithValue("Site", "ParkHouseSchool.SleepingDragons");
                command.ExecuteNonQuery();
                Session["ParkHouseSchool.SleepingDragons.counted"] = "true";
            }

            conn.Close();
            return retval;
        }
    }
}
