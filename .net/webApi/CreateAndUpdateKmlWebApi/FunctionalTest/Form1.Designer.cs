namespace FunctionalTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PostGpsPositionsFromFilesWithFileName = new Button();
            panel1 = new Panel();
            log = new TextBox();
            panel2 = new Panel();
            label3 = new Label();
            address = new TextBox();
            folderName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            kmlFileName = new TextBox();
            tbGpsLocationsPath = new TextBox();
            label4 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // PostGpsPositionsFromFilesWithFileName
            // 
            PostGpsPositionsFromFilesWithFileName.Dock = DockStyle.Bottom;
            PostGpsPositionsFromFilesWithFileName.Location = new Point(0, 427);
            PostGpsPositionsFromFilesWithFileName.Name = "PostGpsPositionsFromFilesWithFileName";
            PostGpsPositionsFromFilesWithFileName.Size = new Size(800, 23);
            PostGpsPositionsFromFilesWithFileName.TabIndex = 0;
            PostGpsPositionsFromFilesWithFileName.Text = "PostGpsPositionsFromFilesWithFileName";
            PostGpsPositionsFromFilesWithFileName.UseVisualStyleBackColor = true;
            PostGpsPositionsFromFilesWithFileName.Click += PostGpsPositionsFromFilesWithFileName_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(log);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 124);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 303);
            panel1.TabIndex = 1;
            // 
            // log
            // 
            log.Dock = DockStyle.Fill;
            log.Location = new Point(0, 0);
            log.Multiline = true;
            log.Name = "log";
            log.Size = new Size(800, 303);
            log.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(label4);
            panel2.Controls.Add(tbGpsLocationsPath);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(address);
            panel2.Controls.Add(folderName);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(kmlFileName);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 124);
            panel2.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 15);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 5;
            label3.Text = "Address:";
            // 
            // address
            // 
            address.Location = new Point(118, 12);
            address.Name = "address";
            address.Size = new Size(608, 23);
            address.TabIndex = 4;
            // 
            // folderName
            // 
            folderName.Location = new Point(118, 95);
            folderName.Name = "folderName";
            folderName.Size = new Size(157, 23);
            folderName.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 95);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 2;
            label2.Text = "Folder name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 69);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 1;
            label1.Text = "Kml file name:";
            // 
            // kmlFileName
            // 
            kmlFileName.Location = new Point(118, 66);
            kmlFileName.Name = "kmlFileName";
            kmlFileName.Size = new Size(608, 23);
            kmlFileName.TabIndex = 0;
            // 
            // tbGpsLocationsPath
            // 
            tbGpsLocationsPath.Location = new Point(118, 41);
            tbGpsLocationsPath.Name = "tbGpsLocationsPath";
            tbGpsLocationsPath.Size = new Size(608, 23);
            tbGpsLocationsPath.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 41);
            label4.Name = "label4";
            label4.Size = new Size(103, 15);
            label4.TabIndex = 7;
            label4.Text = "Gps location path:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(PostGpsPositionsFromFilesWithFileName);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button PostGpsPositionsFromFilesWithFileName;
        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private TextBox kmlFileName;
        private TextBox log;
        private TextBox folderName;
        private Label label2;
        private Label label3;
        private TextBox address;
        private Label label4;
        private TextBox tbGpsLocationsPath;
    }
}
