using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ClosedXML.Excel;
using IRA_Sampler.Properties;

namespace IRA_Sampler;

public class review_s : Form
{
	private string otfile = "";

	private DataSet ds = new DataSet();

	private IContainer components = null;

	private GroupBox groupBox1;

	private DataGridView dataGridView1;

	private DataGridViewTextBoxColumn date;

	private DataGridViewTextBoxColumn user;

	private DataGridViewTextBoxColumn total;

	private DataGridViewTextBoxColumn rate;

	private DataGridViewTextBoxColumn count;

	private DataGridViewTextBoxColumn score;

	private DataGridViewTextBoxColumn comms;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem exportToExcelToolStripMenuItem1;

	private SaveFileDialog saveFileDialog1;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel toolStripStatusLabel1;

	public review_s()
	{
		InitializeComponent();
	}

	private void review_s_Load(object sender, EventArgs e)
	{
		read_data();
	}

	private void read_data()
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			using FileStream inStream = new FileStream(Settings.Default.o_location + "/sample_data.xml", FileMode.Open, FileAccess.Read);
			xmlDocument.Load(inStream);
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Sample");
			for (int i = 0; i < elementsByTagName.Count; i++)
			{
				XmlElement xmlElement = (XmlElement)xmlDocument.GetElementsByTagName("Sample")[i];
				XmlElement xmlElement2 = (XmlElement)xmlDocument.GetElementsByTagName("date")[i];
				XmlElement xmlElement3 = (XmlElement)xmlDocument.GetElementsByTagName("user")[i];
				XmlElement xmlElement4 = (XmlElement)xmlDocument.GetElementsByTagName("p_size")[i];
				XmlElement xmlElement5 = (XmlElement)xmlDocument.GetElementsByTagName("s_rate")[i];
				XmlElement xmlElement6 = (XmlElement)xmlDocument.GetElementsByTagName("s_count")[i];
				XmlElement xmlElement7 = (XmlElement)xmlDocument.GetElementsByTagName("score")[i];
				XmlElement xmlElement8 = (XmlElement)xmlDocument.GetElementsByTagName("comments")[i];
				dataGridView1.Rows.Add(xmlElement2.InnerText, xmlElement3.InnerText, xmlElement4.InnerText, xmlElement5.InnerText, xmlElement6.InnerText, xmlElement7.InnerText, xmlElement8.InnerText);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error Occured..\n\n" + ex.Message);
		}
	}

	private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void exportToExcelToolStripMenuItem1_Click(object sender, EventArgs e)
	{
		ds.ReadXml(Settings.Default.o_location + "/sample_data.xml");
		saveFileDialog1.ShowDialog(this);
	}

	private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		XLWorkbook val = new XLWorkbook();
		otfile = saveFileDialog1.FileName;
		val.get_Worksheets().Add(ds.Tables[0], "sample_data");
		val.SaveAs(otfile);
		toolStripStatusLabel1.Text = "Report Exported. Click here to open the report";
	}

	private void toolStripStatusLabel1_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start(otfile);
			toolStripStatusLabel1.Text = "-";
			otfile = "";
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
		this.components = new System.ComponentModel.Container();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.dataGridView1 = new System.Windows.Forms.DataGridView();
		this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.user = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.count = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.score = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.comms = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.exportToExcelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
		this.contextMenuStrip1.SuspendLayout();
		this.statusStrip1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.statusStrip1);
		this.groupBox1.Controls.Add(this.dataGridView1);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(739, 498);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Sample Details";
		this.dataGridView1.AllowUserToAddRows = false;
		this.dataGridView1.AllowUserToDeleteRows = false;
		this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView1.Columns.AddRange(this.date, this.user, this.total, this.rate, this.count, this.score, this.comms);
		this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.LightGray;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle;
		this.dataGridView1.Location = new System.Drawing.Point(3, 17);
		this.dataGridView1.Name = "dataGridView1";
		this.dataGridView1.ReadOnly = true;
		this.dataGridView1.RowHeadersVisible = false;
		this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dataGridView1.Size = new System.Drawing.Size(733, 453);
		this.dataGridView1.TabIndex = 0;
		this.date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		this.date.HeaderText = "Date";
		this.date.Name = "date";
		this.date.ReadOnly = true;
		this.date.Width = 150;
		this.user.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.user.HeaderText = "Performed By";
		this.user.Name = "user";
		this.user.ReadOnly = true;
		this.user.Width = 95;
		this.total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.total.HeaderText = "Pool Size";
		this.total.Name = "total";
		this.total.ReadOnly = true;
		this.total.Width = 73;
		this.rate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.rate.HeaderText = "Sample %";
		this.rate.Name = "rate";
		this.rate.ReadOnly = true;
		this.rate.Width = 77;
		this.count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.count.HeaderText = "Sample Count";
		this.count.Name = "count";
		this.count.ReadOnly = true;
		this.count.Width = 97;
		this.score.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.score.HeaderText = "Quality Score";
		this.score.Name = "score";
		this.score.ReadOnly = true;
		this.score.Width = 94;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.comms.DefaultCellStyle = dataGridViewCellStyle2;
		this.comms.HeaderText = "Comments";
		this.comms.Name = "comms";
		this.comms.ReadOnly = true;
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.exportToExcelToolStripMenuItem1 });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(151, 26);
		this.exportToExcelToolStripMenuItem1.Name = "exportToExcelToolStripMenuItem1";
		this.exportToExcelToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
		this.exportToExcelToolStripMenuItem1.Text = "Export to Excel";
		this.exportToExcelToolStripMenuItem1.Click += new System.EventHandler(exportToExcelToolStripMenuItem1_Click);
		this.saveFileDialog1.DefaultExt = "xlsx";
		this.saveFileDialog1.Filter = "Excel File|.xlsx";
		this.saveFileDialog1.Title = "Export Sample Data to Excel";
		this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(saveFileDialog1_FileOk);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripStatusLabel1 });
		this.statusStrip1.Location = new System.Drawing.Point(3, 473);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(733, 22);
		this.statusStrip1.TabIndex = 1;
		this.statusStrip1.Text = "statusStrip1";
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		this.toolStripStatusLabel1.Size = new System.Drawing.Size(12, 17);
		this.toolStripStatusLabel1.Text = "-";
		this.toolStripStatusLabel1.Click += new System.EventHandler(toolStripStatusLabel1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(739, 498);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "review_s";
		this.Text = "Sample Report";
		base.Load += new System.EventHandler(review_s_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
		this.contextMenuStrip1.ResumeLayout(false);
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
