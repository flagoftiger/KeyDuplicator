using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

namespace KeyDuplicator
{
	static class Program
	{
		const int KEY_SIZE = 255;
		static bool[] KEY_FILTER = new bool[KEY_SIZE];
		const int KEY_START = 0x13; // VK_PAUSE

		private static bool s_running = false;
		private static KeyDupForm s_form = null;

		internal class GlobalKeyboardHookController : IDisposable
		{
			private GlobalKeyboardHook m_globalKeyboardHook;

			public void SetupKeyboardHooks()
			{
				m_globalKeyboardHook = new GlobalKeyboardHook();
				m_globalKeyboardHook.KeyboardPressed += OnKeyPressed;
			}

			private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
			{
				if (e.KeyboardData.VirtualCode == KEY_START && e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
				{
					s_running = !s_running;
					if (s_running)
					{
						if (!ProcessUtils.GetAppHandles())
						{
							MessageBox.Show("Please make sure you have at least two instances of the app.");
							s_running = false;
						}
					}
					s_form.SetOnOffLable(s_running);
					return;
				}

				if (s_running)
				{
					Console.WriteLine(String.Format("OnKeyPressed {0} {1:X}", e.KeyboardState, e.KeyboardData.VirtualCode));

					// Filter keys and return if it's not needed
					if (KEY_FILTER[e.KeyboardData.VirtualCode] /*&& e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown*/)
					{
						e.Handled = ProcessUtils.SendKeyMessage(e);
					}
				}
			}

			public void Dispose()
			{
				m_globalKeyboardHook?.Dispose();
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Initialize();
			Application.Run(s_form);
		}

		static void Initialize()
		{
			// Key filter
			// http://www.kbdedit.com/manual/low_level_vk_list.html
			for (int i = 0; i < KEY_SIZE; i++)
			{
				KEY_FILTER[i] = false;
			}
			KEY_FILTER['W'] = true; // VK_KEY_W
			KEY_FILTER['R'] = true; // VK_KEY_R
			KEY_FILTER['Z'] = true; // VK_KEY_Z
			KEY_FILTER[0x20] = true; // VK_SPACE
			KEY_FILTER[0x6E] = true; // VK_DECIMAL

			// Set global keyboard hook
			GlobalKeyboardHookController controller = new GlobalKeyboardHookController();
			controller.SetupKeyboardHooks();

			// Initialize Form
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			s_form = new KeyDupForm();
			s_form.SetOnOffLable(s_running);
		}

	}
}
