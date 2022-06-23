using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;
using ClosedXML.Excel;
using Cyotek.Windows.Forms;
using IRA_Sampler.Properties;

namespace IRA_Sampler;

public class Main_Form : Form
{
	private delegate void SetTextCallback(string range, string count, string user);

	private class user_vals
	{
		public int pots { get; set; }

		public int total_errors { get; set; }

		public double score { get; set; }
	}

	private List<string> images = new List<string>();

	private int err_count = 0;

	private Settings sets = Settings.Default;

	private List<DataGridViewRow> rows = new List<DataGridViewRow>();

	private bool ischanging;

	private string FileName;

	public Docked_Form usrd = null;

	private IContainer components = null;

	private StatusStrip statusStrip1;

	private BackgroundWorker cal_per;

	private BackgroundWorker load_samples;

	private ToolStripStatusLabel toolStripStatusLabel1;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem fileToolStripMenuItem;

	private ToolStripMenuItem saveCurrentToolStripMenuItem;

	private ToolStripMenuItem samplesToolStripMenuItem;

	private ToolStripMenuItem pickToolStripMenuItem;

	private ToolStripMenuItem clearToolStripMenuItem;

	private ToolStripMenuItem settingsToolStripMenuItem;

	private ToolStripMenuItem generalToolStripMenuItem;

	private ToolStripMenuItem aboutToolStripMenuItem;

	private Label label6;

	private Label label3;

	private Label label7;

	private Label label2;

	private NumericUpDown numericUpDown1;

	private Label label1;

	private Label label8;

	private Label label5;

	private Label label4;

	private Label label12;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem addCommentsToolStripMenuItem;

	private ToolStripTextBox toolStripTextBox1;

	private Label label15;

	private TextBox textBox1;

	private Label label14;

	private ToolStripMenuItem finishToolStripMenuItem;

	private SplitContainer splitContainer1;

	private SplitContainer splitContainer2;

	private GroupBox groupBox3;

	private DataGridView dataGridView3;

	public GroupBox groupBox4;

	public DataGridView dataGridView2;

	private Timer timer2;

	public SplitContainer splitContainer3;

	private ToolStripStatusLabel toolStripStatusLabel2;

	private ToolStripProgressBar toolStripProgressBar1;

	private DataGridViewTextBoxColumn i;

	private DataGridViewTextBoxColumn err;

	private ToolStripMenuItem autoDetectToolStripMenuItem;

	private Label label9;

	public GroupBox groupBox2;

	public GroupBox groupBox1;

	private PictureBox pictureBox5;

	public DataGridView dataGridView1;

	private Timer timer1;

	private ToolStripMenuItem helpToolStripMenuItem;

	private Label label10;

	private Label label11;

	private Label label13;

	private DataGridViewTextBoxColumn iname;

	private DataGridViewTextBoxColumn img_path;

	private DataGridViewTextBoxColumn usr;

	private DataGridViewComboBoxColumn err_found;

	private DataGridViewTextBoxColumn p_count;

	private DataGridViewTextBoxColumn t_errors;

	private DataGridViewTextBoxColumn q_score;

	private DataGridViewTextBoxColumn comms;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private ImageBox pictureBox1;

	public Main_Form()
	{
		InitializeComponent();
	}

