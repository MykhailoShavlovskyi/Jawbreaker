using System;
using GXPEngine;
using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace GXPEngine
{
	public class MyGame : Game
	{
		Menu _menu;
		Level _level;
		Credits _credits;
		WinScreen _winScreen;
		ScoreScreen _scoreScreen;
		LevelSelection _levelSelection;
		GameProgressData _progressData;
		SoundChannel MainGameChannel;
		SoundChannel InGameChannel;

		Sound GameMusic;
		Sound InGameMusic;

		bool IsPlaying = false;
		bool LevelIsPlaying = false;
	

		string _State = "";
		string[] levels = new string[] {"platforms1.tmx","platforms2.tmx","platforms3.tmx","platforms4.tmx","platforms5.tmx","platforms6.tmx","platforms7.tmx","platforms8.tmx"};  

		public MyGame() : base(1080,720, false)
		{
			_progressData = new GameProgressData ();
			LoadData ();
			SetState("Menu");
		}

		#region *Game States*
		public void SetState(string state, int levelnumber = 0)
		{
			stopState ();
			_State = state;
			startState (levelnumber);
		}

		public void startState(int levelnumber)                                                   
		{
			switch(_State)
			{
			case "Menu":
				_menu = new Menu (this);
				AddChild (_menu);

				if (!IsPlaying) 
				{
					GameMusic = new Sound ("MainMenuMusic.wav", true, true);
					MainGameChannel = GameMusic.Play (false, 1);
					IsPlaying = true;
				}
				break;

			case "ScoreScreen":
				_scoreScreen = new ScoreScreen (this, levelnumber, _progressData);
				AddChild (_scoreScreen);

				SaveData ();
				break;

			case "WinScreen":
				if (LevelIsPlaying)
				{
					InGameChannel.Stop ();
					LevelIsPlaying = false;
				}

				if (!IsPlaying)
				{
					GameMusic = new Sound ("MainMenuMusic.wav", true, true);
					MainGameChannel = GameMusic.Play (false, 1);
					IsPlaying = true;
				}

				_winScreen = new WinScreen (this);
				AddChild (_winScreen);

				SaveData ();
				break;

			case "Level":
				MainGameChannel.Stop ();
				IsPlaying = false;

				if (!LevelIsPlaying)
				{
					InGameMusic = new Sound ("InGameMusic.wav", true, true);
					InGameChannel = InGameMusic.Play (false, 1);
					LevelIsPlaying = true;
				}

				_level = new Level (this, new TMXParser ().Parse (levels [levelnumber - 1]), _progressData, levelnumber);
				AddChild (_level);

				SaveData ();
				break;

			case "LevelSelection":
				if (LevelIsPlaying)
				{
			    	InGameChannel.Stop ();
				    LevelIsPlaying = false;
				}

				_levelSelection = new LevelSelection (this, _progressData);
				AddChild (_levelSelection);

				if (!IsPlaying)
				{
					GameMusic = new Sound ("MainMenuMusic.wav", true, true);
					MainGameChannel = GameMusic.Play (false, 1);
					IsPlaying = true;
				}

				SaveData ();
				break;

			case "Credits":
				_credits = new Credits (this);
				AddChild (_credits);
				break;

			default:
				break;
			}
		}

		public void stopState()
		{
			switch(_State)
			{
			case "Menu":
				_menu.Destroy ();
				break;
			case "ScoreScreen":
				_scoreScreen.Destroy ();
				break;
			case "LevelSelection":
				_levelSelection.Destroy();
				break;
			case "Level":
				_level.Destroy();
				break;
			case "WinScreen":
				_winScreen.Destroy();
				break;
			case "Credits":
				_credits.Destroy();
				break;
			}
		}
		#endregion

		static void Main()
		{
			new MyGame().Start();
		}

		void Update()
		{

		}

		public void SaveData()
		{
			XmlSerializer writer = new XmlSerializer (typeof(GameProgressData));
			var path = ".//Xmldata.xml"; //.//saves intodebug folder
			FileStream file = File.Create (path);
			writer.Serialize (file, _progressData);
			file.Close();
		}

		public void LoadData()
		{
			XmlSerializer reader = new XmlSerializer (typeof(GameProgressData));
			StreamReader file = new StreamReader ("Xmldata.xml");
			_progressData = (GameProgressData)reader.Deserialize (file);
			file.Close ();
		}
	}
}

