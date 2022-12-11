namespace CSNT_Lab_4
{
    partial class _mainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this._B_Input = new System.Windows.Forms.Button();
            this._L2 = new System.Windows.Forms.Label();
            this._L1 = new System.Windows.Forms.Label();
            this._L3 = new System.Windows.Forms.Label();
            this._L4 = new System.Windows.Forms.Label();
            this._L5 = new System.Windows.Forms.Label();
            this._L6 = new System.Windows.Forms.Label();
            this._L_IP = new System.Windows.Forms.Label();
            this._L_Mask = new System.Windows.Forms.Label();
            this._L_NodeAmount = new System.Windows.Forms.Label();
            this._L_Broadcast = new System.Windows.Forms.Label();
            this._L_NetworkAddress = new System.Windows.Forms.Label();
            this._inputIP = new System.Windows.Forms.MaskedTextBox();
            this._inputBitSize = new System.Windows.Forms.MaskedTextBox();
            this._L7 = new System.Windows.Forms.Label();
            this._DGV_Nodes = new System.Windows.Forms.DataGridView();
            this._CB_Div = new System.Windows.Forms.CheckBox();
            this._TB_Div = new System.Windows.Forms.TextBox();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostsNeeds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostsAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetworkAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Broadcast = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Range = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._DGV_Nodes)).BeginInit();
            this.SuspendLayout();
            // 
            // _B_Input
            // 
            this._B_Input.Location = new System.Drawing.Point(247, 31);
            this._B_Input.Name = "_B_Input";
            this._B_Input.Size = new System.Drawing.Size(75, 23);
            this._B_Input.TabIndex = 2;
            this._B_Input.Text = "Ввод";
            this._B_Input.UseVisualStyleBackColor = true;
            this._B_Input.Click += new System.EventHandler(this._B_Input_Click);
            // 
            // _L2
            // 
            this._L2.AutoSize = true;
            this._L2.Location = new System.Drawing.Point(16, 104);
            this._L2.Name = "_L2";
            this._L2.Size = new System.Drawing.Size(69, 13);
            this._L2.TabIndex = 3;
            this._L2.Text = "Маска сети:";
            // 
            // _L1
            // 
            this._L1.AutoSize = true;
            this._L1.Location = new System.Drawing.Point(16, 76);
            this._L1.Name = "_L1";
            this._L1.Size = new System.Drawing.Size(20, 13);
            this._L1.TabIndex = 4;
            this._L1.Text = "IP:";
            // 
            // _L3
            // 
            this._L3.AutoSize = true;
            this._L3.Location = new System.Drawing.Point(16, 134);
            this._L3.Name = "_L3";
            this._L3.Size = new System.Drawing.Size(107, 13);
            this._L3.TabIndex = 5;
            this._L3.Text = "Колличество узлов:";
            // 
            // _L4
            // 
            this._L4.AutoSize = true;
            this._L4.Location = new System.Drawing.Point(16, 196);
            this._L4.Name = "_L4";
            this._L4.Size = new System.Drawing.Size(94, 13);
            this._L4.TabIndex = 6;
            this._L4.Text = "Широковещание:";
            // 
            // _L5
            // 
            this._L5.AutoSize = true;
            this._L5.Location = new System.Drawing.Point(16, 172);
            this._L5.Name = "_L5";
            this._L5.Size = new System.Drawing.Size(67, 13);
            this._L5.TabIndex = 7;
            this._L5.Text = "Адрес сети:";
            // 
            // _L6
            // 
            this._L6.AutoSize = true;
            this._L6.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._L6.Location = new System.Drawing.Point(167, 32);
            this._L6.Name = "_L6";
            this._L6.Size = new System.Drawing.Size(17, 23);
            this._L6.TabIndex = 8;
            this._L6.Text = "/";
            // 
            // _L_IP
            // 
            this._L_IP.AutoSize = true;
            this._L_IP.Location = new System.Drawing.Point(151, 76);
            this._L_IP.Name = "_L_IP";
            this._L_IP.Size = new System.Drawing.Size(16, 13);
            this._L_IP.TabIndex = 9;
            this._L_IP.Text = "...";
            // 
            // _L_Mask
            // 
            this._L_Mask.AutoSize = true;
            this._L_Mask.Location = new System.Drawing.Point(151, 104);
            this._L_Mask.Name = "_L_Mask";
            this._L_Mask.Size = new System.Drawing.Size(16, 13);
            this._L_Mask.TabIndex = 10;
            this._L_Mask.Text = "...";
            // 
            // _L_NodeAmount
            // 
            this._L_NodeAmount.AutoSize = true;
            this._L_NodeAmount.Location = new System.Drawing.Point(151, 134);
            this._L_NodeAmount.Name = "_L_NodeAmount";
            this._L_NodeAmount.Size = new System.Drawing.Size(16, 13);
            this._L_NodeAmount.TabIndex = 11;
            this._L_NodeAmount.Text = "...";
            // 
            // _L_Broadcast
            // 
            this._L_Broadcast.AutoSize = true;
            this._L_Broadcast.Location = new System.Drawing.Point(151, 196);
            this._L_Broadcast.Name = "_L_Broadcast";
            this._L_Broadcast.Size = new System.Drawing.Size(16, 13);
            this._L_Broadcast.TabIndex = 12;
            this._L_Broadcast.Text = "...";
            // 
            // _L_NetworkAddress
            // 
            this._L_NetworkAddress.AutoSize = true;
            this._L_NetworkAddress.Location = new System.Drawing.Point(151, 172);
            this._L_NetworkAddress.Name = "_L_NetworkAddress";
            this._L_NetworkAddress.Size = new System.Drawing.Size(16, 13);
            this._L_NetworkAddress.TabIndex = 13;
            this._L_NetworkAddress.Text = "...";
            // 
            // _inputIP
            // 
            this._inputIP.Location = new System.Drawing.Point(15, 33);
            this._inputIP.Mask = "099/099/099/099";
            this._inputIP.Name = "_inputIP";
            this._inputIP.Size = new System.Drawing.Size(146, 20);
            this._inputIP.TabIndex = 14;
            this._inputIP.Text = "19216811 8";
            this._inputIP.TextChanged += new System.EventHandler(this.maskedTextBox1_TextChanged);
            // 
            // _inputBitSize
            // 
            this._inputBitSize.Location = new System.Drawing.Point(184, 33);
            this._inputBitSize.Mask = "09";
            this._inputBitSize.Name = "_inputBitSize";
            this._inputBitSize.Size = new System.Drawing.Size(31, 20);
            this._inputBitSize.TabIndex = 16;
            this._inputBitSize.Text = "24";
            this._inputBitSize.TextChanged += new System.EventHandler(this._inputBitSize_TextChanged);
            // 
            // _L7
            // 
            this._L7.AutoSize = true;
            this._L7.Location = new System.Drawing.Point(16, 238);
            this._L7.Name = "_L7";
            this._L7.Size = new System.Drawing.Size(124, 13);
            this._L7.TabIndex = 18;
            this._L7.Text = "Разбиение на подсети:";
            // 
            // _DGV_Nodes
            // 
            this._DGV_Nodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._DGV_Nodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._DGV_Nodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.HostsNeeds,
            this.HostsAll,
            this.Mask,
            this.NetworkAddress,
            this.Broadcast,
            this.Range});
            this._DGV_Nodes.Enabled = false;
            this._DGV_Nodes.Location = new System.Drawing.Point(12, 293);
            this._DGV_Nodes.Name = "_DGV_Nodes";
            this._DGV_Nodes.ReadOnly = true;
            this._DGV_Nodes.Size = new System.Drawing.Size(694, 256);
            this._DGV_Nodes.TabIndex = 19;
            this._DGV_Nodes.Visible = false;
            // 
            // _CB_Div
            // 
            this._CB_Div.AutoSize = true;
            this._CB_Div.Location = new System.Drawing.Point(154, 237);
            this._CB_Div.Name = "_CB_Div";
            this._CB_Div.Size = new System.Drawing.Size(76, 17);
            this._CB_Div.TabIndex = 20;
            this._CB_Div.Text = "Включено";
            this._CB_Div.UseVisualStyleBackColor = true;
            this._CB_Div.CheckedChanged += new System.EventHandler(this._CB_Div_CheckedChanged);
            // 
            // _TB_Div
            // 
            this._TB_Div.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._TB_Div.Enabled = false;
            this._TB_Div.Location = new System.Drawing.Point(12, 261);
            this._TB_Div.Name = "_TB_Div";
            this._TB_Div.Size = new System.Drawing.Size(694, 20);
            this._TB_Div.TabIndex = 21;
            this._TB_Div.Visible = false;
            // 
            // Number
            // 
            this.Number.FillWeight = 60F;
            this.Number.HeaderText = "Номер сети";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 60;
            // 
            // HostsNeeds
            // 
            this.HostsNeeds.FillWeight = 60F;
            this.HostsNeeds.HeaderText = "Нужно хостов";
            this.HostsNeeds.Name = "HostsNeeds";
            this.HostsNeeds.ReadOnly = true;
            this.HostsNeeds.Width = 60;
            // 
            // HostsAll
            // 
            this.HostsAll.FillWeight = 60F;
            this.HostsAll.HeaderText = "Всего хостов";
            this.HostsAll.Name = "HostsAll";
            this.HostsAll.ReadOnly = true;
            this.HostsAll.Width = 60;
            // 
            // Mask
            // 
            this.Mask.HeaderText = "Маска";
            this.Mask.Name = "Mask";
            this.Mask.ReadOnly = true;
            // 
            // NetworkAddress
            // 
            this.NetworkAddress.HeaderText = "Адрес сети";
            this.NetworkAddress.Name = "NetworkAddress";
            this.NetworkAddress.ReadOnly = true;
            // 
            // Broadcast
            // 
            this.Broadcast.HeaderText = "Широковещание";
            this.Broadcast.Name = "Broadcast";
            this.Broadcast.ReadOnly = true;
            // 
            // Range
            // 
            this.Range.FillWeight = 300F;
            this.Range.HeaderText = "Пласт адресов";
            this.Range.MinimumWidth = 200;
            this.Range.Name = "Range";
            this.Range.ReadOnly = true;
            this.Range.Width = 300;
            // 
            // _mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 561);
            this.Controls.Add(this._TB_Div);
            this.Controls.Add(this._CB_Div);
            this.Controls.Add(this._DGV_Nodes);
            this.Controls.Add(this._L7);
            this.Controls.Add(this._inputBitSize);
            this.Controls.Add(this._inputIP);
            this.Controls.Add(this._L_NetworkAddress);
            this.Controls.Add(this._L_Broadcast);
            this.Controls.Add(this._L_NodeAmount);
            this.Controls.Add(this._L_Mask);
            this.Controls.Add(this._L_IP);
            this.Controls.Add(this._L6);
            this.Controls.Add(this._L5);
            this.Controls.Add(this._L4);
            this.Controls.Add(this._L3);
            this.Controls.Add(this._L1);
            this.Controls.Add(this._L2);
            this.Controls.Add(this._B_Input);
            this.Name = "_mainWindow";
            this.ShowIcon = false;
            this.Text = "CSNT Lab 5";
            ((System.ComponentModel.ISupportInitialize)(this._DGV_Nodes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button _B_Input;
        private System.Windows.Forms.Label _L2;
        private System.Windows.Forms.Label _L1;
        private System.Windows.Forms.Label _L3;
        private System.Windows.Forms.Label _L4;
        private System.Windows.Forms.Label _L5;
        private System.Windows.Forms.Label _L6;
        private System.Windows.Forms.Label _L_IP;
        private System.Windows.Forms.Label _L_Mask;
        private System.Windows.Forms.Label _L_NodeAmount;
        private System.Windows.Forms.Label _L_Broadcast;
        private System.Windows.Forms.Label _L_NetworkAddress;
        private System.Windows.Forms.MaskedTextBox _inputIP;
        private System.Windows.Forms.MaskedTextBox _inputBitSize;
        private System.Windows.Forms.Label _L7;
        private System.Windows.Forms.DataGridView _DGV_Nodes;
        private System.Windows.Forms.CheckBox _CB_Div;
        private System.Windows.Forms.TextBox _TB_Div;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostsNeeds;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostsAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mask;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetworkAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Broadcast;
        private System.Windows.Forms.DataGridViewTextBoxColumn Range;
    }
}

