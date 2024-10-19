namespace LacpSniffer.Data.Models
{
    public static class LacpExtensions
    {
        /// <summary>
        /// Converts bytes array to <see cref="LacpPacket"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>LacpPacket if the packet is valid, packet with zero values - otherwise</returns>
        public static LacpPacket ToLacpPacket(this byte[] bytes)
        {
            return new(
                bytes[..6],
                bytes[6..12],
                bytes[12..14],
                bytes[14],
                bytes[15],
                bytes[16],
                bytes[17],
                bytes[18..20],
                bytes[20..26],
                bytes[26..28],
                bytes[28..30],
                bytes[30..32],
                bytes[32],
                bytes[33..36],
                bytes[36],
                bytes[37],
                bytes[38..40],
                bytes[40..46],
                bytes[46..48],
                bytes[48..50],
                bytes[50..52],
                bytes[52],
                bytes[53..56],
                bytes[56],
                bytes[57],
                bytes[58..60],
                bytes[60..72],
                bytes[72],
                bytes[73],
                bytes[74..]
            );
        }

        public static bool IsLacpPacket(this byte[] bytes)
            => bytes[12..14].SequenceEqual(LacpPacket.TypeLengthOfLacpPacket)
            && bytes[..6].SequenceEqual(LacpPacket.LacpDestinationAddress);

        public static string ToHexString(this byte[] bytes)
            => string.Join("", bytes.Select(x => x.ToString("x2")));

        public static string ToMacAddressString(this byte[] bytes)
            => bytes.Length < 6 ? "none" : string.Join(":", bytes[..6].Select(x => x.ToString("x2")));

        public static string ToLacpPortState(this byte b, string postfix = " ")
        {
            return $"{((b & 1) == 1 ? "Active" : "Passive")}{postfix}"
                + $"{((b & 2) == 2 ? "Fast" : "Slow")}{postfix}"
                + $"Port is {((b & 4) == 4 ? "" : "not ")}aggregating{postfix}"
                + $"Port is {((b & 8) == 8 ? "synchronized" : "not usable/standby")}{postfix}"
                + $"Port is {((b & 16) == 16 ? "" : "not ")}collecting{postfix}"
                + $"{((b & 32) != 32 ? "not " : "")}distributing{postfix}"
                + $"Packet is {((b & 64) != 64 ? "not" : "")}defaulted{postfix}"
                + $"{((b & 128) != 128 ? "not" : "")}expired";
        }
    }
}