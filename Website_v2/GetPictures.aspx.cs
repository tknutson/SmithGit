using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteManagement;

public partial class Architecture : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var dbMgr = new DatabaseManager();

        try
        {
            string category = Request.QueryString["category"];
            Page.Title = category;

            var photoInfoList = dbMgr.getPhotoInfo(System.Configuration.ConfigurationManager.AppSettings[category].ToString());
            var photoInfoList_thumbs = photoInfoList;

            photoInfo.DataSource = photoInfoList;
            photoInfo.DataBind();

            photoInfo1.DataSource = photoInfoList;
            photoInfo1.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message + "\r\n" + Ex.StackTrace);
        }
    }
}