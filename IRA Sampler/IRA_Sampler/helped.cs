using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IRA_Sampler;

public class helped : Form
{
	private IContainer components = null;

	private GroupBox groupBox1;

	private Label label1;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label9;

	public helped()
	{
		InitializeComponent();
	}

	private void label9_Click(object sender, EventArgs e)
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Location = new System.Drawing.Point(6, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(334, 164);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Hot Keys";
		this.label9.AutoSize = true;
		this.label9.ForeColor = System.Drawing.SystemColors.Highlight;
		this.label9.Location = new System.Drawing.Point(27, 124);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(279, 28);
		this.label9.TabIndex = 8;
		this.label9.Text = "- \"Y\" and \"N\" Key is used in Samples Table in Row \r\n\"Errors\" for giving Yes/No in that particular Image";
		this.label9.Click += new System.EventHandler(label9_Click);
		this.label8.AutoSize = true;
		this.label8.ForeColor = System.Drawing.SystemColors.Highlight;
		this.label8.Location = new System.Drawing.Point(146, 92);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 14);
		this.label8.TabIndex = 7;
		this.label8.Text = "Finish Task";
		this.label7.AutoSize = true;
		this.label7.ForeColor = System.Drawing.SystemColors.Highlight;
		this.label7.Location = new System.Drawing.Point(146, 69);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(64, 14);
		this.label7.TabIndex = 6;
		this.label7.Text = "Clear Data";
		this.label6.AutoSize = true;
		this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
		this.label6.Location = new System.Drawing.Point(146, 45);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(182, 14);
		this.label6.TabIndex = 5;
		this.label6.Text = "Pick Files from Specified Folders";
		this.label5.AutoSize = true;
		this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
		this.label5.Location = new System.Drawing.Point(146, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(105, 14);
		this.label5.TabIndex = 4;
		this.label5.Text = "Save Current State";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 92);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 14);
		this.label4.TabIndex = 3;
		this.label4.Text = "CTRL + F : ";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 69);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(71, 14);
		this.label3.TabIndex = 2;
		this.label3.Text = "CTRL + DEL : ";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 14);
		this.label2.TabIndex = 1;
		this.label2.Text = "CTRL + P :";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(99, 14);
		this.label1.TabIndex = 0;
		this.label1.Text = "CTRL + SHIFT + S : ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		base.ClientSize = new System.Drawing.Size(346, 173);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "helped";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Help";
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}