	private void dataGridView1_SelectionChanged(object sender, EventArgs e)
	{
		if (dataGridView1.SelectedRows.Count <= 0)
		{
			return;
		}
		try
		{
			Image image = Image.FromFile(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
			if (Settings.Default.allowpopup)
			{
				usrd.pictureBox1.set_Image((Image)(object)image);
			}
			else
			{
				pictureBox1.set_Image((Image)(object)image);
			}
			if (sets.enable_ImageDetection && (dataGridView1.SelectedRows[0].Cells[4].Value == "" || dataGridView1.SelectedRows[0].Cells[4].Value == null) && Settings.Default.process_ir)
			{
				detect_Squares();
			}
		}
		catch (Exception exc)
		{
			show_err(exc);
		}
	}

	private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.C)
		{
			contextMenuStrip1.Show(dataGridView1, new Point(0, 0));
		}
	}

	private void af_FieldUpdate(object sender, ValueEventArgs e)
	{
		try
		{
			dataGridView1.BeginEdit(selectAll: true);
			if (e.Someth_property != null)
			{
				ischanging = true;
				int num = 0;
				dataGridView1.SelectedRows[0].Cells[7].Value = null;
				string text = "";
				foreach (KeyValuePair<string, int> item in e.Someth_property)
				{
					foreach (DataGridViewColumn column in dataGridView1.Columns)
					{
						if (column.HeaderText == item.Key)
						{
							dataGridView1.SelectedRows[0].Cells[column.Index].Value = item.Value;
						}
					}
					num += item.Value;
					object obj = text;
					text = string.Concat(obj, item.Key, " - ", item.Value, "    ");
				}
				dataGridView1.SelectedRows[0].Cells[4].Value = e.pots_property;
				dataGridView1.SelectedRows[0].Cells[5].Value = num;
				dataGridView1.SelectedRows[0].Cells[6].Value = e.percent_property;
				dataGridView1.SelectedRows[0].Cells[7].Value = text;
			}
			else
			{
				dataGridView1.SelectedRows[0].Cells[3].Value = "";
			}
		}
		catch
		{
		}
		finally
		{
			dataGridView1.ClearSelection();
			ischanging = false;
			user_Status();
			dataGridView1.EndEdit();
		}
	}

	private void calc_per()
	{
		try
		{
			double num = dataGridView1.Rows.Count - err_count;
			double num2 = num / (double)dataGridView1.Rows.Count;
			double value = num2 * 100.0;
			label4.Text = Math.Round(value, 0).ToString();
		}
		catch (Exception exc)
		{
			show_err(exc);
		}
	}

	private void Setrange(string range, string count, string user)
	{
		try
		{
			if (dataGridView1.InvokeRequired)
			{
				SetTextCallback method = Setrange;
				Invoke(method, range, count, user);
				return;
			}
			label2.Text = count;
			if (Settings.Default.process_ir)
			{
				label11.Text = "IR/LR";
			}
			else if (Settings.Default.process_2d)
			{
				label11.Text = "2D Annotation";
			}
			string name = new DirectoryInfo(user).Name;
			DataGridViewRow dataGridViewRow = new DataGridViewRow();
			dataGridViewRow.CreateCells(dataGridView1);
			dataGridViewRow.Cells[0].Value = Path.GetFileNameWithoutExtension(range);
			dataGridViewRow.Cells[1].Value = range.ToString();
			dataGridViewRow.Cells[2].Value = name.ToString();
			rows.Add(dataGridViewRow);
		}
		catch
		{
		}
		finally
		{
		}
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		if (Settings.Default.sh_err_comments)
		{
			dataGridView1.Columns[comms.Index].Visible = true;
		}
		else
		{
			dataGridView1.Columns[comms.Index].Visible = false;
		}
		if (Settings.Default.process_ir)
		{
			StringEnumerator enumerator = Settings.Default.err_list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();
					DataGridViewCell dataGridViewCell2 = (dataGridViewColumn.CellTemplate = new DataGridViewTextBoxCell());
					dataGridViewColumn.Name = current.Replace(' ', '_');
					dataGridViewColumn.HeaderText = current;
					dataGridViewColumn.Visible = Settings.Default.sh_err_desc;
					dataGridView1.Columns.Add(dataGridViewColumn);
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
		else if (Settings.Default.process_2d)
		{
			StringEnumerator enumerator = Settings.Default.err_list_2d.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();
					DataGridViewCell dataGridViewCell2 = (dataGridViewColumn.CellTemplate = new DataGridViewTextBoxCell());
					dataGridViewColumn.Name = current.Replace(' ', '_');
					dataGridViewColumn.HeaderText = current;
					dataGridViewColumn.Visible = Settings.Default.sh_err_desc;
					dataGridView1.Columns.Add(dataGridViewColumn);
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
		Settings.Default.allowpopup = false;
		Settings.Default.Save();
		Settings.Default.Reload();
		timer1.Start();
		check_rows();
		numericUpDown1.Value = Settings.Default.percent;
	}

	private void numericUpDown1_ValueChanged(object sender, EventArgs e)
	{
		Settings.Default.percent = Convert.ToInt32(numericUpDown1.Value);
		Settings.Default.Save();
		Settings.Default.Reload();
	}

	private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			Settings.Default.percent = Convert.ToInt32(numericUpDown1.Value);
			Settings.Default.Save();
			Settings.Default.Reload();
		}
	}

	private void clear_vals()
	{
		pictureBox1.set_Image((Image)null);
		dataGridView1.Rows.Clear();
		dataGridView2.Rows.Clear();
		dataGridView3.Rows.Clear();
		label2.Text = "-";
		label3.Text = "-";
		label4.Text = "-";
		label11.Text = "-";
		groupBox2.Text = "Images";
		toolStripStatusLabel2.Text = "Current samples closed.. Click here to view report.";
	}

	private void show_err(Exception exc)
	{
		MessageBox.Show("Error Occurred..\n\n" + exc.Message, "Error Ocuured");
	}

	private void getfiles(string directory)
	{
		string text = "";
		text = Directory.EnumerateFiles(sets.s_location, "*.png", SearchOption.AllDirectories).Count().ToString();
		string name = new DirectoryInfo(directory).Name;
		if (Settings.Default.user_list.Contains(name))
		{
			return;
		}
		Random rnd = new Random();
		IEnumerable<string> source = Directory.EnumerateFiles(directory, "*.png", SearchOption.AllDirectories);
		int num = source.Count();
		decimal percent = Settings.Default.percent;
		decimal d = (decimal)num * percent / 100m;
		int count = Convert.ToInt32(Math.Round(d, 0));
		List<string> source2 = source.OrderBy((string x) => rnd.Next()).Take(count).ToList();
		IEnumerable<string> enumerable = source2.Distinct();
		foreach (string item in enumerable)
		{
			Setrange(item, text, Path.GetDirectoryName(item));
		}
	}

	private void load_samples_DoWork(object sender, DoWorkEventArgs e)
	{
		err_count = 0;
		try
		{
			if (Directory.GetDirectories(sets.s_location).Length <= 0)
			{
				return;
			}
			foreach (string item in Directory.EnumerateDirectories(sets.s_location))
			{
				getfiles(item);
			}
		}
		catch (Exception ex2)
		{
			DirectoryNotFoundException ex = new DirectoryNotFoundException();
			if (ex2.HResult == ex.HResult)
			{
				MessageBox.Show("Sampling Directory is not valid..");
			}
			else
			{
				MessageBox.Show("Error Occurred :\n\n" + ex2.Message);
			}
		}
	}

	private void load_samples_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		load_samples.Dispose();
		dataGridView1.Rows.AddRange(rows.ToArray());
		rows.Clear();
		dataGridView1.Enabled = true;
		toolStripStatusLabel2.Text = "Samples Loaded.";
		toolStripProgressBar1.Visible = false;
		label3.Text = dataGridView1.Rows.Count.ToString();
		user_Status();
		pictureBox5.Visible = false;
		dataGridView1.Visible = true;
		dataGridView1.PerformLayout();
	}

	private void user_Status()
	{
		try
		{
			double num = 0.0;
			double num2 = 0.0;
			foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
			{
				if (item.Cells[6].Value != null)
				{
					num += Convert.ToDouble(item.Cells[6].Value);
				}
			}
			num2 = num / (double)dataGridView1.Rows.Count;
			label4.Text = Math.Round(num2, 0).ToString();
			DataTable dataTable = new DataTable();
			foreach (DataGridViewColumn column in dataGridView1.Columns)
			{
				dataTable.Columns.Add(column.HeaderText);
			}
			foreach (DataGridViewRow item2 in (IEnumerable)dataGridView1.Rows)
			{
				DataRow dataRow = dataTable.NewRow();
				foreach (DataGridViewCell cell in item2.Cells)
				{
					dataRow[cell.ColumnIndex] = cell.Value;
				}
				dataTable.Rows.Add(dataRow);
			}
			Dictionary<string, user_vals> dictionary = new Dictionary<string, user_vals>();
			foreach (DataGridViewRow item3 in (IEnumerable)dataGridView1.Rows)
			{
				if (!dictionary.ContainsKey(item3.Cells[2].Value.ToString()))
				{
					dictionary.Add(item3.Cells[2].Value.ToString(), new user_vals
					{
						pots = Convert.ToInt32(item3.Cells[4].Value),
						total_errors = Convert.ToInt32(item3.Cells[5].Value),
						score = 1.0
					});
				}
				else
				{
					user_vals user_vals = dictionary[item3.Cells[2].Value.ToString()];
					user_vals.pots += Convert.ToInt32(item3.Cells[4].Value);
					user_vals.total_errors += Convert.ToInt32(item3.Cells[5].Value);
				}
			}
			dataGridView3.Rows.Clear();
			foreach (KeyValuePair<string, user_vals> item4 in dictionary)
			{
				dataGridView3.Rows.Add(item4.Key, item4.Value.pots, item4.Value.total_errors);
			}
		}
		catch
		{
		}
	}

	private void button2_Click_1(object sender, EventArgs e)
	{
		DialogResult dialogResult = MessageBox.Show("Clear Existing data ?\nData won't be saved.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
		if (dialogResult == DialogResult.Yes)
		{
			clear_vals();
		}
	}

	private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (dataGridView1.Rows.Count > 0 && !ischanging)
			{
				Errors_Form errors_Form = new Errors_Form();
				DataGridViewComboBoxCell dataGridViewComboBoxCell = (DataGridViewComboBoxCell)dataGridView1.SelectedRows[0].Cells[3];
				if (dataGridViewComboBoxCell.Value.ToString() == "Yes")
				{
					errors_Form.FieldUpdate += af_FieldUpdate;
					if (Settings.Default.enable_ImageDetection && Settings.Default.process_ir)
					{
						errors_Form.numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[4].Value);
					}
					errors_Form.ShowDialog();
				}
				else if (dataGridViewComboBoxCell.Value.ToString() == "No")
				{
					dataGridView1.SelectedRows[0].Cells[5].Value = 0;
					dataGridView1.SelectedRows[0].Cells[6].Value = 100;
					dataGridView1.SelectedRows[0].Cells[7].Value = null;
				}
				else if (!(dataGridViewComboBoxCell.Value.ToString() == "FR"))
				{
				}
			}
			dataGridView1.Invalidate();
		}
		catch
		{
		}
		finally
		{
			user_Status();
		}
	}

	private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
	{
		try
		{
			if (dataGridView1.CurrentCell.Value.ToString() == "Yes")
			{
				dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		catch
		{
		}
	}

	private void pick_samples()
	{
		dataGridView1.Enabled = false;
		dataGridView1.DoubleBuffered(enable: true);
		pictureBox5.Visible = true;
		dataGridView1.Visible = false;
		textBox1.Text = Environment.UserName;
		dataGridView1.Rows.Clear();
		numericUpDown1.Enabled = false;
		pictureBox1.set_Image((Image)null);
		if (!load_samples.IsBusy)
		{
			toolStripStatusLabel2.Text = "Loading Samples...";
			toolStripProgressBar1.Visible = true;
			load_samples.RunWorkerAsync();
		}
	}

	private void pickToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView1.Rows.Count > 0)
		{
			DialogResult dialogResult = MessageBox.Show("Previous samples are still present and won't be saved..\nLoad new session ?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
			if (dialogResult == DialogResult.OK)
			{
				pick_samples();
			}
		}
		else
		{
			pick_samples();
		}
	}

	private void reportToolStripMenuItem_Click(object sender, EventArgs e)
	{
		review_s review_s2 = new review_s();
		review_s2.ShowDialog(this);
	}

	private void generalToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Settings_Form settings_Form = new Settings_Form();
		settings_Form.ShowDialog(this);
	}

	private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
	{
		About_Form about_Form = new About_Form();
		about_Form.ShowDialog();
	}

	private void saveCurrentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Expected O, but got Unknown
		FileName = Settings.Default.o_location + "/" + Environment.UserName + "_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".xlsx";
		DataSet dataSet = new DataSet();
		DataTable dataTable = new DataTable();
		dataTable.TableName = "Sample_Data";
		foreach (DataGridViewColumn column in dataGridView1.Columns)
		{
			dataTable.Columns.Add(column.HeaderText);
		}
		dataTable.Columns["Pots/Annotations"].DataType = typeof(int);
		dataTable.Columns["Total Errors"].DataType = typeof(int);
		dataTable.Columns["Score (%)"].DataType = typeof(int);
		foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
		{
			DataRow dataRow = dataTable.NewRow();
			foreach (DataGridViewCell cell in item.Cells)
			{
				dataRow[cell.ColumnIndex] = cell.Value ?? DBNull.Value;
			}
			dataTable.Rows.Add(dataRow);
		}
		dataSet.Tables.Add(dataTable);
		DataTable dataTable2 = new DataTable();
		dataTable2.TableName = "Comments_Data";
		foreach (DataGridViewColumn column2 in dataGridView2.Columns)
		{
			dataTable2.Columns.Add(column2.HeaderText);
		}
		foreach (DataGridViewRow item2 in (IEnumerable)dataGridView2.Rows)
		{
			DataRow dataRow = dataTable2.NewRow();
			foreach (DataGridViewCell cell2 in item2.Cells)
			{
				dataRow[cell2.ColumnIndex] = cell2.Value;
			}
			dataTable2.Rows.Add(dataRow);
		}
		dataSet.Tables.Add(dataTable2);
		DataTable dataTable3 = new DataTable();
		dataTable3.TableName = "User_Errors_Data";
		foreach (DataGridViewColumn column3 in dataGridView3.Columns)
		{
			dataTable3.Columns.Add(column3.HeaderText);
		}
		dataTable3.Columns["Pots/Annotations"].DataType = typeof(int);
		dataTable3.Columns["Errors"].DataType = typeof(int);
		foreach (DataGridViewRow item3 in (IEnumerable)dataGridView3.Rows)
		{
			DataRow dataRow = dataTable3.NewRow();
			foreach (DataGridViewCell cell3 in item3.Cells)
			{
				dataRow[cell3.ColumnIndex] = cell3.Value;
			}
			dataTable3.Rows.Add(dataRow);
		}
		dataSet.Tables.Add(dataTable3);
		DataTable dataTable4 = new DataTable();
		dataTable4.TableName = "Sample_info_Data";
		dataTable4.Columns.Add("Parameter");
		dataTable4.Columns.Add("Value");
		dataTable4.Rows.Add("Sample Pool Size", label2.Text);
		dataTable4.Rows.Add("Sample Rate", Settings.Default.percent);
		dataTable4.Rows.Add("Samples Picked", label3.Text);
		dataTable4.Rows.Add("QCer", textBox1.Text);
		dataTable4.Rows.Add("Quality Score", label4.Text);
		dataSet.Tables.Add(dataTable4);
		XLWorkbook val = new XLWorkbook();
		try
		{
			for (int i = 0; i < dataSet.Tables.Count; i++)
			{
				val.get_Worksheets().Add(dataSet.Tables[i], dataSet.Tables[i].TableName);
			}
			val.get_Style().get_Alignment().set_Horizontal((XLAlignmentHorizontalValues)0);
			((IXLFontBase)val.get_Style().get_Font()).set_Bold(true);
			val.SaveAs(FileName);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
	{
		check_rows();
	}

	private void check_rows()
	{
		if (dataGridView1.Rows.Count > 0)
		{
			finishToolStripMenuItem.Enabled = true;
			contextMenuStrip1.Enabled = true;
		}
		else
		{
			finishToolStripMenuItem.Enabled = false;
			contextMenuStrip1.Enabled = false;
		}
	}

	private void clearToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView1.Rows.Count > 0)
		{
			DialogResult dialogResult = MessageBox.Show("Clear Existing data ?\nData won't be saved.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (dialogResult == DialogResult.Yes)
			{
				clear_vals();
			}
		}
	}

	private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			dataGridView2.Rows.Insert(0, dataGridView1.SelectedCells[0].Value.ToString(), toolStripTextBox1.Text);
			toolStripTextBox1.Clear();
			contextMenuStrip1.Close();
		}
	}

	private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
	{
		Settings.Default.percent = Convert.ToInt32(numericUpDown1.Value);
		Settings.Default.Save();
		Settings.Default.Reload();
	}

	private void DrawBitmapWithBorder(Bitmap bmp, Point pos, Graphics g)
	{
		using (Brush brush = new SolidBrush(Color.Black))
		{
			g.FillRectangle(brush, pos.X - 1000, pos.Y - 1000, bmp.Width + 1000, bmp.Height + 1000);
		}
		g.DrawImage(bmp, pos);
	}

	private unsafe void detect_Squares()
	{
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ImageBox val = null;
			Image image = (Image)(object)((!Settings.Default.allowpopup) ? pictureBox1.get_Image() : usrd.pictureBox1.get_Image());
			Bitmap bitmap = (Bitmap)image;
			if (Settings.Default.fill_colour)
			{
			}
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using SolidBrush brush = new SolidBrush(Color.Black);
				graphics.FillRectangle(brush, new Rectangle(0, 1023, 1920, 20));
			}
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using SolidBrush brush = new SolidBrush(Color.Black);
				graphics.FillRectangle(brush, new Rectangle(1903, 0, 17, 1080));
			}
			Bitmap bitmap2 = new Bitmap(bitmap);
			Graphics graphics2 = Graphics.FromImage(bitmap2);
			using (graphics2)
			{
				Color oldColor = Color.FromArgb(44, 57, 71);
				Color newColor = Color.FromArgb(0, 0, 0);
				ColorMap[] array = new ColorMap[1]
				{
					new ColorMap()
				};
				array[0].OldColor = oldColor;
				array[0].NewColor = newColor;
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetRemapTable(array);
				Rectangle destRect = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
				graphics2.DrawImage(bitmap2, destRect, 0, 0, destRect.Width, destRect.Height, GraphicsUnit.Pixel, imageAttributes);
			}
			Bitmap bitmap3 = new Bitmap(bitmap2);
			Graphics graphics4 = Graphics.FromImage(bitmap3);
			using (graphics4)
			{
				Color oldColor2 = Color.FromArgb(45, 57, 70);
				Color newColor2 = Color.FromArgb(0, 0, 0);
				ColorMap[] array2 = new ColorMap[1]
				{
					new ColorMap()
				};
				array2[0].OldColor = oldColor2;
				array2[0].NewColor = newColor2;
				ImageAttributes imageAttributes2 = new ImageAttributes();
				imageAttributes2.SetRemapTable(array2);
				Rectangle destRect2 = new Rectangle(0, 0, bitmap3.Width, bitmap3.Height);
				graphics4.DrawImage(bitmap3, destRect2, 0, 0, destRect2.Width, destRect2.Height, GraphicsUnit.Pixel, imageAttributes2);
			}
			Bitmap bitmap4 = new Bitmap(bitmap3);
			Graphics graphics5 = Graphics.FromImage(bitmap4);
			using (graphics5)
			{
				Color oldColor3 = Color.FromArgb(0, 201, 255);
				Color newColor3 = Color.FromArgb(0, 0, 0);
				ColorMap[] array3 = new ColorMap[1]
				{
					new ColorMap()
				};
				array3[0].OldColor = oldColor3;
				array3[0].NewColor = newColor3;
				ImageAttributes imageAttributes3 = new ImageAttributes();
				imageAttributes3.SetRemapTable(array3);
				Rectangle destRect3 = new Rectangle(0, 0, bitmap4.Width, bitmap4.Height);
				graphics5.DrawImage(bitmap4, destRect3, 0, 0, destRect3.Width, destRect3.Height, GraphicsUnit.Pixel, imageAttributes3);
			}
			Bitmap bitmap5 = new Bitmap(bitmap4);
			Graphics graphics6 = Graphics.FromImage(bitmap5);
			using (graphics6)
			{
				Color oldColor4 = Color.FromArgb(32, 202, 253);
				Color newColor4 = Color.FromArgb(0, 0, 0);
				ColorMap[] array4 = new ColorMap[1]
				{
					new ColorMap()
				};
				array4[0].OldColor = oldColor4;
				array4[0].NewColor = newColor4;
				ImageAttributes imageAttributes4 = new ImageAttributes();
				imageAttributes4.SetRemapTable(array4);
				Rectangle destRect4 = new Rectangle(0, 0, bitmap5.Width, bitmap5.Height);
				graphics6.DrawImage(bitmap5, destRect4, 0, 0, destRect4.Width, destRect4.Height, GraphicsUnit.Pixel, imageAttributes4);
			}
			Bitmap bitmap6 = new Bitmap(bitmap5);
			Graphics graphics7 = Graphics.FromImage(bitmap6);
			using (graphics7)
			{
				Color oldColor5 = Color.FromArgb(85, 205, 32);
				Color newColor5 = Color.FromArgb(0, 0, 0);
				ColorMap[] array5 = new ColorMap[1]
				{
					new ColorMap()
				};
				array5[0].OldColor = oldColor5;
				array5[0].NewColor = newColor5;
				ImageAttributes imageAttributes5 = new ImageAttributes();
				imageAttributes5.SetRemapTable(array5);
				Rectangle destRect5 = new Rectangle(0, 0, bitmap6.Width, bitmap6.Height);
				graphics7.DrawImage(bitmap6, destRect5, 0, 0, destRect5.Width, destRect5.Height, GraphicsUnit.Pixel, imageAttributes5);
			}
			using (Graphics graphics = Graphics.FromImage(bitmap6))
			{
				using SolidBrush brush = new SolidBrush(Color.Red);
				graphics.FillRectangle(brush, new Rectangle(0, 0, 150, 220));
			}
			BlobCounter val2 = new BlobCounter();
			((BlobCounterBase)val2).set_FilterBlobs(true);
			((BlobCounterBase)val2).set_MinHeight(100);
			((BlobCounterBase)val2).set_MinWidth(100);
			((BlobCounterBase)val2).set_MaxHeight(190);
			((BlobCounterBase)val2).set_MaxWidth(190);
			((BlobCounterBase)val2).ProcessImage((Bitmap)(object)bitmap6);
			Blob[] objectsInformation = ((BlobCounterBase)val2).GetObjectsInformation();
			SimpleShapeChecker val3 = new SimpleShapeChecker();
			int num = 0;
			Blob[] array6 = objectsInformation;
			List<IntPoint> list = default(List<IntPoint>);
			foreach (Blob val4 in array6)
			{
				List<IntPoint> blobsEdgePoints = (List<IntPoint>)(object)((BlobCounterBase)val2).GetBlobsEdgePoints(val4);
				if (val3.IsQuadrilateral((List<IntPoint>)(object)blobsEdgePoints, ref *(List<IntPoint>*)(&list)) && val3.CheckPolygonSubType((List<IntPoint>)(object)list) == sets.shape_type)
				{
					num++;
					val.set_Image((Image)(object)bitmap);
				}
			}
			dataGridView1.SelectedCells[4].Value = num;
		}
		catch
		{
		}
		finally
		{
		}
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		DialogResult dialogResult = MessageBox.Show("Exit application ?\nAny unsaved data will be lost..", "Exit Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
		e.Cancel = dialogResult == DialogResult.Cancel;
	}

	private void dataGridView2_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (dataGridView1.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
			{
				if (item.Cells[0].Value.ToString().Equals(dataGridView2.SelectedCells[0].Value))
				{
					item.Selected = true;
					break;
				}
			}
		}
		catch
		{
		}
	}

	private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		if (e.Value != null && decimal.TryParse(e.Value.ToString(), out var result))
		{
			e.Value = Math.Round(result, 2);
		}
	}

	private void finishToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DialogResult dialogResult = MessageBox.Show("Finish Sampling of current data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		if (dialogResult == DialogResult.Yes)
		{
			saveCurrentToolStripMenuItem_Click(null, null);
			clear_vals();
			check_rows();
			numericUpDown1.Enabled = true;
		}
	}

	private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
	{
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (Settings.Default.allowpopup)
		{
			splitContainer3.Panel2Collapsed = true;
		}
		else
		{
			splitContainer3.Panel2Collapsed = false;
		}
	}

	private void autoDetectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (Settings.Default.enable_ImageDetection)
		{
			Settings.Default.enable_ImageDetection = false;
		}
		else
		{
			Settings.Default.enable_ImageDetection = true;
		}
		Settings.Default.Save();
		Settings.Default.Reload();
	}

	private void toolStripStatusLabel2_Click(object sender, EventArgs e)
	{
		if (FileName == null || !(FileName != ""))
		{
			return;
		}
		try
		{
			Process.Start(FileName);
		}
		catch
		{
		}
		finally
		{
			FileName = "";
			toolStripStatusLabel2.Text = "-";
		}
	}

	private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
	{
	}

	private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		e.Control.KeyPress -= Column1_KeyPress;
		if (dataGridView1.CurrentCell.ColumnIndex == 4 && e.Control is TextBox textBox)
		{
			textBox.KeyPress += Column1_KeyPress;
		}
	}

	private void Column1_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void timer2_Tick(object sender, EventArgs e)
	{
	}

	private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
	{
	}

	private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
	}

	private void helpToolStripMenuItem_Click(object sender, EventArgs e)
	{
		helped helped2 = new helped();
		helped2.ShowDialog();
	}

	private void label11_Click(object sender, EventArgs e)
	{
	}

	private void pictureBox1_MouseDoubleClick_1(object sender, MouseEventArgs e)
	{
		usrd = new Docked_Form();
		usrd.Show();
		splitContainer3.Panel2Collapsed = true;
		usrd.pictureBox1.set_Image(pictureBox1.get_Image());
		Settings.Default.allowpopup = true;
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
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRA_Sampler.Main_Form));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.addCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.cal_per = new System.ComponentModel.BackgroundWorker();
		this.load_samples = new System.ComponentModel.BackgroundWorker();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.samplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.finishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.generalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.autoDetectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.splitContainer3 = new System.Windows.Forms.SplitContainer();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.pictureBox5 = new System.Windows.Forms.PictureBox();
		this.dataGridView1 = new System.Windows.Forms.DataGridView();
		this.iname = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.img_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.usr = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.err_found = new System.Windows.Forms.DataGridViewComboBoxColumn();
		this.p_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.t_errors = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.q_score = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.comms = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.dataGridView3 = new System.Windows.Forms.DataGridView();
		this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.dataGridView2 = new System.Windows.Forms.DataGridView();
		this.i = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.err = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.timer2 = new System.Windows.Forms.Timer(this.components);
		this.label9 = new System.Windows.Forms.Label();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.pictureBox1 = new ImageBox();
		this.contextMenuStrip1.SuspendLayout();
		this.statusStrip1.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer3).BeginInit();
		this.splitContainer3.Panel1.SuspendLayout();
		this.splitContainer3.Panel2.SuspendLayout();
		this.splitContainer3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
		this.splitContainer2.Panel1.SuspendLayout();
		this.splitContainer2.Panel2.SuspendLayout();
		this.splitContainer2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView3).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView2).BeginInit();
		this.groupBox2.SuspendLayout();
		base.SuspendLayout();
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.addCommentsToolStripMenuItem });
		this.contextMenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(201, 26);
		this.addCommentsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.addCommentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripTextBox1 });
		this.addCommentsToolStripMenuItem.Name = "addCommentsToolStripMenuItem";
		this.addCommentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
		this.addCommentsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
		this.addCommentsToolStripMenuItem.Text = "Add Comments";
		this.toolStripTextBox1.AutoSize = false;
		this.toolStripTextBox1.Name = "toolStripTextBox1";
		this.toolStripTextBox1.Size = new System.Drawing.Size(200, 23);
		this.toolStripTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(toolStripTextBox1_KeyDown);
		this.statusStrip1.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripStatusLabel2, this.toolStripProgressBar1 });
		this.statusStrip1.Location = new System.Drawing.Point(0, 611);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(1107, 22);
		this.statusStrip1.TabIndex = 3;
		this.statusStrip1.Text = "statusStrip1";
		this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
		this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 17);
		this.toolStripStatusLabel2.Text = "-";
		this.toolStripStatusLabel2.Click += new System.EventHandler(toolStripStatusLabel2_Click);
		this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		this.toolStripProgressBar1.Name = "toolStripProgressBar1";
		this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
		this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
		this.toolStripProgressBar1.Visible = false;
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		this.toolStripStatusLabel1.Size = new System.Drawing.Size(12, 17);
		this.toolStripStatusLabel1.Text = "-";
		this.load_samples.WorkerSupportsCancellation = true;
		this.load_samples.DoWork += new System.ComponentModel.DoWorkEventHandler(load_samples_DoWork);
		this.load_samples.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(load_samples_RunWorkerCompleted);
		this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
		this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.menuStrip1.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.fileToolStripMenuItem, this.samplesToolStripMenuItem, this.settingsToolStripMenuItem, this.helpToolStripMenuItem, this.aboutToolStripMenuItem, this.autoDetectToolStripMenuItem });
		this.menuStrip1.Location = new System.Drawing.Point(1, 2);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(271, 24);
		this.menuStrip1.TabIndex = 13;
		this.menuStrip1.Text = "menuStrip1";
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.saveCurrentToolStripMenuItem });
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
		this.fileToolStripMenuItem.Text = "File";
		this.saveCurrentToolStripMenuItem.Name = "saveCurrentToolStripMenuItem";
		this.saveCurrentToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
		this.saveCurrentToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.saveCurrentToolStripMenuItem.Text = "Save Current";
		this.saveCurrentToolStripMenuItem.Click += new System.EventHandler(saveCurrentToolStripMenuItem_Click);
		this.samplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.pickToolStripMenuItem, this.clearToolStripMenuItem, this.finishToolStripMenuItem });
		this.samplesToolStripMenuItem.Name = "samplesToolStripMenuItem";
		this.samplesToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
		this.samplesToolStripMenuItem.Text = "Samples";
		this.pickToolStripMenuItem.Name = "pickToolStripMenuItem";
		this.pickToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Control;
		this.pickToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
		this.pickToolStripMenuItem.Text = "Pick";
		this.pickToolStripMenuItem.Click += new System.EventHandler(pickToolStripMenuItem_Click);
		this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
		this.clearToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete | System.Windows.Forms.Keys.Control;
		this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
		this.clearToolStripMenuItem.Text = "Clear";
		this.clearToolStripMenuItem.Click += new System.EventHandler(clearToolStripMenuItem_Click);
		this.finishToolStripMenuItem.Name = "finishToolStripMenuItem";
		this.finishToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control;
		this.finishToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
		this.finishToolStripMenuItem.Text = "Finish";
		this.finishToolStripMenuItem.Click += new System.EventHandler(finishToolStripMenuItem_Click);
		this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.generalToolStripMenuItem });
		this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
		this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
		this.settingsToolStripMenuItem.Text = "Settings";
		this.generalToolStripMenuItem.Name = "generalToolStripMenuItem";
		this.generalToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
		this.generalToolStripMenuItem.Text = "General";
		this.generalToolStripMenuItem.Click += new System.EventHandler(generalToolStripMenuItem_Click);
		this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
		this.helpToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
		this.helpToolStripMenuItem.Text = "Help";
		this.helpToolStripMenuItem.Click += new System.EventHandler(helpToolStripMenuItem_Click);
		this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
		this.aboutToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
		this.aboutToolStripMenuItem.Text = "About";
		this.aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click_1);
		this.autoDetectToolStripMenuItem.Name = "autoDetectToolStripMenuItem";
		this.autoDetectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control;
		this.autoDetectToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
		this.autoDetectToolStripMenuItem.Text = "Auto_Detect";
		this.autoDetectToolStripMenuItem.Visible = false;
		this.autoDetectToolStripMenuItem.Click += new System.EventHandler(autoDetectToolStripMenuItem_Click);
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(921, 616);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(98, 14);
		this.label6.TabIndex = 19;
		this.label6.Text = "Samples Picked :";
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1046, 616);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(11, 14);
		this.label3.TabIndex = 17;
		this.label3.Text = "-";
		this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(760, 616);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(62, 14);
		this.label7.TabIndex = 18;
		this.label7.Text = "Pool Size :";
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(852, 616);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(11, 14);
		this.label2.TabIndex = 16;
		this.label2.Text = "-";
		this.numericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.numericUpDown1.DecimalPlaces = 1;
		this.numericUpDown1.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown1.Increment = new decimal(new int[4] { 1, 0, 0, 65536 });
		this.numericUpDown1.Location = new System.Drawing.Point(716, 4);
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Size = new System.Drawing.Size(47, 21);
		this.numericUpDown1.TabIndex = 15;
		this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.numericUpDown1.Value = new decimal(new int[4] { 1, 0, 0, 65536 });
		this.numericUpDown1.ValueChanged += new System.EventHandler(numericUpDown1_ValueChanged_1);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9f);
		this.label1.Location = new System.Drawing.Point(617, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(92, 14);
		this.label1.TabIndex = 14;
		this.label1.Text = "Sampling Rate :";
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1083, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(16, 14);
		this.label8.TabIndex = 24;
		this.label8.Text = "%";
		this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(991, 7);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(42, 14);
		this.label5.TabIndex = 23;
		this.label5.Text = "Score :";
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1057, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(11, 14);
		this.label4.TabIndex = 22;
		this.label4.Text = "-";
		this.label12.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label12.Location = new System.Drawing.Point(902, 612);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(2, 24);
		this.label12.TabIndex = 26;
		this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(792, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(55, 14);
		this.label15.TabIndex = 30;
		this.label15.Text = "QC User :";
		this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox1.Enabled = false;
		this.textBox1.Location = new System.Drawing.Point(855, 4);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(107, 21);
		this.textBox1.TabIndex = 33;
		this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label14.Location = new System.Drawing.Point(980, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(2, 24);
		this.label14.TabIndex = 34;
		this.splitContainer3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.splitContainer3.Location = new System.Drawing.Point(12, 29);
		this.splitContainer3.Name = "splitContainer3";
		this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
		this.splitContainer3.Panel2.Controls.Add(this.groupBox2);
		this.splitContainer3.Size = new System.Drawing.Size(1089, 574);
		this.splitContainer3.SplitterDistance = 530;
		this.splitContainer3.TabIndex = 37;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
		this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
		this.splitContainer1.Size = new System.Drawing.Size(530, 574);
		this.splitContainer1.SplitterDistance = 386;
		this.splitContainer1.TabIndex = 37;
		this.groupBox1.Controls.Add(this.pictureBox5);
		this.groupBox1.Controls.Add(this.dataGridView1);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox1.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(530, 386);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Samples";
		this.pictureBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox5.Image = (System.Drawing.Image)resources.GetObject("pictureBox5.Image");
		this.pictureBox5.Location = new System.Drawing.Point(3, 51);
		this.pictureBox5.Name = "pictureBox5";
		this.pictureBox5.Size = new System.Drawing.Size(524, 333);
		this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox5.TabIndex = 14;
		this.pictureBox5.TabStop = false;
		this.pictureBox5.Visible = false;
		this.dataGridView1.AllowUserToAddRows = false;
		this.dataGridView1.AllowUserToDeleteRows = false;
		this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView1.Columns.AddRange(this.iname, this.img_path, this.usr, this.err_found, this.p_count, this.t_errors, this.q_score, this.comms);
		this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
		this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dataGridView1.Location = new System.Drawing.Point(3, 17);
		this.dataGridView1.MultiSelect = false;
		this.dataGridView1.Name = "dataGridView1";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
		this.dataGridView1.RowHeadersVisible = false;
		this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dataGridView1.Size = new System.Drawing.Size(524, 366);
		this.dataGridView1.TabIndex = 0;
		this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellContentClick);
		this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
		this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
		this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(dataGridView1_CurrentCellDirtyStateChanged);
		this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
		this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);
		this.dataGridView1.SelectionChanged += new System.EventHandler(dataGridView1_SelectionChanged);
		this.iname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		this.iname.HeaderText = "Images";
		this.iname.Name = "iname";
		this.iname.ReadOnly = true;
		this.iname.Width = 66;
		this.img_path.HeaderText = "Image Path";
		this.img_path.Name = "img_path";
		this.img_path.ReadOnly = true;
		this.img_path.Visible = false;
		this.usr.HeaderText = "User";
		this.usr.Name = "usr";
		this.usr.ReadOnly = true;
		this.usr.Width = 75;
		this.err_found.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
		this.err_found.HeaderText = "Errors";
		this.err_found.Items.AddRange("Yes", "No", "FR");
		this.err_found.Name = "err_found";
		this.err_found.Width = 74;
		this.p_count.HeaderText = "Pots/Annotations";
		this.p_count.Name = "p_count";
		this.p_count.Width = 90;
		this.t_errors.HeaderText = "Total Errors";
		this.t_errors.MinimumWidth = 2;
		this.t_errors.Name = "t_errors";
		this.t_errors.ReadOnly = true;
		this.t_errors.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.t_errors.Width = 74;
		this.q_score.HeaderText = "Score (%)";
		this.q_score.MinimumWidth = 2;
		this.q_score.Name = "q_score";
		this.q_score.ReadOnly = true;
		this.q_score.Width = 75;
		this.comms.HeaderText = "Error Comments";
		this.comms.Name = "comms";
		this.comms.ReadOnly = true;
		this.comms.Width = 74;
		this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		this.splitContainer2.Name = "splitContainer2";
		this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
		this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
		this.splitContainer2.Size = new System.Drawing.Size(530, 184);
		this.splitContainer2.SplitterDistance = 205;
		this.splitContainer2.TabIndex = 0;
		this.groupBox3.Controls.Add(this.dataGridView3);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(205, 184);
		this.groupBox3.TabIndex = 35;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "User Status";
		this.dataGridView3.AllowUserToAddRows = false;
		this.dataGridView3.AllowUserToDeleteRows = false;
		this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView3.Columns.AddRange(this.Column1, this.Column2, this.Column3);
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DarkGray;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView3.DefaultCellStyle = dataGridViewCellStyle4;
		this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dataGridView3.Location = new System.Drawing.Point(3, 17);
		this.dataGridView3.MultiSelect = false;
		this.dataGridView3.Name = "dataGridView3";
		this.dataGridView3.ReadOnly = true;
		this.dataGridView3.RowHeadersVisible = false;
		this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
		this.dataGridView3.Size = new System.Drawing.Size(199, 164);
		this.dataGridView3.TabIndex = 0;
		this.Column1.FillWeight = 84.53374f;
		this.Column1.HeaderText = "User";
		this.Column1.Name = "Column1";
		this.Column1.ReadOnly = true;
		this.Column2.FillWeight = 139.3241f;
		this.Column2.HeaderText = "Pots/Annotations";
		this.Column2.Name = "Column2";
		this.Column2.ReadOnly = true;
		this.Column3.FillWeight = 76.14214f;
		this.Column3.HeaderText = "Errors";
		this.Column3.Name = "Column3";
		this.Column3.ReadOnly = true;
		this.groupBox4.Controls.Add(this.dataGridView2);
		this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox4.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(0, 0);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(321, 184);
		this.groupBox4.TabIndex = 6;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Comments";
		this.dataGridView2.AllowUserToAddRows = false;
		this.dataGridView2.AllowUserToResizeColumns = false;
		this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView2.Columns.AddRange(this.i, this.err);
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DarkGray;
		dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle5;
		this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dataGridView2.Location = new System.Drawing.Point(3, 17);
		this.dataGridView2.MultiSelect = false;
		this.dataGridView2.Name = "dataGridView2";
		this.dataGridView2.ReadOnly = true;
		this.dataGridView2.RowHeadersVisible = false;
		this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dataGridView2.Size = new System.Drawing.Size(315, 164);
		this.dataGridView2.TabIndex = 0;
		this.dataGridView2.SelectionChanged += new System.EventHandler(dataGridView2_SelectionChanged);
		this.i.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.i.HeaderText = "Image Name";
		this.i.Name = "i";
		this.i.ReadOnly = true;
		this.i.Width = 91;
		this.err.HeaderText = "Comments";
		this.err.Name = "err";
		this.err.ReadOnly = true;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.pictureBox1);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox2.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(555, 574);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Images";
		this.timer2.Tick += new System.EventHandler(timer2_Tick);
		this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label9.Location = new System.Drawing.Point(781, 2);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(2, 24);
		this.label9.TabIndex = 38;
		this.timer1.Enabled = true;
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.label10.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(524, 616);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(92, 14);
		this.label10.TabIndex = 40;
		this.label10.Text = "Process Name : ";
		this.label11.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(620, 616);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(11, 14);
		this.label11.TabIndex = 39;
		this.label11.Text = "-";
		this.label11.Click += new System.EventHandler(label11_Click);
		this.label13.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label13.Location = new System.Drawing.Point(741, 612);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(2, 24);
		this.label13.TabIndex = 41;
		this.pictureBox1.set_AllowDoubleClick(true);
		this.pictureBox1.set_AllowUnfocusedMouseWheel(true);
		((System.Windows.Forms.Control)(object)this.pictureBox1).BackColor = System.Drawing.SystemColors.Control;
		((ScrollControl)this.pictureBox1).set_BorderStyle(System.Windows.Forms.BorderStyle.FixedSingle);
		((System.Windows.Forms.Control)(object)this.pictureBox1).Cursor = System.Windows.Forms.Cursors.Cross;
		((System.Windows.Forms.Control)(object)this.pictureBox1).Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.set_GridColor((System.Drawing.Color)System.Drawing.SystemColors.ButtonHighlight);
		this.pictureBox1.set_GridColorAlternate((System.Drawing.Color)System.Drawing.SystemColors.Control);
		((System.Windows.Forms.Control)(object)this.pictureBox1).Location = new System.Drawing.Point(3, 17);
		((System.Windows.Forms.Control)(object)this.pictureBox1).Name = "pictureBox1";
		((System.Windows.Forms.Control)(object)this.pictureBox1).Size = new System.Drawing.Size(549, 554);
		((System.Windows.Forms.Control)(object)this.pictureBox1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.pictureBox1).MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDoubleClick_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1107, 633);
		base.Controls.Add(this.label13);
		base.Controls.Add(this.label10);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.splitContainer3);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label14);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.label15);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.numericUpDown1);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.menuStrip1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MainMenuStrip = this.menuStrip1;
		base.Name = "Main_Form";
		this.Text = "IRA Sampler";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		this.contextMenuStrip1.ResumeLayout(false);
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
		this.splitContainer3.Panel1.ResumeLayout(false);
		this.splitContainer3.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer3).EndInit();
		this.splitContainer3.ResumeLayout(false);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
		this.splitContainer2.Panel1.ResumeLayout(false);
		this.splitContainer2.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
		this.splitContainer2.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dataGridView3).EndInit();
		this.groupBox4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dataGridView2).EndInit();
		this.groupBox2.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
