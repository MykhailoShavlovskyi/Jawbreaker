using System;
using System.Xml.Serialization;
using GXPEngine;

namespace GXPEngine
{ 
    [XmlRootAttribute("map")]
    public class Map
	{
		[XmlAttribute("version")]
		public string Version ;

		[XmlAttribute("orientation")]
		public string Orientation ;

		[XmlAttribute("renderorder")]
		public string RenderOreder ;

		[XmlAttribute("width")]
		public int Width = 0;

		[XmlAttribute("height")]
		public int Height ;

		[XmlAttribute("tilewidth")]
		public int TileWidth;

		[XmlAttribute("tileheight")]
		public int TileHeight;

		[XmlAttribute("nextobjectid")]
		public int NextObjectId ;

		[XmlElement("tileset")]
		public TileSet[] TileSet;

		[XmlElement("layer")]
		public Layer[] Layer;

		[XmlElement("objectgroup")]
		public ObjectGroup[] ObjectGroup;

		public Map ()
		{
		}

		public override string ToString()
		{
			string output = "";
			output += ("map.Version = " + Version);
			output += ("\nmap.Orirentation = " + Orientation);
			output += ("\nmap.RenderOrder = " + RenderOreder);
			output += ("\nmap.Width = " + Width);
			output += ("\nmap.Height = " + Height);
			output += ("\nmap.TileWidth = " + TileWidth);
			output += ("\nmap.TileHeight = " + TileHeight);
			output += ("\nmap.NextObjectid = " + NextObjectId);
			output += ("\n-------------------------------------------------------------------");
			if (TileSet != null) 
			for (int d = 0; d < TileSet.Length; d++)
			{
				output+= TileSet [d].ToString ();
			}
			if (Layer != null) 
			for (int d = 0; d < Layer.Length; d++)
			{
				output+= Layer [d].ToString ();
			}
			if (ObjectGroup != null) 
			for (int d = 0; d < ObjectGroup.Length; d++)
			{
				output+= ObjectGroup [d].ToString ();
			}
			return output;
		}
	}


	[XmlRootAttribute("tileset")]
	public class TileSet
	{
		[XmlAttribute("firstgid")]
		public int Firstgid ;

		[XmlAttribute("name")]
		public string Name ;

		[XmlAttribute("tilewidth")]
		public int TileWidth;

		[XmlAttribute("tileheight")]
		public int TileHeight;

		[XmlAttribute("tilecount")]
		public int TileCount;

		[XmlElement("image")]
		public Image Image;

		[XmlElement("tile")]
		public Tile[] Tile;

		public TileSet()
		{
		}

		public override string ToString()
		{
			string output = "";
			output += ("\ntileset.firstgid = " + Firstgid);
			output += ("\ntileset.name = " + Name);
			output += ("\ntileset.tilewidth = " + TileWidth);
			output += ("\ntileset.tileheight = " + TileHeight);
			output += ("\ntileset.tilecount = " + TileCount);
			output += Image.ToString ();
			if (Tile != null) 
			{
				for (int d = 0; d < Tile.Length; d++) 
				{
					output += Tile [d].ToString ();
				}
			}
			output += ("\n-------------------------------------------------------------------");
			return output;
		}
	}

	[XmlRootAttribute("imagesource")]
	public class Image
	{
		[XmlAttribute("source")]
		public string Source;

		[XmlAttribute("width")]
		public int Width;

		[XmlAttribute("height")]
		public int Height;

		public Image()
		{
		}
		public override string ToString()
		{
			string output = "";
			output += ("\nimage.source = " + Source);
			output += ("\nimage.width = " + Width);
			output += ("\nimage.heigth = " + Height);
			output += ("\n-------------------------------------------------------------------");
			return output;
		}
	}
		

	[XmlRootAttribute("tile")]
	public class Tile
	{
		[XmlAttribute("id")]
		public int Id;

		[XmlElement("properties")]
		public Properties Properties;

		public Tile()
		{
		}
		public override string ToString()
		{
			string output = "";
			output += ("\ntile.id = " + Id);
			output += Properties.ToString ();
			return output;
		}
	}

	[XmlRootAttribute("properties")]
	public class Properties
	{
		[XmlElement("property")]
		public Property[] Property;

