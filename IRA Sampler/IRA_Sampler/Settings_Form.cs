using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AForge.Math.Geometry;
using IRA_Sampler.Properties;

namespace IRA_Sampler;

public class Settings_Form : Form
{
	private Settings settings = Settings.Default;

	private IContainer components = null;

	private GroupBox groupBox1;

	private Button button2;

	private TextBox textBox2;

	private Label label2;

	private Button button1;

	private TextBox textBox1;

	private Label label1;

	private GroupBox groupBox2;

	private GroupBox groupBox4;

	private ListBox listBox1;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem removeToolStripMenuItem;

	private GroupBox groupBox3;

	private RadioButton radioButton2;

	private TextBox textBox3;

	private RadioButton radioButton1;

	private FolderBrowserDialog folderBrowserDialog1;

	private FolderBrowserDialog folderBrowserDialog2;

	private GroupBox groupBox5;

	private ComboBox comboBox1;

	private Label label3;

	private CheckBox checkBox1;

	private CheckBox checkBox2;

	private ToolTip toolTip1;

	private GroupBox groupBox6;

	private CheckBox checkBox4;

	private CheckBox checkBox3;

	private GroupBox groupBox7;

	private RadioButton radioButton4;

	private RadioButton radioButton3;

	public Settings_Form()
	{
		InitializeComponent();
	}

	private void Settings_Load(object sender, EventArgs e)
	{
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		textBox1.Text = settings.s_location;
		textBox2.Text = settings.o_location;
		if (Settings.Default.process_ir)
		{
			radioButton3.Checked = true;
		}
		else
		{
			radioButton3.Checked = true;
		}
		if (Settings.Default.process_2d)
		{
			radioButton4.Checked = true;
		}
		else
		{
			radioButton4.Checked = false;
		}
		if (radioButton3.Checked)
		{
			if (Settings.Default.enable_ImageDetection)
			{
				checkBox1.Checked = true;
			}
			else
			{
				checkBox1.Checked = false;
			}
			if (Settings.Default.fill_colour)
			{
				checkBox2.Checked = true;
			}
			else
			{
				checkBox2.Checked = false;
			}
		}
		if (Settings.Default.sh_err_desc)
		{
			checkBox3.Checked = true;
		}
		else
		{
			checkBox3.Checked = false;
		}
		if (Settings.Default.sh_err_comments)
		{
			checkBox4.Checked = true;
		}
		else
		{
			checkBox4.Checked = false;
		}
		comboBox1.SelectedText = ((object)Settings.Default.shape_type).ToString();
	}

	private void Settings_FormClosing(object sender, FormClosingEventArgs e)
	{
		settings.s_location = textBox1.Text;
		settings.o_location = textBox2.Text;
		if (radioButton3.Checked)
		{
			if (checkBox1.Checked)
			{
				Settings.Default.enable_ImageDetection = true;
			}
			else
			{
				Settings.Default.enable_ImageDetection = false;
			}
			if (checkBox2.Checked)
			{
				Settings.Default.fill_colour = true;
			}
			else
			{
				Settings.Default.fill_colour = false;
			}
			if (comboBox1.SelectedIndex == 0)
			{
				settings.shape_type = (PolygonSubType)5;
			}
			else if (comboBox1.SelectedIndex == 1)
			{
				settings.shape_type = (PolygonSubType)3;
			}
		}
		if (checkBox3.Checked)
		{
			Settings.Default.sh_err_desc = true;
		}
		else
		{
			Settings.Default.sh_err_desc = false;
		}
		if (checkBox4.Checked)
		{
			Settings.Default.sh_err_comments = true;
		}
		else
		{
			Settings.Default.sh_err_comments = false;
		}
		if (radioButton3.Checked)
		{
			Settings.Default.process_ir = true;
		}
		else
		{
			Settings.Default.process_ir = false;
		}
		if (radioButton4.Checked)
		{
			Settings.Default.process_2d = true;
		}
		else
		{
			Settings.Default.process_2d = false;
		}
		settings.Save();
		settings.Reload();
	}

	private void radioButton1_CheckedChanged(object sender, EventArgs e)
	{
		if (radioButton1.Checked)
		{
			get_user();
		}
	}

