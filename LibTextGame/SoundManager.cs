using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTextGame
{
	public class SoundManager:IDisposable
	{
		//TODO: Attempt cross-fading on music?
		//TODO: SFX support?
		private string _CurrentMusicFile;
		public string CurrentMusicFile
		{
			get
			{
				return _CurrentMusicFile;
			}
		}
		public ConcurrentQueue<string> QueuedMusicFiles { get; set; }
		private NAudio.Wave.IWavePlayer _wPlayer;
		private NAudio.Wave.WaveStream _musicProvider;
		public bool IsMusicQueued
		{
			get
			{
				string sVal = null;
				return (QueuedMusicFiles.TryPeek(out sVal));
			}
		}

		public SoundManager()
		{
			_wPlayer = new NAudio.Wave.WaveOut();
			QueuedMusicFiles = new ConcurrentQueue<string>();
		}
		public void QueueMusic(string sMusicPath, bool bThrowOnError = false)
		{
			if (File.Exists(sMusicPath))
			{
				QueuedMusicFiles.Enqueue(sMusicPath);
			}
			else if (Directory.Exists(sMusicPath))
			{
				string[] sFilesInDir = Directory.GetFiles(sMusicPath);
				//TODO: Recursive (i.e. Sub-directories)?
				for (int i = 0; i < sFilesInDir.Length; i++)
				{//TODO: Random loading?
					QueuedMusicFiles.Enqueue(sFilesInDir[i]);
				}
			}
			else
			{
				if (bThrowOnError)
				{
					throw new FileNotFoundException(string.Format("File '{0}' could not be found.", sMusicPath));
				}
				else
				{
				}
			}
		}
		public void PlayMusic(string sFilePath, bool bThrowOnError = false)
		{
			if (File.Exists(sFilePath))
			{
				//try
				//{
				NAudio.Wave.WaveStream iwpNewFile = GetProvider(sFilePath);
				_wPlayer.Stop();
				if (_musicProvider != null)
				{
					_musicProvider.Dispose();
					_musicProvider = null;
				}
				_musicProvider = iwpNewFile;
				_wPlayer.Init(_musicProvider);
				_wPlayer.Play();
				_CurrentMusicFile = sFilePath;
				//}
				//catch (Exception ex)
				//{

				//}
			}
			else
			{
				if (bThrowOnError)
				{
					throw new FileNotFoundException(string.Format("File '{0}' could not be found.", sFilePath));
				}
				else
				{
				}
			}
		}
		public void PlayNextInQueue(bool throwOnNoFilesQueued = false, bool EmptyQueue = false)
		{
			string sNextFile = null;
			if (QueuedMusicFiles.TryDequeue(out sNextFile))
			{
				PlayMusic(sNextFile);
			}
			else
			{
				if (throwOnNoFilesQueued)
				{
					throw new ArgumentOutOfRangeException("No music files queued for playback.");
				}
			}
			if (EmptyQueue)
			{
				while (QueuedMusicFiles.TryDequeue(out sNextFile))
				{
				}
			}
		}
		private NAudio.Wave.WaveStream GetProvider(string sFile)
		{
			string sExt = Path.GetExtension(sFile);
			NAudio.Wave.WaveStream iwpNewFile = null;
			try
			{
				switch (sExt.ToLower())
				{
					case ".ogg":
						iwpNewFile = new NAudio.Vorbis.VorbisWaveReader(sFile);
						break;
					case ".mp3":
					case ".wav":
					default:
						iwpNewFile = new NAudio.Wave.AudioFileReader(sFile);
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Error reading file {0}: {1}", sFile, ex.Message));
				//Handle exception
			}
			return iwpNewFile;
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (_wPlayer != null)
			{
				_wPlayer.Stop();
				_wPlayer.Dispose();
				_wPlayer = null;
			}
			if (_musicProvider != null)
			{
				_musicProvider.Dispose();
				_musicProvider = null;
			}
		}

		#endregion
	}
}
