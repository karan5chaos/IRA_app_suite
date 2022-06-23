using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using IRA_Sampler.Properties;

namespace IRA_Sampler;

public class Docked_Form : Form
{
	private Main_Form f1 = null;

	public int ImageWidth;

	public int ImageHeight;

	public float ImageScale = 1f;

	private IContainer components = null;

	public GroupBox groupBox1;

	public ImageBox pictureBox1;

	public Docked_Form()
	{
		InitializeComponent();
	}

	private void groupBox3_Enter(object sender, EventArgs e)
	{
	}

	private void usr_stats_FormClosing(object sender, FormClosingEventArgs e)
	{
		Settings.Default.allowpopup = false;
		Settings.Default.Save();
		Settings.Default.Reload();
	}

	private void usr_stats_Load(object sender, EventArgs e)
	{
		base.MouseWheel += picImage1_MouseWheel;
		f1 = new Main_Form();
	}

	private void picImage1_MouseWheel(object sender, MouseEventArgs e)
	{
		ImageScale += (float)e.Delta * 0.000833333354f;
		if (ImageScale < 0f)
		{
			ImageScale = 0f;
		}
		((Control)(object)pictureBox1).Size = new Size((int)((float)ImageWidth * ImageScale), (int)((float)ImageHeight * ImageScale));
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
	}

	private void pictureBox1_MouseEnter(object sender, EventArgs e)
	{
		if (!((Control)(object)pictureBox1).Focused)
		{
			((Control)(object)pictureBox1).Focus();
		}
	}

	private void pictureBox1_MouseHover(object sender, EventArgs e)
	{
		((Control)(object)pictureBox1).Focus();
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
	}

	public void SetGamma(double red, double green, double blue)
	{
		Bitmap bitmap = (Bitmap)(object)pictureBox1.get_Image();
		Bitmap bitmap2 = (Bitmap)bitmap.Clone();
		byte[] array = CreateGammaArray(red);
		byte[] array2 = CreateGammaArray(green);
		byte[] array3 = CreateGammaArray(blue);
		for (int i = 0; i < bitmap2.Width; i++)
		{
			for (int j = 0; j < bitmap2.Height; j++)
			{
				Color pixel = bitmap2.GetPixel(i, j);
				bitmap2.SetPixel(i, j, Color.FromArgb(array[pixel.R], array2[pixel.G], array3[pixel.B]));
			}
		}
		pictureBox1.set_Image((Image)(object)(Bitmap)bitmap2.Clone());
	}

	private byte[] CreateGammaArray(double color)
	{
		byte[] array = new byte[256];
		for (int i = 0; i < 256; i++)
		{
			array[i] = (byte)Math.Min(255, (int)(255.0 * Math.Pow((double)i / 255.0, 1.0 / color) + 0.5));
		}
		return array;
	}

	private void checkBox2_CheckedChanged(object sender, EventArgs e)
	{
	}

	public void SetContrast(double contrast)
	{
		Bitmap bitmap = (Bitmap)(object)pictureBox1.get_Image();
		Bitmap bitmap2 = (Bitmap)bitmap.Clone();
		if (contrast < -100.0)
		{
			contrast = -100.0;
		}
		if (contrast > 100.0)
		{
			contrast = 100.0;
		}
		contrast = (100.0 + contrast) / 100.0;
		contrast *= contrast;
		for (int i = 0; i < bitmap2.Width; i++)
		{
			for (int j = 0; j < bitmap2.Height; j++)
			{
				Color pixel = bitmap2.GetPixel(i, j);
				double num = (double)(int)pixel.R / 255.0;
				num -= 0.5;
				num *= contrast;
				num += 0.5;
				num *= 255.0;
				if (num < 0.0)
				{
					num = 0.0;
				}
				if (num > 255.0)
				{
					num = 255.0;
				}
				double num2 = (double)(int)pixel.G / 255.0;
				num2 -= 0.5;
				num2 *= contrast;
				num2 += 0.5;
				num2 *= 255.0;
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				if (num2 > 255.0)
				{
					num2 = 255.0;
				}
				double num3 = (double)(int)pixel.B / 255.0;
				num3 -= 0.5;
				num3 *= contrast;
				num3 += 0.5;
				num3 *= 255.0;
				if (num3 < 0.0)
				{
					num3 = 0.0;
				}
				if (num3 > 255.0)
				{
					num3 = 255.0;
				}
				bitmap2.SetPixel(i, j, Color.FromArgb((byte)num, (byte)num2, (byte)num3));
			}
		}
		pictureBox1.set_Image((Image)(object)(Bitmap)bitmap2.Clone());
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.pictureBox1 = new ImageBox();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.AutoSize = true;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.pictureBox1);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(649, 505);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Image";
		this.pictureBox1.set_AllowUnfocusedMouseWheel(true);
		((ScrollControl)this.pictureBox1).set_BorderStyle(System.Windows.Forms.BorderStyle.FixedSingle);
		((System.Windows.Forms.Control)(object)this.pictureBox1).Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.set_GridColor((System.Drawing.Color)System.Drawing.SystemColors.ButtonHighlight);
		this.pictureBox1.set_GridColorAlternate((System.Drawing.Color)System.Drawing.SystemColors.Control);
		((System.Windows.Forms.Control)(object)this.pictureBox1).Location = new System.Drawing.Point(3, 17);
		((System.Windows.Forms.Control)(object)this.pictureBox1).Name = "pictureBox1";
		((System.Windows.Forms.Control)(object)this.pictureBox1).Size = new System.Drawing.Size(643, 485);
		((System.Windows.Forms.Control)(object)this.pictureBox1).TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(649, 505);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "Docked_Form";
		base.ShowIcon = false;
		this.Text = "Image";
		base.TopMost = true;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(usr_stats_FormClosing);
		base.Load += new System.EventHandler(usr_stats_Load);
		this.groupBox1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
