﻿namespace GUI.Features.Ticket.subTicket
{
    partial class HistoryTicketControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(23, 29);
            button1.Name = "button1";
            button1.Size = new Size(206, 29);
            button1.TabIndex = 0;
            button1.Text = "tìm vé admin";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(809, 474);
            button2.Name = "button2";
            button2.Size = new Size(206, 29);
            button2.TabIndex = 1;
            button2.Text = "tìm vé admin";
            button2.UseVisualStyleBackColor = true;
            // 
            // HistoryTicketControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "HistoryTicketControl";
            Size = new Size(1219, 763);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}
