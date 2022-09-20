/*
    Written by Stanley Varghese
    This module (LibraryConsoleDesigner.cs) is auto generated file for WinForms elements I added via UI in visual studio
    Module contains logic for setting everything up behind the scenes. I simply chose elements, positioning, and assigned element names via designer UI 
    I did not manipulate this file outside of this comment header as it is auto generated
*/
namespace DBProgramming
{
    partial class LibraryConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TitleLabel = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.PublisherLabel = new System.Windows.Forms.Label();
            this.PublisherComboBox = new System.Windows.Forms.ComboBox();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.Isbn_label = new System.Windows.Forms.Label();
            this.AuthorTextBox = new System.Windows.Forms.TextBox();
            this.IsbnTextBox = new System.Windows.Forms.TextBox();
            this.DeweyTextBox = new System.Windows.Forms.TextBox();
            this.DeweyNumberLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.DataGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.EditionTextBox = new System.Windows.Forms.TextBox();
            this.EditionLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.PageNumLabel = new System.Windows.Forms.Label();
            this.PageNumTextBox = new System.Windows.Forms.TextBox();
            this.DateLabel = new System.Windows.Forms.Label();
            this.DateTextBox = new System.Windows.Forms.TextBox();
            this.DateFormatLabel = new System.Windows.Forms.Label();
            this.CongressLabel = new System.Windows.Forms.Label();
            this.congressTextBox = new System.Windows.Forms.TextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Location = new System.Drawing.Point(61, 19);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(30, 13);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title:";
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Location = new System.Drawing.Point(88, 16);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(816, 20);
            this.TitleTextBox.TabIndex = 1;
            // 
            // PublisherLabel
            // 
            this.PublisherLabel.AutoSize = true;
            this.PublisherLabel.Location = new System.Drawing.Point(38, 137);
            this.PublisherLabel.Name = "PublisherLabel";
            this.PublisherLabel.Size = new System.Drawing.Size(53, 13);
            this.PublisherLabel.TabIndex = 2;
            this.PublisherLabel.Text = "Publisher:";
            // 
            // PublisherComboBox
            // 
            this.PublisherComboBox.AllowDrop = true;
            this.PublisherComboBox.FormattingEnabled = true;
            this.PublisherComboBox.Location = new System.Drawing.Point(88, 129);
            this.PublisherComboBox.Name = "PublisherComboBox";
            this.PublisherComboBox.Size = new System.Drawing.Size(221, 21);
            this.PublisherComboBox.TabIndex = 3;
            this.PublisherComboBox.SelectedIndexChanged += new System.EventHandler(this.PublisherComboBox_SelectedIndexChanged);
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(39, 48);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(52, 13);
            this.AuthorLabel.TabIndex = 4;
            this.AuthorLabel.Text = "Author(s):";
            // 
            // Isbn_label
            // 
            this.Isbn_label.AutoSize = true;
            this.Isbn_label.Location = new System.Drawing.Point(56, 75);
            this.Isbn_label.Name = "Isbn_label";
            this.Isbn_label.Size = new System.Drawing.Size(35, 13);
            this.Isbn_label.TabIndex = 5;
            this.Isbn_label.Text = "ISBN:";
            // 
            // AuthorTextBox
            // 
            this.AuthorTextBox.Location = new System.Drawing.Point(88, 45);
            this.AuthorTextBox.Name = "AuthorTextBox";
            this.AuthorTextBox.Size = new System.Drawing.Size(816, 20);
            this.AuthorTextBox.TabIndex = 6;
            // 
            // IsbnTextBox
            // 
            this.IsbnTextBox.Location = new System.Drawing.Point(88, 72);
            this.IsbnTextBox.Name = "IsbnTextBox";
            this.IsbnTextBox.Size = new System.Drawing.Size(221, 20);
            this.IsbnTextBox.TabIndex = 7;
            // 
            // DeweyTextBox
            // 
            this.DeweyTextBox.Location = new System.Drawing.Point(88, 101);
            this.DeweyTextBox.Name = "DeweyTextBox";
            this.DeweyTextBox.Size = new System.Drawing.Size(221, 20);
            this.DeweyTextBox.TabIndex = 9;
            // 
            // DeweyNumberLabel
            // 
            this.DeweyNumberLabel.AutoSize = true;
            this.DeweyNumberLabel.Location = new System.Drawing.Point(8, 104);
            this.DeweyNumberLabel.Name = "DeweyNumberLabel";
            this.DeweyNumberLabel.Size = new System.Drawing.Size(83, 13);
            this.DeweyNumberLabel.TabIndex = 8;
            this.DeweyNumberLabel.Text = "Dewey Number:";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(744, 206);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 10;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(842, 206);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 11;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(935, 206);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // DataGrid
            // 
            this.DataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Location = new System.Drawing.Point(15, 235);
            this.DataGrid.MultiSelect = false;
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGrid.Size = new System.Drawing.Size(1105, 397);
            this.DataGrid.TabIndex = 13;
            this.DataGrid.SelectionChanged += new System.EventHandler(this.DataGrid_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(367, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 15;
            // 
            // EditionTextBox
            // 
            this.EditionTextBox.Location = new System.Drawing.Point(416, 102);
            this.EditionTextBox.Name = "EditionTextBox";
            this.EditionTextBox.Size = new System.Drawing.Size(29, 20);
            this.EditionTextBox.TabIndex = 17;
            // 
            // EditionLabel
            // 
            this.EditionLabel.AutoSize = true;
            this.EditionLabel.Location = new System.Drawing.Point(373, 105);
            this.EditionLabel.Name = "EditionLabel";
            this.EditionLabel.Size = new System.Drawing.Size(42, 13);
            this.EditionLabel.TabIndex = 18;
            this.EditionLabel.Text = "Edition:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 635);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1132, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(145, 17);
            this.toolStripStatusLabel1.Text = "Mode: Entering New Book";
            // 
            // PageNumLabel
            // 
            this.PageNumLabel.AutoSize = true;
            this.PageNumLabel.Location = new System.Drawing.Point(361, 136);
            this.PageNumLabel.Name = "PageNumLabel";
            this.PageNumLabel.Size = new System.Drawing.Size(54, 13);
            this.PageNumLabel.TabIndex = 21;
            this.PageNumLabel.Text = "PageNum";
            // 
            // PageNumTextBox
            // 
            this.PageNumTextBox.Location = new System.Drawing.Point(416, 129);
            this.PageNumTextBox.Name = "PageNumTextBox";
            this.PageNumTextBox.Size = new System.Drawing.Size(51, 20);
            this.PageNumTextBox.TabIndex = 22;
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(6, 190);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(85, 13);
            this.DateLabel.TabIndex = 23;
            this.DateLabel.Text = "Publication Date";
            // 
            // DateTextBox
            // 
            this.DateTextBox.Location = new System.Drawing.Point(92, 187);
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(100, 20);
            this.DateTextBox.TabIndex = 24;
            // 
            // DateFormatLabel
            // 
            this.DateFormatLabel.AutoSize = true;
            this.DateFormatLabel.Location = new System.Drawing.Point(199, 189);
            this.DateFormatLabel.Name = "DateFormatLabel";
            this.DateFormatLabel.Size = new System.Drawing.Size(106, 13);
            this.DateFormatLabel.TabIndex = 25;
            this.DateFormatLabel.Text = "*Format MM/dd/yyyy";
            // 
            // CongressLabel
            // 
            this.CongressLabel.AutoSize = true;
            this.CongressLabel.Location = new System.Drawing.Point(325, 75);
            this.CongressLabel.Name = "CongressLabel";
            this.CongressLabel.Size = new System.Drawing.Size(94, 13);
            this.CongressLabel.TabIndex = 26;
            this.CongressLabel.Text = "Congress Number:";
            // 
            // congressTextBox
            // 
            this.congressTextBox.Location = new System.Drawing.Point(416, 71);
            this.congressTextBox.Name = "congressTextBox";
            this.congressTextBox.Size = new System.Drawing.Size(150, 20);
            this.congressTextBox.TabIndex = 27;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(18, 156);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(63, 13);
            this.DescriptionLabel.TabIndex = 28;
            this.DescriptionLabel.Text = "Description:";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Location = new System.Drawing.Point(88, 156);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(816, 20);
            this.DescriptionTextBox.TabIndex = 29;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(1027, 205);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 30;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // LibraryConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1132, 657);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.DescriptionTextBox);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.congressTextBox);
            this.Controls.Add(this.CongressLabel);
            this.Controls.Add(this.DateFormatLabel);
            this.Controls.Add(this.DateTextBox);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.PageNumTextBox);
            this.Controls.Add(this.PageNumLabel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.EditionLabel);
            this.Controls.Add(this.EditionTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataGrid);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.DeweyTextBox);
            this.Controls.Add(this.DeweyNumberLabel);
            this.Controls.Add(this.IsbnTextBox);
            this.Controls.Add(this.AuthorTextBox);
            this.Controls.Add(this.Isbn_label);
            this.Controls.Add(this.AuthorLabel);
            this.Controls.Add(this.PublisherComboBox);
            this.Controls.Add(this.PublisherLabel);
            this.Controls.Add(this.TitleTextBox);
            this.Controls.Add(this.TitleLabel);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "LibraryConsole";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Label PublisherLabel;
        private System.Windows.Forms.ComboBox PublisherComboBox;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.Label Isbn_label;
        private System.Windows.Forms.TextBox AuthorTextBox;
        private System.Windows.Forms.TextBox IsbnTextBox;
        private System.Windows.Forms.TextBox DeweyTextBox;
        private System.Windows.Forms.Label DeweyNumberLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.DataGridView DataGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EditionTextBox;
        private System.Windows.Forms.Label EditionLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label PageNumLabel;
        private System.Windows.Forms.TextBox PageNumTextBox;
        private System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.TextBox DateTextBox;
        private System.Windows.Forms.Label DateFormatLabel;
        private System.Windows.Forms.Label CongressLabel;
        private System.Windows.Forms.TextBox congressTextBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Button DeleteButton;
    }
}

