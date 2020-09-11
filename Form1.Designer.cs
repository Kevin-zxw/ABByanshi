namespace ABByanshi
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.ConnectABB = new System.Windows.Forms.CheckBox();
            this.CommunicationMsg = new System.Windows.Forms.TextBox();
            this.TextBoxInfo = new System.Windows.Forms.TextBox();
            this.Test = new System.Windows.Forms.Button();
            this.TimerNetMonitor = new System.Windows.Forms.Timer(this.components);
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 633);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "ABB连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ConnectABB
            // 
            this.ConnectABB.AutoSize = true;
            this.ConnectABB.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ConnectABB.Location = new System.Drawing.Point(195, 633);
            this.ConnectABB.Name = "ConnectABB";
            this.ConnectABB.Size = new System.Drawing.Size(58, 23);
            this.ConnectABB.TabIndex = 1;
            this.ConnectABB.Text = "ABB";
            this.ConnectABB.UseVisualStyleBackColor = true;
            this.ConnectABB.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // CommunicationMsg
            // 
            this.CommunicationMsg.Location = new System.Drawing.Point(30, 381);
            this.CommunicationMsg.Multiline = true;
            this.CommunicationMsg.Name = "CommunicationMsg";
            this.CommunicationMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CommunicationMsg.Size = new System.Drawing.Size(181, 246);
            this.CommunicationMsg.TabIndex = 2;
            // 
            // TextBoxInfo
            // 
            this.TextBoxInfo.Location = new System.Drawing.Point(486, 381);
            this.TextBoxInfo.Multiline = true;
            this.TextBoxInfo.Name = "TextBoxInfo";
            this.TextBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxInfo.Size = new System.Drawing.Size(176, 246);
            this.TextBoxInfo.TabIndex = 71;
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(195, 655);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(84, 36);
            this.Test.TabIndex = 72;
            this.Test.Text = "测试";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // TimerNetMonitor
            // 
            this.TimerNetMonitor.Tick += new System.EventHandler(this.TimerNetMonitor_Tick);
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(874, 381);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(100, 21);
            this.TextBox1.TabIndex = 73;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 733);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.TextBoxInfo);
            this.Controls.Add(this.CommunicationMsg);
            this.Controls.Add(this.ConnectABB);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox ConnectABB;
        private System.Windows.Forms.TextBox CommunicationMsg;
        private System.Windows.Forms.TextBox TextBoxInfo;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Timer TimerNetMonitor;
        private System.Windows.Forms.TextBox TextBox1;
    }
}

