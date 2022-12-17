using System;

namespace CSNT_Lab_4
{
    class IP
    {
        public static byte[] _ip = new byte[4];
        public static byte[] _mask = new byte[4];
        public static byte[] _wildMask = new byte[4];
        public static byte _bitSize;
        public const int V = 4;


        public static int SetIP(string ip)
        {
            string[] oktets = ip.Split('.');
            byte[] bytes = new byte[V];

            for (int i = 0; i < V; i++)
            {
                try
                {
                    bytes[i] = Convert.ToByte(oktets[i].Trim(' '));
                }
                catch
                {
                    return -1;
                }
            }

            _ip = bytes;
            return 1;
        }

        public static int SetMask(int bitSize)
        {
            _bitSize = (byte)bitSize;
			
			if (_bitSize > 32)
			{
				_bitSize = 0;
				return -1;
			}
			
			string binWord = string.Empty.PadLeft(32 - _bitSize, '0');

			for (int i = 0; i < _bitSize; i++)
			{
                binWord = binWord.Insert(0, "1");
			}

			for (int i = 0; i < V; i++)
            {
				_mask[i] = Convert.ToByte(binWord.Substring(i * 8, 8), 2);
                _wildMask[i] = Convert.ToByte(Convert.ToString(~_mask[i], 2).Substring(24), 2);
            }
			
            return 1;

        }

        public static string GetIP()
        {
            string res = string.Empty;

            for (int i = 0; i < V; i++)
            {
                res += _ip[i] + ".";
            }

            return res.Substring(0, res.Length - 1);
        }

        public static string GetMask()
        {
            string res = string.Empty;

            for (int i = 0; i < V; i++)
            {
                res += _mask[i] + ".";
            }

            return res.Substring(0, res.Length - 1);
        }

        public static string GetNetworkAddress()
        {
            string res = string.Empty;

            for (int i = 0; i < V; i++)
            {
                res += (_ip[i] & _mask[i]) + ".";
            }

            return res.Substring(0, res.Length - 1);
        }

        public static string GetBroadcast()
        {
            string res = string.Empty;

            for (int i = 0; i < V; i++)
            {
                res += (_ip[i] | _wildMask[i]) + ".";
            }

            return res.Substring(0, res.Length - 1);
        }

        public static uint GetNodesAmount()
        {
            return (uint)Math.Pow(2, 32 - _bitSize) - 2;
        }

        public static string GetNodesAmountString()
        {
            return (Math.Pow(2, 32 - _bitSize) - 2).ToString() + " + 2 (Адрес сети и Broadcast)";
        }

        public static string GetFirstAddress()
        {
            string fa = BinaryWord.IncBinaryWord(BinaryWord.GetBinaryWord(GetNetworkAddress(), '.'));
            string res = string.Empty;

            for (int i = 0; i < V; i++)
            {
                res += Convert.ToByte(fa.Substring(i * 8, 8), 2).ToString() + ".";
            }

            return res.Substring(0, res.Length - 1);
        }

        public static string GetLastAddress()
        {
            string fa = BinaryWord.DecBinaryWord(BinaryWord.GetBinaryWord(GetBroadcast(), '.'));
            string res = string.Empty;

            for (int i = 0; i < V; i++)
            {
                res += Convert.ToByte(fa.Substring(i * 8, 8), 2).ToString() + ".";
            }

            return res.Substring(0, res.Length - 1);
        }


    }
}
