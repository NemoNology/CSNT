namespace LacpSniffer.Data.Models
{
    public struct LacpPacket
    {
        /// <summary>
        /// The protocol type for LACPDU in the packet header
        /// </summary>
        public static readonly int LacpDuPacketType = 0x8809;
        /// <summary>
        /// LACP packets are sent with multicast group MAC-address 01:80:C2:00:00:02
        /// </summary>
        public static readonly byte[] MulticastMacAddress = new byte[6] { 0x01, 0x80, 0xC2, 0x00, 0x00, 0x02 };

        /// <summary>
        /// Destination MAC-address;<br/>
        /// <i>6 bytes</i>;
        /// </summary>
        public byte[] DestinationMacAddress;
        /// <summary>
        /// Source MAC-address;<br/>
        /// <i>6 bytes</i>;
        /// </summary>
        public byte[] SourceMacAddress;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] Length_Type;
        public byte Subtype;
        public byte VersionNumber;
        public byte ActorTlvType;
        public byte ActorInformationLength;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] ActorSystemPriority;
        /// <summary>
        /// ;<br/>
        /// <i>6 bytes</i>;
        /// </summary>
        public byte[] ActorSystem;
        /// <summary>
        /// Number which indicates the port channel configured ID of actor (sender);<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] ActorKey;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] ActorPortPriority;
        /// <summary>
        /// Number which represents the number of the sender’s physical port within the port channel;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] ActorPort;
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
        /// ;<br/>
        /// <i>3 bytes</i>;
        /// </summary>
        public byte[] ActorReserved;
        public byte PartnerTlvType;
        public byte PartnerInformationLength;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] PartnerSystemPriority;
        /// <summary>
        /// ;<br/>
        /// <i>6 bytes</i>;
        /// </summary>
        public byte[] PartnerSystem;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] PartnerKey;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] PartnerPortPriority;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] PartnerPort;
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
        /// <summary>
        /// ;<br/>
        /// <i>3 bytes</i>;
        /// </summary>
        public byte[] PartnerReserved;
        /// <summary>
        /// 
        /// </summary>
        public byte CollectorTlvType;
        public byte CollectorInformationLength;
        /// <summary>
        /// ;<br/>
        /// <i>2 bytes</i>;
        /// </summary>
        public byte[] CollectorMaxDelay;
        /// <summary>
        /// ;<br/>
        /// <i>12 bytes</i>;
        /// </summary>
        public byte[] CollectorReserved;
        public byte TerminatorTlvType;
        public byte TerminatorLength;
        /// <summary>
        /// ;<br/>
        /// <i>50 bytes</i>;
        /// </summary>
        public byte[] Reserved;
        /// <summary>
        /// ;<br/>
        /// <i>4 bytes</i>;
        /// </summary>
        public byte[] FCS;

        /// <summary>
        /// Usual constructor
        /// </summary>
        public LacpPacket(byte[] destinationMacAddress, byte[] sourceMacAddress, byte[] length_type, byte subtype,
                          byte versionNumber, byte actorTlvType, byte actorInformationLength, byte[] actorSystemPriority,
                          byte[] actorSystem, byte[] actorKey, byte[] actorPortPriority, byte[] actorPort,
                          byte actorState, byte[] actorReserved, byte partnerTlvType, byte partnerInformationLength,
                          byte[] partnerSystemPriority, byte[] partnerSystem, byte[] partnerKey,
                          byte[] partnerPortPriority, byte[] partnerPort, byte partnerState, byte[] partnerReserved,
                          byte collectorTlvType, byte collectorInformationLength, byte[] collectorMaxDelay,
                          byte[] collectorReserved, byte terminatorTlvType, byte terminatorLength, byte[] reserved,
                          byte[] fcs)
        {
            DestinationMacAddress = destinationMacAddress;
            SourceMacAddress = sourceMacAddress;
            Length_Type = length_type;
            Subtype = subtype;
            VersionNumber = versionNumber;
            ActorTlvType = actorTlvType;
            ActorInformationLength = actorInformationLength;
            ActorSystemPriority = actorSystemPriority;
            ActorSystem = actorSystem;
            ActorKey = actorKey;
            ActorPortPriority = actorPortPriority;
            ActorPort = actorPort;
            ActorState = actorState;
            ActorReserved = actorReserved;
            PartnerTlvType = partnerTlvType;
            PartnerInformationLength = partnerInformationLength;
            PartnerSystemPriority = partnerSystemPriority;
            PartnerSystem = partnerSystem;
            PartnerKey = partnerKey;
            PartnerPortPriority = partnerPortPriority;
            PartnerPort = partnerPort;
            PartnerState = partnerState;
            PartnerReserved = partnerReserved;
            CollectorTlvType = collectorTlvType;
            CollectorInformationLength = collectorInformationLength;
            CollectorMaxDelay = collectorMaxDelay;
            CollectorReserved = collectorReserved;
            TerminatorTlvType = terminatorTlvType;
            TerminatorLength = terminatorLength;
            Reserved = reserved;
            FCS = fcs;
        }

        /// <summary>
        /// Initialize new LACP packet with zero values
        /// </summary>
        public LacpPacket()
        {
            SourceMacAddress = new byte[6];
            DestinationMacAddress = new byte[6];
            Length_Type = new byte[2];
            Subtype = 0;
            VersionNumber = 0;
            ActorTlvType = 0;
            ActorInformationLength = 0;
            ActorSystemPriority = new byte[2];
            ActorSystem = new byte[6];
            ActorKey = new byte[6];
            ActorPortPriority = new byte[2];
            ActorPort = new byte[6];
            ActorState = 0;
            ActorReserved = new byte[3];
            PartnerTlvType = 0;
            PartnerInformationLength = 0;
            PartnerSystemPriority = new byte[2];
            PartnerSystem = new byte[6];
            PartnerKey = new byte[6];
            PartnerPortPriority = new byte[2];
            PartnerPort = new byte[6];
            PartnerState = 0;
            PartnerReserved = new byte[3];
            CollectorTlvType = 0;
            CollectorInformationLength = 0;
            CollectorMaxDelay = new byte[4];
            CollectorReserved = new byte[12];
            TerminatorTlvType = 0;
            TerminatorLength = 0;
            Reserved = new byte[50];
            FCS = new byte[4];
        }
    }
}