using System;
using System.Windows.Forms;
using System.Linq;

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
            _L_IP.Text = _inputIP.Text.Replace(" ", "");

            int bitSize = Convert.ToInt32(_inputBitSize.Text);

            if (bitSize > 32)
            {
                _L_Mask.Text = "Маска не может быть больше 32!";
                _L_Broadcast.Text = "Маска не может быть больше 32!";
                _L_NetworkAddress.Text = "Маска не может быть больше 32!";
                _L_IP.Text = "Маска не может быть больше 32!";
                _L_NodeAmount.Text = "Маска не может быть больше 32!";
                return;
            }

            _L_NodeAmount.Text = (Math.Pow(2, 32 - bitSize) - 2).ToString() + " + 2 (Адрес сети и Broadcast)";

            string[] masks = { ".0.0.0", ".0.0", ".0" , "" };
            string[] masks2 = { "", "255.", "255.255.", "255.255.255." };
            string[] masks3 = { "0", "128", "192", "224", "240", "248", "252", "254", "255" };

                
            _L_Mask.Text = masks2[bitSize / 8] + masks3[bitSize - ((bitSize / 8) * 8)] + masks[bitSize / 8];


			if (_CB_Div.Checked)
			{
				
				// todo: Network div

				string[] nodes = _TB_Div.Text.Split(' ');
                int[] nodesInt = new int[nodes.Length];

                for (int i = 0; i < nodes.Length; i++)
                {
                    nodesInt[i] = Convert.ToInt32(nodes[i]);
                }

                if (nodesInt.Sum() + 2 * nodes.Length > Convert.ToInt64(_L_NodeAmount.Text))
                {

                }

                Array.Sort(nodes);
                Array.Reverse(nodes);
				string lastIP = _L_IP.Text;
			}



            masks = _L_IP.Text.Split('.');
            masks2 = _L_Mask.Text.Split('.');


            for (int i = 0; i < 3; i++)
            {
                masks3[i] = Convert.ToString(~Convert.ToByte(masks2[i + 1]), 2).Substring(24);
            }


            _L_NetworkAddress.Text = (Convert.ToInt32(masks[0]) & Convert.ToInt32(masks2[0])) + "."
                + (Convert.ToInt32(masks[1]) & Convert.ToInt32(masks2[1])) + "."
                + (Convert.ToInt32(masks[2]) & Convert.ToInt32(masks2[2])) + "."
                + (Convert.ToInt32(masks[3]) & Convert.ToInt32(masks2[3]));

            _L_Broadcast.Text = (Convert.ToInt32(masks[0]) & Convert.ToInt32(masks2[0])) + "."
                + (Convert.ToInt32(masks[1]) | Convert.ToByte(masks3[0], 2)) + "."
                + (Convert.ToInt32(masks[2]) | Convert.ToByte(masks3[1], 2)) + "."
                + (Convert.ToInt32(masks[3]) | Convert.ToByte(masks3[2], 2));


        }

        private void _inputBitSize_TextChanged(object sender, EventArgs e)
        {
            if (_inputBitSize.Text.EndsWith("99"))
            {
                _inputIP.Text = "192.168.11 .8  ";

                _inputBitSize.Text = "24";
            }
        }

        private void _CB_Div_CheckedChanged(object sender, EventArgs e)
        {
            _TB_Div.Enabled = !_TB_Div.Enabled;
            _TB_Div.Visible = !_TB_Div.Visible;
            _DGV_Nodes.Enabled = !_DGV_Nodes.Enabled;
            _DGV_Nodes.Visible = !_DGV_Nodes.Visible;
        }
    }
}
