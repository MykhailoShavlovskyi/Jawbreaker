using System;

namespace GXPEngine
{
	public class SuperTongue : GameObject
	{
		#region Variables
		Ball _ball;
		MyGame _game;
		Level _level;
		GameProgressData _progressData;

		Sprite t1 = new Sprite ("tt1.png");
		Sprite t2 = new Sprite ("tt2.png");
		Sprite t3 = new Sprite ("tt3.png");

		Vec2 start = new Vec2(0, 0);
		Vec2 ton1 = new Vec2(48,0);
		Vec2 ton2 = new Vec2(96,0);
		Vec2 ton3 = new Vec2(48,0);

		bool DeathSoundIsPlaying = false;

		LineSegment tongue1 = new LineSegment(new Vec2(0,0), new Vec2(0,0), 0x00000000, 5);
		LineSegment tongue2= new LineSegment(new Vec2(0,0), new Vec2(0,0),0x00000000, 5);
		LineSegment tongue3 = new LineSegment (new Vec2 (0,0), new Vec2 (0,0), 0x00000000, 5);

		float tongueAngle = 0;

		float angle1;
		float angle2;
		float angle3;

		public bool preview = true;

		int _whichLevel;

		public bool eat = false;
		#endregion

		public SuperTongue (MyGame game,Level level, Ball ball, GameProgressData progressData, int whichLevel)
		{
			_game = game;
			_level = level;
			_ball = ball;
			_progressData = progressData;
			_whichLevel = whichLevel;

			AddChild (t1);
			t1.SetOrigin (t1.width/2,t1.height-5);

			AddChild (t2);
			t2.SetOrigin (t2.width/2,t2.height-5);

			AddChild (t3);
			t3.SetOrigin (t3.width / 2-1, t3.height-5);//-10); 

			AddChild (tongue1);
			AddChild (tongue2);
			AddChild (tongue3);
		}

