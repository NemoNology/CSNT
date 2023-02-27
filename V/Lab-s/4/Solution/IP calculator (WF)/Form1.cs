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

        Network network = new Network();

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
                network.IPString = inputIP.Text.Replace(" ", "");
                network.BitMask = (int)inputMask.Value;
            }
            catch (Exception exc)
            {
                status.Text = exc.Message;
                return;
            }

            outputIP.Text = network.IPString;
            outputMask.Text = network.BitMask.ToString();
            outputNetworkClass.Text = network.NetworkClass;
            outputNetworkAddress.Text = network.Address;
            outputBroadcast.Text = network.Broadcast;
            outputNetworkMask.Text = network.NetworkMaskString;
            outputWildMask.Text = network.WildMaskString;
            outputHostsAmount.Text = network.HostsAmount.ToString() + " + 2 (Broadcast and Network Address)";
            outputFirstAddress.Text = network.FirstAddress;
            outputLastAddress.Text = network.LastAddress;


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

                if (sum > network.HostsAmount)
                {
                    status.Text = "Needed hosts amount more that there is in given Network";
                    return;
                }

                Array.Sort(numbers);
                Array.Reverse(numbers);

                outputSubnetworks.Rows.Clear();

                network.IPString = network.Address;

                foreach (var item in numbers)
                {
                    int bitMask = network.BitMask;

                    while (item < (uint)(Math.Pow(2, network.DigitsAmountInIP - bitMask) - 2))
                    {
                        bitMask++;
                    }

                    bitMask--;

                    network.BitMask = bitMask;

                    outputSubnetworks.Rows.Add(
                        network.IPString,
                        network.NetworkMaskString + " / " + bitMask.ToString(),
                        item,
                        network.HostsAmount,
                        network.HostsAmount - item,
                        network.FirstAddress + " - " + network.LastAddress,
                        network.Address,
                        network.Broadcast
                        );

                    network.IPString = network.Broadcast;

                    network.IPInt += 1;
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

            network.IPInt = rnd.Next(int.MinValue, int.MaxValue);
            network.BitMask = rnd.Next(0, 32);

            inputIP.Text = network.IPStringWithSpaces;
            inputMask.Value = network.BitMask;

            ButtonInput_Click(sender, e);
        }
    }
}
