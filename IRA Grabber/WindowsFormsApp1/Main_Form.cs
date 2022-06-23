using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Math.Geometry;
using Microsoft.Office.Interop.Outlook;
using Resolve.HotKeys;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1;

public class Main_Form : Form
{
	private const uint MOUSEEVENTF_WHEEL = 2048u;

	private HotKey xHotKey;

	private HotKey iraHotKey;

	private string temp_file = "";

	private bool hasimg = false;

	private DataTable ira = null;

	private DataTable lidar = null;

	private DataTable ann = null;

	private List<string> imags = new List<string>();

	private int cont = 0;

	private bool mouseDown;

	private Point lastLocation;

	private IContainer components = null;

	private Button button1;

	private System.Windows.Forms.Timer timer1;

	private Label label1;

	private Label label3;

	private Label label2;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private NotifyIcon notifyIcon1;

	private Panel panel2;

	private Button button3;

	private Button button2;

	private Label label9;

	private Button button5;

	private Button button4;

	private ToolTip toolTip1;

	private NotifyIcon notifyIcon2;

	public Main_Form()
	{
		InitializeComponent();
	}

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

	private details GetActiveWindowTitle()
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		IntPtr foregroundWindow = GetForegroundWindow();
		Screen primary_screen = Screen.FromHandle(foregroundWindow);
		if (GetWindowText(foregroundWindow, stringBuilder, 256) > 0)
		{
			details details2 = new details();
			details2.handler_name = stringBuilder.ToString();
			details2.primary_screen = primary_screen;
			return details2;
		}
		return null;
	}

	public static Bitmap CombineBitmap(List<string> files)
	{
		List<Bitmap> list = new List<Bitmap>();
		Bitmap bitmap = null;
		try
		{
			int num = 0;
			int num2 = 0;
			foreach (string file in files)
			{
				Bitmap bitmap2 = new Bitmap(file);
				if (bitmap2.Width > num)
				{
					num = bitmap2.Width;
				}
				num2 += bitmap2.Height;
				list.Add(bitmap2);
			}
			bitmap = new Bitmap(num, num2);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.Black);
				int num3 = 0;
				foreach (Bitmap item in list)
				{
					graphics.DrawImage(item, new Rectangle(0, num3, item.Width, item.Height));
					num3 += item.Height;
				}
			}
			return bitmap;
		}
		catch (Exception)
		{
			bitmap?.Dispose();
			throw;
		}
		finally
		{
			foreach (Bitmap item2 in list)
			{
				item2.Dispose();
			}
		}
	}

	private void Auto_mode()
	{
		string handler_name = GetActiveWindowTitle().handler_name;
		if (handler_name.Contains("Image") && handler_name.Contains("Review"))
		{
			IRA_mode();
			Settings.Default.mode = "IRA";
		}
		else if (handler_name.Contains("LIDAR") && handler_name.Contains("REVIEWER"))
		{
			LIDAR_mode();
			Settings.Default.mode = "LIDAR";
		}
		else if (handler_name.Contains("2D") && handler_name.Contains("Annotation"))
		{
			twoD_mode();
			Settings.Default.mode = "2D";
		}
		Label label = label7;
		label.Text = label.Text + " (" + Settings.Default.mode + ")";
	}

	private void IRA_mode()
	{
		string handler_name = GetActiveWindowTitle().handler_name;
		Screen primary_screen = GetActiveWindowTitle().primary_screen;
		if (!handler_name.Contains("Image") || !handler_name.Contains("Review"))
		{
			return;
		}
		temp_file = Settings.Default.setpath + "/" + Settings.Default.mode + "/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName + "/task_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".png";
		using (Bitmap bitmap = new Bitmap(primary_screen.Bounds.Width, primary_screen.Bounds.Height, PixelFormat.Format32bppArgb))
		{
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.CopyFromScreen(primary_screen.Bounds.X, primary_screen.Bounds.Y, 0, 0, primary_screen.Bounds.Size, CopyPixelOperation.SourceCopy);
			}
			detect_Squares(bitmap).Save(temp_file, ImageFormat.Png);
			ira.Rows.Add(Path.GetFileName(temp_file), DateTime.Now.Hour);
		}
		throw_Notification();
	}

	private void throw_Notification()
	{
		if (Settings.Default.notif_style == 1)
		{
			FlashWindow.Flash(this, 1u);
		}
		notifyIcon1.ShowBalloonTip(1000);
	}

	private void LIDAR_mode()
	{
		string handler_name = GetActiveWindowTitle().handler_name;
		Screen primary_screen = GetActiveWindowTitle().primary_screen;
		if (!handler_name.Contains("LIDAR") || !handler_name.Contains("REVIEWER"))
		{
			return;
		}
		temp_file = Settings.Default.setpath + "/" + Settings.Default.mode + "/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName + "/task_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".png";
		using Bitmap bitmap = new Bitmap(3840, 1080, PixelFormat.Format32bppArgb);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.CopyFromScreen(primary_screen.Bounds.X, primary_screen.Bounds.Y, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
		}
		bitmap.Save(temp_file, ImageFormat.Png);
		lidar.Rows.Add(Path.GetFileName(temp_file), DateTime.Now.Hour);
		throw_Notification();
	}

	private void twoD_mode()
	{
		try
		{
			string handler_name = GetActiveWindowTitle().handler_name;
			Screen primary_screen = GetActiveWindowTitle().primary_screen;
			if (!handler_name.Contains("2D") || !handler_name.Contains("Annotation"))
			{
				return;
			}
			if (!hasimg)
			{
				string tempFileName = Path.GetTempFileName();
				using (Bitmap bitmap = new Bitmap(primary_screen.WorkingArea.Width, primary_screen.WorkingArea.Height, PixelFormat.Format32bppArgb))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.CopyFromScreen(primary_screen.WorkingArea.X, primary_screen.WorkingArea.Y, 0, 0, primary_screen.WorkingArea.Size, CopyPixelOperation.SourceCopy);
					}
					bitmap.Save(tempFileName);
					imags.Add(tempFileName);
				}
				hasimg = true;
				notifyIcon2.ShowBalloonTip(1000);
				return;
			}
			temp_file = Settings.Default.setpath + "/" + Settings.Default.mode + "/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName + "/task_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".png";
			Cursor.Position = new Point(primary_screen.WorkingArea.Width / 2, primary_screen.WorkingArea.Height / 2);
			for (int i = 0; i < 6; i++)
			{
				mouse_event(2048u, 0, 0, -1200, 0);
			}
			Thread.Sleep(500);
			for (int j = 0; j < 3; j++)
			{
				string tempFileName = Path.GetTempFileName();
				mouse_event(2048u, 0, 0, 120, 0);
				using Bitmap bitmap = new Bitmap(primary_screen.WorkingArea.Width, primary_screen.WorkingArea.Height, PixelFormat.Format32bppArgb);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.CopyFromScreen(primary_screen.WorkingArea.X, primary_screen.WorkingArea.Y, 0, 0, primary_screen.WorkingArea.Size, CopyPixelOperation.SourceCopy);
				}
				bitmap.Save(tempFileName);
				imags.Add(tempFileName);
			}
			Bitmap bitmap2 = CombineBitmap(imags);
			bitmap2.Save(temp_file, ImageFormat.Png);
			bitmap2.Dispose();
			ann.Rows.Add(Path.GetFileName(temp_file), DateTime.Now.Hour);
			throw_Notification();
			hasimg = false;
			imags.Clear();
		}
		catch
		{
		}
		finally
		{
		}
	}

	public Bitmap detect_Squares(Bitmap img)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		try
		{
			img.ChangeColour(193, 193, 193, 0, 0, 0);
			img.ChangeColour(241, 241, 241, 0, 0, 0);
			img.ChangeColour(44, 57, 71, 0, 0, 0);
			img.ChangeColour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 0, 0);
			BlobCounter val = new BlobCounter();
			((BlobCounterBase)val).set_FilterBlobs(true);
			((BlobCounterBase)val).set_MinHeight(153);
			((BlobCounterBase)val).set_MinWidth(153);
			((BlobCounterBase)val).set_MaxHeight(190);
			((BlobCounterBase)val).set_MaxWidth(190);
			((BlobCounterBase)val).ProcessImage((Bitmap)(object)img);
			Blob[] objectsInformation = ((BlobCounterBase)val).GetObjectsInformation();
			SimpleShapeChecker val2 = new SimpleShapeChecker();
			Blob[] array = objectsInformation;
			foreach (Blob val3 in array)
			{
				cont++;
			}
		}
		catch
		{
		}
		finally
		{
		}
		return img;
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			xHotKey = new HotKey(Keys.Insert);
			iraHotKey = new HotKey(Keys.S, (ModifierKey)3);
			xHotKey.add_Pressed((EventHandler)(object)new EventHandler(xHotKey_Pressed));
			iraHotKey.add_Pressed((EventHandler)(object)new EventHandler(iraHotKey_Pressed));
			xHotKey.Register();
			iraHotKey.Register();
		}
		catch
		{
		}
		ira = new DataTable();
		ira.Columns.Add("filename");
		ira.Columns.Add("time");
		lidar = new DataTable();
		lidar.Columns.Add("filename");
		lidar.Columns.Add("time");
		ann = new DataTable();
		ann.Columns.Add("filename");
		ann.Columns.Add("time");
		timer1.Start();
		if (!Directory.Exists(Settings.Default.setpath + "/IRA/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName))
		{
			Directory.CreateDirectory(Settings.Default.setpath + "/IRA/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName);
		}
		if (!Directory.Exists(Settings.Default.setpath + "/LIDAR/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName))
		{
			Directory.CreateDirectory(Settings.Default.setpath + "/LIDAR/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName);
		}
		if (!Directory.Exists(Settings.Default.setpath + "/2D/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName))
		{
			Directory.CreateDirectory(Settings.Default.setpath + "/2D/" + DateTime.Today.ToLongDateString() + "/" + Environment.UserName);
		}
		label7.Text = Settings.Default.mode;
	}

	private void iraHotKey_Pressed(object sender, EventArgs e)
	{
		switch (Settings.Default.mode)
		{
		case "IRA":
			IRA_mode();
			break;
		case "LIDAR":
			LIDAR_mode();
			break;
		}
	}

	private void xHotKey_Pressed(object sender, EventArgs e)
	{
		twoD_mode();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		label4.Text = cont.ToString();
		if (Settings.Default.mode == "LIDAR" || Settings.Default.mode == "2D")
		{
			label4.Visible = false;
			label5.Visible = false;
		}
		else
		{
			label4.Visible = true;
			label5.Visible = true;
		}
		try
		{
			string handler_name = GetActiveWindowTitle().handler_name;
			if (Settings.Default.Auto_mode)
			{
				if (handler_name.Contains("Image") && handler_name.Contains("Review"))
				{
					Settings.Default.mode = "IRA";
					label3.Text = ira.Rows.Count.ToString();
				}
				else if (handler_name.Contains("LIDAR") && handler_name.Contains("REVIEWER"))
				{
					Settings.Default.mode = "LIDAR";
					label3.Text = lidar.Rows.Count.ToString();
				}
				else if (handler_name.Contains("2D") && handler_name.Contains("Annotation"))
				{
					Settings.Default.mode = "2D";
					label3.Text = ann.Rows.Count.ToString();
				}
				else
				{
					Settings.Default.mode = "Auto";
					int num = ira.Rows.Count + lidar.Rows.Count + ann.Rows.Count;
					label3.Text = num.ToString();
				}
			}
			label7.Text = Settings.Default.mode;
		}
		catch
		{
		}
		finally
		{
		}
	}

	[DllImport("user32.dll")]
	private static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Settings_Form settings_Form = new Settings_Form();
		settings_Form.ShowDialog(this);
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (ira.Rows.Count <= 0 && lidar.Rows.Count <= 0 && ann.Rows.Count <= 0)
		{
			return;
		}
		try
		{
			Microsoft.Office.Interop.Outlook.Application application = (Microsoft.Office.Interop.Outlook.Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0006F03A-0000-0000-C000-000000000046")));
			AddressEntry addressEntry = application.Session.CurrentUser.AddressEntry;
			ExchangeUser exchangeUserManager = addressEntry.GetExchangeUser().GetExchangeUserManager();
			string text = Settings.Default.log_path + "/" + exchangeUserManager.Name + "/IRA";
			string text2 = Settings.Default.log_path + "/" + exchangeUserManager.Name + "/LIDAR";
			string text3 = Settings.Default.log_path + "/" + exchangeUserManager.Name + "/2D";
			if (addressEntry.Type == "EX" && exchangeUserManager != null)
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
				}
			}
			string text4 = "";
			string text5 = "";
			string text6 = "";
			if (ira.Rows.Count > 0)
			{
				if (!Directory.Exists(text + "/.logs"))
				{
					Directory.CreateDirectory(text + "/.logs");
				}
				foreach (DataRow row in ira.Rows)
				{
					object obj = text4;
					text4 = string.Concat(obj, row[0], "-", row[1], ",", Environment.NewLine);
				}
				File.AppendAllText(text + "/.logs/" + Environment.UserName + "_" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + "_t-" + label3.Text + "_p-" + label4.Text + ".log", text4);
			}
			if (lidar.Rows.Count > 0)
			{
				if (!Directory.Exists(text2 + "/.logs"))
				{
					Directory.CreateDirectory(text2 + "/.logs");
				}
				foreach (DataRow row2 in lidar.Rows)
				{
					object obj = text5;
					text5 = string.Concat(obj, row2[0], "-", row2[1], ",", Environment.NewLine);
				}
				File.AppendAllText(text2 + "/.logs/" + Environment.UserName + "_" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + "_t-" + label3.Text + ".log", text5);
			}
			if (ann.Rows.Count <= 0)
			{
				return;
			}
			if (!Directory.Exists(string.Concat(ann, "/.logs")))
			{
				Directory.CreateDirectory(string.Concat(ann, "/.logs"));
			}
			foreach (DataRow row3 in ann.Rows)
			{
				object obj = text6;
				text6 = string.Concat(obj, row3[0], "-", row3[1], ",", Environment.NewLine);
			}
			File.AppendAllText(text3 + "/.logs/" + Environment.UserName + "_" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + "_t-" + label3.Text + ".log", text6);
		}
		catch (Exception)
		{
			MessageBox.Show("Error Occured..\nError Code : Err-01", "Err-01", MessageBoxButtons.OK, MessageBoxIcon.None);
		}
	}

	private void button2_Click_1(object sender, EventArgs e)
	{
		Close();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void panel2_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		lastLocation = e.Location;
	}

	private void panel2_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown)
		{
			base.Location = new Point(base.Location.X - lastLocation.X + e.X, base.Location.Y - lastLocation.Y + e.Y);
			Update();
		}
	}

	private void panel2_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	private void button3_MouseEnter(object sender, EventArgs e)
	{
		button3.BackColor = Color.LightSkyBlue;
	}

	private void button3_MouseLeave(object sender, EventArgs e)
	{
		button3.BackColor = panel2.BackColor;
	}

	private void button2_MouseEnter(object sender, EventArgs e)
	{
		button2.BackColor = Color.DarkRed;
	}

	private void button2_MouseLeave(object sender, EventArgs e)
	{
		button2.BackColor = panel2.BackColor;
	}

	private void button5_MouseEnter(object sender, EventArgs e)
	{
		button5.BackColor = Color.Gainsboro;
	}

	private void button4_MouseEnter(object sender, EventArgs e)
	{
		button4.BackColor = Color.SlateBlue;
	}

	private void button5_Click(object sender, EventArgs e)
	{
		Settings_Form settings_Form = new Settings_Form();
		settings_Form.ShowDialog(this);
	}

	private void button4_Click(object sender, EventArgs e)
	{
		About_Form about_Form = new About_Form();
		about_Form.ShowDialog(this);
	}

	private void button5_MouseLeave(object sender, EventArgs e)
	{
		button5.BackColor = panel2.BackColor;
	}

	private void button4_MouseLeave(object sender, EventArgs e)
	{
		button4.BackColor = panel2.BackColor;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsFormsApp1.Main_Form));
		this.button1 = new System.Windows.Forms.Button();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
		this.panel2 = new System.Windows.Forms.Panel();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.button4 = new System.Windows.Forms.Button();
		this.button5 = new System.Windows.Forms.Button();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.notifyIcon2 = new System.Windows.Forms.NotifyIcon(this.components);
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(175, 96);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(19, 19);
		this.button1.TabIndex = 4;
		this.button1.UseVisualStyleBackColor = true;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(5, 99);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 13);
		this.label1.TabIndex = 5;
		this.label1.Text = "Service Status :";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(5, 55);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(40, 13);
		this.label2.TabIndex = 6;
		this.label2.Text = "Count :";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(80, 55);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(10, 13);
		this.label3.TabIndex = 7;
		this.label3.Text = "-";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(80, 77);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(10, 13);
		this.label4.TabIndex = 9;
		this.label4.Text = "-";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(5, 77);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(33, 13);
		this.label5.TabIndex = 8;
		this.label5.Text = "Pots :";
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label6.Location = new System.Drawing.Point(1, 94);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(193, 2);
		this.label6.TabIndex = 10;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(63, 34);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(10, 13);
		this.label7.TabIndex = 12;
		this.label7.Text = "-";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 34);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(38, 13);
		this.label8.TabIndex = 11;
		this.label8.Text = "Mode :";
		this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.notifyIcon1.BalloonTipText = "Image Captured";
		this.notifyIcon1.BalloonTipTitle = "Capture";
		this.notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
		this.notifyIcon1.Text = "Capture";
		this.notifyIcon1.Visible = true;
		this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
		this.panel2.Controls.Add(this.button5);
		this.panel2.Controls.Add(this.button4);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.button3);
		this.panel2.Controls.Add(this.button2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(195, 29);
		this.panel2.TabIndex = 14;
		this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(panel2_MouseDown);
		this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(panel2_MouseMove);
		this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(panel2_MouseUp);
		this.button2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.button2.FlatAppearance.BorderSize = 0;
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button2.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button2.Image = (System.Drawing.Image)resources.GetObject("button2.Image");
		this.button2.Location = new System.Drawing.Point(162, 0);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(33, 29);
		this.button2.TabIndex = 0;
		this.button2.TabStop = false;
		this.toolTip1.SetToolTip(this.button2, "Close");
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click_1);
		this.button2.MouseEnter += new System.EventHandler(button2_MouseEnter);
		this.button2.MouseLeave += new System.EventHandler(button2_MouseLeave);
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.button3.FlatAppearance.BorderSize = 0;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(130, 0);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(33, 29);
		this.button3.TabIndex = 1;
		this.button3.TabStop = false;
		this.toolTip1.SetToolTip(this.button3, "Minimize");
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.button3.MouseEnter += new System.EventHandler(button3_MouseEnter);
		this.button3.MouseLeave += new System.EventHandler(button3_MouseLeave);
		this.label9.AutoSize = true;
		this.label9.Enabled = false;
		this.label9.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 8);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(44, 13);
		this.label9.TabIndex = 2;
		this.label9.Text = "Capture";
		this.button4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button4.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.button4.FlatAppearance.BorderSize = 0;
		this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button4.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button4.Image = (System.Drawing.Image)resources.GetObject("button4.Image");
		this.button4.Location = new System.Drawing.Point(98, 0);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(33, 29);
		this.button4.TabIndex = 3;
		this.button4.TabStop = false;
		this.toolTip1.SetToolTip(this.button4, "About");
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.button4.MouseEnter += new System.EventHandler(button4_MouseEnter);
		this.button4.MouseLeave += new System.EventHandler(button4_MouseLeave);
		this.button5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button5.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.button5.FlatAppearance.BorderSize = 0;
		this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button5.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button5.Image = (System.Drawing.Image)resources.GetObject("button5.Image");
		this.button5.Location = new System.Drawing.Point(66, 0);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(33, 29);
		this.button5.TabIndex = 4;
		this.button5.TabStop = false;
		this.toolTip1.SetToolTip(this.button5, "Settings");
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		this.button5.MouseEnter += new System.EventHandler(button5_MouseEnter);
		this.button5.MouseLeave += new System.EventHandler(button5_MouseLeave);
		this.notifyIcon2.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.notifyIcon2.BalloonTipText = "Primary Image Identified.\r\nClick Insert to capture before finishing the current task.";
		this.notifyIcon2.BalloonTipTitle = "Capture Identified";
		this.notifyIcon2.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon2.Icon");
		this.notifyIcon2.Text = "notifyIcon2";
		this.notifyIcon2.Visible = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(195, 116);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.Name = "Main_Form";
		this.Text = "Capture";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
