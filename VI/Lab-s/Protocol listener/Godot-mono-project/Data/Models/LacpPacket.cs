namespace LacpSniffer.Data.Models
{
    /// <summary>
    /// Packet of LACP - LACPDU
    /// </summary>
    public struct LacpPacket
    {
        public const int LENGTH = 124;
        /// <summary>
        /// The protocol type for LACPDU in the packet header
        /// </summary>
        public static readonly byte[] TypeLengthOfLacpPacket = { 0x88, 0x09 };
        public static readonly byte SubtypeOfLacpPacket = 0x01;
        /// <summary>
        /// LACP packets are sent with multicast group MAC-address 01:80:C2:00:00:02
        /// </summary>
        public static readonly byte[] LacpDestinationAddress = new byte[6] { 0x01, 0x80, 0xC2, 0x00, 0x00, 0x02 };

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
        public byte[] TypeLength;
        public byte Subtype;
        public byte VersionNumber;
        public byte ActorTlv;
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
        public byte[] ActorSystemMacAddress;
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
        public byte PartnerTlv;
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
        public byte[] PartnerSystemMacAddress;
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
        public byte CollectorTlv;
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
        public byte TerminatorTlv;
        public byte TerminatorLength;
        /// <summary>
        /// ;<br/>
        /// <i>50 bytes</i>;
        /// </summary>
        public byte[] Reserved;

        public readonly string FullInfoAsString
            => $"LACP Packet\n\t"
            + $"Source MAC-address: {SourceMacAddress.ToMacAddressString()}\n\t"
            + $"LACP version: {VersionNumber:x2}\n\t"
            + $"Actor TLV: {ActorTlv:x2}\n\t"
            + $"Actor system priority: {ActorSystemPriority.ToHexString()}\n\t"
            + $"Actor system address: {ActorSystemMacAddress.ToMacAddressString()}\n\t"
            + $"Actor key: {ActorKey.ToHexString()}\n\t"
            + $"Actor port priority: {ActorPortPriority.ToHexString()}\n\t"
            + $"Actor port: {ActorPort.ToHexString()}\n\t"
            + $"Actor state: {ActorState} [{Convert.ToString(ActorState, 2).PadLeft(8, '0')}]\n\t\t"
            + $"{ActorState.ToLacpPortState("\n\t\t")}\n\t"
            + $"Actor reversed: {ActorReserved.ToHexString()}\n\t"
            + $"Partner TLV: {PartnerTlv:x2}\n\t"
            + $"Partner system priority: {PartnerSystemPriority.ToHexString()}\n\t"
            + $"Partner system address: {PartnerSystemMacAddress.ToMacAddressString()}\n\t"
            + $"Partner key: {PartnerKey.ToHexString()}\n\t"
            + $"Partner port priority: {PartnerPortPriority.ToHexString()}\n\t"
            + $"Partner port: {PartnerPort.ToHexString()}\n\t"
            + $"Partner state: {PartnerState} [{Convert.ToString(PartnerState, 2).PadLeft(8, '0')}]\n\t\t"
            + $"{PartnerState.ToLacpPortState("\n\t\t")}\n\t"
            + $"Partner reversed: {PartnerReserved.ToHexString()}\n\t"
            + $"Collector TLV: {CollectorTlv:x2}\n\t"
            + $"Collector max delay: {CollectorMaxDelay.ToHexString()}\n\t"
            + $"Collector reserved: {CollectorReserved.ToHexString()}\n\t"
            + $"Terminator TLV: {TerminatorTlv:x2}\n\t"
            + $"Terminator length: {TerminatorTlv:x2}\n\t"
            + $"Reserved: {Reserved.ToHexString()}\n\t"
            + $"(Packet length: {LENGTH - 50 + Reserved.Length})"
            ;

        /// <summary>
        /// Usual constructor
        /// </summary>
        /// <param name="destinationMacAddress">MAC-address of packet receiver <c>6 bytes; Ethernet2</c></param>
        /// <param name="sourceMacAddress">MAC-address of packet sender <c>6 bytes; Ethernet2</c></param>
        /// <param name="length_type">Packet Length/Type <c>Const value - 0x8809; 2 bytes; Ethernet2</c></param>
        /// <param name="subtype">Packet subtype <c>Const value - 0x01; LACPDU</c></param>
        /// <param name="versionNumber">LACP version number</param>
        /// <param name="actorTlvType"></param>
        /// <param name="actorInformationLength"></param>
        /// <param name="actorSystemPriority"></param>
        /// <param name="actorSystem"></param>
        /// <param name="actorKey"></param>
        /// <param name="actorPortPriority"></param>
        /// <param name="actorPort"></param>
        /// <param name="actorState"></param>
        /// <param name="actorReserved"></param>
        /// <param name="partnerTlvType"></param>
        /// <param name="partnerInformationLength"></param>
        /// <param name="partnerSystemPriority"></param>
        /// <param name="partnerSystem"></param>
        /// <param name="partnerKey"></param>
        /// <param name="partnerPortPriority"></param>
        /// <param name="partnerPort"></param>
        /// <param name="partnerState"></param>
        /// <param name="partnerReserved"></param>
        /// <param name="collectorTlvType"></param>
        /// <param name="collectorInformationLength"></param>
        /// <param name="collectorMaxDelay"></param>
        /// <param name="collectorReserved"></param>
        /// <param name="terminatorTlvType"></param>
        /// <param name="terminatorLength"></param>
        /// <param name="reserved"></param>
        /// <param name="fcs"></param>
        public LacpPacket(byte[] destinationMacAddress, byte[] sourceMacAddress, byte[] length_type, byte subtype,
                          byte versionNumber, byte actorTlvType, byte actorInformationLength, byte[] actorSystemPriority,
                          byte[] actorSystem, byte[] actorKey, byte[] actorPortPriority, byte[] actorPort,
                          byte actorState, byte[] actorReserved, byte partnerTlvType, byte partnerInformationLength,
                          byte[] partnerSystemPriority, byte[] partnerSystem, byte[] partnerKey,
                          byte[] partnerPortPriority, byte[] partnerPort, byte partnerState, byte[] partnerReserved,
                          byte collectorTlvType, byte collectorInformationLength, byte[] collectorMaxDelay,
                          byte[] collectorReserved, byte terminatorTlvType, byte terminatorLength, byte[] reserved)
        {
            DestinationMacAddress = destinationMacAddress;
            SourceMacAddress = sourceMacAddress;
            TypeLength = length_type;
            Subtype = subtype;
            VersionNumber = versionNumber;
            ActorTlv = actorTlvType;
            ActorInformationLength = actorInformationLength;
            ActorSystemPriority = actorSystemPriority;
            ActorSystemMacAddress = actorSystem;
            ActorKey = actorKey;
            ActorPortPriority = actorPortPriority;
            ActorPort = actorPort;
            ActorState = actorState;
            ActorReserved = actorReserved;
            PartnerTlv = partnerTlvType;
            PartnerInformationLength = partnerInformationLength;
            PartnerSystemPriority = partnerSystemPriority;
            PartnerSystemMacAddress = partnerSystem;
            PartnerKey = partnerKey;
            PartnerPortPriority = partnerPortPriority;
            PartnerPort = partnerPort;
            PartnerState = partnerState;
            PartnerReserved = partnerReserved;
            CollectorTlv = collectorTlvType;
            CollectorInformationLength = collectorInformationLength;
            CollectorMaxDelay = collectorMaxDelay;
            CollectorReserved = collectorReserved;
            TerminatorTlv = terminatorTlvType;
            TerminatorLength = terminatorLength;
            Reserved = reserved;
        }

        /// <summary>
        /// Initialize new LACP packet with zero values
        /// </summary>
        public LacpPacket()
        {
            SourceMacAddress = new byte[6];
            DestinationMacAddress = new byte[6];
            TypeLength = new byte[2];
            Subtype = 0;
            VersionNumber = 0;
            ActorTlv = 0;
            ActorInformationLength = 0;
            ActorSystemPriority = new byte[2];
            ActorSystemMacAddress = new byte[6];
            ActorKey = new byte[6];
            ActorPortPriority = new byte[2];
            ActorPort = new byte[6];
            ActorState = 0;
            ActorReserved = new byte[3];
            PartnerTlv = 0;
            PartnerInformationLength = 0;
            PartnerSystemPriority = new byte[2];
            PartnerSystemMacAddress = new byte[6];
            PartnerKey = new byte[6];
            PartnerPortPriority = new byte[2];
            PartnerPort = new byte[6];
            PartnerState = 0;
            PartnerReserved = new byte[3];
            CollectorTlv = 0;
            CollectorInformationLength = 0;
            CollectorMaxDelay = new byte[4];
            CollectorReserved = new byte[12];
            TerminatorTlv = 0;
            TerminatorLength = 0;
            Reserved = new byte[50];
        }

        public override readonly string ToString()
            => $"LACP Packet {{ Dest: {DestinationMacAddress.ToMacAddressString()}"
                + $"; Src: {SourceMacAddress.ToMacAddressString()}"
            + $"; Type/Length: 0x{string.Join("", TypeLength.Select(x => x.ToString("x2")))};"
                + $"; Version: {VersionNumber:x2}; }}";
    }
}