using Helpers;
using PX.Common;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.Reports;
using PX.Data.Update;
using PX.Objects.AR;
using PX.Reports;
using PX.Reports.Controls;
using PX.Reports.Data;
using PX.SM;
using ReportViewer.DAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ReportViewer
{
  public class FormViewerSetupMaint : PXGraph<FormViewerSetupMaint>
  {

        [InjectDependency]
        protected IReportLoaderService ReportLoader { get; private set; }

        [InjectDependency]
        protected internal PX.Reports.IReportDataBinder ReportDataBinder { get; private set; }

        #region Properties

        private static readonly Regex a = new Regex("(?<Path>.*)[?]ID=(?<File>[^&|\\s]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #endregion

        #region Views

        public PXSave<FormViewerSetup> Save;
        public PXCancel<FormViewerSetup> Cancel;

        public PXSelect<FormViewerSetup> Setup;
        public PXSelect<FormViewerSetupDetails> Details;

        #endregion

        #region Selects

        public PXSelect<ARInvoice, Where<ARInvoice.docType, Equal<Required<ARInvoice.docType>>, And<ARInvoice.refNbr, Equal<Required<ARInvoice.refNbr>>>>> Invoice;

        #endregion

        #region Events Handlers

        protected void _(Events.RowUpdating<FormViewerSetup> e)
        {
            if (!(e.NewRow is FormViewerSetup row)) return;

            if (string.IsNullOrEmpty(row.Password))
            {
                PXUIFieldAttribute.SetError<FormViewerSetup.password>(e.Cache, row, "Password empty");
            }

            if (string.IsNullOrEmpty(row.ConfirmPassword))
            {
                PXUIFieldAttribute.SetError<FormViewerSetup.confirmPassword>(e.Cache, row, "Password empty");
            }

            if (row.Password?.Trim() != row.ConfirmPassword?.Trim())
            {
                PXUIFieldAttribute.SetError<FormViewerSetup.confirmPassword>(e.Cache, row, "Password not match");
            }
        }
        #endregion

        #region Public

        public byte[] GenerateReport(System.Web.HttpRequest pageRequest)
        {
            var parameters = new Dictionary<string, string>();
            var reportID = pageRequest.QueryString["ScreenID"];
            parameters["DocType"] = pageRequest.QueryString["DocType"];
            parameters["RefNbr"] = pageRequest.QueryString["RefNbr"];

            var report = LoadReport(reportID, null);
            ReportLoader.InitDefaultReportParameters(report, parameters);
            ReportNode reportNode = ReportDataBinder.CheckIfNull(nameof(ReportDataBinder)).ProcessReportDataBinding(report);

            var data = PX.Reports.Mail.Message.GenerateReport(reportNode, RenderType.FilterPdf).First();

            return data;
        }


        public bool ValidateUser(System.Web.HttpRequest pageRequest)
        {
            var invoice = SelectFrom<ARInvoice>.View.Select(this).RowCast<ARInvoice>().FirstOrDefault(x =>
                x.DocType == pageRequest["DocType"] && x.RefNbr == pageRequest["RefNbr"]);

            var existedCustomer = Details.Select().RowCast<FormViewerSetupDetails>()
                .FirstOrDefault(x => x.CustomerID == invoice?.CustomerID);
            if (existedCustomer == null) return false;

            var userIP = IpProcessor.GetClientIP(pageRequest);

            if (IPAddress.TryParse(userIP, out var correctUserIP))
            {
                var isValid = IpProcessor.ValidateHostAddress(new string(existedCustomer.IPAddress.Where(c => !char.IsWhiteSpace(c)).ToArray()), correctUserIP);
                return isValid;
            }

            return false;
        }




        #endregion

        #region Private
        private Report LoadReport(string reportID, IPXResultset incoming)
        {
            SiteMap siteMap = PXSelect<SiteMap, Where<PX.SM.SiteMap.screenID, Equal<Required<PX.SM.SiteMap.screenID>>>>.Select(this, reportID);
            var report = new Report();
            var match = a.Match(siteMap.Url);
            if (string.IsNullOrEmpty(match.Value))
                return null;
            var reportName = match.Groups["File"].Value;
            var length = reportName.IndexOf("#", StringComparison.Ordinal);
            if (length > 0)
                reportName = reportName.Substring(0, length);
            report.Name = siteMap.Title;
            if (!match.ToString().ToLower().Contains("rmlauncher.aspx"))
            {
                report.LoadByName(reportName, null);
                var soapNavigator = new SoapNavigator(this, incoming);
                report.DataSource = soapNavigator;
                report.ApplyRules(report);
            }
            report.SchemaUrl = reportName;
            return report;
        }



        #endregion

        #region Overriden
        public override void Persist()
        {
            base.Persist();
            var userName = Setup.Current.UserName;
            var userPassword = Setup.Current.Password;
            string credsSave = $"{userName}\r\n{userPassword}";
            var folder = PXInstanceHelper.AppDataFolder;
            System.IO.File.WriteAllText($"{folder}\\crd.ddt", credsSave);
        }

        #endregion

    }
}