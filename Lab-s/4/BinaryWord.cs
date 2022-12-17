using System;

namespace CSNT_Lab_4
{
    class BinaryWord
    {

        public static string GetBinaryWord(string word, char separator)
        {
            string[] words = word.Split(separator);
            string res = string.Empty;

            foreach (string s in words)
            {
                res += Convert.ToString(Convert.ToByte(s), 2).PadLeft(8, '0');
            }

            return res;
        }

        public static string BinaryWordToByte(string word, char separator)
        {
            string res = string.Empty;

            for (int i = 0; i < word.Length / 8; i++)
            {
                res += Convert.ToByte(word.Substring(i * 8, 8), 2).ToString() + separator;
            }

            return res.Substring(0, res.Length - 1);
        }

        public static string IncBinaryWord(string binaryWord, uint number = 1)
        {
            return Convert.ToString(Convert.ToUInt32(binaryWord, 2) + number, 2);
        }

        public static string DecBinaryWord(string binaryWord, uint number = 1)
        {
            return Convert.ToString(Convert.ToUInt32(binaryWord, 2) - number, 2);
        }
    }
}
