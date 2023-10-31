using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Helpers;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.Common;
using PX.Objects.SO;
using PX.Objects.TX;
using PX.SM;
using ReportViewer.DAC;

namespace OrderAttachments
{

    public class OrderAttachmentsMaint : PXGraph<OrderAttachmentsMaint>
    {
        public static bool IsActive()
        {
            return true;
        }
        #region Views
        public SelectFrom<OrderAttachment>.View UploadedFiles;

        public IEnumerable uploadedFiles()
        {
            var objPageRequest = HttpContext.Current.Request;
            var result = new List<OrderAttachment>();
            if (ValidateCustomer(objPageRequest, out var customerId))
            {
                result.AddRange(GetOrders(customerId));
                result.AddRange(GetShipments(customerId));
                result.AddRange(GetInvoices(customerId));

            }
            return result;
        }        
        #endregion

        #region Public
        public string GetHtml()
        {
            var doc = new XmlDocument();
            var bodyNode = doc.CreateElement("body");
            var tableNode = doc.CreateElement("table");
            foreach (var item in UploadedFiles.Select())
            {
                var orderAttachment = (OrderAttachment)item;
                var trNode = doc.CreateElement("tr");

                var tdNode1 = doc.CreateElement("td");
                tdNode1.InnerText = orderAttachment.OrderType.ToString();
                trNode.AppendChild(tdNode1);

                var tdNode2 = doc.CreateElement("td");
                tdNode2.InnerText = orderAttachment.OrderNbr.ToString();
                trNode.AppendChild(tdNode2);

                var tdNode3 = doc.CreateElement("td");
                var aNode = doc.CreateElement("a");
                aNode.InnerText = orderAttachment.FileName.ToString();
                var hrefAttribute = doc.CreateAttribute("href");
                hrefAttribute.Value = $"/Api/DownloadFile.aspx?fileID={orderAttachment.FileID}&fileName={orderAttachment.FileName}";
                aNode.Attributes.Append(hrefAttribute);
                tdNode3.AppendChild(aNode);
                trNode.AppendChild(tdNode3);

                tableNode.AppendChild(trNode);
            }
            bodyNode.AppendChild(tableNode);
            var styleNode = doc.CreateElement("style");
            styleNode.InnerText = "table {\r\n  border-collapse: collapse; \r\n} table, td, th {\r\n  border: 1px solid;\r\n}";
            bodyNode.AppendChild(styleNode);
            doc.AppendChild(bodyNode);

            return doc.InnerXml;
        }

        public byte[] GetFile(string fileID)
        {
            var uploadFileMaintenance = PXGraph.CreateInstance<UploadFileMaintenance>();
            var guidFileID = Guid.Parse(fileID);            
            var file = uploadFileMaintenance.GetFile(guidFileID);
            if (file == null) return null;
            return file.BinData;
        }
        #endregion

        #region Private
        private IEnumerable<OrderAttachment> GetOrders(int customerId)
        {
            var query = new SelectFrom<SOOrder>.
                    InnerJoin<NoteDoc>.
                        On<NoteDoc.noteID.IsEqual<SOOrder.noteID>>.
                    InnerJoin<UploadFile>.
                        On<UploadFile.fileID.IsEqual<NoteDoc.fileID>>.
                    Where<SOOrder.customerID.IsEqual<@P.AsInt>.
                        And<UploadFile.primaryScreenID.IsEqual<Constants.SalesOrdersScreenId>>>();
            var view = query.CreateView(this);
            var selectedData = view.SelectMulti(new object[] { customerId });
            view.Clear();
            var result = new List<OrderAttachment>();
            foreach (PXResult<SOOrder, NoteDoc, UploadFile> item in selectedData)
            {
                var orderItem = (SOOrder)item;
                var fileItem = (UploadFile)item;
                result.Add(new OrderAttachment()
                {
                    OrderType = orderItem.OrderType,
                    OrderNbr = orderItem.OrderNbr,
                    FileName = fileItem.Name,
                    FileID = fileItem.FileID
                });
            }
            return result;
        }

        private IEnumerable<OrderAttachment> GetShipments(int customerId)
        {
            var query = new SelectFrom<SOShipment>.
                    InnerJoin<SOOrderShipment>.
                        On<SOShipment.shipmentNbr.IsEqual<SOOrderShipment.shipmentNbr>>.
                    InnerJoin<NoteDoc>.
                        On<NoteDoc.noteID.IsEqual<SOShipment.noteID>>.
                    InnerJoin<UploadFile>.
                        On<UploadFile.fileID.IsEqual<NoteDoc.fileID>>.
                    Where<SOShipment.customerID.IsEqual<@P.AsInt>.
                        And<UploadFile.primaryScreenID.IsEqual<Constants.ShipmentsScreenId>>>();
            var view = query.CreateView(this);
            var selectedData = view.SelectMulti(new object[] { customerId });
            view.Clear();
            var result = new List<OrderAttachment>();
            foreach (PXResult<SOShipment, SOOrderShipment, NoteDoc, UploadFile> item in selectedData)
            {
                var orderItem = (SOOrderShipment)item;
                var fileItem = (UploadFile)item;
                result.Add(new OrderAttachment()
                {
                    OrderType = orderItem.OrderType,
                    OrderNbr = orderItem.OrderNbr,
                    FileName = fileItem.Name,
                    FileID = fileItem.FileID
                });
            }
            return result;
        }

        private IEnumerable<OrderAttachment> GetInvoices(int customerId)
        {
            var query = new SelectFrom<SOInvoice>.
                    InnerJoin<NoteDoc>.
                        On<NoteDoc.noteID.IsEqual<SOInvoice.noteID>>.
                    InnerJoin<UploadFile>.
                        On<UploadFile.fileID.IsEqual<NoteDoc.fileID>>.
                    Where<SOInvoice.customerID.IsEqual<@P.AsInt>.
                        And<UploadFile.primaryScreenID.IsEqual<Constants.InvoiceScreenId>>>();
            var view = query.CreateView(this);
            var selectedData = view.SelectMulti(new object[] { customerId });
            view.Clear();
            var result = new List<OrderAttachment>();
            foreach (PXResult<SOInvoice, NoteDoc, UploadFile> item in selectedData)
            {
                var orderItem = (SOInvoice)item;
                var fileItem = (UploadFile)item;
                result.Add(new OrderAttachment()
                {
                    OrderType = orderItem.SOOrderType,
                    OrderNbr = orderItem.SOOrderNbr,
                    FileName = fileItem.Name,
                    FileID = fileItem.FileID
                });
            }
            return result;
        }

        private bool ValidateCustomer(HttpRequest pageRequest, out int customerId)
        {
            customerId = 0;
            var userIP = IpProcessor.GetClientIP(pageRequest);
            if (IPAddress.TryParse(userIP, out var actualUserIP))
            {
                var exitedCustomer = SelectFrom<FormViewerSetupDetails>.View.
                    Select(this).
                    RowCast<FormViewerSetupDetails>().
                    FirstOrDefault(customer =>
                        IpProcessor.ValidateHostAddress(new string(customer.IPAddress.Where(c => !char.IsWhiteSpace(c)).ToArray()), actualUserIP));
                if (exitedCustomer == null) return false;
                customerId = exitedCustomer.CustomerID.Value;
                return true;
            }

            return false;
        }

        

        #endregion
    }
}