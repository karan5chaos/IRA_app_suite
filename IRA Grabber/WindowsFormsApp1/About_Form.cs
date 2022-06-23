using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1;

public class About_Form : Form
{
	private IContainer components = null;

	private Label label1;

	private GroupBox groupBox1;

	private Label label2;

	private Label label3;

	public About_Form()
	{
		InitializeComponent();
		label1.Text = Application.ProductName;
	}

	private void About_Load(object sender, EventArgs e)
	{
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			Process.Start("https://github.com/gmamaladze/globalmousekeyhook");
		}
		catch
		{
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
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(50, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "label1";
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 31);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(251, 96);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Credits";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(11, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(228, 65);
		this.label2.TabIndex = 0;
		this.label2.Text = "Concept, Ideation- Nishant Joshi\r\n\r\nDevelopment, Low-Level Testing - Karan Piprani\r\n\r\nUAT, Feedback - Gaurav Gawade";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 136);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(245, 13);
		this.label3.TabIndex = 2;
		this.label3.Text = "Icons are used under free for comercial use licence";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(262, 159);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "About_Form";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "About";
		base.Load += new System.EventHandler(About_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