		void Update()
		{
			float lengthToTheBall = new Vec2 (_ball.x - x, _ball.y - y).Length ();

			if (lengthToTheBall < 200 && !_level.dead)
			{
				_level.SetEatingAnimation ();

				if (!DeathSoundIsPlaying) 
				{
					Sound click = new Sound ("MonsterDeath.wav");
					click.Play ();
					DeathSoundIsPlaying = true;
				}

				if (new Vec2 (_ball.x - x, _ball.y - y).GetAngleDegrees () > 190 && new Vec2 (_ball.x - x, _ball.y - y).GetAngleDegrees () < 350)
				{
					eat = true;
					_level.anglestep = 0;
					angle1 = -(float)((180 / Math.PI) * Math.Acos ((lengthToTheBall / 4) / 50));
					angle2 = (float)((180 / Math.PI) * Math.Acos ((lengthToTheBall / 4) / 50));
					angle3 = -(float)((180 / Math.PI) * Math.Acos ((lengthToTheBall / 4) / 50));
					_ball._acceleration.Set (new Vec2 (0, 0));
					_ball.velocity.Set (new Vec2 (x - _ball.x, y - _ball.y).Normalize ().Scale (lengthToTheBall / 40));
				}

				if (lengthToTheBall < 2) 
				{
					_level.UpdateProgressData ();
					_progressData.LastResults = _level.GetAmmountStars ();
					_level.UnlockNexTLevel ();
					_game.SetState ("ScoreScreen", _whichLevel);
					_game.SetXY (0, 0);
				}
			} 

			if (!eat)
			{

				if (preview)
				{
					tongue1.start.Set (start);
					ton1.SetAngleDegrees (0);
					t1.rotation =  + 90;
					tongue1.end.Set (tongue1.start.Clone ().Add (ton1));

					tongue2.start.Set (tongue1.end);
					ton2.SetAngleDegrees (0);
					t2.rotation = 0 + 90;
					tongue2.end.Set (tongue2.start.Clone ().Add (ton2));

					tongue3.start.Set (tongue2.end);
					ton3.SetAngleDegrees (0);
					t3.rotation = 0 + 90;
					tongue3.end.Set (tongue3.start.Clone ().Add (ton3));

					rotation = -90;
				} 
				else 
				{
					tongue1.start.Set (start);

					float ballangle = new Vec2 (_ball.x - x, _ball.y - y).GetAngleDegrees ();
					float leftlimit = 240;
					float rightlimit = 300;

					if (ballangle > leftlimit && ballangle < rightlimit) 
					{            
						ton1.SetAngleDegrees (ballangle);
						t1.rotation = ballangle + 90;
						tongue1.end.Set (tongue1.start.Clone ().Add (ton1));
						tongue2.start.Set (tongue1.end);
						ballangle = new Vec2 (_ball.x - x + ton1.x, _ball.y - y + ton1.y).GetAngleDegrees ();
						ton2.SetAngleDegrees (ballangle);
						t2.rotation = ballangle + 90;
						tongue2.end.Set (tongue2.start.Clone ().Add (ton2));
						tongue3.start.Set (tongue2.end);
						ballangle = new Vec2 (_ball.x - x + ton1.x + ton2.x, _ball.y - y + ton1.y + ton2.y).GetAngleDegrees ();                                
						ton3.SetAngleDegrees (ballangle);
						t3.rotation = ballangle + 90;
						tongue3.end.Set (tongue3.start.Clone ().Add (ton3));
					} 
					else if (ballangle > 90 && ballangle < 240) 
					{
						ballangle = 240;


						ton1.SetAngleDegrees (ballangle);
						t1.rotation = ballangle + 90;
						tongue1.end.Set (tongue1.start.Clone ().Add (ton1));
						tongue2.start.Set (tongue1.end);

						ton2.SetAngleDegrees (ballangle);
						t2.rotation = ballangle + 90;
						tongue2.end.Set (tongue2.start.Clone ().Add (ton2));
						tongue3.start.Set (tongue2.end);
						  
						ton3.SetAngleDegrees (ballangle);
						t3.rotation = ballangle + 90;
						tongue3.end.Set (tongue3.start.Clone ().Add (ton3));
					} 
					else if ((ballangle < 90 && ballangle > 0) || (ballangle > 300) && (ballangle < 360)) 
					{
						ballangle = 300;
						ton1.SetAngleDegrees (ballangle);
						t1.rotation = ballangle + 90;
						tongue1.end.Set (tongue1.start.Clone ().Add (ton1));
						tongue2.start.Set (tongue1.end);

						ton2.SetAngleDegrees (ballangle);
						t2.rotation = ballangle + 90;
						tongue2.end.Set (tongue2.start.Clone ().Add (ton2));
						tongue3.start.Set (tongue2.end);

						ton3.SetAngleDegrees (ballangle);
						t3.rotation = ballangle + 90;
						tongue3.end.Set (tongue3.start.Clone ().Add (ton3));
					}
				}
			}
			else
			{
				tongue1.start.Set (start);
				ton1.SetAngleDegrees (angle1);
				t1.rotation = angle1 + 90;
				tongue1.end.Set (tongue1.start.Clone ().Add (ton1));

				tongue2.start.Set (tongue1.end);
				ton2.SetAngleDegrees (angle2);
				t2.rotation = angle2 + 90;
				tongue2.end.Set (tongue2.start.Clone ().Add (ton2));

				tongue3.start.Set (tongue2.end);
				ton3.SetAngleDegrees (angle3);
				t3.rotation = angle3 + 90;
				tongue3.end.Set (tongue3.start.Clone ().Add (ton3));
			}

			t1.SetXY (tongue1.start.x, tongue1.start.y);
			t2.SetXY (tongue2.start.x, tongue2.start.y);
			t3.SetXY (tongue3.start.x, tongue3.start.y);

			if (eat) 
			{
				this.Turn (new Vec2 (_ball.x - x, _ball.y - y).GetAngleDegrees () - tongueAngle);
				tongueAngle = new Vec2 (_ball.x - x, _ball.y - y).GetAngleDegrees ();
			}
		}
	}
}
