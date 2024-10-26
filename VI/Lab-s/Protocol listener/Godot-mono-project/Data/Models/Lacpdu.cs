using System.Net.NetworkInformation;
#nullable enable

namespace LACPsniffer.Data.Models
{
    record Tlv(byte Tag, byte Length, byte[] Value)
    {
        public int Size => 2 + Value.Length;

        public static bool TryParse(byte[] bytes, out Tlv? tlv)
        {
            tlv = null;

            if (bytes.Length < 2)
                return false;

            var tag = bytes[0];
            var length = bytes[1];
            byte[] value;
            if (length + 2 > bytes.Length)
                value = bytes[2..];
            else
                value = bytes[2..(length + 2)];

            tlv = new(tag, length, value);
            return true;
        }
    }

    readonly struct Lacpdu
    {
        public const int MINIMUM_LENGTH = 15;
        public static readonly PhysicalAddress ConstDestinationMacAddress = new(new byte[6] { 0x01, 0x80, 0xC2, 0x00, 0x00, 0x02 });
        public static readonly byte[] ConstTypeLength = { 0x88, 0x09 };
        public static readonly byte ConstSubtype = 0x01;

        public readonly PhysicalAddress DestinationMacAddress = ConstDestinationMacAddress;
        public readonly PhysicalAddress SourceMacAddress;
        public readonly byte[] TypeLength = ConstTypeLength;
        public readonly byte Subtype = ConstSubtype;
        public readonly byte? Version;
        public readonly Tlv[] Tlvs;

        public Lacpdu(PhysicalAddress sourceMacAddress, Tlv[] tlvs, byte? version = null) : this()
        {
            SourceMacAddress = sourceMacAddress;
            Version = version;
            Tlvs = tlvs;
        }

        public static bool TryParse(byte[] data, out Lacpdu? lacpdu)
        {
            lacpdu = null;

            if (data.Length < MINIMUM_LENGTH)
                return false;

            if (!(data[..6].SequenceEqual(ConstDestinationMacAddress.GetAddressBytes())
                && data[12..14].SequenceEqual(ConstTypeLength)
                && data[14] == ConstSubtype))
            {
                return false;
            }

            PhysicalAddress sourceMacAddress = new(data[6..12]);

            if (data.Length < 16)
            {
                lacpdu = new(sourceMacAddress, Array.Empty<Tlv>());
                return true;
            }

            var version = data[15];
            var currentByteCounter = 16;
            List<Tlv> tlvs = new();

            while (Tlv.TryParse(data[currentByteCounter..], out Tlv? tlv))
            {
                currentByteCounter += tlv!.Size;
                tlvs.Add(tlv);
                if (tlv.Tag == 0)
                    break;
            }

            lacpdu = new(sourceMacAddress, tlvs.ToArray(), version);

            return true;
        }

        public override readonly string ToString() =>
            $"LACPDU: {{ dst: slow ({DestinationMacAddress.ToMacAddressString()}); src: {SourceMacAddress.ToMacAddressString()}; tl: {TypeLength.ToHexString()}; version: 0x{Version:x2} }}";

        public readonly string FullInfo
        {
            get
            {
                var fullInfo = $"LACP Packet\n\t"
                + $"Destination MAC-address: {DestinationMacAddress.ToMacAddressString()}\n\t"
                + $"Source MAC-address: {SourceMacAddress.ToMacAddressString()}\n\t"
                + $"Type/Length: {TypeLength.ToHexString()}\n\t"
                + $"Subtype: 0x{Subtype:x2}\n\t"
                + $"LACP version: 0x{Version:x2}";

                foreach (var tlv in Tlvs)
                    fullInfo += $"\n\t{tlv.LacpduTlvInfo()}";

                fullInfo += $"\n\tLength: {MINIMUM_LENGTH + Tlvs.Sum(tlv => tlv.Size)}";

                return fullInfo;
            }
        }
    }
}