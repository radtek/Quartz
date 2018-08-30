namespace Senparc.Joson.WindowsForm
{
    partial class JosonJiang
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JosonJiang));
            this.label2 = new System.Windows.Forms.Label();
            this.msgAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxs = new System.Windows.Forms.ComboBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.EmContent = new System.Windows.Forms.Label();
            this.labels = new System.Windows.Forms.Label();
            this.btnStopSend = new System.Windows.Forms.Button();
            this.butSeting = new System.Windows.Forms.Button();
            this.butSend = new System.Windows.Forms.Button();
            this.buttons = new System.Windows.Forms.Button();
            this.labelTask = new System.Windows.Forms.Label();
            this.checkBoxs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(21, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "信息接收者：";
            // 
            // msgAccount
            // 
            this.msgAccount.Location = new System.Drawing.Point(113, 45);
            this.msgAccount.Name = "msgAccount";
            this.msgAccount.Size = new System.Drawing.Size(473, 21);
            this.msgAccount.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(21, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "邮件标题|微信内容->发送类型";
            // 
            // comboBoxs
            // 
            this.comboBoxs.FormattingEnabled = true;
            this.comboBoxs.Location = new System.Drawing.Point(194, 80);
            this.comboBoxs.Name = "comboBoxs";
            this.comboBoxs.Size = new System.Drawing.Size(390, 20);
            this.comboBoxs.TabIndex = 9;
            this.comboBoxs.SelectedIndexChanged += new System.EventHandler(this.comboBoxs_SelectedIndexChanged);
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(111, 119);
            this.textBoxTitle.Multiline = true;
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(475, 110);
            this.textBoxTitle.TabIndex = 11;
            // 
            // textBoxContent
            // 
            this.textBoxContent.Location = new System.Drawing.Point(111, 233);
            this.textBoxContent.Multiline = true;
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(475, 152);
            this.textBoxContent.TabIndex = 13;
            // 
            // EmContent
            // 
            this.EmContent.AutoSize = true;
            this.EmContent.BackColor = System.Drawing.Color.White;
            this.EmContent.Location = new System.Drawing.Point(23, 250);
            this.EmContent.Name = "EmContent";
            this.EmContent.Size = new System.Drawing.Size(65, 12);
            this.EmContent.TabIndex = 12;
            this.EmContent.Text = "邮件内容：";
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.BackColor = System.Drawing.Color.White;
            this.labels.ForeColor = System.Drawing.Color.Green;
            this.labels.Location = new System.Drawing.Point(23, 426);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(137, 12);
            this.labels.TabIndex = 14;
            this.labels.Text = "正在尝试连接服务器....";
            this.labels.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStopSend
            // 
            this.btnStopSend.Location = new System.Drawing.Point(489, 454);
            this.btnStopSend.Name = "btnStopSend";
            this.btnStopSend.Size = new System.Drawing.Size(97, 23);
            this.btnStopSend.TabIndex = 18;
            this.btnStopSend.Text = "停止发送服务";
            this.btnStopSend.UseVisualStyleBackColor = true;
            this.btnStopSend.Click += new System.EventHandler(this.btnStopSend_Click);
            // 
            // butSeting
            // 
            this.butSeting.Location = new System.Drawing.Point(200, 454);
            this.butSeting.Name = "butSeting";
            this.butSeting.Size = new System.Drawing.Size(75, 23);
            this.butSeting.TabIndex = 17;
            this.butSeting.Text = "模板设置";
            this.butSeting.UseVisualStyleBackColor = true;
            this.butSeting.Click += new System.EventHandler(this.butSeting_Click);
            // 
            // butSend
            // 
            this.butSend.Location = new System.Drawing.Point(388, 454);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(95, 23);
            this.butSend.TabIndex = 16;
            this.butSend.Text = "启动发送任务";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // buttons
            // 
            this.buttons.Location = new System.Drawing.Point(290, 454);
            this.buttons.Name = "buttons";
            this.buttons.Size = new System.Drawing.Size(92, 23);
            this.buttons.TabIndex = 15;
            this.buttons.Text = "测试发送服务";
            this.buttons.UseVisualStyleBackColor = true;
            this.buttons.Click += new System.EventHandler(this.buttons_Click);
            // 
            // labelTask
            // 
            this.labelTask.AutoSize = true;
            this.labelTask.BackColor = System.Drawing.Color.White;
            this.labelTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelTask.Location = new System.Drawing.Point(118, 400);
            this.labelTask.Name = "labelTask";
            this.labelTask.Size = new System.Drawing.Size(107, 12);
            this.labelTask.TabIndex = 19;
            this.labelTask.Text = "等待执行任务中...";
            // 
            // checkBoxs
            // 
            this.checkBoxs.AutoSize = true;
            this.checkBoxs.BackColor = System.Drawing.Color.White;
            this.checkBoxs.Location = new System.Drawing.Point(24, 398);
            this.checkBoxs.Name = "checkBoxs";
            this.checkBoxs.Size = new System.Drawing.Size(84, 16);
            this.checkBoxs.TabIndex = 20;
            this.checkBoxs.Text = "多线程发送";
            this.checkBoxs.UseVisualStyleBackColor = false;
            // 
            // JosonJiang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BacklightImg = global::Senparc.Joson.WindowsForm.Properties.Resources.all_inside_bkg;
            this.BacklightLTRB = new System.Drawing.Rectangle(10, 80, 10, 70);
            this.ClientSize = new System.Drawing.Size(629, 525);
            this.Controls.Add(this.checkBoxs);
            this.Controls.Add(this.labelTask);
            this.Controls.Add(this.btnStopSend);
            this.Controls.Add(this.butSeting);
            this.Controls.Add(this.butSend);
            this.Controls.Add(this.buttons);
            this.Controls.Add(this.labels);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.EmContent);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.msgAccount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(629, 525);
            this.MinimumSize = new System.Drawing.Size(629, 525);
            this.Name = "JosonJiang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "多线程委托发送微信、邮件--创维数字@蒋寒鹏（SDT12872）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JosonJiang_FormClosing);
            this.Controls.SetChildIndex(this.msgAccount, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.comboBoxs, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBoxTitle, 0);
            this.Controls.SetChildIndex(this.EmContent, 0);
            this.Controls.SetChildIndex(this.textBoxContent, 0);
            this.Controls.SetChildIndex(this.labels, 0);
            this.Controls.SetChildIndex(this.buttons, 0);
            this.Controls.SetChildIndex(this.butSend, 0);
            this.Controls.SetChildIndex(this.butSeting, 0);
            this.Controls.SetChildIndex(this.btnStopSend, 0);
            this.Controls.SetChildIndex(this.labelTask, 0);
            this.Controls.SetChildIndex(this.checkBoxs, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox msgAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxs;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Label EmContent;
        private System.Windows.Forms.Label labels;
        private System.Windows.Forms.Button btnStopSend;
        private System.Windows.Forms.Button butSeting;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.Button buttons;
        private System.Windows.Forms.Label labelTask;
        private System.Windows.Forms.CheckBox checkBoxs;
    }
}

