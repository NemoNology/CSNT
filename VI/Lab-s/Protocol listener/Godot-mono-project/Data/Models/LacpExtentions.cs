using LacpSniffer.Data.Models;

namespace LACPsniffer.Data.Models
{
    public static class LacpExtentions
    {
        /// <summary>
        /// Converts bytes array to <see cref="LacpPacket"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>LacpPacket if the packet is valid, packet with zero values - otherwise</returns>
        public static LacpPacket ToLacpPacket(this byte[] bytes)
        {
            if (bytes.Length < 128)
                return new();
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
                bytes[74..124],
                bytes[124..]
            );
        }
    }
}