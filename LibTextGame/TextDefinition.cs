using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil;

namespace LibTextGame
{
	public class TextDefinition
	{
		public string ID { get; set; }
		//public List<string> Tags { get; set; }
		public Dictionary<string, List<TextSegment>> SegmentStyles { get; set; }//Default (with "" as key) will be what's displayed by default. Alternate keys may be selected for the same segment.
		public TextDefinition()
		{
			SegmentStyles = new Dictionary<string, List<TextSegment>>();
		}
	}
}
