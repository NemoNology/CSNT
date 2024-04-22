using System.Net;
using System.Net.NetworkInformation;

namespace CSNT.Clientserverchat.Data.Models
{
    public class NetHelper
    {
        public static bool IsPortForTransportProtocolAvailable(int port, bool isUdp)
        {
            var props = IPGlobalProperties.GetIPGlobalProperties();
            foreach (IPEndPoint endPoint in isUdp ?
                props.GetActiveUdpListeners() : props.GetActiveTcpListeners())
            {
                if (endPoint.Port == port)
                    return false;
            }
            return true;
        }

        public static bool IsThereActiveListenerWithSpecifiedAddress(IPEndPoint endPoint, bool isUdp)
        {
            var props = IPGlobalProperties.GetIPGlobalProperties();
            foreach (IPEndPoint listenerEndPoint in isUdp ?
                props.GetActiveUdpListeners() : props.GetActiveTcpListeners())
            {
                if (listenerEndPoint.Address == endPoint.Address
                    && listenerEndPoint.Port == endPoint.Port)
                    return true;
            }

            return false;
        }
    }
}