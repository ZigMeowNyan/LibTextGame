using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTextGame
{
	public static class Extensions
	{
		public static byte? ByteFromHex(this string hex, int iStartIndex = 0, int iLength = 2)
		{
			try
			{
				switch(iLength)
				{
					case 2:
						return (byte)((GetHexVal(hex[iStartIndex]) << 4) + (GetHexVal(hex[iStartIndex + 1])));
					case 1:
						return (byte)(GetHexVal(hex[iStartIndex]));
					default:
						return null;
				}
			}
			catch (Exception ex)
			{
				//LogObj.Error("string.GetByteArray(): Error parsing string", ex);
				return null;
			}
		}
		public static byte[] BytesFromHex(this string hex, int iStartIndex = 0, int iLength = -1)
		{
			try
			{
				int iStringLength;
				if (iLength <= 0)
				{
					iStringLength = hex.Length;
				}
				else
				{
					iStringLength = iStartIndex + iLength;
				}
				byte[] arr = new byte[iStringLength >> 1];

				for (int i = iStartIndex; i < iStringLength >> 1; ++i)
				{
					if (i + 1 < iStringLength)
						arr[i] = (byte)((GetHexVal(hex[i]) << 4) + (GetHexVal(hex[i + 1])));
					else
						arr[i] = (byte)(GetHexVal(hex[i]));
				}

				return arr;
			}
			catch (Exception ex)
			{
				//LogObj.Error("string.GetByteArray(): Error parsing string", ex);
				return null;
			}
		}
		public static int GetHexVal(char hex)
		{
			int val = (int)hex;
			return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
		}
	}
}
