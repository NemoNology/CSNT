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
            this._TB_bitSize = new System.Windows.Forms.TextBox();
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
            this.SuspendLayout();
            // 
            // _TB_bitSize
            // 
            this._TB_bitSize.Location = new System.Drawing.Point(184, 34);
            this._TB_bitSize.Name = "_TB_bitSize";
            this._TB_bitSize.Size = new System.Drawing.Size(46, 20);
            this._TB_bitSize.TabIndex = 1;
            this._TB_bitSize.Text = "24";
            this._TB_bitSize.TextChanged += new System.EventHandler(this._TB_bitSize_TextChanged);
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
            this._L2.Location = new System.Drawing.Point(12, 105);
            this._L2.Name = "_L2";
            this._L2.Size = new System.Drawing.Size(69, 13);
            this._L2.TabIndex = 3;
            this._L2.Text = "Маска сети:";
            // 
            // _L1
            // 
            this._L1.AutoSize = true;
            this._L1.Location = new System.Drawing.Point(12, 77);
            this._L1.Name = "_L1";
            this._L1.Size = new System.Drawing.Size(20, 13);
            this._L1.TabIndex = 4;
            this._L1.Text = "IP:";
            // 
            // _L3
            // 
            this._L3.AutoSize = true;
            this._L3.Location = new System.Drawing.Point(12, 135);
            this._L3.Name = "_L3";
            this._L3.Size = new System.Drawing.Size(107, 13);
            this._L3.TabIndex = 5;
            this._L3.Text = "Колличество узлов:";
            // 
            // _L4
            // 
            this._L4.AutoSize = true;
            this._L4.Location = new System.Drawing.Point(12, 175);
            this._L4.Name = "_L4";
            this._L4.Size = new System.Drawing.Size(94, 13);
            this._L4.TabIndex = 6;
            this._L4.Text = "Широковещание:";
            // 
            // _L5
            // 
            this._L5.AutoSize = true;
            this._L5.Location = new System.Drawing.Point(12, 200);
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
            this._L_IP.Location = new System.Drawing.Point(147, 77);
            this._L_IP.Name = "_L_IP";
            this._L_IP.Size = new System.Drawing.Size(16, 13);
            this._L_IP.TabIndex = 9;
            this._L_IP.Text = "...";
            // 
            // _L_Mask
            // 
            this._L_Mask.AutoSize = true;
            this._L_Mask.Location = new System.Drawing.Point(147, 105);
            this._L_Mask.Name = "_L_Mask";
            this._L_Mask.Size = new System.Drawing.Size(16, 13);
            this._L_Mask.TabIndex = 10;
            this._L_Mask.Text = "...";
            // 
            // _L_NodeAmount
            // 
            this._L_NodeAmount.AutoSize = true;
            this._L_NodeAmount.Location = new System.Drawing.Point(147, 135);
            this._L_NodeAmount.Name = "_L_NodeAmount";
            this._L_NodeAmount.Size = new System.Drawing.Size(16, 13);
            this._L_NodeAmount.TabIndex = 11;
            this._L_NodeAmount.Text = "...";
            // 
            // _L_Broadcast
            // 
            this._L_Broadcast.AutoSize = true;
            this._L_Broadcast.Location = new System.Drawing.Point(147, 175);
            this._L_Broadcast.Name = "_L_Broadcast";
            this._L_Broadcast.Size = new System.Drawing.Size(16, 13);
            this._L_Broadcast.TabIndex = 12;
            this._L_Broadcast.Text = "...";
            // 
            // _L_NetworkAddress
            // 
            this._L_NetworkAddress.AutoSize = true;
            this._L_NetworkAddress.Location = new System.Drawing.Point(147, 200);
            this._L_NetworkAddress.Name = "_L_NetworkAddress";
            this._L_NetworkAddress.Size = new System.Drawing.Size(16, 13);
            this._L_NetworkAddress.TabIndex = 13;
            this._L_NetworkAddress.Text = "...";
            // 
            // _inputIP
            // 
            this._inputIP.Location = new System.Drawing.Point(15, 33);
            this._inputIP.Name = "_inputIP";
            this._inputIP.Size = new System.Drawing.Size(146, 20);
            this._inputIP.TabIndex = 14;
            this._inputIP.TextChanged += new System.EventHandler(this.maskedTextBox1_TextChanged);
            // 
            // _mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 561);
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
            this.Controls.Add(this._TB_bitSize);
            this.Name = "_mainWindow";
            this.ShowIcon = false;
            this.Text = "CSNT Lab 5";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox _TB_bitSize;
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
    }
}

