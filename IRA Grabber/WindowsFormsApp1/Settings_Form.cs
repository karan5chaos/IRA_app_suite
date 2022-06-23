using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1;

public class Settings_Form : Form
{
	private IContainer components = null;

	private Timer timer1;

	private GroupBox groupBox1;

	private Button button1;

	private TextBox textBox1;

	private GroupBox groupBox2;

	private CheckBox checkBox1;

	private ErrorProvider errorProvider1;

	private FolderBrowserDialog folderBrowserDialog1;

	private GroupBox groupBox3;

	private RadioButton radioButton3;

	private RadioButton radioButton2;

	private RadioButton radioButton1;

	private RadioButton radioButton4;

	private GroupBox groupBox4;

	private RadioButton radioButton7;

	private RadioButton radioButton8;

	private ToolTip toolTip1;

	public Settings_Form()
	{
		InitializeComponent();
	}

	private void Form2_Load(object sender, EventArgs e)
	{
		textBox1.Text = Settings.Default.setpath;
		if (Settings.Default.setboot)
		{
			checkBox1.Checked = true;
		}
		else
		{
			checkBox1.Checked = false;
		}
		if (Settings.Default.Auto_mode)
		{
			radioButton1.Checked = true;
		}
		else if (Settings.Default.mode == "IRA")
		{
			radioButton2.Checked = true;
		}
		else if (Settings.Default.mode == "LIDAR")
		{
			radioButton3.Checked = true;
		}
		else if (Settings.Default.mode == "2D")
		{
			radioButton4.Checked = true;
		}
		if (Settings.Default.notif_style == 0)
		{
			radioButton8.Checked = true;
		}
		else
		{
			radioButton7.Checked = true;
		}
	}

	private void Form2_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (checkBox1.Checked)
		{
			Settings.Default.setboot = true;
		}
		else
		{
			Settings.Default.setboot = false;
		}
		if (radioButton1.Checked)
		{
			Settings.Default.mode = "Auto";
			Settings.Default.Auto_mode = true;
		}
		else if (radioButton2.Checked)
		{
			Settings.Default.mode = "IRA";
			Settings.Default.Auto_mode = false;
		}
		else if (radioButton3.Checked)
		{
			Settings.Default.mode = "LIDAR";
			Settings.Default.Auto_mode = false;
		}
		else if (radioButton4.Checked)
		{
			Settings.Default.mode = "2D";
			Settings.Default.Auto_mode = false;
		}
		if (radioButton7.Checked)
		{
			Settings.Default.notif_style = 1;
		}
		else if (radioButton8.Checked)
		{
			Settings.Default.notif_style = 0;
		}
		Settings.Default.setpath = textBox1.Text;
		Settings.Default.Save();
		Settings.Default.Reload();
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
		if (checkBox1.Checked)
		{
			registryKey.SetValue(Application.ProductName, Application.ExecutablePath.ToString());
			Settings.Default.setboot = true;
		}
		else
		{
			registryKey.DeleteValue(Application.ProductName, throwOnMissingValue: false);
			Settings.Default.setboot = false;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		folderBrowserDialog1.ShowDialog(this);
		textBox1.Text = folderBrowserDialog1.SelectedPath;
	}

	private void button1_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void changeLogPathToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void button1_MouseClick(object sender, MouseEventArgs e)
	{
	}

