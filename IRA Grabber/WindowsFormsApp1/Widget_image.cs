using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1;

public class Widget_image : Form
{
	private IContainer components = null;

	public SplitContainer splitContainer1;

	private Timer timer1;

	private GroupBox groupBox2;

	public PictureBox pictureBox1;

	private GroupBox groupBox3;

	public PictureBox pictureBox2;

	private GroupBox groupBox1;

	private Button button3;

	private Button button2;

	private Button button1;

	public Widget_image()
	{
		InitializeComponent();
	}

	private void Widget_image_Load(object sender, EventArgs e)
	{
		timer1.Start();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (Settings.Default.mode == "2D")
		{
			splitContainer1.Panel2Collapsed = false;
		}
		else
		{
			splitContainer1.Panel2Collapsed = true;
		}
	}

	private void button1_Click(object sender, EventArgs e)
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
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		base.SuspendLayout();
		this.splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.splitContainer1.Location = new System.Drawing.Point(2, 3);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
		this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
		this.splitContainer1.Size = new System.Drawing.Size(468, 232);
		this.splitContainer1.SplitterDistance = 231;
		this.splitContainer1.TabIndex = 0;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.groupBox1.Controls.Add(this.button3);
		this.groupBox1.Controls.Add(this.button2);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.groupBox1.Location = new System.Drawing.Point(0, 233);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(473, 59);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Settings";
		this.button1.Location = new System.Drawing.Point(5, 20);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(113, 23);
		this.button1.TabIndex = 0;
		this.button1.Text = "Clear Primary Image";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Location = new System.Drawing.Point(160, 20);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(122, 23);
		this.button2.TabIndex = 1;
		this.button2.Text = "Clear Secondary Image";
		this.button2.UseVisualStyleBackColor = true;
		this.button3.Location = new System.Drawing.Point(319, 20);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(148, 23);
		this.button3.TabIndex = 2;
		this.button3.Text = "Clear images and recapture";
		this.button3.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.pictureBox1);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(231, 232);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Primary Image";
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Location = new System.Drawing.Point(3, 17);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(225, 212);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 1;
		this.pictureBox1.TabStop = false;
		this.groupBox3.Controls.Add(this.pictureBox2);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(233, 232);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Secondary Image";
		this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox2.Location = new System.Drawing.Point(3, 17);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(227, 212);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox2.TabIndex = 2;
		this.pictureBox2.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(473, 292);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.splitContainer1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
		base.Name = "Widget_image";
		this.Text = "Widget";
		base.Load += new System.EventHandler(Widget_image_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		this.splitContainer1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.groupBox3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		base.ResumeLayout(false);
	}
}
