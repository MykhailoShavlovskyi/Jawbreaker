using System;

namespace GXPEngine
{
	public class Credits : GameObject
	{
		MyGame _game;
		Sprite button;
		Sprite button2;

		public Credits (MyGame game)
		{
			Sprite background = new Sprite ("MenuBack.png");
			AddChild (background);

			Sprite credits = new Sprite ("credits.png");
			credits.scale = 0.7f;

			credits.SetXY (game.width / 2 - credits.width/2, 355);
			AddChild (credits);

			_game = game;
			CreateButtons ();
		}

		private void CreateButtons () 
		{
			button = new Sprite ("B unpressed NEW.png");
			button.scale = 0.8f;
			AddChild (button);
			button.SetXY (_game.width/2 - button.width/2, 580);

			button2 = new Sprite ("B NEW.png");
			button2.scale = 0.8f;
			button2.alpha = 0;
			AddChild (button2);
			button2.SetXY (_game.width/2 - button2.width/2, 580);
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
					_game.SetState ("Menu");
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

