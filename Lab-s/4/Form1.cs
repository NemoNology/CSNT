using System;
using System.Windows.Forms;
using System.Linq;

using BW = CSNT_Lab_4.BinaryWord;

namespace CSNT_Lab_4
{
    public partial class _mainWindow : Form
    {
        public _mainWindow()
        {
            InitializeComponent();
            _CB_Div.Checked = false;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (_inputIP.Text.EndsWith(" ") && _inputIP.Text.IndexOf('.', _inputIP.SelectionStart) == 2)
            {
                _inputIP.Text += "  ";
            }
        }

        private void _B_Input_Click(object sender, EventArgs e)
        {
            _L_Info.Text = _L_Mask.Text = _L_Broadcast.Text = _L_NetworkAddress.Text = _L_IP.Text = _L_NodeAmount.Text = "...";

            if (IP.SetIP(_inputIP.Text) == -1)
            { 
                _L_Info.Text = "Был введён некорекктный IP адрес!";

                return;
            }

            int bitSize = Convert.ToInt32(_inputBitSize.Text);

            if (bitSize > 30 || bitSize == 0)
            {
                _L_Info.Text = "Не делайте маску больше 30 или нулевой!";

                return;
            }

            IP.SetMask(bitSize);

            _L_Mask.Text = IP.GetMask();
            _L_Broadcast.Text = IP.GetBroadcast();
            _L_NetworkAddress.Text = IP.GetNetworkAddress();
            _L_IP.Text = IP.GetIP();
            _L_NodeAmount.Text = IP.GetNodesAmountString();



			if (_TB_Div.Text != string.Empty)
			{
				
				string[] nodes = _TB_Div.Text.Split(' ');
                int[] nodesInt = new int[nodes.Length];
                _DGV_Nodes.Rows.Clear();

                for (int i = 0; i < nodes.Length; i++)
                {
                    try
                    {
                        nodesInt[i] = Convert.ToInt32(nodes[i]);
                    }
                    catch
                    {
                        _L_Info.Text = "Неверно указаны размеры подсетей!";
                        return;
                    }
                }

                uint sum = uint.MinValue;

                foreach (int node in nodesInt)
                {
                    sum += (uint)Math.Pow(2, Math.Ceiling(Math.Log(node + 2, 2))) - 2;
                }

                if (IP.GetNodesAmount() < nodesInt.Sum() || IP.GetNodesAmount() < sum)
                {
                    _L_Info.Text = "Количество нужных подсетей превышает возможное количество подсетей в сети" +
                        "\nили сеть невозможно разделить на требуемое количество подсетей!";
                    return;
                }

                Array.Sort(nodesInt);
                Array.Reverse(nodesInt);
                bitSize = 1;

                foreach (int ns in nodesInt)
                {
                    IP.SetIP(IP.GetNetworkAddress());
                    IP.SetMask(32 - (int)Math.Ceiling(Math.Log(ns + 2, 2)));
                    _DGV_Nodes.Rows.Add(bitSize, ns, IP.GetNodesAmount(), IP.GetMask() + " \\ " + IP._bitSize, IP.GetNetworkAddress(),
                        IP.GetBroadcast(), IP.GetFirstAddress() + " - " + IP.GetLastAddress());
                    IP.SetIP(BW.BinaryWordToByte(BW.IncBinaryWord(BW.GetBinaryWord(IP.GetNetworkAddress(), '.'), (uint)Math.Pow(2, 32 - IP._bitSize)), '.'));
                    bitSize++;

                }

            }

        }

        private void _inputBitSize_TextChanged(object sender, EventArgs e)
        {
            if (_inputBitSize.Text.EndsWith("99"))
            {
                _inputIP.Text = "192.168.11 .8  ";

                _inputBitSize.Text = "24";

                _TB_Div.Text = "60 10 10 10";
            }
        }

        private void _TB_Div_Enter(object sender, EventArgs e)
        {
            _L_Info.Text = "Вводите размеры подсетей через пробел \nПример:\t128 30 29 11";
        }

        private void _TB_Div_Leave(object sender, EventArgs e)
        {
            _L_Info.Text = "...";
        }

        private void _inputIP_Enter(object sender, EventArgs e)
        {
            _L_Info.Text = "Чтобы получить страртовый набор \nвведите значение 99 в маску";
        }

        private void _inputIP_Leave(object sender, EventArgs e)
        {
            _L_Info.Text = "...";
        }
        
    }
}
