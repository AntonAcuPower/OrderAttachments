using System;
using OrderAttachments;
using PX.Data;

public partial class Api_OrderAttachments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var graph = PXGraph.CreateInstance<OrderAttachmentsMaint>();
        var data = graph.GetFile(Request.QueryString["fileID"]);
        if (data == null) return;
        Response.ContentType= "application/octet-stream";
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", Request.QueryString["fileName"]));
        Response.BinaryWrite(data);
        Response.End();

    }
}