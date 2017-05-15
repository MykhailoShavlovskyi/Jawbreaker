using System;
using System.Xml.Serialization;
using GXPEngine;

namespace GXPEngine
{
	[XmlRootAttribute("progressdata")]
	public class GameProgressData
	{
		[XmlAttribute("level1")]
		public bool Level1 = false;
		[XmlAttribute("level2")]
		public bool Level2 = true;
		[XmlAttribute("level3")]
		public bool Level3 = true;
		[XmlAttribute("level4")]
		public bool Level4 = true;
		[XmlAttribute("level5")]
		public bool Level5 = true;
		[XmlAttribute("level6")]
		public bool Level6 = true;
		[XmlAttribute("level7")]
		public bool Level7 = true;
		[XmlAttribute("level8")]
		public bool Level8 = true;

		[XmlAttribute("showtutorial")]
		public bool showTutorial = true;

		[XmlAttribute("level1stars")]
		public int Level1Stars = 0;
		[XmlAttribute("level2stars")]
		public int Level2Stars = 0;
		[XmlAttribute("level3stars")]
		public int Level3Stars = 0;
		[XmlAttribute("level4stars")]
		public int Level4Stars = 0;
		[XmlAttribute("level5stars")]
		public int Level5Stars = 0;
		[XmlAttribute("level6stars")]
		public int Level6Stars = 0;
		[XmlAttribute("level7stars")]
		public int Level7Stars = 0;
		[XmlAttribute("level8stars")]
		public int Level8Stars = 0;

		[XmlAttribute("lastresults")]
		public int LastResults = 0;

		public GameProgressData ()
		{
		}
	}
}

