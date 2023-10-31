using System;
using System.Linq;
using OrderAttachments;
using PX.Data;

public partial class Api_OrderAttachments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var url = Request.Url.ToString().Replace(Request.Url.LocalPath.Split('/').Last(), "");
        var graph = PXGraph.CreateInstance<OrderAttachmentsMaint>();
        Response.Write(graph.GetHtml(url));

        
    }
}