	private void get_user()
	{
		if (Settings.Default.user_list.Count <= 0)
		{
			return;
		}
		listBox1.Items.Clear();
		StringEnumerator enumerator = Settings.Default.user_list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				listBox1.Items.Add(current);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

	private void get_err()
	{
		StringEnumerator enumerator;
		if (radioButton3.Checked)
		{
			if (Settings.Default.err_list.Count <= 0)
			{
				return;
			}
			listBox1.Items.Clear();
			enumerator = Settings.Default.err_list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					listBox1.Items.Add(current);
				}
				return;
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		if (!radioButton4.Checked || Settings.Default.err_list_2d.Count <= 0)
		{
			return;
		}
		listBox1.Items.Clear();
		enumerator = Settings.Default.err_list_2d.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				listBox1.Items.Add(current);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable2)
			{
				disposable2.Dispose();
			}
		}
	}

	private void radioButton2_CheckedChanged(object sender, EventArgs e)
	{
		if (radioButton2.Checked)
		{
			get_err();
		}
	}

	private void textBox3_MouseEnter(object sender, EventArgs e)
	{
	}

	private void textBox3_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			if (radioButton1.Checked && !Settings.Default.user_list.Contains(textBox3.Text))
			{
				Settings.Default.user_list.Add(textBox3.Text);
				Settings.Default.Save();
				Settings.Default.Reload();
				get_user();
			}
			else if (radioButton2.Checked && !Settings.Default.err_list.Contains(textBox3.Text))
			{
				Settings.Default.err_list.Add(textBox3.Text);
				Settings.Default.Save();
				Settings.Default.Reload();
				get_err();
			}
		}
	}

	private void removeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (radioButton1.Checked)
		{
			Settings.Default.user_list.Remove(listBox1.SelectedItem.ToString());
			Settings.Default.Save();
			Settings.Default.Reload();
			get_user();
		}
		else if (radioButton2.Checked)
		{
			if (Settings.Default.process_ir)
			{
				Settings.Default.err_list.Remove(listBox1.SelectedItem.ToString());
			}
			else if (Settings.Default.process_2d)
			{
				Settings.Default.err_list_2d.Remove(listBox1.SelectedItem.ToString());
			}
			Settings.Default.Save();
			Settings.Default.Reload();
			get_err();
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		folderBrowserDialog1.ShowDialog(this);
		if (folderBrowserDialog1.SelectedPath != null && folderBrowserDialog1.SelectedPath != "")
		{
			textBox1.Text = folderBrowserDialog1.SelectedPath;
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		folderBrowserDialog2.ShowDialog(this);
		if (folderBrowserDialog2.SelectedPath != null && folderBrowserDialog2.SelectedPath != "")
		{
			textBox2.Text = folderBrowserDialog2.SelectedPath;
		}
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void checkBox3_CheckedChanged(object sender, EventArgs e)
	{
		Settings.Default.sh_err_desc = true;
	}

	private void checkBox4_CheckedChanged(object sender, EventArgs e)
	{
		Settings.Default.sh_err_comments = true;
	}

	private void radioButton4_CheckedChanged(object sender, EventArgs e)
	{
		if (radioButton4.Checked)
		{
			checkBox1.Checked = false;
			checkBox2.Checked = false;
			checkBox1.Enabled = false;
			checkBox2.Enabled = false;
		}
		else if (radioButton3.Checked)
		{
			checkBox1.Enabled = true;
			checkBox2.Enabled = true;
		}
	}

	private void radioButton3_CheckedChanged(object sender, EventArgs e)
	{
		if (radioButton3.Checked)
		{
			checkBox1.Enabled = true;
			checkBox2.Enabled = true;
		}
		else
		{
			checkBox1.Enabled = false;
			checkBox2.Enabled = false;
		}
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.button2 = new System.Windows.Forms.Button();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.listBox1 = new System.Windows.Forms.ListBox();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.radioButton2 = new System.Windows.Forms.RadioButton();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.radioButton1 = new System.Windows.Forms.RadioButton();
		this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
		this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.checkBox2 = new System.Windows.Forms.CheckBox();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.checkBox4 = new System.Windows.Forms.CheckBox();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.radioButton4 = new System.Windows.Forms.RadioButton();
		this.radioButton3 = new System.Windows.Forms.RadioButton();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.contextMenuStrip1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.groupBox6.SuspendLayout();
		this.groupBox7.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.button2);
		this.groupBox1.Controls.Add(this.textBox2);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add(this.textBox1);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Location = new System.Drawing.Point(12, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(347, 87);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Path Settings";
		this.button2.Location = new System.Drawing.Point(301, 49);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(37, 23);
		this.button2.TabIndex = 5;
		this.button2.Text = "...";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.textBox2.Location = new System.Drawing.Point(115, 51);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(180, 21);
		this.textBox2.TabIndex = 4;
		this.textBox2.TabStop = false;
		this.toolTip1.SetToolTip(this.textBox2, "Set location for exporting reports");
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(12, 54);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(87, 13);
		this.label2.TabIndex = 3;
		this.label2.Text = "Output Location :";
		this.button1.Location = new System.Drawing.Point(301, 23);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(37, 23);
		this.button1.TabIndex = 2;
		this.button1.Text = "...";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.textBox1.Location = new System.Drawing.Point(115, 25);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(180, 21);
		this.textBox1.TabIndex = 1;
		this.textBox1.TabStop = false;
		this.toolTip1.SetToolTip(this.textBox1, "Set folder location of image files");
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 28);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Samples Location :";
		this.groupBox2.Controls.Add(this.groupBox4);
		this.groupBox2.Controls.Add(this.groupBox3);
		this.groupBox2.Location = new System.Drawing.Point(12, 306);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(347, 260);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.groupBox4.Controls.Add(this.listBox1);
		this.groupBox4.Location = new System.Drawing.Point(12, 114);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(323, 132);
		this.groupBox4.TabIndex = 5;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "List";
		this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
		this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listBox1.FormattingEnabled = true;
		this.listBox1.Location = new System.Drawing.Point(3, 17);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(317, 112);
		this.listBox1.TabIndex = 0;
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.removeToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(142, 26);
		this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
		this.removeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
		this.removeToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
		this.removeToolStripMenuItem.Text = "Remove";
		this.removeToolStripMenuItem.Click += new System.EventHandler(removeToolStripMenuItem_Click);
		this.groupBox3.Controls.Add(this.radioButton2);
		this.groupBox3.Controls.Add(this.textBox3);
		this.groupBox3.Controls.Add(this.radioButton1);
		this.groupBox3.Location = new System.Drawing.Point(12, 21);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(323, 88);
		this.groupBox3.TabIndex = 4;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Add New";
		this.radioButton2.AutoSize = true;
		this.radioButton2.Location = new System.Drawing.Point(174, 27);
		this.radioButton2.Name = "radioButton2";
		this.radioButton2.Size = new System.Drawing.Size(104, 17);
		this.radioButton2.TabIndex = 3;
		this.radioButton2.TabStop = true;
		this.radioButton2.Text = "Error Description";
		this.radioButton2.UseVisualStyleBackColor = true;
		this.radioButton2.CheckedChanged += new System.EventHandler(radioButton2_CheckedChanged);
		this.textBox3.Location = new System.Drawing.Point(46, 50);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(232, 21);
		this.textBox3.TabIndex = 1;
		this.textBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox3_KeyDown);
		this.textBox3.MouseEnter += new System.EventHandler(textBox3_MouseEnter);
		this.radioButton1.AutoSize = true;
		this.radioButton1.Location = new System.Drawing.Point(46, 27);
		this.radioButton1.Name = "radioButton1";
		this.radioButton1.Size = new System.Drawing.Size(74, 17);
		this.radioButton1.TabIndex = 2;
		this.radioButton1.TabStop = true;
		this.radioButton1.Text = "Username";
		this.radioButton1.UseVisualStyleBackColor = true;
		this.radioButton1.CheckedChanged += new System.EventHandler(radioButton1_CheckedChanged);
		this.groupBox5.Controls.Add(this.checkBox2);
		this.groupBox5.Controls.Add(this.comboBox1);
		this.groupBox5.Controls.Add(this.label3);
		this.groupBox5.Controls.Add(this.checkBox1);
		this.groupBox5.Location = new System.Drawing.Point(12, 149);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(347, 85);
		this.groupBox5.TabIndex = 6;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Pots Detection (Experimental)";
		this.checkBox2.AutoSize = true;
		this.checkBox2.Location = new System.Drawing.Point(15, 54);
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.Size = new System.Drawing.Size(98, 17);
		this.checkBox2.TabIndex = 3;
		this.checkBox2.Text = "Fill Pixel Colour";
		this.toolTip1.SetToolTip(this.checkBox2, "Fill certain image colours (may increase pots detection accuracy)");
		this.checkBox2.UseVisualStyleBackColor = true;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Items.AddRange(new object[2] { "Square", "Rectangle" });
		this.comboBox1.Location = new System.Drawing.Point(251, 24);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(81, 21);
		this.comboBox1.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(180, 27);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 13);
		this.label3.TabIndex = 1;
		this.label3.Text = "Shape Type :";
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(15, 26);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(137, 17);
		this.checkBox1.TabIndex = 0;
		this.checkBox1.Text = "Enable Image Detection";
		this.toolTip1.SetToolTip(this.checkBox1, "Enable Auto-Detection for Pots");
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.checkBox3.AutoSize = true;
		this.checkBox3.Location = new System.Drawing.Point(15, 26);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(120, 17);
		this.checkBox3.TabIndex = 0;
		this.checkBox3.Text = "Error Desc. Columns";
		this.toolTip1.SetToolTip(this.checkBox3, "Show/Hide Error-Wise Columns\r\n(Takes effect when application is restarted)");
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox3.CheckedChanged += new System.EventHandler(checkBox3_CheckedChanged);
		this.checkBox4.AutoSize = true;
		this.checkBox4.Location = new System.Drawing.Point(192, 26);
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.Size = new System.Drawing.Size(140, 17);
		this.checkBox4.TabIndex = 1;
		this.checkBox4.Text = "Error Comments Column";
		this.toolTip1.SetToolTip(this.checkBox4, "Show/Hide Error Comments Column\r\n(Takes effect when application is restarted)");
		this.checkBox4.UseVisualStyleBackColor = true;
		this.checkBox4.CheckedChanged += new System.EventHandler(checkBox4_CheckedChanged);
		this.groupBox6.Controls.Add(this.checkBox4);
		this.groupBox6.Controls.Add(this.checkBox3);
		this.groupBox6.Location = new System.Drawing.Point(12, 240);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(347, 60);
		this.groupBox6.TabIndex = 7;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Show/Hide Gridview Column";
		this.groupBox7.Controls.Add(this.radioButton4);
		this.groupBox7.Controls.Add(this.radioButton3);
		this.groupBox7.Location = new System.Drawing.Point(13, 96);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(346, 50);
		this.groupBox7.TabIndex = 8;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Process Name";
		this.radioButton4.AutoSize = true;
		this.radioButton4.Location = new System.Drawing.Point(182, 20);
		this.radioButton4.Name = "radioButton4";
		this.radioButton4.Size = new System.Drawing.Size(92, 17);
		this.radioButton4.TabIndex = 1;
		this.radioButton4.TabStop = true;
		this.radioButton4.Text = "2D Annotation";
		this.radioButton4.UseVisualStyleBackColor = true;
		this.radioButton4.CheckedChanged += new System.EventHandler(radioButton4_CheckedChanged);
		this.radioButton3.AutoSize = true;
		this.radioButton3.Location = new System.Drawing.Point(14, 21);
		this.radioButton3.Name = "radioButton3";
		this.radioButton3.Size = new System.Drawing.Size(49, 17);
		this.radioButton3.TabIndex = 0;
		this.radioButton3.TabStop = true;
		this.radioButton3.Text = "IR/LR";
		this.radioButton3.UseVisualStyleBackColor = true;
		this.radioButton3.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(371, 577);
		base.Controls.Add(this.groupBox7);
		base.Controls.Add(this.groupBox6);
		base.Controls.Add(this.groupBox5);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Settings_Form";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Settings";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Settings_FormClosing);
		base.Load += new System.EventHandler(Settings_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.contextMenuStrip1.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		base.ResumeLayout(false);
	}
}
