namespace Otgruzka
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxDPost = new System.Windows.Forms.ComboBox();
            this.comboBoxVes = new System.Windows.Forms.ComboBox();
            this.comboBoxStand = new System.Windows.Forms.ComboBox();
            this.comboBoxKlass = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxPlav = new System.Windows.Forms.ComboBox();
            this.comboBoxPak = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxProfil = new System.Windows.Forms.ComboBox();
            this.comboBoxDlina = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1337, 775);
            this.dataGridView1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьToolStripMenuItem,
            this.закрытьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1337, 24);
            this.menuStrip1.TabIndex = 19;
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.обновитьToolStripMenuItem.Text = "Обновить";
            this.обновитьToolStripMenuItem.Click += new System.EventHandler(this.обновитьToolStripMenuItem_Click);
            // 
            // закрытьToolStripMenuItem
            // 
            this.закрытьToolStripMenuItem.Name = "закрытьToolStripMenuItem";
            this.закрытьToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.закрытьToolStripMenuItem.Text = "Закрыть";
            this.закрытьToolStripMenuItem.Click += new System.EventHandler(this.закрытьToolStripMenuItem_Click);
            // 
            // comboBoxDPost
            // 
            this.comboBoxDPost.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxDPost.FormattingEnabled = true;
            this.comboBoxDPost.Location = new System.Drawing.Point(1169, 0);
            this.comboBoxDPost.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxDPost.Name = "comboBoxDPost";
            this.comboBoxDPost.Size = new System.Drawing.Size(115, 21);
            this.comboBoxDPost.TabIndex = 16;
            this.comboBoxDPost.SelectedIndexChanged += new System.EventHandler(this.comboBoxDPost_SelectedIndexChanged);
            // 
            // comboBoxVes
            // 
            this.comboBoxVes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxVes.FormattingEnabled = true;
            this.comboBoxVes.Location = new System.Drawing.Point(1002, 0);
            this.comboBoxVes.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxVes.Name = "comboBoxVes";
            this.comboBoxVes.Size = new System.Drawing.Size(128, 21);
            this.comboBoxVes.TabIndex = 15;
            this.comboBoxVes.SelectedIndexChanged += new System.EventHandler(this.comboBoxVes_SelectedIndexChanged);
            // 
            // comboBoxStand
            // 
            this.comboBoxStand.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxStand.FormattingEnabled = true;
            this.comboBoxStand.Location = new System.Drawing.Point(501, 0);
            this.comboBoxStand.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxStand.Name = "comboBoxStand";
            this.comboBoxStand.Size = new System.Drawing.Size(106, 21);
            this.comboBoxStand.TabIndex = 13;
            this.comboBoxStand.SelectedIndexChanged += new System.EventHandler(this.comboBoxStand_SelectedIndexChanged);
            // 
            // comboBoxKlass
            // 
            this.comboBoxKlass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxKlass.FormattingEnabled = true;
            this.comboBoxKlass.Location = new System.Drawing.Point(334, 0);
            this.comboBoxKlass.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxKlass.Name = "comboBoxKlass";
            this.comboBoxKlass.Size = new System.Drawing.Size(108, 21);
            this.comboBoxKlass.TabIndex = 12;
            this.comboBoxKlass.SelectedIndexChanged += new System.EventHandler(this.comboBoxKlass_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1337, 0);
            this.panel1.TabIndex = 20;
            // 
            // comboBoxPlav
            // 
            this.comboBoxPlav.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxPlav.FormattingEnabled = true;
            this.comboBoxPlav.Location = new System.Drawing.Point(668, 0);
            this.comboBoxPlav.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxPlav.Name = "comboBoxPlav";
            this.comboBoxPlav.Size = new System.Drawing.Size(106, 21);
            this.comboBoxPlav.TabIndex = 17;
            // 
            // comboBoxPak
            // 
            this.comboBoxPak.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxPak.FormattingEnabled = true;
            this.comboBoxPak.Location = new System.Drawing.Point(835, 0);
            this.comboBoxPak.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxPak.Name = "comboBoxPak";
            this.comboBoxPak.Size = new System.Drawing.Size(106, 21);
            this.comboBoxPak.TabIndex = 18;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.comboBoxDPost, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxPak, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxVes, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxProfil, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxPlav, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxDlina, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxKlass, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxStand, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1337, 22);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // comboBoxProfil
            // 
            this.comboBoxProfil.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxProfil.FormattingEnabled = true;
            this.comboBoxProfil.Location = new System.Drawing.Point(0, 0);
            this.comboBoxProfil.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxProfil.Name = "comboBoxProfil";
            this.comboBoxProfil.Size = new System.Drawing.Size(126, 21);
            this.comboBoxProfil.TabIndex = 2;
            this.comboBoxProfil.SelectedIndexChanged += new System.EventHandler(this.comboBoxProfil_SelectedIndexChanged);
            // 
            // comboBoxDlina
            // 
            this.comboBoxDlina.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.comboBoxDlina.FormattingEnabled = true;
            this.comboBoxDlina.Location = new System.Drawing.Point(167, 0);
            this.comboBoxDlina.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxDlina.Name = "comboBoxDlina";
            this.comboBoxDlina.Size = new System.Drawing.Size(133, 21);
            this.comboBoxDlina.TabIndex = 11;
            this.comboBoxDlina.SelectedIndexChanged += new System.EventHandler(this.comboBoxDlina_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1337, 823);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Наличие";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxDPost;
        private System.Windows.Forms.ComboBox comboBoxVes;
        private System.Windows.Forms.ComboBox comboBoxStand;
        private System.Windows.Forms.ComboBox comboBoxKlass;
        private System.Windows.Forms.ToolStripMenuItem закрытьToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxPak;
        private System.Windows.Forms.ComboBox comboBoxPlav;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBoxProfil;
        private System.Windows.Forms.ComboBox comboBoxDlina;
    }
}

