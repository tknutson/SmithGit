﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteManagement;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var dbMgr = new DatabaseManager();        

        try
        {
            var photoInfoList = dbMgr.getFrontPagePhotoInfo();

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