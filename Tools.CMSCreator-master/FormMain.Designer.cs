namespace Tools.CMSCreator
{
	partial class FormMain
	{
		/// <summary>
		/// Требуется переменная конструктора.
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
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
            this.radioButtonSign = new System.Windows.Forms.RadioButton();
            this.radioButtonCheck = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelTXT = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // radioButtonSign
            // 
            this.radioButtonSign.AutoSize = true;
            this.radioButtonSign.Location = new System.Drawing.Point(12, 12);
            this.radioButtonSign.Name = "radioButtonSign";
            this.radioButtonSign.Size = new System.Drawing.Size(119, 17);
            this.radioButtonSign.TabIndex = 0;
            this.radioButtonSign.TabStop = true;
            this.radioButtonSign.Text = "Создание подписи";
            this.radioButtonSign.UseVisualStyleBackColor = true;
            // 
            // radioButtonCheck
            // 
            this.radioButtonCheck.AutoSize = true;
            this.radioButtonCheck.Location = new System.Drawing.Point(12, 35);
            this.radioButtonCheck.Name = "radioButtonCheck";
            this.radioButtonCheck.Size = new System.Drawing.Size(120, 17);
            this.radioButtonCheck.TabIndex = 1;
            this.radioButtonCheck.TabStop = true;
            this.radioButtonCheck.Text = "Проверка подписи";
            this.radioButtonCheck.UseVisualStyleBackColor = true;
            this.radioButtonCheck.CheckedChanged += new System.EventHandler(this.RadioButtonCheck_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(354, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
           
            // 
            // labelTXT
            // 
            this.labelTXT.AutoSize = true;
            this.labelTXT.Enabled = false;
            this.labelTXT.Location = new System.Drawing.Point(161, 31);
            this.labelTXT.Name = "labelTXT";
            this.labelTXT.Size = new System.Drawing.Size(187, 26);
            this.labelTXT.TabIndex = 3;
            this.labelTXT.Text = "Формат файла \r\nдля которого проверяется подпись\r\n";
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 370);
            this.Controls.Add(this.labelTXT);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radioButtonCheck);
            this.Controls.Add(this.radioButtonSign);
            this.Name = "FormMain";
            this.Text = "CMS/Sign Creator";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
        private int i=0;
		private System.Windows.Forms.RadioButton radioButtonSign;
		private System.Windows.Forms.RadioButton radioButtonCheck;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelTXT;
    }
}

