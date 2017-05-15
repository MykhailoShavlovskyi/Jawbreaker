using System;
using System.Collections.Generic;
using GXPEngine;

namespace GXPEngine
{
	public class WinScreen : GameObject
	{
		Sprite button;
		Sprite button2;

		MyGame _myGame;
	
		public WinScreen (MyGame myGame)
		{
			_myGame = myGame;

			Sprite background = new Sprite ("levelselectback.png");
			AddChild (background);

			Sprite levelCleared = new Sprite ("win screen.png");
			levelCleared.SetXY (myGame.width / 2 - levelCleared.width/2, myGame.height / 2 - levelCleared.height/2-50);
			AddChild (levelCleared);

			button = new Sprite ("B unpressed NEW.png");
			button.scale = 0.8f;
			AddChild (button);
			button.SetXY (myGame.width/2 - button.width/2, 580);


			button2 = new Sprite ("B NEW.png");
			button2.scale = 0.8f;
			button2.alpha = 0;
			AddChild (button2);
			button2.SetXY (myGame.width/2 - button2.width/2, 580);

			Sound click = new Sound ("WinSound.wav");
			click.Play ();
		}

		void Update ()
		{
			if ((Input.mouseX >= button.x && Input.mouseX <= button.x + button.width) && (Input.mouseY >= button.y && Input.mouseY <= button.y + button.height))
			{
				button.alpha = 0;
				button2.alpha = 1;

				if (Input.GetMouseButtonDown (0)) 
				{
					Sound click = new Sound ("Click.wav");
					click.Play ();
					_myGame.SetState ("Menu");
				}
			}
			else 
			{
				button.alpha = 1;
				button2.alpha = 0;
			}
		}
	}
}

