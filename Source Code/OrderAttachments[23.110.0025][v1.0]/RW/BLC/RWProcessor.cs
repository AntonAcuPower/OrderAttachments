using CommonServiceLocator;
using PX.Data;
using PX.Data.Update;
using System;
using System.Web;

namespace ReportViewer
{
    public class RWProcessor
    {
        private readonly ILoginUiService _loginUiService = ServiceLocator.Current.GetInstance<ILoginUiService>();

        public byte[] GetReportByParams(HttpRequest pageRequest)
        {
            var credentials = GetCredFromFile(PXInstanceHelper.AppDataFolder);

            var userName = !string.IsNullOrEmpty(pageRequest["CompanyName"])
                ? string.Join("", credentials[0].Trim(), "@", pageRequest["CompanyName"])
                : credentials[0].Trim();

            var status = _loginUiService.LoginUser(ref userName, credentials[1].Trim());

            if (!status) return null;
            var reportProcessor = PXGraph.CreateInstance<FormViewerSetupMaint>();
            if (!reportProcessor.ValidateUser(pageRequest)) return null;

            var res = reportProcessor.GenerateReport(pageRequest);

            return res;
        }

        private static string[] GetCredFromFile(string folder)
        {
            return System.IO.File.ReadAllText($"{folder}\\\\crd.ddt").Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);
        }
    }
}