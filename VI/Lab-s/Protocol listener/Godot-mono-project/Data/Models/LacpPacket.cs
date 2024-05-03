namespace LacpSniffer.Data.Models
{
    public struct LacpPacket
    {
        /// <summary>
        /// The protocol type for LACPDU in the packet header
        /// </summary>
        public static readonly int LacpDuPacketType = 0x8809;
        /// <summary>
        /// LACP packets are sent with multicast group MAC address 01:80:C2:00:00:02
        /// </summary>
        public static readonly byte[] MulticastMacAddress = new byte[6] { 0x01, 0x80, 0xC2, 0x00, 0x00, 0x02 };

        /// <summary>
        /// Source MAC-address
        /// </summary>
        public byte[] SourceMacAddress;
        /// <summary>
        /// Destination MAC-address
        /// </summary>
        public byte[] DestinationMacAddress;
        /// <summary>
        /// Number which indicates the port channel configured ID of actor (sender)
        /// </summary>
        public int ActorKey;
        /// <summary>
        /// Number which represents the number of the sender’s physical port within the port channel
        /// </summary>
        public int ActorPort;
        /// <summary>
        /// <b>Bit 1: Activity</b> - it is <c>true</c> to indicate LACP active mode, <c>false</c> to indicate passive mode<br/>
        /// <b>Bit 2: Timeout</b> - It is <c>true</c> to indicate the device is requesting a fast (1s) transmit interval of its partner, <c>false</c> to indicate that a slow (30s) transmit interval is being requested. <br/>
        /// <b>Bit 3: Aggregation</b> - It is <c>true</c> to indicate that the port is configured for aggregation (typically always <c>true</c>)<br/>
        /// <b>Bit 4: Synchronization</b> - It is <c>true</c> to indicate that the system is ready and willing to use this link in the bundle to carry traffic. <c>false</c> to indicate the link is not usable or is in standby mode.<br/>
        /// <b>Bit 5: Collecting</b> - It is <c>true</c> to indicate that traffic received on this interface will be processed by the device. <c>false</c> otherwise.<br/>
        /// <b>Bit 6: Distributing</b> - It is <c>true</c> to indicate that the device is using this link transmit traffic. <c>false</c> otherwise.<br/>
        /// <b>Bit 7: Expired</b> - It is <c>true</c> to indicate that no LACPDUs have been received by the device during the past 3 intervals. <c>false</c> when at least one LACPDU has been received within the past three intervals.<br/>
        /// <b>Bit 8: Defaulted</b> - It is <c>true</c> to indicate that no LACPDUs have been received during the past 6 intervals. <c>false</c> when at least one LACPDU has been received within the past 6 intervals. Once the defaulted flag transitions to set, any stored partner information is flushed.
        /// </summary>
        public byte ActorState;
        /// <summary>
        /// <b>Bit 1: Activity</b> - it is <c>true</c> to indicate LACP active mode, <c>false</c> to indicate passive mode<br/>
        /// <b>Bit 2: Timeout</b> - It is <c>true</c> to indicate the device is requesting a fast (1s) transmit interval of its partner, <c>false</c> to indicate that a slow (30s) transmit interval is being requested. <br/>
        /// <b>Bit 3: Aggregation</b> - It is <c>true</c> to indicate that the port is configured for aggregation (typically always <c>true</c>)<br/>
        /// <b>Bit 4: Synchronization</b> - It is <c>true</c> to indicate that the system is ready and willing to use this link in the bundle to carry traffic. <c>false</c> to indicate the link is not usable or is in standby mode.<br/>
        /// <b>Bit 5: Collecting</b> - It is <c>true</c> to indicate that traffic received on this interface will be processed by the device. <c>false</c> otherwise.<br/>
        /// <b>Bit 6: Distributing</b> - It is <c>true</c> to indicate that the device is using this link transmit traffic. <c>false</c> otherwise.<br/>
        /// <b>Bit 7: Expired</b> - It is <c>true</c> to indicate that no LACPDUs have been received by the device during the past 3 intervals. <c>false</c> when at least one LACPDU has been received within the past three intervals.<br/>
        /// <b>Bit 8: Defaulted</b> - It is <c>true</c> to indicate that no LACPDUs have been received during the past 6 intervals. <c>false</c> when at least one LACPDU has been received within the past 6 intervals. Once the defaulted flag transitions to set, any stored partner information is flushed.
        /// </summary>
        public byte PartnerState;

        public LacpPacket(
            byte[] sourceMacAddress,
            byte[] destinationMacAddress,
            int actorKey,
            int actorPort,
            byte actorState,
            byte partnerState)
        {
            SourceMacAddress = sourceMacAddress;
            DestinationMacAddress = destinationMacAddress;
            ActorKey = actorKey;
            ActorPort = actorPort;
            ActorState = actorState;
            PartnerState = partnerState;
        }
    }
}