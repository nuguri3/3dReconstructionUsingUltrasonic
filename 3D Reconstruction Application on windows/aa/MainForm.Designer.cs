namespace aa
{
    partial class Betman
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
                //simpleOpenGlControl1.DestroyContexts();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Betman));
            this.simpleOpenGlControl = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.move_Bt = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.stop_Bt = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.present_Arrow = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.totalDistance_box = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.yAccel_box = new System.Windows.Forms.TextBox();
            this.xAccel_box = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.post_Arrow = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cancel_Bt = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.set_Bt = new System.Windows.Forms.Button();
            this.manual_Rbt = new System.Windows.Forms.RadioButton();
            this.auto_Rbt = new System.Windows.Forms.RadioButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.present_Arrow)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.post_Arrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleOpenGlControl
            // 
            this.simpleOpenGlControl.AccumBits = ((byte)(0));
            this.simpleOpenGlControl.AutoCheckErrors = false;
            this.simpleOpenGlControl.AutoFinish = false;
            this.simpleOpenGlControl.AutoMakeCurrent = true;
            this.simpleOpenGlControl.AutoSwapBuffers = true;
            this.simpleOpenGlControl.BackColor = System.Drawing.Color.Black;
            this.simpleOpenGlControl.ColorBits = ((byte)(32));
            this.simpleOpenGlControl.DepthBits = ((byte)(16));
            this.simpleOpenGlControl.Location = new System.Drawing.Point(12, 12);
            this.simpleOpenGlControl.Name = "simpleOpenGlControl";
            this.simpleOpenGlControl.Size = new System.Drawing.Size(1325, 941);
            this.simpleOpenGlControl.StencilBits = ((byte)(0));
            this.simpleOpenGlControl.TabIndex = 0;
            // 
            // move_Bt
            // 
            this.move_Bt.Location = new System.Drawing.Point(1617, 21);
            this.move_Bt.Name = "move_Bt";
            this.move_Bt.Size = new System.Drawing.Size(112, 70);
            this.move_Bt.TabIndex = 1;
            this.move_Bt.Text = "Move";
            this.move_Bt.UseVisualStyleBackColor = true;
            this.move_Bt.Click += new System.EventHandler(this.move_Bt_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(1534, 527);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(235, 388);
            this.listBox1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1535, 508);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "Coordinates";
            // 
            // stop_Bt
            // 
            this.stop_Bt.Location = new System.Drawing.Point(1618, 97);
            this.stop_Bt.Name = "stop_Bt";
            this.stop_Bt.Size = new System.Drawing.Size(112, 70);
            this.stop_Bt.TabIndex = 2;
            this.stop_Bt.Text = "Sensing";
            this.stop_Bt.UseVisualStyleBackColor = true;
            this.stop_Bt.Click += new System.EventHandler(this.stop_Bt_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.present_Arrow);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(1343, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 201);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Navigation";
            // 
            // present_Arrow
            // 
            this.present_Arrow.Image = ((System.Drawing.Image)(resources.GetObject("present_Arrow.Image")));
            this.present_Arrow.InitialImage = ((System.Drawing.Image)(resources.GetObject("present_Arrow.InitialImage")));
            this.present_Arrow.Location = new System.Drawing.Point(84, 46);
            this.present_Arrow.Name = "present_Arrow";
            this.present_Arrow.Size = new System.Drawing.Size(110, 108);
            this.present_Arrow.TabIndex = 0;
            this.present_Arrow.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1343, 394);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "Total Distance";
            // 
            // totalDistance_box
            // 
            this.totalDistance_box.Location = new System.Drawing.Point(1343, 416);
            this.totalDistance_box.Name = "totalDistance_box";
            this.totalDistance_box.Size = new System.Drawing.Size(157, 21);
            this.totalDistance_box.TabIndex = 18;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.yAccel_box);
            this.groupBox2.Controls.Add(this.xAccel_box);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(1345, 467);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 180);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accelation meter";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(9, 37);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(137, 21);
            this.textBox7.TabIndex = 8;
            // 
            // yAccel_box
            // 
            this.yAccel_box.Location = new System.Drawing.Point(27, 138);
            this.yAccel_box.Name = "yAccel_box";
            this.yAccel_box.Size = new System.Drawing.Size(119, 21);
            this.yAccel_box.TabIndex = 6;
            // 
            // xAccel_box
            // 
            this.xAccel_box.Location = new System.Drawing.Point(27, 88);
            this.xAccel_box.Name = "xAccel_box";
            this.xAccel_box.Size = new System.Drawing.Size(119, 21);
            this.xAccel_box.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 141);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "Y:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "Accelation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Move";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.post_Arrow);
            this.groupBox3.Location = new System.Drawing.Point(1343, 229);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 153);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Past";
            // 
            // post_Arrow
            // 
            this.post_Arrow.Image = ((System.Drawing.Image)(resources.GetObject("post_Arrow.Image")));
            this.post_Arrow.Location = new System.Drawing.Point(31, 25);
            this.post_Arrow.Name = "post_Arrow";
            this.post_Arrow.Size = new System.Drawing.Size(110, 108);
            this.post_Arrow.TabIndex = 0;
            this.post_Arrow.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1342, 700);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(186, 215);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1341, 683);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "System status";
            // 
            // cancel_Bt
            // 
            this.cancel_Bt.Location = new System.Drawing.Point(1691, 919);
            this.cancel_Bt.Name = "cancel_Bt";
            this.cancel_Bt.Size = new System.Drawing.Size(95, 42);
            this.cancel_Bt.TabIndex = 28;
            this.cancel_Bt.Text = "Cancel";
            this.cancel_Bt.UseVisualStyleBackColor = true;
            this.cancel_Bt.Click += new System.EventHandler(this.cancel_Bt_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1345, 921);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(344, 39);
            this.pictureBox3.TabIndex = 29;
            this.pictureBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.set_Bt);
            this.groupBox4.Controls.Add(this.manual_Rbt);
            this.groupBox4.Controls.Add(this.auto_Rbt);
            this.groupBox4.Location = new System.Drawing.Point(1542, 237);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(227, 144);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mode Setting";
            // 
            // set_Bt
            // 
            this.set_Bt.Location = new System.Drawing.Point(127, 50);
            this.set_Bt.Name = "set_Bt";
            this.set_Bt.Size = new System.Drawing.Size(77, 48);
            this.set_Bt.TabIndex = 10;
            this.set_Bt.Text = "Set";
            this.set_Bt.UseVisualStyleBackColor = true;
            this.set_Bt.Click += new System.EventHandler(this.set_Bt_Click);
            // 
            // manual_Rbt
            // 
            this.manual_Rbt.AutoSize = true;
            this.manual_Rbt.Location = new System.Drawing.Point(15, 82);
            this.manual_Rbt.Name = "manual_Rbt";
            this.manual_Rbt.Size = new System.Drawing.Size(65, 16);
            this.manual_Rbt.TabIndex = 9;
            this.manual_Rbt.TabStop = true;
            this.manual_Rbt.Text = "manual";
            this.manual_Rbt.UseVisualStyleBackColor = true;
            // 
            // auto_Rbt
            // 
            this.auto_Rbt.AutoSize = true;
            this.auto_Rbt.Location = new System.Drawing.Point(15, 50);
            this.auto_Rbt.Name = "auto_Rbt";
            this.auto_Rbt.Size = new System.Drawing.Size(79, 16);
            this.auto_Rbt.TabIndex = 8;
            this.auto_Rbt.TabStop = true;
            this.auto_Rbt.Text = "Automatic";
            this.auto_Rbt.UseVisualStyleBackColor = true;
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 112500;
            this.serialPort1.PortName = "COM4";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(1648, 193);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(121, 20);
            this.cmbPort.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1557, 435);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 37);
            this.button1.TabIndex = 32;
            this.button1.Text = "Left";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1667, 435);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(68, 37);
            this.button2.TabIndex = 33;
            this.button2.Text = "Right";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Betman
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1791, 965);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbPort);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.cancel_Bt);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.totalDistance_box);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.stop_Bt);
            this.Controls.Add(this.move_Bt);
            this.Controls.Add(this.simpleOpenGlControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Betman";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Betman";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.present_Arrow)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.post_Arrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl simpleOpenGlControl;
        private System.Windows.Forms.Button move_Bt;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button stop_Bt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox present_Arrow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox totalDistance_box;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox yAccel_box;
        private System.Windows.Forms.TextBox xAccel_box;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox post_Arrow;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button cancel_Bt;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button set_Bt;
        private System.Windows.Forms.RadioButton manual_Rbt;
        private System.Windows.Forms.RadioButton auto_Rbt;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;



    }
}

