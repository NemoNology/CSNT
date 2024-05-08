namespace CSNT.Clientserverchat.Data.Models
{
    public enum ClientState
    {
        /// <summary>
        /// Client not connected to server or disconnected already
        /// </summary>
        Disconnected,
        /// <summary>
        /// Client is connecting
        /// </summary>
        Connecting,
        /// <summary>
        /// Client connected to server
        /// </summary>
        Connected
    }
}