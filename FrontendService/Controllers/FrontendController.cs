using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace FrontendService.Controllers
{
    public class FrontendController : Controller
    {
        public string Index()
        {
            // Setting for service fabric communication
            string backendServiceName = "BackendService";
            string httpGatewayPort = "80";

            string gatewayAddress = String.Format(
                    "http://{0}:{1}/Services/{2}/$/ResolvePartition?api-version=1.0",
                    GetInternalGatewayAddress(),
                    httpGatewayPort,
                    backendServiceName);

            // Call Backend service
            //var client = new RestClient("http://localhost:50324/");
            //var request = new RestRequest("api/status", Method.GET);
            //IRestResponse response = client.Execute(request);
            //var content = response.Content; 
            return $"WELCOME TO FRONTEND SERVICE!  -  THE BACKEND RESPONSE: {gatewayAddress}";
        }

        // Temporary workaround for Windows Server 2016 networking. See https://blogs.technet.microsoft.com/virtualization/2016/05/25/windows-nat-winnat-capabilities-and-limitations/
        // Due to a bug you cannot call the host public IP Address from within a container. The workaround is to find the address
        // of the gateway in the container and use this to communicate to the host. This will be fixed in a future Windows Server 2016 release
        private string GetInternalGatewayAddress()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.GetIPProperties().GatewayAddresses != null &&
                    networkInterface.GetIPProperties().GatewayAddresses.Count > 0)
                {
                    foreach (GatewayIPAddressInformation gatewayAddr in networkInterface.GetIPProperties().GatewayAddresses)
                    {
                        if (gatewayAddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return gatewayAddr.Address.ToString();
                        }
                    }
                }
            }
            throw new ArgumentNullException("internalgatewayaddress");
        }

        ////Get the listening IP address of the host for the Frontend service
        //private string GetIpAddress()
        //{
        //    IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        //    IPAddress ipAddress = hostEntry.AddressList.FirstOrDefault(
        //        ip =>
        //            (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
        //    if (ipAddress != null)
        //    {
        //        return ipAddress.ToString();
        //    }
        //    throw new InvalidOperationException("HostIpAddress");
        //}
    }
}