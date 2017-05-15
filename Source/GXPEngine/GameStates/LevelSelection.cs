using System;
using System.Collections.Generic;
using GXPEngine;

namespace GXPEngine
{
	public class LevelSelection : GameObject
	{
		MyGame _myGame;
		Sprite Button;
		Sprite Button2;
		Sprite ButtonBack;
		Sprite ButtonBack2;
		GameProgressData _progressData;
		private List <Sprite> _buttonList = new List<Sprite> ();
		private List <Sprite> _buttonList2 = new List<Sprite> ();
		string[] buttons = new string[] {"L1 unpressed.png","L2 unpressed.png","L3 unpressed.png","L4 unpressed.png","L5 unpressed.png","L6 unpressed.png","L7 unpressed.png","L8 unpressed.png"};  
		string[] buttons2 = new string[] {"L1.png","L2.png","L3.png","L4.png","L5.png","L6.png","L7.png","L8.png"};  

		public LevelSelection (MyGame mygame, GameProgressData progressData)
		{
			Sprite background = new Sprite ("levelselectback.png");
			AddChild (background);
		
			_progressData = progressData;
			_myGame = mygame;

			for (int i = 0; i < 8; i++) 
			{
				Button = new Sprite (buttons[i]);
				Button.scale =0.8f;

				AddChild (Button);
				if (i < 4) 
				{
					Button.SetXY (_myGame.width/2 - _myGame.width/6-Button.width / 2, 70+40+i*110);
					_buttonList.Add (Button);
				}
				else
				{
					Button.SetXY (_myGame.width/2 + _myGame.width/6-Button.width / 2, 70+40+i*110-4*110);
				    _buttonList.Add (Button);
				}

				Button2 = new Sprite (buttons2[i]);
				Button2.scale =0.8f;

				Button2.alpha = 0f;
				AddChild (Button2);
				if (i < 4) 
				{
					Button2.SetXY (_myGame.width/2 - _myGame.width/6-Button2.width / 2, 70+40+i*110);
					_buttonList2.Add (Button2);
				}
				else
				{
					Button2.SetXY (_myGame.width/2 + _myGame.width/6-Button2.width / 2, 70+40+i*110-4*110);
					_buttonList2.Add (Button2);
				}

				switch (i+1)
				{
				case 1:
					if (_progressData.Level1 )
					{
						CreateLock ();
					}
					for (int j = 0; j < _progressData.Level1Stars; j++)
					{
						CreateStars (j);
					}
					break;
				case 2:
					if (_progressData.Level2) 
					{
						CreateLock ();
					
					}
					for (int j = 0; j < _progressData.Level2Stars; j++)
					{
						CreateStars (j);
					}
					break;
				case 3:
					if (_progressData.Level3 )
					{
						CreateLock ();
					
					}
					for (int j = 0; j < _progressData.Level3Stars; j++)
					{
						CreateStars (j);
					}
					break;
				case 4:
					if (_progressData.Level4)
					{
						CreateLock ();

					}
					for (int j = 0; j < _progressData.Level4Stars; j++) 
					{
						CreateStars (j);
					}
					break;
				case 5:
					if (_progressData.Level5 ) 
					{
						CreateLock ();

					}
					for (int j = 0; j < _progressData.Level5Stars; j++)
					{
						CreateStars (j);
					}
					break;
				case 6:
					if (_progressData.Level6)
					{
						CreateLock ();

					}
					for (int j = 0; j < _progressData.Level6Stars; j++)
					{
						CreateStars (j);
					}
					break;
				case 7:
					if (_progressData.Level7)
					{
						CreateLock ();

					}
					for (int j = 0; j < _progressData.Level7Stars; j++) 
					{
						CreateStars (j);
					}
					break;
				case 8:
					if (_progressData.Level8) 
					{
						CreateLock ();
					
					}
					for (int j = 0; j < _progressData.Level8Stars; j++) 
					{
						CreateStars (j);
					}
					break;
				}

				ButtonBack = new Sprite ("B unpressed NEW.png");
				ButtonBack.scale = 0.8f;
				AddChild (ButtonBack);
				ButtonBack.SetXY (_myGame.width/2 - ButtonBack.width/2,_myGame.height-150);

				ButtonBack2 = new Sprite ("B NEW.png");
				ButtonBack2.scale = 0.8f;
				ButtonBack2.alpha = 0;
				AddChild (ButtonBack2);
				ButtonBack2.SetXY (_myGame.width/2 - ButtonBack2.width/2,_myGame.height-150);
			}
		}

		void CreateLock () 
		{
			Sprite Lock = new Sprite ("Lock.png");
			AddChild (Lock);
			Lock.SetXY (Button.x-3, Button.y+3);
			Lock.y += Button.height - Lock.height;
		}

		void CreateStars (int star)
		{
			Sprite Star = new Sprite ("Star2.png");
			AddChild (Star);
			Star.scale = 0.79f;
			Star.SetXY (Button.x +63 + star*Star.width+star, Button.y+32);
		}

		bool IsLocked (int i) 
		{
			switch (i+1) 
			{
			case 1:
				return Convert.ToBoolean(_progressData.Level1);
			case 2:
				return Convert.ToBoolean(_progressData.Level2);
			case 3:
				return Convert.ToBoolean(_progressData.Level3);
			case 4:
				return Convert.ToBoolean(_progressData.Level4);
			case 5:
				return Convert.ToBoolean(_progressData.Level5);
			case 6:
				return Convert.ToBoolean(_progressData.Level6);
			case 7:
				return Convert.ToBoolean(_progressData.Level7);
			case 8:
				return Convert.ToBoolean(_progressData.Level8);
			}

			return false;
		}

		void Update () 
		{
			foreach (Sprite _button in _buttonList) 
			{
				if (Input.mouseX > _button.x && Input.mouseX < _button.x + _button.width && Input.mouseY > _button.y && Input.mouseY < _button.y + _button.height)
				{

					_button.alpha = 0;
					_buttonList2 [_buttonList.IndexOf (_button)].alpha = 1;
					if (Input.GetMouseButtonDown (0)) {
						
						if (!IsLocked (_buttonList.IndexOf (_button))) {
							Sound click = new Sound ("Click.wav");
							click.Play ();
							_myGame.SetState ("Level", _buttonList.IndexOf (_button) + 1);
						}
					}
				} 
				else 
				{
					_button.alpha = 1;

					_buttonList2 [_buttonList.IndexOf (_button)].alpha = 0;
			    }
			}

			if (Input.mouseX > ButtonBack.x && Input.mouseX < ButtonBack.x + ButtonBack.width && Input.mouseY > ButtonBack.y && Input.mouseY < ButtonBack.y + ButtonBack.height) 
			{
				ButtonBack.alpha = 0;
				ButtonBack2.alpha = 1;
				if (Input.GetMouseButtonDown (0))
				{
					Sound click = new Sound ("Click.wav");
					click.Play ();
					_myGame.SetState ("Menu");
				}
			}

			else 
			{
				ButtonBack.alpha = 1;
				ButtonBack2.alpha = 0;
			}
		}
	}
}

