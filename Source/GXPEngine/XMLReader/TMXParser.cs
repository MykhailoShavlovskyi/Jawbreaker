using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using GXPEngine;

namespace GXPEngine
{
	public class TMXParser : GameObject
	{
		public TMXParser () 
		{
		}

		public Map Parse(string filename)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(Map));
			TextReader reader = new StreamReader (filename);

			Map map = serializer.Deserialize (reader) as Map;
			reader.Close ();

			//Console.WriteLine (map);

			return map;
		}
	}
}