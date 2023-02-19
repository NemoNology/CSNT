using System;
using System.Linq;
using System.Windows.Forms;

namespace IP_calculator__WF_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Calculate available information by IP and Mask 
        /// <br/> And Cut Network to SubNetworks, if possible
        /// </summary>
        private void ButtonInput_Click(object sender, EventArgs e)
        {
            status.Text = "...";

            if (inputMask.Value > 32 || inputMask.Value < 0)
            {
                status.Text = "Incorrect bitMask";
                return;
            }

            try
            {
                Network.IPString = inputIP.Text.Replace(" ", "");
                Network.BitMask = (int)inputMask.Value;
            }
            catch (Exception exc)
            {
                status.Text = exc.Message;
                return;
            }

            outputIP.Text = Network.IPString;
            outputMask.Text = Network.BitMask.ToString();
            outputNetworkClass.Text = Network.NetworkClass;
            outputNetworkAddress.Text = Network.Address;
            outputBroadcast.Text = Network.Broadcast;
            outputNetworkMask.Text = Network.NetworkMaskString;
            outputWildMask.Text = Network.WildMaskString;
            outputHostsAmount.Text = Network.HostsAmount.ToString() + " + 2 (Broadcast and Network Address)";
            outputFirstAddress.Text = Network.FirstAddress;
            outputLastAddress.Text = Network.LastAddress;


            if (inputSubnetworks.Text.Length != 0)
            {
                string[] buffer = inputSubnetworks.Text.Split(' ');
                uint[] numbers = new uint[buffer.Length]; 

                try
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        numbers[i] = Convert.ToUInt32(buffer[i]);
                    }
                }
                catch
                {
                    status.Text = "Incorrect value in SubNetworks field";
                    return;
                }

                numbers = Array.FindAll<uint>(numbers, x => x != 0);

                uint sum = 0;

                foreach (var item in numbers)
                {
                    sum += (uint)Math.Pow(2, Math.Ceiling(Math.Log(item, 2)));
                }

                if (sum > Network.HostsAmount)
                {
                    status.Text = "Needed hosts amount more that there is in given Network";
                    return;
                }

                Array.Sort(numbers);
                Array.Reverse(numbers);

                outputSubnetworks.Rows.Clear();

                Network.IPString = Network.Address;

                foreach (var item in numbers)
                {
                    int bitMask = Network.BitMask;

                    while (item < (uint)(Math.Pow(2, Network.DigitsAmountInIP - bitMask) - 2))
                    {
                        bitMask++;
                    }

                    bitMask--;

                    Network.BitMask = bitMask;

                    outputSubnetworks.Rows.Add(
                        Network.IPString,
                        Network.NetworkMaskString + " / " + bitMask.ToString(),
                        item,
                        Network.HostsAmount,
                        Network.HostsAmount - item,
                        Network.FirstAddress + " - " + Network.LastAddress,
                        Network.Address,
                        Network.Broadcast
                        );

                    Network.IPString = Network.Broadcast;

                    Network.IPInt += 1;
                }
            }
        }

        private void HotKeys(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return ||
                e.KeyCode == Keys.Enter)
            {
                ButtonInput_Click(sender, e);
            }
        }

        /// <summary>
        /// Calculating when inputMask.Value Changed
        /// </summary>
        private void FastTravel(object sender, EventArgs e)
        {
            if (inputMask.Value == 99)
            {
                inputIP.Text = "19216811 2";
                inputMask.Value = 24;
                inputSubnetworks.Text = "100 60 60 40 20";
            }
        }

        /// <summary>
        /// Generate random values for IP and Mask
        /// </summary>
        private void ButtonRandom_Click(object sender, EventArgs e)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            Network.IPInt = rnd.Next(int.MinValue, int.MaxValue);
            Network.BitMask = rnd.Next(0, 32);

            inputIP.Text = Network.IPStringWithSpaces;
            inputMask.Value = Network.BitMask;

            ButtonInput_Click(sender, e);
        }
    }
}
