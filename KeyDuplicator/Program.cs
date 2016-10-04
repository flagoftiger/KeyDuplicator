using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyDuplicator
{


	static class Program
	{
		internal class GlobalKeyboardHookController : IDisposable
		{
			private GlobalKeyboardHook _globalKeyboardHook;

			public void SetupKeyboardHooks()
			{
				_globalKeyboardHook = new GlobalKeyboardHook();
				_globalKeyboardHook.KeyboardPressed += OnKeyPressed;
			}

			private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
			{
				//MessageBox.Show(e.KeyboardData.VirtualCode.ToString());
				//Debug.WriteLine(e.KeyboardData.VirtualCode);

				if (e.KeyboardData.VirtualCode != GlobalKeyboardHook.VkSnapshot)
					return;

				// seems, not needed in the life.
				//if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown &&
				//    e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
				//{
				//    MessageBox.Show("Alt + Print Screen");
				//    e.Handled = true;
				//}
				//else

				if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
				{
					MessageBox.Show("Print Screen");
					e.Handled = true;
				}
			}

			public void Dispose()
			{
				_globalKeyboardHook?.Dispose();
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			GlobalKeyboardHookController controller = new GlobalKeyboardHookController();
			controller.SetupKeyboardHooks();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