	private void radioButton3_CheckedChanged(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
		this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.radioButton4 = new System.Windows.Forms.RadioButton();
		this.radioButton3 = new System.Windows.Forms.RadioButton();
		this.radioButton2 = new System.Windows.Forms.RadioButton();
		this.radioButton1 = new System.Windows.Forms.RadioButton();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.radioButton7 = new System.Windows.Forms.RadioButton();
		this.radioButton8 = new System.Windows.Forms.RadioButton();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider1).BeginInit();
		this.groupBox3.SuspendLayout();
		this.groupBox4.SuspendLayout();
		base.SuspendLayout();
		this.timer1.Interval = 1000;
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add(this.textBox1);
		this.groupBox1.Location = new System.Drawing.Point(2, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(265, 49);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Set Save Path";
		this.button1.Location = new System.Drawing.Point(226, 18);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(33, 23);
		this.button1.TabIndex = 1;
		this.button1.Text = "...";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(button1_KeyDown);
		this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(button1_MouseClick);
		this.textBox1.Location = new System.Drawing.Point(6, 19);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(214, 21);
		this.textBox1.TabIndex = 0;
		this.groupBox2.Controls.Add(this.checkBox1);
		this.groupBox2.Enabled = false;
		this.groupBox2.Location = new System.Drawing.Point(2, 58);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(265, 43);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Start-up";
		this.toolTip1.SetToolTip(this.groupBox2, "Depricated (Will be removed in next version)");
		this.checkBox1.AutoSize = true;
		this.checkBox1.Enabled = false;
		this.checkBox1.Location = new System.Drawing.Point(6, 20);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(87, 17);
		this.checkBox1.TabIndex = 0;
		this.checkBox1.Text = "Start on Boot";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.errorProvider1.ContainerControl = this;
		this.groupBox3.Controls.Add(this.radioButton4);
		this.groupBox3.Controls.Add(this.radioButton3);
		this.groupBox3.Controls.Add(this.radioButton2);
		this.groupBox3.Controls.Add(this.radioButton1);
		this.groupBox3.Location = new System.Drawing.Point(2, 107);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(265, 47);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Mode";
		this.radioButton4.AutoSize = true;
		this.radioButton4.Location = new System.Drawing.Point(166, 20);
		this.radioButton4.Name = "radioButton4";
		this.radioButton4.Size = new System.Drawing.Size(97, 17);
		this.radioButton4.TabIndex = 3;
		this.radioButton4.TabStop = true;
		this.radioButton4.Text = "2D Annotations";
		this.radioButton4.UseVisualStyleBackColor = true;
		this.radioButton3.AutoSize = true;
		this.radioButton3.Location = new System.Drawing.Point(108, 20);
		this.radioButton3.Name = "radioButton3";
		this.radioButton3.Size = new System.Drawing.Size(52, 17);
		this.radioButton3.TabIndex = 2;
		this.radioButton3.TabStop = true;
		this.radioButton3.Text = "LIDAR";
		this.radioButton3.UseVisualStyleBackColor = true;
		this.radioButton3.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
		this.radioButton2.AutoSize = true;
		this.radioButton2.Location = new System.Drawing.Point(59, 21);
		this.radioButton2.Name = "radioButton2";
		this.radioButton2.Size = new System.Drawing.Size(40, 17);
		this.radioButton2.TabIndex = 1;
		this.radioButton2.TabStop = true;
		this.radioButton2.Text = "IRA";
		this.radioButton2.UseVisualStyleBackColor = true;
		this.radioButton1.AutoSize = true;
		this.radioButton1.Location = new System.Drawing.Point(6, 21);
		this.radioButton1.Name = "radioButton1";
		this.radioButton1.Size = new System.Drawing.Size(47, 17);
		this.radioButton1.TabIndex = 0;
		this.radioButton1.TabStop = true;
		this.radioButton1.Text = "Auto";
		this.radioButton1.UseVisualStyleBackColor = true;
		this.groupBox4.Controls.Add(this.radioButton7);
		this.groupBox4.Controls.Add(this.radioButton8);
		this.groupBox4.Location = new System.Drawing.Point(2, 160);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(265, 47);
		this.groupBox4.TabIndex = 4;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Notofications";
		this.radioButton7.AutoSize = true;
		this.radioButton7.Location = new System.Drawing.Point(201, 21);
		this.radioButton7.Name = "radioButton7";
		this.radioButton7.Size = new System.Drawing.Size(55, 17);
		this.radioButton7.TabIndex = 1;
		this.radioButton7.TabStop = true;
		this.radioButton7.Text = "Popup";
		this.toolTip1.SetToolTip(this.radioButton7, "Shows a popup to notify image capture (May cause delays)");
		this.radioButton7.UseVisualStyleBackColor = true;
		this.radioButton8.AutoSize = true;
		this.radioButton8.Location = new System.Drawing.Point(6, 21);
		this.radioButton8.Name = "radioButton8";
		this.radioButton8.Size = new System.Drawing.Size(91, 17);
		this.radioButton8.TabIndex = 0;
		this.radioButton8.TabStop = true;
		this.radioButton8.Text = "Flash Window";
		this.toolTip1.SetToolTip(this.radioButton8, "Flashes taskbar window to notify image capture (Default)");
		this.radioButton8.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(270, 212);
		base.Controls.Add(this.groupBox4);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "Settings_Form";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Settings";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form2_FormClosing);
		base.Load += new System.EventHandler(Form2_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider1).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		base.ResumeLayout(false);
	}
}
