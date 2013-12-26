using System;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using System.Linq;

namespace ParkHouseSchool
{
    public partial class Julia : System.Web.UI.MasterPage
    {
        public string pageCount = "";

        public void Page_Load(object sender, EventArgs e)
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
            try
            {
                if (Request["nocount"] == null )
                    pageCount = GetCount().ToString();
                else
                    pageCount = "(local)";
            }
            catch (Exception)
            {
                // proabbly on my laptop
            }
        }

        private string IpAddress()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDRD_FOR"];
            if (ip != null) return ip;

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        int GetCount()
        {
            WebUtils.HitManager ht = new WebUtils.HitManager();
            int retval = ht.GetHitCount("ParkHouseSchool"); 
            if (Session["counted"] == null)
            {
                retval++;
                ht.IncrementHitCount("ParkHouseSchool");
                Session["counted"] = "true";
            }
            return retval;
        }
    }
}
