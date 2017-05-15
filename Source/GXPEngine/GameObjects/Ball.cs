using System;
using System.Drawing;

namespace GXPEngine
{
	public class Ball : Canvas
	{
		MyGame _game;

		public Vec2 position;
		public Vec2 velocity;
		public Vec2 _acceleration;

		public readonly int radius;
		private Color _ballColor;

		Sprite candy;

		bool candyball;

		public Ball (int pRadius, Vec2 pPosition = null, Vec2 pVelocity = null, Vec2 acceleration = null, Color? pColor = null, bool candyball = false):base (pRadius*2, pRadius*2)
		{
			
			_acceleration = acceleration;
			radius = pRadius;
			SetOrigin (radius, radius);

			position = pPosition ?? Vec2.zero;
			velocity = pVelocity ?? Vec2.zero;
			_ballColor = pColor ?? Color.Blue;

			draw ();
			Step ();

			if (candyball) 
			{
				candy = new Sprite ("candy.png");
				candy.SetOrigin (candy.width / 2, candy.height / 2);
				AddChild (candy);
			}
		}

		private void draw()
		{
			graphics.Clear (Color.Empty);
			graphics.FillEllipse (
				new SolidBrush (_ballColor),
				0, 0, 2 * radius, 2 * radius
			);
		}

		public void Step(bool skipGravity = false)
		{
			if (position == null || velocity == null)
				return;

			position.Add (velocity);

			if (!skipGravity)
			{
				velocity.Add (_acceleration);
			}

			x = position.x;
			y = position.y;
		}

		public Color ballColor 
		{
			get 
			{
				return _ballColor;
			}

			set 
			{
				_ballColor = value;
				draw ();
			}
		}

		public void SetAlpha(float newalpha)
		{
			candy.alpha = newalpha;
		}
	}
}
