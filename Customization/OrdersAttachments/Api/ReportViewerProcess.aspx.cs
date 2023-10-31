using System;
using System.Web.UI;
using ReportViewer;

public partial class Api_ReportViewerProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var objPageRequest = Page.Request;
            var data = new RWProcessor().GetReportByParams(objPageRequest);
            
            if (data != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", data.Length.ToString());
                Response.BinaryWrite(data);
            }
            else
            {
                throw new Exception("You don't have a permission.");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}