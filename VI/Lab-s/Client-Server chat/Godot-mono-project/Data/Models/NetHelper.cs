using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace CSNT.Clientserverchat.Data.Models
{
    public class NetHelper
    {
        public static bool IsAddressForTransportProtocolAvailable(IPEndPoint endPoint, bool isUdp)
        {
            var props = IPGlobalProperties.GetIPGlobalProperties();
            foreach (IPEndPoint listenerEndPoint in isUdp ?
                props.GetActiveUdpListeners() : props.GetActiveTcpListeners())
            {
                if (listenerEndPoint.Port == endPoint.Port
                    && listenerEndPoint.Address.Equals(endPoint.Address))
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
                if (listenerEndPoint.Port == endPoint.Port
                    && listenerEndPoint.Address.Equals(endPoint.Address))
                    return true;
            }

            return false;
        }
    }
}