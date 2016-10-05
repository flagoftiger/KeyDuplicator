namespace KeyDuplicator
{
	partial class KeyDupForm
	{
		private const string ON_TEXT = "[ON] press PAUSE to stop";
		private const string OFF_TEXT = "[OFF] press PAUSE to start";

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

		public void SetOnOffLable(bool on)
		{
			if (on)
			{
				this.m_onOffLable.Text = ON_TEXT;
			}
			else
			{
				this.m_onOffLable.Text = OFF_TEXT;
			}
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_onOffLable = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_onOffLable
			// 
			this.m_onOffLable.AutoSize = true;
			this.m_onOffLable.Location = new System.Drawing.Point(12, 23);
			this.m_onOffLable.Name = "m_onOffLable";
			this.m_onOffLable.Size = new System.Drawing.Size(0, 13);
			this.m_onOffLable.TabIndex = 0;
			// 
			// KeyDupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(188, 65);
			this.Controls.Add(this.m_onOffLable);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "KeyDupForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "KeyDup";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label m_onOffLable;
	}
}

