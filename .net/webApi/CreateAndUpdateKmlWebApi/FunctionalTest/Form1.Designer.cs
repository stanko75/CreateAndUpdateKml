﻿namespace FunctionalTest
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
            label5 = new Label();
            imagesPath = new TextBox();
            label4 = new Label();
            tbGpsLocationsPath = new TextBox();
            label3 = new Label();
            address = new TextBox();
            folderName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            kmlFileName = new TextBox();
            UploadImage = new Button();
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
            panel1.Location = new Point(0, 159);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 268);
            panel1.TabIndex = 1;
            // 
            // log
            // 
            log.Dock = DockStyle.Fill;
            log.Location = new Point(0, 0);
            log.Multiline = true;
            log.Name = "log";
            log.Size = new Size(800, 268);
            log.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(imagesPath);
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
            panel2.Size = new Size(800, 159);
            panel2.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 124);
            label5.Name = "label5";
            label5.Size = new Size(75, 15);
            label5.TabIndex = 9;
            label5.Text = "Images path:";
            // 
            // imagesPath
            // 
            imagesPath.Location = new Point(118, 121);
            imagesPath.Name = "imagesPath";
            imagesPath.Size = new Size(608, 23);
            imagesPath.TabIndex = 8;
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
            // tbGpsLocationsPath
            // 
            tbGpsLocationsPath.Location = new Point(118, 41);
            tbGpsLocationsPath.Name = "tbGpsLocationsPath";
            tbGpsLocationsPath.Size = new Size(608, 23);
            tbGpsLocationsPath.TabIndex = 6;
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
            // UploadImage
            // 
            UploadImage.Dock = DockStyle.Bottom;
            UploadImage.Location = new Point(0, 404);
            UploadImage.Name = "UploadImage";
            UploadImage.Size = new Size(800, 23);
            UploadImage.TabIndex = 3;
            UploadImage.Text = "UploadImage";
            UploadImage.UseVisualStyleBackColor = true;
            UploadImage.Click += UploadImage_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(UploadImage);
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
        private Button UploadImage;
        private Label label5;
        private TextBox imagesPath;
    }
}
