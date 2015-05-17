using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTextGame
{
	public interface IDisplay
	{
		void Display(TextDefinition tDef, string sKey = "");
	}
}
