using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IRA_Sampler.Properties;
using Microsoft.Office.Interop.Outlook;

namespace IRA_Sampler;

public class About_Form : Form
{
	private IContainer components = null;

	private GroupBox groupBox1;

	private Label label2;

	private Label label1;

	private GroupBox groupBox2;

	private Label label3;

	private Label label4;

	private Button button1;

	public About_Form()
	{
		InitializeComponent();
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			Process.Start("http://www.gnu.org/licenses/lgpl-3.0.en.html");
		}
		catch
		{
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			Process[] processesByName = Process.GetProcessesByName("OUTLOOK");
			Microsoft.Office.Interop.Outlook.Application application = (Microsoft.Office.Interop.Outlook.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0006F03A-0000-0000-C000-000000000046")));
			if (processesByName.Length != 0)
			{
				application = Marshal.GetActiveObject("Outlook.Application") as Microsoft.Office.Interop.Outlook.Application;
			}
			MailItem mailItem = (dynamic)application.CreateItem(OlItemType.olMailItem);
			mailItem.Recipients.Add("karan.piprani@here.com");
			mailItem.Recipients.Add("gaurav.gawade@here.com");
			mailItem.Recipients.Add("nishant.joshi@here.com");
			mailItem.Subject = "IRA Sampler tool feedback";
			mailItem.Body = "Thank you!";
			mailItem.Importance = OlImportance.olImportanceHigh;
			mailItem.Display(false);
			if (mailItem.Recipients.ResolveAll())
			{
				mailItem.Send();
				MessageBox.Show("Thank you for your feedback :)");
			}
		}
		catch
		{
			MessageBox.Show("feedback sending failed . .");
		}
	}

	private void About_Load(object sender, EventArgs e)
	{
		if (Settings.Default.thanks)
		{
			button1.Enabled = false;
		}
		else
		{
			button1.Enabled = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRA_Sampler.About_Form));
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Location = new System.Drawing.Point(12, 39);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(200, 116);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Credits";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(8, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(186, 91);
		this.label2.TabIndex = 0;
		this.label2.Text = "> Concept - Nishant Joshi.\r\n> Developer - Karan Piprani.\r\n> Coordination - Gaurav Gawade.\r\n\r\n\r\n> Others - Jugal Nathani for assisting in\r\nimproving pots detection algorithm.";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(123, 27);
		this.label1.TabIndex = 0;
		this.label1.Text = "IRA Sampler";
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Location = new System.Drawing.Point(12, 161);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(200, 73);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Resources";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(10, 27);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(180, 26);
		this.label3.TabIndex = 0;
		this.label3.Text = "Icons are used and distributed under\r\nFree for commercial use license.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(9, 245);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(132, 13);
		this.label4.TabIndex = 2;
		this.label4.Text = "Liked our work ? Thank Us !";
		this.button1.Location = new System.Drawing.Point(147, 240);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(65, 23);
		this.button1.TabIndex = 3;
		this.button1.Text = "Thanks :)";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(224, 267);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "About";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "About";
		base.Load += new System.EventHandler(About_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
