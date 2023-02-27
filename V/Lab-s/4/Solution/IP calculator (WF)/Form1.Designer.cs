
namespace IP_calculator__WF_
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputIP = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.inputMask = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonInput = new System.Windows.Forms.Button();
            this.outputIP = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.outputMask = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.outputNetworkClass = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.outputHostsAmount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.outputFirstAddress = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.outputLastAddress = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.outputNetworkAddress = new System.Windows.Forms.Label();
            this.outputBroadcast = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.outputNetworkMask = new System.Windows.Forms.Label();
            this.outputWildMask = new System.Windows.Forms.Label();
            this.inputSubnetworks = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.outputSubnetworks = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonRandom = new System.Windows.Forms.Button();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostsAmountNeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostsAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostsAmountSpare = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetworkRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetworkAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Broadcast = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.inputMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputSubnetworks)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputIP
            // 
            this.inputIP.Location = new System.Drawing.Point(38, 28);
            this.inputIP.Mask = "099.099.099.099";
            this.inputIP.Name = "inputIP";
            this.inputIP.Size = new System.Drawing.Size(100, 20);
            this.inputIP.TabIndex = 0;
            this.inputIP.Text = "19216811 2";
            this.inputIP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotKeys);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP:";
            // 
            // inputMask
            // 
            this.inputMask.Location = new System.Drawing.Point(162, 29);
            this.inputMask.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.inputMask.Name = "inputMask";
            this.inputMask.Size = new System.Drawing.Size(47, 20);
            this.inputMask.TabIndex = 2;
            this.inputMask.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.inputMask.ValueChanged += new System.EventHandler(this.FastTravel);
            this.inputMask.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotKeys);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "IP:";
            // 
            // buttonInput
            // 
            this.buttonInput.Location = new System.Drawing.Point(250, 28);
            this.buttonInput.Name = "buttonInput";
            this.buttonInput.Size = new System.Drawing.Size(75, 23);
            this.buttonInput.TabIndex = 3;
            this.buttonInput.Text = "Ввод";
            this.buttonInput.UseVisualStyleBackColor = true;
            this.buttonInput.Click += new System.EventHandler(this.ButtonInput_Click);
            // 
            // outputIP
            // 
            this.outputIP.AutoSize = true;
            this.outputIP.Location = new System.Drawing.Point(98, 79);
            this.outputIP.Name = "outputIP";
            this.outputIP.Size = new System.Drawing.Size(16, 13);
            this.outputIP.TabIndex = 1;
            this.outputIP.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Mask:";
            // 
            // outputMask
            // 
            this.outputMask.AutoSize = true;
            this.outputMask.Location = new System.Drawing.Point(98, 104);
            this.outputMask.Name = "outputMask";
            this.outputMask.Size = new System.Drawing.Size(16, 13);
            this.outputMask.TabIndex = 1;
            this.outputMask.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Network Class:";
            // 
            // outputNetworkClass
            // 
            this.outputNetworkClass.AutoSize = true;
            this.outputNetworkClass.Location = new System.Drawing.Point(98, 131);
            this.outputNetworkClass.Name = "outputNetworkClass";
            this.outputNetworkClass.Size = new System.Drawing.Size(16, 13);
            this.outputNetworkClass.TabIndex = 1;
            this.outputNetworkClass.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(249, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Hosts Amount:";
            // 
            // outputHostsAmount
            // 
            this.outputHostsAmount.AutoSize = true;
            this.outputHostsAmount.Location = new System.Drawing.Point(360, 155);
            this.outputHostsAmount.Name = "outputHostsAmount";
            this.outputHostsAmount.Size = new System.Drawing.Size(16, 13);
            this.outputHostsAmount.TabIndex = 1;
            this.outputHostsAmount.Text = "...";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(249, 180);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "First Host Address:";
            // 
            // outputFirstAddress
            // 
            this.outputFirstAddress.AutoSize = true;
            this.outputFirstAddress.Location = new System.Drawing.Point(360, 180);
            this.outputFirstAddress.Name = "outputFirstAddress";
            this.outputFirstAddress.Size = new System.Drawing.Size(16, 13);
            this.outputFirstAddress.TabIndex = 1;
            this.outputFirstAddress.Text = "...";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(249, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Last Host Address:";
            // 
            // outputLastAddress
            // 
            this.outputLastAddress.AutoSize = true;
            this.outputLastAddress.Location = new System.Drawing.Point(360, 205);
            this.outputLastAddress.Name = "outputLastAddress";
            this.outputLastAddress.Size = new System.Drawing.Size(16, 13);
            this.outputLastAddress.TabIndex = 1;
            this.outputLastAddress.Text = "...";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(249, 79);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Network Address:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(249, 104);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "BroadCast:";
            // 
            // outputNetworkAddress
            // 
            this.outputNetworkAddress.AutoSize = true;
            this.outputNetworkAddress.Location = new System.Drawing.Point(360, 79);
            this.outputNetworkAddress.Name = "outputNetworkAddress";
            this.outputNetworkAddress.Size = new System.Drawing.Size(16, 13);
            this.outputNetworkAddress.TabIndex = 1;
            this.outputNetworkAddress.Text = "...";
            // 
            // outputBroadcast
            // 
            this.outputBroadcast.AutoSize = true;
            this.outputBroadcast.Location = new System.Drawing.Point(360, 104);
            this.outputBroadcast.Name = "outputBroadcast";
            this.outputBroadcast.Size = new System.Drawing.Size(16, 13);
            this.outputBroadcast.TabIndex = 1;
            this.outputBroadcast.Text = "...";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(494, 79);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Network Mask:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(494, 104);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(60, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "Wild Mask:";
            // 
            // outputNetworkMask
            // 
            this.outputNetworkMask.AutoSize = true;
            this.outputNetworkMask.Location = new System.Drawing.Point(594, 79);
            this.outputNetworkMask.Name = "outputNetworkMask";
            this.outputNetworkMask.Size = new System.Drawing.Size(16, 13);
            this.outputNetworkMask.TabIndex = 1;
            this.outputNetworkMask.Text = "...";
            // 
            // outputWildMask
            // 
            this.outputWildMask.AutoSize = true;
            this.outputWildMask.Location = new System.Drawing.Point(594, 104);
            this.outputWildMask.Name = "outputWildMask";
            this.outputWildMask.Size = new System.Drawing.Size(16, 13);
            this.outputWildMask.TabIndex = 1;
            this.outputWildMask.Text = "...";
            // 
            // inputSubnetworks
            // 
            this.inputSubnetworks.Location = new System.Drawing.Point(92, 267);
            this.inputSubnetworks.Name = "inputSubnetworks";
            this.inputSubnetworks.Size = new System.Drawing.Size(740, 20);
            this.inputSubnetworks.TabIndex = 4;
            this.inputSubnetworks.Text = "100 60 20 60";
            this.inputSubnetworks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotKeys);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "SubNetworks:";
            // 
            // outputSubnetworks
            // 
            this.outputSubnetworks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputSubnetworks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.outputSubnetworks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IP,
            this.Mask,
            this.HostsAmountNeed,
            this.HostsAmount,
            this.HostsAmountSpare,
            this.NetworkRange,
            this.NetworkAddress,
            this.Broadcast});
            this.outputSubnetworks.Location = new System.Drawing.Point(15, 293);
            this.outputSubnetworks.Name = "outputSubnetworks";
            this.outputSubnetworks.Size = new System.Drawing.Size(817, 285);
            this.outputSubnetworks.TabIndex = 5;
            this.outputSubnetworks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotKeys);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(844, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(16, 17);
            this.status.Text = "...";
            // 
            // buttonRandom
            // 
            this.buttonRandom.Location = new System.Drawing.Point(497, 29);
            this.buttonRandom.Name = "buttonRandom";
            this.buttonRandom.Size = new System.Drawing.Size(158, 23);
            this.buttonRandom.TabIndex = 3;
            this.buttonRandom.Text = "Случайные значения";
            this.buttonRandom.UseVisualStyleBackColor = true;
            this.buttonRandom.Click += new System.EventHandler(this.ButtonRandom_Click);
            // 
            // IP
            // 
            this.IP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            this.IP.Width = 42;
            // 
            // Mask
            // 
            this.Mask.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Mask.HeaderText = "Mask";
            this.Mask.Name = "Mask";
            this.Mask.ReadOnly = true;
            this.Mask.Width = 58;
            // 
            // HostsAmountNeed
            // 
            this.HostsAmountNeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.HostsAmountNeed.HeaderText = "Host Amount Need";
            this.HostsAmountNeed.MinimumWidth = 50;
            this.HostsAmountNeed.Name = "HostsAmountNeed";
            this.HostsAmountNeed.ReadOnly = true;
            this.HostsAmountNeed.Width = 50;
            // 
            // HostsAmount
            // 
            this.HostsAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.HostsAmount.HeaderText = "Hosts Amount";
            this.HostsAmount.MinimumWidth = 50;
            this.HostsAmount.Name = "HostsAmount";
            this.HostsAmount.ReadOnly = true;
            this.HostsAmount.Width = 50;
            // 
            // HostsAmountSpare
            // 
            this.HostsAmountSpare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.HostsAmountSpare.HeaderText = "Spare Hosts Amount";
            this.HostsAmountSpare.MinimumWidth = 50;
            this.HostsAmountSpare.Name = "HostsAmountSpare";
            this.HostsAmountSpare.ReadOnly = true;
            this.HostsAmountSpare.Width = 50;
            // 
            // NetworkRange
            // 
            this.NetworkRange.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NetworkRange.HeaderText = "Network Range";
            this.NetworkRange.Name = "NetworkRange";
            this.NetworkRange.ReadOnly = true;
            // 
            // NetworkAddress
            // 
            this.NetworkAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.NetworkAddress.HeaderText = "Network Address";
            this.NetworkAddress.MinimumWidth = 100;
            this.NetworkAddress.Name = "NetworkAddress";
            this.NetworkAddress.ReadOnly = true;
            // 
            // Broadcast
            // 
            this.Broadcast.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.Broadcast.HeaderText = "BroadCast";
            this.Broadcast.MinimumWidth = 100;
            this.Broadcast.Name = "Broadcast";
            this.Broadcast.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 603);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.outputSubnetworks);
            this.Controls.Add(this.inputSubnetworks);
            this.Controls.Add(this.buttonRandom);
            this.Controls.Add(this.buttonInput);
            this.Controls.Add(this.inputMask);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outputIP);
            this.Controls.Add(this.outputLastAddress);
            this.Controls.Add(this.outputWildMask);
            this.Controls.Add(this.outputBroadcast);
            this.Controls.Add(this.outputFirstAddress);
            this.Controls.Add(this.outputNetworkMask);
            this.Controls.Add(this.outputNetworkAddress);
            this.Controls.Add(this.outputHostsAmount);
            this.Controls.Add(this.outputNetworkClass);
            this.Controls.Add(this.outputMask);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputIP);
            this.Name = "Form1";
            this.Text = "IP Calculator";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotKeys);
            ((System.ComponentModel.ISupportInitialize)(this.inputMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputSubnetworks)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox inputIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown inputMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonInput;
        private System.Windows.Forms.Label outputIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label outputMask;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label outputNetworkClass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label outputHostsAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label outputFirstAddress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label outputLastAddress;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label outputNetworkAddress;
        private System.Windows.Forms.Label outputBroadcast;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label outputNetworkMask;
        private System.Windows.Forms.Label outputWildMask;
        private System.Windows.Forms.TextBox inputSubnetworks;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView outputSubnetworks;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.Button buttonRandom;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mask;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostsAmountNeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostsAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostsAmountSpare;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetworkRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetworkAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Broadcast;
    }
}

