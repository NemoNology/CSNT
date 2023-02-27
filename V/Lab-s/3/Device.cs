using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CSNT_Lab_3
{
    internal class Device
    {
        private List<byte> _MAC = new List<byte>(6);
        private List<byte> _IPv4 = new List<byte>(4);

        public void SetMAC(List<byte> MAC)
        {
            try
            {
                if (MAC.Count == 6)
                {
                    _MAC = MAC;
                }
                else
                {
                    throw new Exception("Подан неверный MAC-адрес!");
                }
            }

            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<byte> GetMac()
        { 
            return _MAC; 
        }

        public void SetIPv4(List<byte> ipv4)
        {
            try
            {
                if (ipv4.Count == 6)
                {
                    _IPv4 = ipv4;
                }
                else
                {
                    throw new Exception("Подан неверный IPv4-адрес!");
                }
            }

            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<byte> GetIPv4()
        {
            return _IPv4;
        }

        public void SendPackage()
        {
            
        }

    }
}
