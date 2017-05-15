using System;
using GXPEngine;

namespace GXPEngine
{
	public class Menu : GameObject
	{
		MyGame _game;

		private Sprite button1;
		private Sprite button2;
		private Sprite button3;
		private Sprite button1p;
		private Sprite button2p;
		private Sprite button3p;
	
		bool quit = false;
		int timer = 15;

		public Menu(MyGame game) 
		{
			_game = game;

			Sprite background = new Sprite ("MenuBack.png");
			AddChild (background);

			CreateButtons ();
		}

		private void CreateButtons () 
		{
			button1 = new Sprite ("P unpressed NEW.png");
			AddChild (button1);
			button1.scale = 0.75f;
			button1.SetXY (_game.width/2 - button1.width/2, 390);

			button2 = new Sprite ("C unpressed NEW.png");
			button2.scale = 0.75f;
			AddChild (button2);
			button2.SetXY (_game.width/2 - button2.width/2, 490);

			button3 = new Sprite ("E unpressed NEW.png");
			button3.scale = 0.75f;
			AddChild (button3);
			button3.SetXY (_game.width/2 - button3.width/2, 590);

			button1p = new Sprite ("P NEW.png");
			AddChild (button1p);
			button1p.scale = 0.75f;
			button1p.SetXY (_game.width/2 - button1p.width/2, 390);
			button1p.alpha = 0;

			button2p = new Sprite ("C NEW.png");
			button2p.scale = 0.75f;
			AddChild (button2p);
			button2p.SetXY (_game.width/2 - button2p.width/2, 490);
			button2p.alpha = 0;

			button3p = new Sprite ("E New.png");
			button3p.scale = 0.75f;
			AddChild (button3p);
			button3p.SetXY (_game.width/2 - button3p.width/2, 590);
			button3p.alpha = 0;
		}

		void Update () 
		{
			if (quit) 
				timer--;
			if (timer == 0)
				_game.Destroy ();
			

			if ((Input.mouseX >= button1.x && Input.mouseX <= button1.x + button1.width) && (Input.mouseY >= button1.y && Input.mouseY <= button1.y + button1.height))
			{
				button1.alpha = 0;
				button1p.alpha = 1;
				if (Input.GetMouseButtonDown (0)) 
				{
					Sound click = new Sound ("Click.wav");
					click.Play ();
					_game.SetState ("LevelSelection");
				}
			} 
			else 
			{
				button1.alpha = 1;
				button1p.alpha = 0;
			}


			if ((Input.mouseX >= button2.x && Input.mouseX <= button2.x + button2.width) && (Input.mouseY >= button2.y && Input.mouseY <= button2.y + button2.height)) 
			{
				button2.alpha = 0;
				button2p.alpha = 1;
			    if (Input.GetMouseButtonDown (0))
				{
				    Sound click = new Sound ("Click.wav");
				    click.Play ();
				    _game.SetState ("Credits");
			    }
			}
			else 
			{
				button2.alpha = 1;
				button2p.alpha = 0;
			}

			if ((Input.mouseX >= button3.x && Input.mouseX <= button3.x + button3.width) && (Input.mouseY >= button3.y && Input.mouseY <= button3.y + button3.height))
			{
				button3.alpha = 0;
				button3p.alpha = 1;
			    if (Input.GetMouseButtonDown (0)) 
				{
			    	Sound click = new Sound ("Click.wav");
			    	click.Play ();
			    	quit = true;
			    }
			}
			else 
			{
				button3.alpha = 1;
				button3p.alpha = 0;
			}
		}
	}
}