		public Properties()
		{
		}

		public override string ToString()
		{
			string output = "";
			for (int d = 0; d <Property.Length; d++) 
			{
				output += Property [d].ToString ();
			}
			return output;
		}
	}

	[XmlRootAttribute("property")]
	public class Property
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("value")]
		public bool Value;

		public Property()
		{
		}
		public override string ToString()
		{
			string output = "";
			output += ("\nproperty.name = " + Name);
			output += ("\nproperty.value = " + Value);
			return output;
		}
	}

	[XmlRootAttribute("layer")]
	public class Layer
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("width")]
		public int Width;

		[XmlAttribute("height")]
		public int Height;

		[XmlElement("data")]
		public Data Data;

		public Layer()
		{
		}

		public override string ToString()
		{
			string output = "";
			output += ("\nlayer.name = " + Name);
			output += ("\nlayer.width = " + Width);
			output += ("\nlayer.heigth = " + Height);
			output += Data.ToString ();
			output += ("-------------------------------------------------------------------");
			return output;
		}
	}

	[XmlRootAttribute("data")]
	public class Data
	{
		[XmlAttribute("encoding")]
		public string Encoding;

		[XmlText]
		public string innerXML;

		public Data()
		{
		}

		public override string ToString()
		{
			string output = "";
			output += ("\ndata.encoding = " + Encoding);
			output += innerXML;
			return output;
		}
	}

	[XmlRootAttribute("objectgroup")]
	public class ObjectGroup
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlElement("object")]
		public Object[] Object;

		public ObjectGroup()
		{
		}

		public override string ToString()
		{
			string output = "";
			output += ("\nobjcetgroup.name = " + Name);
			output += ("\n-------------------------------------------------------------------");
			for (int d = 0; d < Object.Length; d++)
			{
				output+= Object [d].ToString ();
			}
			return output;
		}
	}

	[XmlRootAttribute("object")]
	public class Object
	{
		[XmlAttribute("id")]
		public string Id;

		[XmlAttribute("gid")]
		public int Gid;

		[XmlAttribute("x")]
		public float x;

		[XmlAttribute("y")]
		public float y;

		[XmlAttribute("width")]
		public int Width;

		[XmlAttribute("heigth")]
		public int Height;

		[XmlAttribute("rotation")]
		public float Rotation;

		[XmlElement("properties")]
		public ObjectProperties ObjectProperties;

		[XmlElement("polyline")]
		public Polyline Polyline;

		[XmlElement("ellipse")]
		public Ellipse Ellipse;

		public Object()
		{

		}
		public override string ToString()
		{
			string output = "";
			output += ("\nobject.id = " + Id);
			output += ("\nobject.gid = " + Gid);
			output += ("\nobject.x = " + x);
			output += ("\nobject.y = " + y);
			output += ("\nobject.width = " + Width);
			output += ("\nobject.heigth = " + Height);
			output += ("\nobject.rotation = " + Rotation);
			if (ObjectProperties != null)
			output += ObjectProperties.ToString ();
			if (Polyline != null)
				output += Polyline.ToString ();
			output += ("\n-------------------------------------------------------------------");
			return output;
		}
	}
		
    [XmlRootAttribute("properties")]
	public class ObjectProperties
	{
		[XmlElement("property")]
		public ObjectProperty[] ObjectProperty;

		public ObjectProperties()
		{
		}
		public override string ToString()
		{
			string output = "";
			for (int d = 0; d <ObjectProperty.Length; d++) 
			{
				output += ObjectProperty [d].ToString ();
			}
			return output;
		}
 	}

	[XmlRootAttribute("property")]
	public class ObjectProperty
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("value")]
		public string Value;

		public ObjectProperty()
		{
		}

		public override string ToString()
		{
			string output = "";
			output += ("\nproperty.name = " + Name);
			output += ("\nproperty.value = " + Value);
			return output;
		}
	}

	[XmlRootAttribute("polyline")]
	public class Polyline
	{
		[XmlAttribute("points")]
		public string Points;

		public Polyline()
		{
		}
		public override string ToString()
		{
			string output = "";
			output += ("\npolyline.points = " + Points);
			return output;
		}
	}

	[XmlRootAttribute("ellipse")]
	public class Ellipse
	{

	}
}

