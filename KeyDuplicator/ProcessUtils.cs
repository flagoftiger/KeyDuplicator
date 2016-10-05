using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KeyDuplicator
{
	static class ProcessUtils
	{
		const string APP_NAME = "Wow-64";
//		const string APP_NAME = "notepad";

		private static List<IntPtr> m_appHandles = null;

		// The GetForegroundWindow function returns a handle to the foreground window
		// (the window  with which the user is currently working).
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);


		public static bool GetAppHandles()
		{
			if (m_appHandles == null)
			{
				m_appHandles = new List<IntPtr>();
			}

			Process[] processes = Process.GetProcessesByName(APP_NAME);
			foreach (Process process in processes)
			{
				m_appHandles.Add(process.MainWindowHandle);
				m_appHandles.AddRange(GetAllChildHandles(process.MainWindowHandle));
				Console.WriteLine(String.Format("GetProcessesByName {0}:{1} {2} m_appHandles:{3}", process.ProcessName, process.MainWindowHandle, process.HandleCount, m_appHandles.Count));
			}
			return m_appHandles.Count > 1;
		}

		public static bool SendKeyMessage(GlobalKeyboardHookEventArgs e)
		{
			// Broadcast
			IntPtr focusedHandle = GetForegroundWindow();
			Console.WriteLine(String.Format("SendKeyMessage1 {0}:{1} {2}", focusedHandle, e.KeyboardState, e.KeyboardData.VirtualCode));
			if (m_appHandles.Contains(focusedHandle))
			{
				Console.WriteLine(String.Format("SendKeyMessage2 {0}:{1} {2}", focusedHandle, e.KeyboardState, e.KeyboardData.VirtualCode));
				foreach (IntPtr handle in m_appHandles)
				{
					PostMessage(handle, (uint)e.KeyboardState, e.KeyboardData.VirtualCode, 0);
				}
				return true;
			}
			return false;
		}

		public static List<IntPtr> GetAllChildHandles(IntPtr mainHandle)
		{
			List<IntPtr> childHandles = new List<IntPtr>();

			GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
			IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

			try
			{
				EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
				EnumChildWindows(mainHandle, childProc, pointerChildHandlesList);
			}
			finally
			{
				gcChildhandlesList.Free();
			}

			return childHandles;
		}

		private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
		{
			GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

			if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
			{
				return false;
			}

			List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
			childHandles.Add(hWnd);

			return true;
		}
	}
}
