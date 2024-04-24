using System.Net;
using System.Net.NetworkInformation;

namespace CSNT.Clientserverchat.Data.Models
{
    public class NetHelper
    {
        public static bool IsAddressForTransportProtocolAvailable(IPEndPoint endPoint, bool isUdp)
        {
            foreach (IPEndPoint listenerEndPoint in
                isUdp ?
                IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveUdpListeners()
                :
                IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners())
            {
                if (listenerEndPoint.Port == endPoint.Port
                    && listenerEndPoint.Address.Equals(endPoint.Address))
                    return false;
            }
            return true;
        }

        public static bool IsThereActiveListenerWithSpecifiedAddress(IPEndPoint endPoint, bool isUdp)
        {
            foreach (IPEndPoint listenerEndPoint in
                isUdp ?
                IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveUdpListeners()
                :
                IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners())
            {
                if (listenerEndPoint.Port == endPoint.Port
                    && listenerEndPoint.Address.Equals(endPoint.Address))
                    return true;
            }

            return false;
        }
    }
}