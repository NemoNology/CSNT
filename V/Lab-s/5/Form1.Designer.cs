
namespace CSNT_Lab_5
{
    partial class Form1
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
            this._connectionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._type = new System.Windows.Forms.ComboBox();
            this._L_Type = new System.Windows.Forms.Label();
            this._speed = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._L_Length = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Result = new System.Windows.Forms.Label();
            this._length = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // _connectionType
            // 
            this._connectionType.FormattingEnabled = true;
            this._connectionType.Location = new System.Drawing.Point(126, 43);
            this._connectionType.Name = "_connectionType";
            this._connectionType.Size = new System.Drawing.Size(121, 21);
            this._connectionType.TabIndex = 0;
            this._connectionType.SelectedIndexChanged += new System.EventHandler(this._connectionType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Линия связи:";
            // 
            // _type
            // 
            this._type.FormattingEnabled = true;
            this._type.Location = new System.Drawing.Point(126, 112);
            this._type.Name = "_type";
            this._type.Size = new System.Drawing.Size(121, 21);
            this._type.TabIndex = 0;
            this._type.SelectedIndexChanged += new System.EventHandler(this._type_SelectedIndexChanged);
            // 
            // _L_Type
            // 
            this._L_Type.AutoSize = true;
            this._L_Type.Location = new System.Drawing.Point(19, 115);
            this._L_Type.Name = "_L_Type";
            this._L_Type.Size = new System.Drawing.Size(35, 13);
            this._L_Type.TabIndex = 1;
            this._L_Type.Text = "Вид...";
            // 
            // _speed
            // 
            this._speed.FormattingEnabled = true;
            this._speed.Location = new System.Drawing.Point(126, 181);
            this._speed.Name = "_speed";
            this._speed.Size = new System.Drawing.Size(121, 21);
            this._speed.TabIndex = 0;
            this._speed.SelectedIndexChanged += new System.EventHandler(this._speed_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 34);
            this.label3.TabIndex = 1;
            this.label3.Text = "Скорость передачи данных:";
            // 
            // _L_Length
            // 
            this._L_Length.AutoSize = true;
            this._L_Length.Location = new System.Drawing.Point(19, 254);
            this._L_Length.Name = "_L_Length";
            this._L_Length.Size = new System.Drawing.Size(76, 13);
            this._L_Length.TabIndex = 1;
            this._L_Length.Text = "Расстояние...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Результат:";
            // 
            // Result
            // 
            this.Result.AutoSize = true;
            this.Result.Location = new System.Drawing.Point(123, 315);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(118, 13);
            this.Result.TabIndex = 1;
            this.Result.Text = "Недостаточно данных";
            // 
            // _length
            // 
            this._length.Location = new System.Drawing.Point(126, 251);
            this._length.Mask = "0999";
            this._length.Name = "_length";
            this._length.PromptChar = ' ';
            this._length.Size = new System.Drawing.Size(121, 20);
            this._length.TabIndex = 2;
            this._length.TextChanged += new System.EventHandler(this._length_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this._length);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._L_Length);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._L_Type);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._speed);
            this.Controls.Add(this._type);
            this.Controls.Add(this._connectionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _connectionType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _type;
        private System.Windows.Forms.Label _L_Type;
        private System.Windows.Forms.ComboBox _speed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _L_Length;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.MaskedTextBox _length;
    }
}

