namespace RustedWarfareTMXViewer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mapView = new System.Windows.Forms.PictureBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tOutput = new System.Windows.Forms.Label();
            this.lCredits = new System.Windows.Forms.Label();
            this.lTodo = new System.Windows.Forms.Label();
            this.tInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mapView)).BeginInit();
            this.SuspendLayout();
            // 
            // mapView
            // 
            this.mapView.BackColor = System.Drawing.Color.Gray;
            this.mapView.Location = new System.Drawing.Point(14, 13);
            this.mapView.Name = "mapView";
            this.mapView.Size = new System.Drawing.Size(558, 558);
            this.mapView.TabIndex = 0;
            this.mapView.TabStop = false;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.Green;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Bahnschrift SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(578, 13);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(267, 48);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import Map";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Teal;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Bahnschrift SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(577, 121);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(269, 48);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Export .png";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tOutput
            // 
            this.tOutput.AutoSize = true;
            this.tOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tOutput.ForeColor = System.Drawing.Color.White;
            this.tOutput.Location = new System.Drawing.Point(12, 574);
            this.tOutput.Name = "tOutput";
            this.tOutput.Size = new System.Drawing.Size(66, 25);
            this.tOutput.TabIndex = 2;
            this.tOutput.Text = "Map0";
            // 
            // lCredits
            // 
            this.lCredits.AutoSize = true;
            this.lCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCredits.ForeColor = System.Drawing.SystemColors.Control;
            this.lCredits.Location = new System.Drawing.Point(578, 427);
            this.lCredits.Name = "lCredits";
            this.lCredits.Size = new System.Drawing.Size(253, 144);
            this.lCredits.TabIndex = 3;
            this.lCredits.Text = resources.GetString("lCredits.Text");
            // 
            // lTodo
            // 
            this.lTodo.AutoSize = true;
            this.lTodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTodo.ForeColor = System.Drawing.SystemColors.Control;
            this.lTodo.Location = new System.Drawing.Point(578, 172);
            this.lTodo.Name = "lTodo";
            this.lTodo.Size = new System.Drawing.Size(168, 80);
            this.lTodo.TabIndex = 4;
            this.lTodo.Text = "UPCOMING FEATURES\r\n- Filters\r\n- Larger map support\r\n- File dimension options\r\n- I" +
    "ndividual layer inspection";
            this.lTodo.Click += new System.EventHandler(this.label1_Click);
            // 
            // tInfo
            // 
            this.tInfo.AutoSize = true;
            this.tInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tInfo.ForeColor = System.Drawing.Color.Lime;
            this.tInfo.Location = new System.Drawing.Point(190, 279);
            this.tInfo.Name = "tInfo";
            this.tInfo.Size = new System.Drawing.Size(200, 20);
            this.tInfo.TabIndex = 5;
            this.tInfo.Text = "THUMBNAIL IMPORTED";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(858, 608);
            this.Controls.Add(this.tInfo);
            this.Controls.Add(this.lTodo);
            this.Controls.Add(this.lCredits);
            this.Controls.Add(this.tOutput);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.mapView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "RusTMX";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mapView;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label tOutput;
        private System.Windows.Forms.Label lCredits;
        private System.Windows.Forms.Label lTodo;
        private System.Windows.Forms.Label tInfo;
    }
}

