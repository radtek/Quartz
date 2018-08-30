namespace Quartz.WindowsForms
{
    partial class SendMsg
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendMsg));
            this.msgAccount = new System.Windows.Forms.TextBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.buttons = new System.Windows.Forms.Button();
            this.comboBoxs = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EmContent = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.butSend = new System.Windows.Forms.Button();
            this.labels = new System.Windows.Forms.Label();
            this.butSeting = new System.Windows.Forms.Button();
            this.backgroundWorkers = new System.ComponentModel.BackgroundWorker();
            this.btnStopSend = new System.Windows.Forms.Button();
            this.WinTimer = new System.Windows.Forms.Timer(this.components);
            this.labelTask = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // msgAccount
            // 
            this.msgAccount.Location = new System.Drawing.Point(155, 59);
            this.msgAccount.Name = "msgAccount";
            this.msgAccount.Size = new System.Drawing.Size(448, 21);
            this.msgAccount.TabIndex = 0;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(155, 122);
            this.textBoxTitle.Multiline = true;
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(448, 110);
            this.textBoxTitle.TabIndex = 1;
            // 
            // buttons
            // 
            this.buttons.Location = new System.Drawing.Point(310, 435);
            this.buttons.Name = "buttons";
            this.buttons.Size = new System.Drawing.Size(92, 23);
            this.buttons.TabIndex = 2;
            this.buttons.Text = "测试发送服务";
            this.buttons.UseVisualStyleBackColor = true;
            this.buttons.Click += new System.EventHandler(this.buttons_Click);
            // 
            // comboBoxs
            // 
            this.comboBoxs.FormattingEnabled = true;
            this.comboBoxs.Location = new System.Drawing.Point(240, 92);
            this.comboBoxs.Name = "comboBoxs";
            this.comboBoxs.Size = new System.Drawing.Size(363, 20);
            this.comboBoxs.TabIndex = 3;
            this.comboBoxs.SelectedIndexChanged += new System.EventHandler(this.comboBoxs_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(67, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "信息接收者：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(67, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "邮件标题|微信内容->发送类型";
            // 
            // EmContent
            // 
            this.EmContent.AutoSize = true;
            this.EmContent.BackColor = System.Drawing.Color.White;
            this.EmContent.Location = new System.Drawing.Point(37, 275);
            this.EmContent.Name = "EmContent";
            this.EmContent.Size = new System.Drawing.Size(65, 12);
            this.EmContent.TabIndex = 6;
            this.EmContent.Text = "邮件内容：";
            // 
            // textBoxContent
            // 
            this.textBoxContent.Location = new System.Drawing.Point(155, 238);
            this.textBoxContent.Multiline = true;
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(448, 162);
            this.textBoxContent.TabIndex = 7;
            // 
            // butSend
            // 
            this.butSend.Location = new System.Drawing.Point(408, 435);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(95, 23);
            this.butSend.TabIndex = 8;
            this.butSend.Text = "启动发送任务";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.BackColor = System.Drawing.Color.White;
            this.labels.Location = new System.Drawing.Point(31, 422);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(113, 12);
            this.labels.TabIndex = 9;
            this.labels.Text = "服务器暂时无法使用";
            this.labels.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butSeting
            // 
            this.butSeting.Location = new System.Drawing.Point(220, 435);
            this.butSeting.Name = "butSeting";
            this.butSeting.Size = new System.Drawing.Size(75, 23);
            this.butSeting.TabIndex = 10;
            this.butSeting.Text = "模板设置";
            this.butSeting.UseVisualStyleBackColor = true;
            this.butSeting.Click += new System.EventHandler(this.butSeting_Click);
            // 
            // backgroundWorkers
            // 
            this.backgroundWorkers.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkers_RunWorkerCompleted);
            // 
            // btnStopSend
            // 
            this.btnStopSend.Location = new System.Drawing.Point(509, 435);
            this.btnStopSend.Name = "btnStopSend";
            this.btnStopSend.Size = new System.Drawing.Size(97, 23);
            this.btnStopSend.TabIndex = 11;
            this.btnStopSend.Text = "停止发送服务";
            this.btnStopSend.UseVisualStyleBackColor = true;
            this.btnStopSend.Click += new System.EventHandler(this.btnStopSend_Click);
            // 
            // WinTimer
            // 
            this.WinTimer.Enabled = true;
            this.WinTimer.Interval = 1000;
            this.WinTimer.Tick += new System.EventHandler(this.WinTimer_Tick);
            // 
            // labelTask
            // 
            this.labelTask.AutoSize = true;
            this.labelTask.BackColor = System.Drawing.Color.White;
            this.labelTask.Location = new System.Drawing.Point(33, 365);
            this.labelTask.Name = "labelTask";
            this.labelTask.Size = new System.Drawing.Size(0, 12);
            this.labelTask.TabIndex = 12;
            // 
            // SendMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(174)))), ((int)(((byte)(233)))));
            this.BacklightImg = global::Quartz.WindowsForms.Properties.Resources.all_inside_bkg;
            this.BacklightLTRB = new System.Drawing.Rectangle(10, 80, 10, 70);
            this.ClientSize = new System.Drawing.Size(629, 503);
            this.Controls.Add(this.labelTask);
            this.Controls.Add(this.btnStopSend);
            this.Controls.Add(this.butSeting);
            this.Controls.Add(this.labels);
            this.Controls.Add(this.butSend);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.EmContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxs);
            this.Controls.Add(this.buttons);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.msgAccount);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormSystemBtnSet = AlSkin.AlForm.AlBaseForm.FormSystemBtn.btn_miniAndbtn_close;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(629, 503);
            this.Name = "SendMsg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信息任务发送";
            this.Load += new System.EventHandler(this.SendMsg_Load);
            this.Controls.SetChildIndex(this.msgAccount, 0);
            this.Controls.SetChildIndex(this.textBoxTitle, 0);
            this.Controls.SetChildIndex(this.buttons, 0);
            this.Controls.SetChildIndex(this.comboBoxs, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.EmContent, 0);
            this.Controls.SetChildIndex(this.textBoxContent, 0);
            this.Controls.SetChildIndex(this.butSend, 0);
            this.Controls.SetChildIndex(this.labels, 0);
            this.Controls.SetChildIndex(this.butSeting, 0);
            this.Controls.SetChildIndex(this.btnStopSend, 0);
            this.Controls.SetChildIndex(this.labelTask, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox msgAccount;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Button buttons;
        private System.Windows.Forms.ComboBox comboBoxs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label EmContent;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.Label labels;
        private System.Windows.Forms.Button butSeting;
        private System.ComponentModel.BackgroundWorker backgroundWorkers;
        private System.Windows.Forms.Button btnStopSend;
        private System.Windows.Forms.Timer WinTimer;
        private System.Windows.Forms.Label labelTask;
    }
}

