using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using IRA_Sampler.Properties;

namespace IRA_Sampler;

public class Errors_Form : Form
{
	public delegate void FieldUpdateHandler(object sender, ValueEventArgs e);

	private double i = 0.0;

	private double result = 0.0;

	private Dictionary<string, int> str = new Dictionary<string, int>();

	private IContainer components = null;

	public DataGridView dataGridView1;

	public Button button1;

	private Label label2;

	private Label label1;

	public Button button2;

	public GroupBox groupBox2;

	public GroupBox groupBox1;

	public GroupBox groupBox3;

	public NumericUpDown numericUpDown1;

	private DataGridViewTextBoxColumn err_type;

	private DataGridViewTextBoxColumn err_count;

	private Timer timer1;

	public event FieldUpdateHandler FieldUpdate;

	public Errors_Form()
	{
		InitializeComponent();
	}

	public void perfcalc()
	{
		try
		{
			i = 0.0;
			foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
			{
				i += Convert.ToDouble(item.Cells[1].Value);
			}
			double num = i / Convert.ToDouble(numericUpDown1.Value);
			result = Math.Round(1.0 - num, 4) * 100.0;
			label1.Text = result.ToString();
		}
		catch
		{
		}
	}

	private void err_desc_Load(object sender, EventArgs e)
	{
		timer1.Start();
		dataGridView1.Rows.Clear();
		if (Settings.Default.process_ir)
		{
			StringEnumerator enumerator = Settings.Default.err_list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					dataGridView1.Rows.Add(current);
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
					dataGridView1.Rows.Add(current);
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
		dataGridView1.Select();
		dataGridView1.SelectedRows[0].Cells[1].Selected = true;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		perfcalc();
		foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
		{
			if (item.Cells[1].Value != null)
			{
				str.Add(item.Cells[0].Value.ToString(), Convert.ToInt32(item.Cells[1].Value));
			}
		}
		Close();
	}

	private void err_desc_FormClosing(object sender, FormClosingEventArgs e)
	{
		ValueEventArgs valueEventArgs = null;
		valueEventArgs = ((str == null) ? new ValueEventArgs(null, 0.0, 0, null) : new ValueEventArgs(str, result, Convert.ToInt32(numericUpDown1.Value), ""));
		this.FieldUpdate(this, valueEventArgs);
	}

	private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			button1_Click(null, null);
		}
	}

	private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		perfcalc();
	}

	private void numericUpDown1_ValueChanged(object sender, EventArgs e)
	{
		perfcalc();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		int num = 0;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
		{
			num += Convert.ToInt32(item.Cells[1].Value);
		}
		if (num > 0)
		{
			button1.Enabled = true;
		}
		else
		{
			button1.Enabled = false;
		}
	}

	private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		e.Control.KeyPress -= Column1_KeyPress;
		if (dataGridView1.CurrentCell.ColumnIndex == 1 && e.Control is TextBox textBox)
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
		this.dataGridView1 = new System.Windows.Forms.DataGridView();
		this.err_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.err_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.button1 = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		base.SuspendLayout();
		this.dataGridView1.AllowUserToAddRows = false;
		this.dataGridView1.AllowUserToDeleteRows = false;
		this.dataGridView1.AllowUserToResizeColumns = false;
		this.dataGridView1.AllowUserToResizeRows = false;
		this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView1.Columns.AddRange(this.err_type, this.err_count);
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.DarkGray;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle;
		this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dataGridView1.Location = new System.Drawing.Point(3, 17);
		this.dataGridView1.Name = "dataGridView1";
		this.dataGridView1.RowHeadersVisible = false;
		this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dataGridView1.Size = new System.Drawing.Size(242, 128);
		this.dataGridView1.TabIndex = 0;
		this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
		this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
		this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(dataGridView1_KeyDown);
		this.err_type.HeaderText = "Error Type";
		this.err_type.Name = "err_type";
		this.err_count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
		this.err_count.HeaderText = "Count";
		this.err_count.Name = "err_count";
		this.err_count.Width = 60;
		this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.button1.Location = new System.Drawing.Point(179, 212);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 3;
		this.button1.Text = "Save";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.groupBox1.Controls.Add(this.numericUpDown1);
		this.groupBox1.Location = new System.Drawing.Point(6, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(74, 53);
		this.groupBox1.TabIndex = 5;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Total Pots";
		this.numericUpDown1.Location = new System.Drawing.Point(6, 20);
		this.numericUpDown1.Maximum = new decimal(new int[4] { 50, 0, 0, 0 });
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Size = new System.Drawing.Size(62, 21);
		this.numericUpDown1.TabIndex = 1;
		this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.numericUpDown1.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numericUpDown1.ValueChanged += new System.EventHandler(numericUpDown1_ValueChanged);
		this.groupBox2.Controls.Add(this.dataGridView1);
		this.groupBox2.Location = new System.Drawing.Point(6, 58);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(248, 148);
		this.groupBox2.TabIndex = 3;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Errors Description";
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Location = new System.Drawing.Point(86, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(168, 53);
		this.groupBox3.TabIndex = 6;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Quality Score";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(112, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(27, 26);
		this.label2.TabIndex = 8;
		this.label2.Text = "%";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(41, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(18, 26);
		this.label1.TabIndex = 7;
		this.label1.Text = "-";
		this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.button2.Location = new System.Drawing.Point(6, 212);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 2;
		this.button2.Text = "Clear";
		this.button2.UseVisualStyleBackColor = true;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(260, 244);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.button1);
		this.DoubleBuffered = true;
		this.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Errors_Form";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		base.TopMost = true;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(err_desc_FormClosing);
		base.Load += new System.EventHandler(err_desc_Load);
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		base.ResumeLayout(false);
	}
}
