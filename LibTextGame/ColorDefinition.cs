using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LibTextGame;
using Jil;
namespace LibTextGame
{
	public class ColorDefinition
	{
		[IgnoreDataMember]
		public byte R { get; set; }
		[IgnoreDataMember]
		public byte B { get; set; }
		[IgnoreDataMember]
		public byte G { get; set; }
		[IgnoreDataMember]
		public byte? A { get; set; }
		[DataMember(Name="Hex")]
		public string HexValue
		{
			get
			{
				return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
			}
			set
			{
				switch (value.Length)
				{
					case 1:
						{
							R = byte.MinValue;
							G = byte.MinValue;
							byte? bVal = value.ByteFromHex(0, 1);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 2:
						{
							R = byte.MinValue;
							G = byte.MinValue;
							byte? bVal = value.ByteFromHex(0);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 3:
						{
							R = byte.MinValue;
							byte? bVal = value.ByteFromHex(0, 1);
							if (bVal.HasValue)
								G = bVal.Value;
							bVal = value.ByteFromHex(1);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 4:
						{
							R = byte.MinValue;
							byte? bVal = value.ByteFromHex(0);
							if (bVal.HasValue)
								G = bVal.Value;
							bVal = value.ByteFromHex(2);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 5:
						{
							byte? bVal = value.ByteFromHex(0, 1);
							if (bVal.HasValue)
								R = bVal.Value;
							bVal = value.ByteFromHex(1);
							if (bVal.HasValue)
								G = bVal.Value;
							bVal = value.ByteFromHex(3);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 6:
						{
							byte? bVal = value.ByteFromHex(0);
							if (bVal.HasValue)
								R = bVal.Value;
							bVal = value.ByteFromHex(2);
							if (bVal.HasValue)
								G = bVal.Value;
							bVal = value.ByteFromHex(4);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 7:
						{
							byte? bVal = value.ByteFromHex(0, 1);
							if (bVal.HasValue)
								A = bVal.Value;
							bVal = value.ByteFromHex(1);
							if (bVal.HasValue)
								R = bVal.Value;
							bVal = value.ByteFromHex(3);
							if (bVal.HasValue)
								G = bVal.Value;
							bVal = value.ByteFromHex(5);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
					case 8:
						{
							byte? bVal = value.ByteFromHex(0);
							if (bVal.HasValue)
								A = bVal.Value;
							bVal = value.ByteFromHex(2);
							if (bVal.HasValue)
								R = bVal.Value;
							bVal = value.ByteFromHex(4);
							if (bVal.HasValue)
								G = bVal.Value;
							bVal = value.ByteFromHex(6);
							if (bVal.HasValue)
								B = bVal.Value;
							break;
						}
				}
			}
		}
		[IgnoreDataMember]
		public string HexValueSansAlpha
		{
			get
			{
				return string.Format("{0:X2}{1:X2}{2:X2}", R, G, B);
			}
		}
		private float GetBrightness()
		{
			float fMin, fMax;
			fMin = Math.Min(Math.Min(R, G), B);
			fMax = Math.Max(Math.Max(R, G), B);
			return (fMin + fMax) / 510;
		}
		private float GetSaturation()
		{
			float fMin, fMax;
			fMin = Math.Min(Math.Min(R, G), B) / 255f;
			fMax = Math.Max(Math.Max(R, G), B) / 255f;
			if (!float.Equals(fMin, fMax))
			{
				float fAverage = (fMin + fMax) / 2;

				if (fAverage <= .5)
				{
					return (fMax - fMin) / (fMax + fMin);
				}
				else
				{
					return (fMax - fMin) / (2 - fMax + fMin);
				}

			}
			return 0f;
		}
		private float GetHue(bool AsDegree=false)
		{
			if (byte.Equals(R, G) && byte.Equals(G, B))
				return 0;
			//int iAddition = 0;// rank the three, subtract the non-Max values, divide by the difference between max and min.
			//if G is max, add 2.
			//if B is max, add 4.
			//multiply by 60 to get values between 0 and 360.
			// - or - divide by 6 to get values between 0 and 1.
			float fMin, fMax;
			float fR = R / 255f;
			float fG = G / 255f;
			float fB = B / 255f;
			fMin = Math.Min(Math.Min(fR, fG), fB);
			fMax = Math.Max(Math.Max(fR, fG), fB);
			float fDelta = fMax - fMin;
			float fHue;
			if (float.Equals(fMax, fR))
			{
				fHue = (fG - fB) / fDelta;
			}
			else if (float.Equals(fMax, fG))
			{
				fHue = 2 + (fB - fR) / fDelta;
			}
			else
			{
				fHue = 4 + (fR - fG) / fDelta;
			}
			if (!AsDegree)
				fHue = fHue / 6;
			else
				fHue = fHue * 60;
			return fHue;
		}
		private Tuple<float, float, float> ToHSL()
		{
			
			float fMin, fMax;
			float fR = R / 255f;
			float fG = G / 255f;
			float fB = B / 255f;
			fMin = Math.Min(Math.Min(fR, fG), fB);
			fMax = Math.Max(Math.Max(fR, fG), fB);
			float fH=0f, fS=0f;
			float fL = (fMax + fMin) / 2;
			if (!float.Equals(fMax, fMin))
			{
				float fDelta = fMax - fMin;
				if (fL > 0.5)
					fS = fDelta / (2 - fMax - fMin);
				else
					fS = fDelta / (fMin + fMin);

				if (float.Equals(fMax, fR))
				{
					fH = (fG - fB) / fDelta;
					if (fG < fB)
						fH += 6;
				}
				else if (float.Equals(fMax, fG))
				{
					fH = (fB - fR) / fDelta + 2;
				}
				else
				{
					fH = (fR - fG) / fDelta + 4;
				}
				fH = fH / 6;
			}
			Tuple<float, float, float> hsl = new Tuple<float, float, float>(fH, fS, fL);
			return hsl;

		}
		private Tuple<float, float> GetMinMax()
		{
			float fMin, fMax;
			fMin = Math.Min(Math.Min(R, G), B);
			fMax = Math.Max(Math.Max(R, G), B);
			return new Tuple<float, float>(fMin, fMax);
		}
		#region Constructors
		public ColorDefinition() { }
		public ColorDefinition(int iRed, int iBlue, int iGreen)
			: this((byte)iRed, (byte)iBlue, (byte)iGreen)
		{
		}
		public ColorDefinition(int iAlpha, int iRed, int iBlue, int iGreen)
			: this((byte)iAlpha, (byte)iRed, (byte)iBlue, (byte)iGreen)
		{
		}
		public ColorDefinition(byte iAlpha, byte iRed, byte iBlue, byte iGreen)
			: this(iRed, iBlue, iGreen)
		{
			A = iAlpha;
		}
		public ColorDefinition(byte iRed, byte iBlue, byte iGreen)
		{
			R = iRed;
			B = iBlue;
			G = iGreen;
		}
		public ColorDefinition(string sColorString)
		{
			if (Char.Equals(sColorString[0], '#'))
			{
				HexValue = sColorString.Substring(1);
			}
			else
			{
				HexValue = sColorString;
				//throw new ArgumentException("This color string is in an unrecognized format.");
			}
		}
		#endregion
		public ConsoleColor GetConsoleColor()
		{
			Tuple<float, float, float> hsl = ToHSL();
			if (hsl.Item2 < 0.5)
			{//Grayish
				switch ((int)(hsl.Item3 * 3.5))
				{
					case 0: return ConsoleColor.Black;
					case 1: return ConsoleColor.DarkGray;
					case 2: return ConsoleColor.Gray;
					default: return ConsoleColor.White;
				}
			}
			//float fHue = GetHue();
			int hue = (int)Math.Round(hsl.Item1 * 6, MidpointRounding.AwayFromZero);
			//float fBrightNess = GetBrightness();
			if (hsl.Item3 < 0.4)
			{//Dark
				switch (hue)
				{
					case 1: return ConsoleColor.DarkYellow;
					case 2: return ConsoleColor.DarkGreen;
					case 3: return ConsoleColor.DarkCyan;
					case 4: return ConsoleColor.DarkBlue;
					case 5: return ConsoleColor.DarkMagenta;
					default: return ConsoleColor.DarkRed;
				}
			}
			else
			{//Light
				switch (hue)
				{
					case 1: return ConsoleColor.Yellow;
					case 2: return ConsoleColor.Green;
					case 3: return ConsoleColor.Cyan;
					case 4: return ConsoleColor.Blue;
					case 5: return ConsoleColor.Magenta;
					default: return ConsoleColor.Red;
				}
			}
		}
		public override string ToString()
		{
			return HexValue;
		}
		public static ColorDefinition FromHexColorString(string sHexColorString)
		{
			return new ColorDefinition(sHexColorString);
		}
	}
}
