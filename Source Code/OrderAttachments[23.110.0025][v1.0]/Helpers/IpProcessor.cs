using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Helpers
{
    public static class IpProcessor
    {
        public static string GetClientIP(HttpRequest request)
        {
            string _ipList = request.Headers["CF-CONNECTING-IP"]?.ToString();
            if (!string.IsNullOrWhiteSpace(_ipList))
            {
                return _ipList.Split(',')[0].Trim();
            }
            else
            {
                _ipList = request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
                if (!string.IsNullOrWhiteSpace(_ipList))
                {
                    return _ipList.Split(',')[0].Trim();
                }
                else
                {
                    _ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrWhiteSpace(_ipList))
                    {
                        return _ipList.Split(',')[0].Trim();
                    }
                    else
                    {
                        return request.ServerVariables["REMOTE_ADDR"]?.ToString()?.Trim();
                    }
                }
            }
        }



        public static bool ValidateHostAddress(string dbAddress, IPAddress userIP)
        {
            var domainAddresses = new List<string>();
            var addressList = dbAddress.Split(';').ToList();
            foreach (var address in addressList)
            {
                if (IPAddress.TryParse(address, out var newAddress))
                {
                    if (newAddress.Equals(userIP))
                    {
                        return true;
                    }
                }
                else
                {
                    domainAddresses.Add(address);
                }
            }

            return domainAddresses.Select(Dns.GetHostAddresses).Any(ipAddress => ipAddress.Contains(userIP));
        }
    }
}
