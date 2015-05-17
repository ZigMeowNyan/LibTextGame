using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibTextGame
{
	public class ConsoleDisplay: IDisplay
	{
		#region IDisplay Members

		public void Display(TextDefinition tDef, string sKey = "")
		{
		//}
		//private void DisplayThread(TextDefinition tDef, string sKey = "")
		//{
			if (tDef.SegmentStyles.ContainsKey(sKey))
			{
				for (int i = 0; i < tDef.SegmentStyles[sKey].Count; i++)
				{
					TextSegment thisSegment = tDef.SegmentStyles[sKey][i];
					string sOutput = thisSegment.ToString();
					if (thisSegment.BackgroundColor != null)
						Console.BackgroundColor = thisSegment.BackgroundColor.GetConsoleColor();
					if (thisSegment.FontColor != null)
						Console.ForegroundColor = thisSegment.FontColor.GetConsoleColor();
					if (!string.IsNullOrEmpty(thisSegment.Text))
					{
						if (thisSegment.DisplayUnitIntervalMS < 0)
						{
							Console.Write(thisSegment.Text);
						}
						else
						{
							switch (thisSegment.DisplayUnit)
							{
								case DisplayUnitEnum.Default:
									break;
								case DisplayUnitEnum.All:
									break;
								case DisplayUnitEnum.Character:
									for (int j = 0; j < thisSegment.Text.Length; j++)
									{
										if (Char.Equals(thisSegment.Text[j], '\n'))
											Console.WriteLine();
										else if (!Char.Equals(thisSegment.Text[j], '\r'))
											Console.Write(thisSegment.Text[j]);
										if (thisSegment.DisplayUnitIntervalMS > 0)
											Thread.Sleep(thisSegment.DisplayUnitIntervalMS);
									}
									break;
								case DisplayUnitEnum.Word:
									for (int j = 0; j < thisSegment.Text.Length; j++)
									{
										if (Char.Equals(thisSegment.Text[j], '\n'))
											Console.WriteLine();
										else if (!Char.Equals(thisSegment.Text[j], '\r'))
											Console.Write(thisSegment.Text[j]);
										if (Char.IsWhiteSpace(thisSegment.Text[j]) && thisSegment.DisplayUnitIntervalMS > 0)
											Thread.Sleep(thisSegment.DisplayUnitIntervalMS);
									}
									break;
								case DisplayUnitEnum.Line:
									for (int j = 0; j < thisSegment.Text.Length; j++)
									{
										if (Char.Equals(thisSegment.Text[j], '\n'))
											Console.WriteLine();
										else if (!Char.Equals(thisSegment.Text[j], '\r'))
											Console.Write(thisSegment.Text[j]);
										if ((Char.Equals('\n', thisSegment.Text)) && thisSegment.DisplayUnitIntervalMS > 0)
											Thread.Sleep(thisSegment.DisplayUnitIntervalMS);
									}
									break;
							}
						}
					}
				}
			}
		}

		#endregion

		//[DllImport("kernel32.dll",EntryPoint="SetConsoleTextAttribute")]
		//static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);
		#region Custom RGB colour set
		//TL;DR: Windows consoles can only display 16 colors. The palettes can be any RGB value, but still 16 colours.
		//Not going to bother with this until we have another display method that supports full RGB values.
		//Perhaps an alternate console method or WinForms/WPF/LG/DX display control.

		//[DllImport("kernel32.dll", EntryPoint="SetConsoleScreenBufferInfoEx", SetLastError = true)]
		//static extern bool SetConsoleScreenBufferInfoEx(
		//	IntPtr ConsoleOutput,
		//	ref CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfoEx
		//	);
		//http://stackoverflow.com/questions/1988833/converting-color-to-consolecolor/11188428#11188428
		//https://msdn.microsoft.com/en-us/library/windows/desktop/ms686039(v=vs.85).aspx
		//https://msdn.microsoft.com/en-us/library/windows/desktop/ms682091(v=vs.85).aspx
		//https://msdn.microsoft.com/en-us/library/windows/desktop/dd183449(v=vs.85).aspx
		//http://stackoverflow.com/questions/20168884/colouring-the-console-windows-with-rgb
		//http://stackoverflow.com/questions/9509278/rgb-specific-console-text-color-c
		//http://pinvoke.net/default.aspx/kernel32.SetConsoleScreenBufferInfoEx
		//http://stackoverflow.com/questions/7937256/changing-text-color-in-c-sharp-console-application/11188358#11188358


		#endregion
	}
}
