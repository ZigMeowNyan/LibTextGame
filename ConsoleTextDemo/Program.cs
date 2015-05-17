using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil;
using LibTextGame;
using NAudio;
using NAudio.Lame;

namespace ConsoleTextDemo
{
	public class Program
	{
		static void Main(string[] args)
		{
			string sMusicPath = "music";
			string sTextPath = "text";
			#region LoadFromArgs
			if (args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string sArg = args[i].ToLower();
					switch (sArg)
					{
						case "-music":
						case "/music":
						case "-m":
						case "/m":
							{
								i++;
								if (args.Length > i)
								{
									sMusicPath = args[i];
								}
							}
							break;
						case "/t":
						case "-t":
						case "/text":
						case "-Text":
							{
								i++;
								if (args.Length > i)
								{
									sTextPath = args[i];
								}
							}
						break;
					}
				}
			}
			#endregion
			using (SoundManager sm = new SoundManager())
			{
				sm.QueueMusic(sMusicPath);
				if (sm.IsMusicQueued)
				{
					sm.PlayNextInQueue();
				}
				else
				{
					Console.WriteLine("No music loaded. Place files in the \"Music\" sub-directory or specify another directory with the -m switch.");
					Console.WriteLine("Press any key to continue.");
					Console.ReadKey();
				}
				//string sOutput = JSON.Serialize(td, Options.ISO8601PrettyPrintExcludeNulls);//Example serialization
				if (System.IO.File.Exists(sTextPath))
				{
					string sFileContents = System.IO.File.ReadAllText(sTextPath);
					TextDefinition td = JSON.Deserialize<TextDefinition>(sFileContents);
					ConsoleDisplay cd = new ConsoleDisplay();
					Task t = new Task(() => cd.Display(td));
					t.Start();
					//Task t = Task.Run(() => cd.Display(td));
					t.Wait();
					sm.PlayNextInQueue();
				}
				else
				{
					Console.WriteLine("No display text loaded. Specify a text file to load with the -t switch.");
				}
				Console.WriteLine("Press any key to exit.");
				Console.ReadKey();
			}
		}
	}
}
