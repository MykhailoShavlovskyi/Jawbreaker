using System;
using System.Collections.Generic;
using GXPEngine;

namespace GXPEngine
{
	public class ScoreScreen : GameObject
	{
		
		Sprite Star;
		MyGame _myGame;
		GameProgressData _progressData;
		private List <Sprite> _buttonList = new List<Sprite> ();
		private List <Sprite> _starList = new List<Sprite> ();

		int _whichLevel;
	
		private Sprite button1;
		private Sprite button2;
		private Sprite button3;

		public ScoreScreen (MyGame myGame, int whichLevel, GameProgressData progressData)
		{
			Sprite background = new Sprite ("levelselectback.png");
			AddChild (background);

			_myGame = myGame;
			_whichLevel = whichLevel;
			_progressData = progressData;

			Sprite levelCleared = new Sprite ("Level clear.png");
			levelCleared.SetXY (myGame.width / 2 - levelCleared.width/2, myGame.height / 2 - levelCleared.height/2);
			AddChild (levelCleared);

			button1 = new Sprite ("hud1.png");
			AddChild (button1);
			button1.SetXY (levelCleared.x+levelCleared.width/2-button1.width*1.5f-10, levelCleared.y+200);
			_buttonList.Add (button1);

			button2 = new Sprite ("Next level.png");
			AddChild (button2);
			button2.SetXY (levelCleared.x+levelCleared.width/2-button2.width/2, levelCleared.y+200);
			_buttonList.Add (button2);

			button3 = new Sprite ("hud2.png");
			AddChild (button3);
			button3.SetXY (levelCleared.x+levelCleared.width/2+button3.width/2+10, levelCleared.y+200);
			_buttonList.Add (button3);

			for (int i = 0; i < _progressData.LastResults; i++) //_progressData.LastResults
			{
				Star = new Sprite ("Yellowclear.png");
				Star.scale = 1.1f;
				AddChild (Star);
				if (i == 0)
				{
					Star.SetXY (levelCleared.x+levelCleared.width/2-Star.width/2, levelCleared.y+72);
					_starList.Add (Star);
				}
				if (i == 1)
				{
					Star.SetXY (levelCleared.x + levelCleared.width / 2 - levelCleared.width / 4 - 16 - Star.width / 2 , levelCleared.y + 99);
					_starList.Add (Star);
				}
				if (i == 2)
				{
					Star.SetXY (levelCleared.x  +levelCleared.width / 2 + levelCleared.width / 4 + 16 - Star.width / 2 , levelCleared.y + 99);
					_starList.Add (Star);
				}
			}
		}

		void Update ()
		{
			foreach (Sprite _button in _buttonList)
			{
				if (Input.GetMouseButtonUp (0) && Input.mouseX > _button.x && Input.mouseX < _button.x + _button.width && Input.mouseY > _button.y && Input.mouseY < _button.y + _button.height )
				{
					
					if (_buttonList.IndexOf (_button) == 0) 
					{
						Sound click = new Sound("Click.wav");
						click.Play ();
						_myGame.SetState ("Level", _whichLevel);  //restart
					}

					if (_buttonList.IndexOf (_button) == 1) 
					{
						if (_whichLevel < 8) 
						{
							Sound click = new Sound("Click.wav");
							click.Play ();
							_myGame.SetState ("Level", _whichLevel + 1);   // next level
						}
						else 
						{
							Sound click = new Sound("Click.wav");
							click.Play ();
							_myGame.SetState ("WinScreen");
						}
					}

					if (_buttonList.IndexOf (_button) == 2)
					{
						Console.WriteLine ("asdada");
						Sound click = new Sound("Click.wav");
						click.Play ();
					    _myGame.SetState ("LevelSelection");//Level Selection
					}
				}
			}
		}
	}
}

