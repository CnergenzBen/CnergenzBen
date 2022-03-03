namespace Tcp
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
            if(disposing && ( components != null ))
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
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.data = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.TEST = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.LedLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tt = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.on1 = new System.Windows.Forms.TextBox();
            this.on2 = new System.Windows.Forms.TextBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button9 = new System.Windows.Forms.Button();
            this.timetext = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(482, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(138, 26);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "0000";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 68);
            this.button1.TabIndex = 1;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 68);
            this.button2.TabIndex = 2;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(166, 205);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 68);
            this.button3.TabIndex = 3;
            this.button3.Text = "connect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // data
            // 
            this.data.AutoSize = true;
            this.data.Location = new System.Drawing.Point(356, 40);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(88, 20);
            this.data.TabIndex = 6;
            this.data.Text = "dataSennd";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(700, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "1";
            // 
            // TEST
            // 
            this.TEST.Location = new System.Drawing.Point(16, 98);
            this.TEST.Name = "TEST";
            this.TEST.Size = new System.Drawing.Size(124, 68);
            this.TEST.TabIndex = 10;
            this.TEST.Text = "LEDCheck";
            this.TEST.UseVisualStyleBackColor = true;
            this.TEST.Click += new System.EventHandler(this.TEST_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1161, 105);
            this.button8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(112, 38);
            this.button8.TabIndex = 11;
            this.button8.Text = "InsertLedLocation";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // LedLocation
            // 
            this.LedLocation.Location = new System.Drawing.Point(1342, 42);
            this.LedLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LedLocation.Name = "LedLocation";
            this.LedLocation.Size = new System.Drawing.Size(148, 26);
            this.LedLocation.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(640, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Action";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1150, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "LedwantToOnLocation";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1342, 105);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 38);
            this.button4.TabIndex = 15;
            this.button4.Text = "showdata";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1161, 152);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(112, 38);
            this.button5.TabIndex = 16;
            this.button5.Text = "Clear";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1342, 152);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(112, 38);
            this.button6.TabIndex = 17;
            this.button6.Text = "send";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(326, 117);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(504, 722);
            this.dataGridView1.TabIndex = 18;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tt
            // 
            this.tt.Location = new System.Drawing.Point(88, 540);
            this.tt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tt.Name = "tt";
            this.tt.Size = new System.Drawing.Size(112, 35);
            this.tt.TabIndex = 19;
            this.tt.Text = "tt";
            this.tt.UseVisualStyleBackColor = true;
            this.tt.Click += new System.EventHandler(this.tt_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1074, 382);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(112, 35);
            this.button7.TabIndex = 20;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // on1
            // 
            this.on1.Location = new System.Drawing.Point(1074, 309);
            this.on1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.on1.Name = "on1";
            this.on1.Size = new System.Drawing.Size(148, 26);
            this.on1.TabIndex = 21;
            this.on1.Text = "0";
            // 
            // on2
            // 
            this.on2.Location = new System.Drawing.Point(1305, 309);
            this.on2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.on2.Name = "on2";
            this.on2.Size = new System.Drawing.Size(148, 26);
            this.on2.TabIndex = 22;
            this.on2.Text = "0";
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(1058, 540);
            this.button9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(112, 35);
            this.button9.TabIndex = 23;
            this.button9.Text = "button9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // timetext
            // 
            this.timetext.Location = new System.Drawing.Point(88, 35);
            this.timetext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timetext.Name = "timetext";
            this.timetext.Size = new System.Drawing.Size(148, 26);
            this.timetext.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1054, 643);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 25;
            this.label3.Text = "label3";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1217, 405);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(509, 414);
            this.richTextBox1.TabIndex = 26;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1738, 872);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.timetext);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.on2);
            this.Controls.Add(this.on1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.tt);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LedLocation);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.TEST);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.data);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label data;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button TEST;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox LedLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button tt;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox on1;
        private System.Windows.Forms.TextBox on2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox timetext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

