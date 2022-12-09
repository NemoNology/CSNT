using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSNT_Lab_4
{
    public partial class _mainWindow : Form
    {
        public _mainWindow()
        {
            InitializeComponent();
            _inputIP.Mask = "099/099/099/099";
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

            string _mask1 = _L_IP.Text.Substring(0, _inputIP.Text.IndexOf('.', 0));
            string _mask3 = _L_IP.Text.Substring(0, _inputIP.Text.LastIndexOf('.'));
            string _mask2 = _mask3.Substring(0, _mask3.LastIndexOf('.'));

            int temp = Convert.ToInt32(_mask1);

            switch (Convert.ToString(temp, 2).Substring(0, 3))
            {
                // todo: cases: "000 - 111"
                // 1 - 126: case 1
                // 128 - 191: case 2
                // 192 - 223: case 3

                case "011":

                    _L_Mask.Text = "255.0.0.0";
                    _L_Broadcast.Text = _mask1 + ".255.255.255";
                    _L_NetworkAddress.Text = _mask1 + ".0.0.0";
                    break;

                case "010":

                    _L_Mask.Text = "255.255.0.0";
                    _L_Broadcast.Text = _mask2 + ".255.255";
                    _L_NetworkAddress.Text = _mask2 + ".0.0";
                    break;

                case "110":

                    _L_Mask.Text = "255.255.255.0";
                    _L_Broadcast.Text = _mask3 + ".255";
                    _L_NetworkAddress.Text = _mask3 + ".0";
                    break;

                default:

                    _L_Mask.Text = "Маску не вычислен";
                    _L_Broadcast.Text = "Широковещательный домен не вычислен";
                    _L_NetworkAddress.Text = "Адрес сети не вычислен";
                    break;

            }

            _L_NodeAmount.Text = Math.Pow(2, 32 - Convert.ToInt32(_TB_bitSize.Text)).ToString();


        }

        private void _TB_bitSize_TextChanged(object sender, EventArgs e)
        {
            if (_TB_bitSize.Text.EndsWith("*"))
            {
                _inputIP.Text = "192.168.11 .8  ";

                _TB_bitSize.Text = "24";
            }

        }
    }
}
