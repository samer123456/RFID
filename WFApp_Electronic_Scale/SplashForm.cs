using System;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
	public partial class SplashForm : Form
	{
		private Timer closeTimer;

		public SplashForm()
		{
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			closeTimer = new Timer();
			closeTimer.Interval = 2000; // 2 seconds
			closeTimer.Tick += (s, args) =>
			{
				closeTimer.Stop();
				closeTimer.Dispose();
				DialogResult = DialogResult.OK;
				Close();
			};
			closeTimer.Start();
		}
	}
}

