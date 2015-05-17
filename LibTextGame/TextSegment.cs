using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTextGame
{
    public class TextSegment
	{
		public ColorDefinition BackgroundColor { get; set; }
		public ColorDefinition FontColor { get; set; }
		public string Text { get; set; }
		public DisplayUnitEnum DisplayUnit { get; set; }
		public int DisplayUnitIntervalMS { get; set; }
		//public List<string> Tags { get; set; }//TODO: Implement DisplayMethod as a collection of tags.
		//TODO: Compare MS display intervals vs. base display values with text multipliers.
		//Would allow for users to accelerate speeds without having it all appear immediately.
		#region Constructors
		public TextSegment()
		{
			//Tags= new List<string>();
		}
		public TextSegment(string sText, DisplayUnitEnum dUnit, int iDisplayInterval, ColorDefinition cFont = null, ColorDefinition cBackGround = null):this()
		{
			Text = sText;
			DisplayUnit = dUnit;
			DisplayUnitIntervalMS = iDisplayInterval;
			BackgroundColor = cBackGround;
			FontColor = cFont;
		}
		public TextSegment(string sText, ColorDefinition cFont = null, ColorDefinition cBackGround = null)
			: this()
		{
			Text = sText;
			BackgroundColor = cBackGround;
			FontColor = cFont;
		}
		#endregion
		#region Methods
		public TextSegment Clone(bool IncludeText = false, bool CopyTags=false)
		{
			TextSegment tNewSeg  =null;
			if (IncludeText)
			tNewSeg = new TextSegment(Text,DisplayUnit,DisplayUnitIntervalMS,FontColor,BackgroundColor);
			else
				tNewSeg = new TextSegment(string.Empty, DisplayUnit, DisplayUnitIntervalMS, FontColor, BackgroundColor);
			//if (CopyTags && Tags != null && Tags.Count > 0)
			//{
			//	for (int i = 0; i < Tags.Count; i++)
			//	{
			//		tNewSeg.Tags.Add(Tags[i]);//Copies the instance references, so don't alter.
			//	}
			//}
			return tNewSeg;
		}
		public TextSegment CloneWithText(string newText, bool CopyTags = false)
		{
			TextSegment tNewSeg = new TextSegment(newText, DisplayUnit, DisplayUnitIntervalMS, FontColor, BackgroundColor);
			//if (CopyTags && Tags != null && Tags.Count > 0)
			//{
			//	for (int i = 0; i < Tags.Count; i++)
			//	{
			//		tNewSeg.Tags.Add(Tags[i]);//Copies the instance references, so don't alter.
			//	}
			//}
			return tNewSeg;
		}
		public override string ToString()
		{
			StringBuilder sbOutput = new StringBuilder();
			sbOutput.Append(Text);
			if (DisplayUnitIntervalMS > 0)
				sbOutput.AppendFormat(" [Display {0}MS / {1}]", DisplayUnitIntervalMS, DisplayUnit.ToString());
			else
				sbOutput.AppendFormat(" [Display {1} immediately]", DisplayUnitIntervalMS, DisplayUnit.ToString());
			if (FontColor != null)
			{
				sbOutput.AppendFormat(" F#{0}", FontColor.ToString());
			}
			if (BackgroundColor != null)
			{
				sbOutput.AppendFormat(" B#{0}", BackgroundColor.ToString());
			}
			return sbOutput.ToString();
		}
		#endregion
	}
}
