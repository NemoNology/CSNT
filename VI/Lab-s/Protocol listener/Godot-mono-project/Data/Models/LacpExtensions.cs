using System.Net.NetworkInformation;
using LACPsniffer.Data.Models;

namespace LacpSniffer.Data.Models
{
    static class LacpExtensions
    {
        public static string ToHexString(this byte[] bytes)
            => "0x" + string.Join("", bytes.Select(x => x.ToString("x2")));

        public static string ToMacAddressString(this byte[] bytes)
            => bytes.Length < 6 ? "none" : string.Join(":", bytes[..6].Select(x => x.ToString("x2")));

        public static string ToMacAddressString(this PhysicalAddress address)
            => string.Join(":", address.GetAddressBytes().Select(x => x.ToString("x2")));

        public static string ToLacpPortState(this byte b)
        {
            return $"\n\t{((b & 1) == 1 ? "Active" : "Passive")}"
                + $"\n\t{((b & 2) == 2 ? "Fast" : "Slow")}"
                + $"\n\tPort is {((b & 4) == 4 ? "" : "not ")}aggregating"
                + $"\n\tPort is {((b & 8) == 8 ? "synchronized" : "not usable/standby")}"
                + $"\n\tPort is {((b & 16) == 16 ? "" : "not ")}collecting"
                + $"\n\t{((b & 32) != 32 ? "not " : "")}distributing"
                + $"\n\tPacket is {((b & 64) != 64 ? "not" : "")}defaulted"
                + $"\n\t{((b & 128) != 128 ? "not" : "")}expired";
        }

        public static string ToDeviceInfo(this byte[] value)
        {
            var info = new Dictionary<string, string>(7)
            {
                { "Raw", $"[{value.ToHexString()}]" },
                { "System priority", "unknown" },
                { "System ID", "unknown" },
                { "Key", "unknown" },
                { "Port priority", "unknown" },
                { "Port", "unknown" },
                { "State", "unknown" },
                { "Reserved (Raw)", "unknown" }
            };

            var len = value.Length;

            if (len >= 2)
                info["System priority"] = value[..2].ToHexString();
            if (len >= 8)
                info["System ID"] = value[2..8].ToHexString();
            if (len >= 10)
                info["Key"] = value[8..10].ToHexString();
            if (len >= 12)
                info["Port priority"] = value[10..12].ToHexString();
            if (len >= 14)
                info["Port"] = value[12..14].ToHexString();
            if (len >= 15)
            {
                info["State"] = value[14].ToLacpPortState();
                info["Reserved (Raw)"] = $"[{value[15..].ToHexString()}]";
            }

            return string.Join("\n", info.Select(x => x.Key + ": " + x.Value));
        }

        public static string LacpduTlvInfo(this Tlv tlv)
        {
            var tagInfo = $"";
            var LengthInfo = $"Length: 0x{tlv.Length:x2}";
            var ValueInfo = $"Value: ";

            switch (tlv.Tag)
            {
                case 0:
                    tagInfo += "Terminator";
                    ValueInfo += tlv.Value.ToHexString();
                    break;
                case >= 1 and <= 2:
                    tagInfo += tlv.Tag == 1 ? "Actor information" : "Partner information";
                    ValueInfo += tlv.Value.ToDeviceInfo();
                    break;
                case 3:
                    tagInfo += "Collector information";
                    ValueInfo += $"Max delay: {tlv.Value[..2].ToHexString()};";
                    break;
                default:
                    tagInfo += "Unknown";
                    ValueInfo += tlv.Value.ToHexString();
                    break;
            }

            return $"Tag: 0x{tlv.Tag:x2} ({tagInfo})\n{LengthInfo}\n{ValueInfo}";
        }
    }
}