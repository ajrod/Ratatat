namespace Ratatat
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            this.PopLabel = new System.Windows.Forms.Label();
            this.currentRat = new System.Windows.Forms.Label();
            this.currentGeneration = new System.Windows.Forms.Label();
            this.currentFitness = new System.Windows.Forms.Label();
            this.avgFitness = new System.Windows.Forms.Label();
            this.runTime = new System.Windows.Forms.Label();
            this.topFitness = new System.Windows.Forms.Label();
            this.mutationLabel = new System.Windows.Forms.Label();
            this.crossoverLabel = new System.Windows.Forms.Label();
            this.eliteLabel = new System.Windows.Forms.Label();
            this.pauseLabel = new System.Windows.Forms.Label();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GameTimer
            // 
            this.GameTimer.Enabled = true;
            this.GameTimer.Interval = 1;
            this.GameTimer.Tick += new System.EventHandler(this.timer_tick);
            // 
            // PopLabel
            // 
            this.PopLabel.AutoSize = true;
            this.PopLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PopLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PopLabel.ForeColor = System.Drawing.Color.Red;
            this.PopLabel.Location = new System.Drawing.Point(821, 40);
            this.PopLabel.Name = "PopLabel";
            this.PopLabel.Size = new System.Drawing.Size(139, 20);
            this.PopLabel.TabIndex = 0;
            this.PopLabel.Text = "Population Size:";
            // 
            // currentRat
            // 
            this.currentRat.AutoSize = true;
            this.currentRat.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.currentRat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRat.ForeColor = System.Drawing.Color.Red;
            this.currentRat.Location = new System.Drawing.Point(818, 251);
            this.currentRat.Name = "currentRat";
            this.currentRat.Size = new System.Drawing.Size(123, 20);
            this.currentRat.TabIndex = 1;
            this.currentRat.Text = "Current Rat: 0";
            // 
            // currentGeneration
            // 
            this.currentGeneration.AutoSize = true;
            this.currentGeneration.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.currentGeneration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentGeneration.ForeColor = System.Drawing.Color.Red;
            this.currentGeneration.Location = new System.Drawing.Point(818, 9);
            this.currentGeneration.Name = "currentGeneration";
            this.currentGeneration.Size = new System.Drawing.Size(119, 20);
            this.currentGeneration.TabIndex = 2;
            this.currentGeneration.Text = "Generation: 0";
            // 
            // currentFitness
            // 
            this.currentFitness.AutoSize = true;
            this.currentFitness.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.currentFitness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentFitness.ForeColor = System.Drawing.Color.Red;
            this.currentFitness.Location = new System.Drawing.Point(818, 285);
            this.currentFitness.Name = "currentFitness";
            this.currentFitness.Size = new System.Drawing.Size(88, 20);
            this.currentFitness.TabIndex = 3;
            this.currentFitness.Text = "Fitness: 0";
            // 
            // avgFitness
            // 
            this.avgFitness.AutoSize = true;
            this.avgFitness.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.avgFitness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avgFitness.ForeColor = System.Drawing.Color.Red;
            this.avgFitness.Location = new System.Drawing.Point(821, 181);
            this.avgFitness.MaximumSize = new System.Drawing.Size(175, 0);
            this.avgFitness.Name = "avgFitness";
            this.avgFitness.Size = new System.Drawing.Size(123, 20);
            this.avgFitness.TabIndex = 4;
            this.avgFitness.Text = "Avg Fitness: 0\r\n";
            this.avgFitness.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // runTime
            // 
            this.runTime.AutoSize = true;
            this.runTime.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.runTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runTime.ForeColor = System.Drawing.Color.Red;
            this.runTime.Location = new System.Drawing.Point(12, 262);
            this.runTime.Name = "runTime";
            this.runTime.Size = new System.Drawing.Size(90, 20);
            this.runTime.TabIndex = 5;
            this.runTime.Text = "Run Time:";
            this.runTime.Visible = false;
            // 
            // topFitness
            // 
            this.topFitness.AutoSize = true;
            this.topFitness.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.topFitness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topFitness.ForeColor = System.Drawing.Color.Red;
            this.topFitness.Location = new System.Drawing.Point(821, 218);
            this.topFitness.Name = "topFitness";
            this.topFitness.Size = new System.Drawing.Size(130, 20);
            this.topFitness.TabIndex = 6;
            this.topFitness.Text = "Best Fitness: 0";
            // 
            // mutationLabel
            // 
            this.mutationLabel.AutoSize = true;
            this.mutationLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.mutationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mutationLabel.ForeColor = System.Drawing.Color.Red;
            this.mutationLabel.Location = new System.Drawing.Point(821, 75);
            this.mutationLabel.Name = "mutationLabel";
            this.mutationLabel.Size = new System.Drawing.Size(123, 20);
            this.mutationLabel.TabIndex = 7;
            this.mutationLabel.Text = "Mutation Rate";
            // 
            // crossoverLabel
            // 
            this.crossoverLabel.AutoSize = true;
            this.crossoverLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.crossoverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crossoverLabel.ForeColor = System.Drawing.Color.Red;
            this.crossoverLabel.Location = new System.Drawing.Point(821, 111);
            this.crossoverLabel.Name = "crossoverLabel";
            this.crossoverLabel.Size = new System.Drawing.Size(133, 20);
            this.crossoverLabel.TabIndex = 8;
            this.crossoverLabel.Text = "Crossover Rate";
            // 
            // eliteLabel
            // 
            this.eliteLabel.AutoSize = true;
            this.eliteLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.eliteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eliteLabel.ForeColor = System.Drawing.Color.Red;
            this.eliteLabel.Location = new System.Drawing.Point(821, 145);
            this.eliteLabel.Name = "eliteLabel";
            this.eliteLabel.Size = new System.Drawing.Size(104, 20);
            this.eliteLabel.TabIndex = 9;
            this.eliteLabel.Text = "Elitism (%): ";
            // 
            // pauseLabel
            // 
            this.pauseLabel.AutoSize = true;
            this.pauseLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pauseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseLabel.ForeColor = System.Drawing.Color.Red;
            this.pauseLabel.Location = new System.Drawing.Point(818, 321);
            this.pauseLabel.Name = "pauseLabel";
            this.pauseLabel.Size = new System.Drawing.Size(89, 20);
            this.pauseLabel.TabIndex = 10;
            this.pauseLabel.Text = "Paused: ..";
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.instructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionsLabel.ForeColor = System.Drawing.Color.Red;
            this.instructionsLabel.Location = new System.Drawing.Point(819, 355);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(279, 225);
            this.instructionsLabel.TabIndex = 11;
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1098, 602);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.pauseLabel);
            this.Controls.Add(this.eliteLabel);
            this.Controls.Add(this.crossoverLabel);
            this.Controls.Add(this.mutationLabel);
            this.Controls.Add(this.topFitness);
            this.Controls.Add(this.runTime);
            this.Controls.Add(this.avgFitness);
            this.Controls.Add(this.currentFitness);
            this.Controls.Add(this.currentGeneration);
            this.Controls.Add(this.currentRat);
            this.Controls.Add(this.PopLabel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Alexs GA Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.Label PopLabel;
        private System.Windows.Forms.Label currentRat;
        private System.Windows.Forms.Label currentGeneration;
        private System.Windows.Forms.Label currentFitness;
        private System.Windows.Forms.Label avgFitness;
        private System.Windows.Forms.Label runTime;
        private System.Windows.Forms.Label topFitness;
        private System.Windows.Forms.Label mutationLabel;
        private System.Windows.Forms.Label crossoverLabel;
        private System.Windows.Forms.Label eliteLabel;
        private System.Windows.Forms.Label pauseLabel;
        private System.Windows.Forms.Label instructionsLabel;
    }
}