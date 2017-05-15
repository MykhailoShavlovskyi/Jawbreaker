using System;
using System.Threading;
using System.IO;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

namespace GXPEngine
{
	public class Level  : GameObject
	{
		Map map;
		Ball _ball;
		MyGame _game;
		SuperTongue supertongue;
		GameProgressData _progressData;

		Sprite candy;
		Sprite background;
		Sprite inventoryStar;
		Sprite cannon;
		Sprite hudStrength;
		Sprite star1;
		Sprite star2;
		Sprite star3;

		Sprite restartButton;
		float restartButtonStartX;
		float restartButtonStartY;

		Sprite levelSelection;
		float levelSelectionStartX;
		float levelSelectionStartY;

		LineSegment strengthLine;

		Vec2 ballVelocity;

		private int _whichLevel;

		public bool ballLaunched = false;
		bool camerasetonthestart = false;
		bool casinoLaunched = false;
		bool IncreaseSpeed = true;
		bool allowLaunch = true;
		bool showPreview = true;
		bool goingDown = true;
		bool failed = false;
		bool alreadyEating = false;
		public bool dead = false;
		bool enoughAnimations = false;
		bool allowCannonAnimation = false;
		bool cannonAnimationStarted = false;

		public float anglestep = 0;
		float strength = 120;
		float groundy =  0;
		float step =50;
		float cannonx;
		float cannony;
		float cannonangle;
		float lastStickyObjectx;
		float lastStickyObjecty;
		float lastNormalObjectx;
		float lastNormalObjecty;
		float lastBouncyObjectx;
		float lastBouncyObjecty;

		int restartTimer = 80;
		int animationTimer =2;
		int animationTimer2 =3;
		int starAnimationTimer = 4;
		int stickySoundTimer = 3;
		int normalSoundTimer = 3;
		int bouncySoundTimer = 3;
		int contactTimer = 3;
		int cannonTimer = 4;

		private List <LineSegment> lines = new List<LineSegment > ();
		private List <LineSegment> stickylines = new List<LineSegment > ();
		private List <LineSegment> bouncylines = new List<LineSegment > ();
		private List <LineSegment> deathLines = new List<LineSegment > ();
		private List <LineSegment> restartLines = new List<LineSegment > ();
		private List <LineSegment> shootingLines = new List<LineSegment > ();

		private List <Ball> corners = new List<Ball > ();
		private List <Ball> stickycorners = new List<Ball > ();
		private List <Ball> Bouncycorners = new List<Ball > ();
		private List <Ball> DeathCorners = new List<Ball > ();

		private List <Sprite> trajectoryPoints = new List<Sprite>();

		private List <Star> levelStars = new List<Star>();
		private List <Sprite> starLocations = new List<Sprite>();
		private List <Sprite> _starList = new List<Sprite> ();

		private List <Sprite> buttonList = new List<Sprite> ();

		private List <AnimationSprite> eventAnimationSprites = new List<AnimationSprite> ();
		private List <AnimationSprite> pickedUpStars = new List<AnimationSprite> ();
		private List <AnimationSprite> constantAnimationSprites = new List<AnimationSprite> ();

		AnimationSprite monsterAnimation; 
		AnimationSprite monsterHitAnimation;
		AnimationSprite cannonAnimation;

		private MouseHandler _ballHandler = null;

		public Level (MyGame game, Map map, GameProgressData progressData, int WhichLevel)
		{
			_whichLevel = WhichLevel;
			_game = game;
			_progressData = progressData;

			//--------------Background--------------
			background = new Sprite ("backgroundnew.png");
			AddChild (background);
			background.SetXY(0, -300);

			//------------------Candy----------------
			_ball = new Ball (32, new Vec2 (755, 210), new Vec2(0, 0),new Vec2(0, 0), Color.HotPink, true);
			AddChild (_ball);
			_ball.alpha = 0;

			candy = new Sprite ("candyshadow.png");
			candy.alpha = 0.8f;
			candy.SetOrigin (candy.width / 2-1, candy.height / 2-1);
			AddChild (candy);

			//---------Create all platoforms---------
			this.map = map;
			ObjectGroup[] objectlayers = map.ObjectGroup;
			for(int j =0; j < objectlayers.Length; j++)
			{
				DrawLevel(objectlayers[j]);
			}

			restartButton = new Sprite ("hud1.png");
			AddChild (restartButton);
			restartButton.SetXY (_game.width - restartButton.width*2-10, 3);
			restartButtonStartX = restartButton.x;
			restartButtonStartY = restartButton.y;
			buttonList.Add (restartButton);

			levelSelection = new Sprite ("hud2.png");
			AddChild (levelSelection);
			levelSelection.SetXY (_game.width - levelSelection.width*2-10+64, 3);
			levelSelectionStartX = levelSelection.x;
			levelSelectionStartY = levelSelection.y;
			buttonList.Add (levelSelection);

			hudStrength = new Sprite ("hud3.png");
			AddChild (hudStrength);
			hudStrength.SetXY (5, 3);

			star1 = new Sprite ("starg.png");
			AddChild (star1);
			star1.SetXY (_game.width/2-star1.width/2,  0);

			star2 = new Sprite ("starg.png");
			AddChild (star2);
			star2.SetXY (_game.width/2-star1.width*2,  0);

			star3 = new Sprite ("starg.png");
			AddChild (star3);
			star3.SetXY (_game.width/2+star1.width,  0);

			strengthLine = new LineSegment (new Vec2 (0,10),  new Vec2 (0,10), 0xff00ff00, 13);
			AddChild (strengthLine);

			_ballHandler = new MouseHandler (game);
		}

		//===================================DRAW LEVEL====================================================================================================================
		private void DrawLevel (ObjectGroup  objectGroup)
		{
			Object[] objects = objectGroup.Object;
			for (int i = 0; i < objects.Length; i++)
			{
				//----------------lines------------------------------
				if (objects [i].Polyline != null)
				{
					string endxy = objects [i].Polyline.Points;
					string[] trashend = endxy.Split (' ');
					string[] xy = trashend[1].Split(',');

					Vec2 lineEndPoint = new Vec2 (float.Parse (xy [0]), float.Parse (xy [1]));
					lineEndPoint.RotateDegrees (objects [i].Rotation);
					LineSegment _line = new LineSegment (new Vec2 (objects [i].x, objects [i].y), new Vec2(objects [i].x, objects [i].y).Add(lineEndPoint), 0x00000000, 1);   // 0xff00ff00
					AddChild (_line);

					if (objects [i].ObjectProperties != null)
					{
						ObjectProperty[] properties = objects [i].ObjectProperties.ObjectProperty;
						for (int a = 0; a < properties.Length; a++)
						{
							if (properties [a].Name == "Stickiness")
							{
								if (properties [a].Value == "Sticky")
								{
									stickylines.Add (_line);
								}
								if (properties [a].Value == "Bouncy")
								{
									bouncylines.Add (_line);
								}
								if (properties [a].Value == "Death")
								{
									deathLines.Add (_line);
								}
								if (properties [a].Value == "Restart") 
								{
									restartLines.Add (_line);
								}
							}
						}
					}
					else
						lines.Add (_line);
				}

				//----------------corners--------------------
				if (objects [i].Ellipse != null)
				{
					Vec2 cornerposition = new Vec2 (objects [i].x, objects [i].y);
					Vec2 cornerpitionadjust = new Vec2 (objects [i].Width / 2, objects [i].Width / 2);
					cornerpitionadjust.RotateDegrees (objects [i].Rotation);
					cornerposition.Add (cornerpitionadjust);

					Ball platformCorner = new Ball (objects[i].Width/2, cornerposition, new Vec2(0, 0),new Vec2(0, 0), Color.Transparent);
					AddChild (platformCorner);

					if (objects [i].ObjectProperties != null)
					{
						ObjectProperty[] properties = objects [i].ObjectProperties.ObjectProperty;
						for (int a = 0; a < properties.Length; a++)
						{
							if (properties [a].Name == "Stickiness")
							{
								if (properties [a].Value == "Sticky")
								{
									stickycorners.Add (platformCorner);
								}
								if (properties [a].Value == "Bouncy")
								{
									Bouncycorners.Add (platformCorner);
								}
								if (properties [a].Value == "Death")
								{
									DeathCorners.Add (platformCorner);
								}
							}
						}
					}
					else
						corners.Add (platformCorner);
				}

				if (objects [i].ObjectProperties != null)
				{
					//----------------textures and objects-------------
					ObjectProperty[] properties = objects [i].ObjectProperties.ObjectProperty;
					for (int a = 0; a < properties.Length; a++)
					{
						if (properties [a].Name == "Texture")
						{
							if (properties [a].Value == "Star2.png")
							{
								Star star = new Star ();
								AddChild (star);
								star.SetXY (objects [i].x, objects [i].y);
								star.Turn (objects [i].Rotation);
								levelStars.Add (star);

								Sprite starPosition = new Sprite ("Star2.png");
								starPosition.SetXY (objects [i].x, objects [i].y);
								starPosition.Turn (objects [i].Rotation);
								starLocations.Add (starPosition);	   
							}
							else
							{
								Sprite texture = new Sprite (properties [a].Value);
								texture.SetOrigin (0, texture.height);
								texture.SetXY (objects [i].x, objects [i].y);


								if (properties [a].Value == "monster.png")
								{
									texture.scale = 1.4f;
									texture.alpha = 0;
									supertongue = new SuperTongue (_game, this, _ball, _progressData, _whichLevel);
									AddChild (supertongue);
									supertongue.SetXY (objects [i].x + 155, objects [i].y-55);

									monsterAnimation = new AnimationSprite ("MonsterStatic.png", 4, 8);
									monsterAnimation.scale = 1.4f;
									monsterAnimation.SetOrigin (0, monsterAnimation.height-50);
									monsterAnimation.SetXY (objects [i].x, objects [i].y);
									AddChild (monsterAnimation);
									monsterAnimation.SetFrame (9);
									constantAnimationSprites.Add (monsterAnimation);
								}
								AddChild (texture);

								if (properties [a].Value == "ground.png")
								{
									texture.alpha = 0.7f;

									for (int l = 0; l < properties.Length; l++)
									{
										if (properties [l].Name == "flip")
										{
											texture.Mirror (true, false);
										}
									}

									groundy = objects [i].y;
								}

								if (properties [a].Value == "cannon.png")
								{
									cannon = texture;

									cannonAnimation = new AnimationSprite ("CannonAnimation.png",3,4);
									cannonAnimation.SetXY (cannon.x, cannon.y);
									cannonAnimation.SetOrigin (0, texture.height);
									cannonAnimation.rotation = texture.rotation;
									AddChild (cannonAnimation);
									constantAnimationSprites.Add (cannonAnimation);
								}

								Vec2 cannonpos;

								if (properties [a].Value == "base2.png") 
								{
									cannonx = texture.x + texture.width / 2;
									cannony = texture.y - texture.height / 4 - 11;

									cannonpos = new Vec2 (cannonx -texture.x, cannony -texture.y);
									cannonpos.RotateDegrees (objects [i].Rotation);
				
									LineSegment rtr = new LineSegment (new Vec2(texture.x,texture.y), new Vec2(texture.x,texture.y).Add(cannonpos),0x00000000, 5);
								
									cannonx = rtr.end.x ;
									cannony = rtr.end.y ;

									cannonangle = objects [i].Rotation;

									_ball.position.x = cannonx;
									_ball.position.y = cannony;
									_ball.x = _ball.position.x;
									_ball.y = _ball.position.y;
								}
							
								texture.Turn (objects [i].Rotation);
							}
						}
					}
				}
			}
		}
		//========================================================================================================================================================================

		//==========================================MOUSEHANDLER===========================================
		private void onMouseDown (GameObject target, MouseEventType type)
		{
			if ( !(Input.mouseX > restartButtonStartX 
				&& Input.mouseX < restartButtonStartX + restartButton.width
				&& Input.mouseY > restartButtonStartY 
				&& Input.mouseY < restartButtonStartY + restartButton.height) 
				&& 
				!(Input.mouseX > levelSelectionStartX 
				&& Input.mouseX < levelSelectionStartX + levelSelection.width
				&& Input.mouseY > levelSelectionStartY
				&& Input.mouseY < levelSelectionStartY + levelSelection.height))
			{
				if (allowLaunch)
				{
					casinoLaunched = true;
					ballVelocity = new Vec2 (10,0);
					ballVelocity.SetAngleDegrees (cannon.rotation-90);
					Console.WriteLine (cannon.rotation - 90);
					_ballHandler.OnMouseMove += onMouseMove;
					_ballHandler.OnMouseUp += onMouseUp;
				}
			}
		}

		private void onMouseMove (GameObject target, MouseEventType type)
		{
			//Console.WriteLine (cannon.rotation);

			Vec2 cannonrot = new Vec2 (10, 0);
			ballVelocity.SetAngleDegrees (cannon.rotation-90);
			//cannonrot.Scale (10);
			//LineSegment testttt = new LineSegment (new Vec2 (cannonx, cannony), cannonrot.Add (new Vec2 (cannonx, cannony)), 0x0ff0ff00, 5);
			//AddChild (testttt);
			//ballVelocity = new Vec2 (-cannonx + Input.mouseX, -cannony + Input.mouseY);
		}

		private void onMouseUp (GameObject target, MouseEventType type)
		{
			

			ballLaunched = true;
			casinoLaunched = false;

			_ballHandler.OnMouseMove -= onMouseMove;
			_ballHandler.OnMouseDown -= onMouseDown;
			_ballHandler.OnMouseUp -= onMouseUp;


			allowCannonAnimation = true;





			//trajectoryPoints.Clear ();
		}
		//====================================================================================================

		//===========================================UPDATE============================================
		void Update ()
		{
			if (allowCannonAnimation) {
				if (cannonTimer <= 0) {
					cannonAnimation.NextFrame ();
					cannonTimer = 4;
				}

				if (cannonAnimation.currentFrame == 8)
				{
					if (!cannonAnimationStarted)
					{
						

						_ball.SetAlpha (1);
						candy.alpha = 0.8f;

						ballVelocity.SetAngleDegrees (cannon.rotation - 90);
						_ball.velocity.Set (ballVelocity.Normalize ().Scale (strength / 35));
						_ball._acceleration.Set (0, 0.1f);
						if (_ball.velocity.x > 0)
							anglestep = 1.7f;
						else
							anglestep = -1.7f;

						AnimationSprite cannonParticles = new AnimationSprite ("cannon particles.png", 4, 4);
						AddChild (cannonParticles);
						cannonParticles.rotation = cannon.rotation - 90;
						cannonParticles.SetOrigin (0, cannonParticles.height / 2);
						cannonParticles.SetXY (_ball.x, _ball.y);
						eventAnimationSprites.Add (cannonParticles);

						Sound cannonSound = new Sound ("CannonShot.wav");
						cannonSound.Play ();
						cannonAnimationStarted = true;
					}
				}

				if (cannonAnimation.currentFrame == 9)
				{
					cannonAnimation.SetFrame (0);

					cannonAnimationStarted = false;
					allowCannonAnimation = false;
				}
			}
			cannon.alpha = 0;
			cannonAnimation.rotation = cannon.rotation;
			cannonAnimation.SetOrigin (cannon.width / 2, cannon.height / 2 + cannon.height / 4);
			cannonAnimation.SetXY (cannonx, cannony);

			bool trajectoryListIsNotEmpty = false;

			foreach (Sprite trajectoryPoint in trajectoryPoints)
			{
				if (trajectoryPoint != null)
					trajectoryListIsNotEmpty = true;
				
				if(trajectoryPoint.alpha > 0.03f)
				trajectoryPoint.alpha -=0.03f;
				else
				{
				trajectoryPoint.Destroy ();
				}


			}
			if(!trajectoryListIsNotEmpty)
			{
				trajectoryPoints.Clear ();
			}


			stickySoundTimer--;
			normalSoundTimer--;
			bouncySoundTimer--;
			contactTimer--;
			cannonTimer--;

			Animate ();

			if (!ballLaunched && !showPreview) 
			{
				cannon.SetOrigin (cannon.width / 2, cannon.height / 2 + cannon.height / 4);
				cannon.SetXY (cannonx, cannony);

				float mouseangle = new Vec2 (-cannonx + Input.mouseX, -cannony + Input.mouseY).GetAngleDegrees ();
				Vec2 shootingpoint = new Vec2 (96, 0);

				if ((mouseangle < (cannonangle % 360) + 360) && (mouseangle > (cannonangle % 360) + 360 - 180)) {
				
					cannon.rotation = (-720 + mouseangle + 90);
					shootingpoint.SetAngleDegrees (-720 + mouseangle);
					_ball.position.x = cannonx + shootingpoint.x;
					_ball.position.y = cannony + shootingpoint.y;
					_ball.SetAlpha (0);
					candy.alpha = 0;
				}


				else if (mouseangle > ((cannonangle + 1440) % 360 + 180) && mouseangle < 360) {
					cannon.rotation = (-720 + mouseangle + 90);
					shootingpoint.SetAngleDegrees (-720 + mouseangle);
					_ball.position.x = cannonx + shootingpoint.x;
					_ball.position.y = cannony + shootingpoint.y;
					_ball.SetAlpha (0);
					candy.alpha = 0;
				}

				else
				{
					shootingpoint.SetAngleDegrees (cannon.rotation-90);
					_ball.position.x = cannonx + shootingpoint.x;
			    	_ball.position.y = cannony + shootingpoint.y;
					_ball.SetAlpha (0);
					candy.alpha = 0;
				}
			}

			//level death boundaries
			if (_ball.x < _game.x-_ball.radius || _ball.x > _game.x + game.width+_ball.radius) {
				Console.WriteLine ("hola");
				restartTimer -=40;
				_ball.velocity.Scale (0);
				_ball._acceleration.Scale (0);
				failed = true;
			}

			if (failed) 
			{
				restartTimer--;
				if (restartTimer <= 0) {
					restartTimer = 80;
					failed = false;
					CheckReset (true);
				}
			}

			if (Input.GetMouseButtonDown (0)
			    && Input.mouseX > restartButtonStartX
			    && Input.mouseX < restartButtonStartX + restartButton.width
			    && Input.mouseY > restartButtonStartY
			    && Input.mouseY < restartButtonStartY + restartButton.height)
			{
				Sound click = new Sound("Click.wav");
				click.Play ();
				CheckReset(true);
			}

			if (Input.GetMouseButtonDown (0)
				&& Input.mouseX > levelSelectionStartX 
				&& Input.mouseX < levelSelectionStartX + levelSelection.width
				&& Input.mouseY > levelSelectionStartY
				&& Input.mouseY < levelSelectionStartY + levelSelection.height
			    && !supertongue.eat)
			{
				Sound click = new Sound("Click.wav");
				click.Play ();
				_ballHandler.OnMouseDown -= onMouseDown;
				_game.SetXY (0, 0);
				_game.SetState ("LevelSelection");
			}
				
			if (showPreview)
			{
				ShowPreview ();
			}
			else
			{
				_ball.rotation += anglestep;

				//------------ball movement------------------------
				for (int k = 1; k < 3; k++)
				{
					MakeStep ();

					if (casinoLaunched)
					{
						LaunchCasinoIndicator ();
					}

					candy.SetXY (_ball.position.x, _ball.position.y);
				}

				//--------Star Collisions--------------------------
				CheckStarCollision();

				//---parallaxscrolling+cameramovement--------------
				if (ballLaunched && !dead) 
				{
					MoveCamera ();
				} 
				else
				{
					float strengthconverter =  ((strength -120)/180)*300;
					strengthLine.end.x =-_game.x +28 ;
					strengthLine.end.y =-_game.y+299 +17;
					strengthLine.start.x = -_game.x+28;
					strengthLine.start.y = -_game.y+17+300-strengthconverter;
				}

				//--reset ball/reset camera/ reset tongue----------
				CheckReset();
			}
		}
		//=============================================================================================


		public void Animate()
		{
			animationTimer--;
			starAnimationTimer--;

			if (starAnimationTimer <= 0)
			{
				foreach (AnimationSprite pickedUpStar in pickedUpStars)
				{
					if (pickedUpStar.currentFrame == pickedUpStar.frameCount - 1)
					{
						pickedUpStar.Destroy ();
					}
					pickedUpStar.NextFrame ();
				}
				starAnimationTimer = 4;
			}


			if (animationTimer <= 0) 
			{
				foreach (AnimationSprite animationSprite in eventAnimationSprites)
				{
					if (animationSprite.currentFrame == animationSprite.frameCount - 1)
					{
						animationSprite.Destroy ();
					}
					animationSprite.NextFrame ();
				}
					


				animationTimer = 2;
				animationTimer2--;

				if (new Vec2 (supertongue.x - _ball.x, supertongue.y - _ball.y).Length () > 300) 
				{
					if (monsterAnimation.currentFrame >= 24 && monsterAnimation.currentFrame <= 28)
					{
						supertongue.scale *= 0.85f;
					}
					else 
					{
						supertongue.scale *= 1.25f;
						if (supertongue.scale > 1)
							supertongue.scale = 1;
					}
				} 
				else 
				{
					supertongue.scale *= 1.4f;
					if(supertongue.scale > 1)
						supertongue.scale = 1;
				}
			}

			if (animationTimer2 <= 0) 
			{
				monsterAnimation.NextFrame ();
				if (monsterHitAnimation != null)
				{
					monsterHitAnimation.NextFrame ();
				}

				if (monsterAnimation.currentFrame == 31) 
				{
					monsterAnimation.SetFrame (0);
				}
				if ((monsterAnimation.currentFrame == 27) && (new Vec2 (supertongue.x - _ball.x, supertongue.y - _ball.y).Length () < 300)  ) 
				{
					monsterAnimation.SetFrame (0);
				}
				animationTimer2 = 3;
			}
		}

		//================================SHOW  LEVEL PREVIEW=====================================
		public void ShowPreview()
		{
			if (goingDown)
			{
				_game.y-= groundy/300;
				if (_game.y <= -groundy + _game.height)
				{
					goingDown = false;
				}
			}
			else
			{
				_game.y+= groundy/300;
				if (_game.y >= 0)
				{
					_ballHandler.OnMouseDown += onMouseDown;
					showPreview = false;
					supertongue.preview = false;
					supertongue.rotation = 0;
				}
			}

			foreach (Sprite button in buttonList)
			{
				button.x = -_game.x + _game.width + buttonList.IndexOf(button)*64 - button.width*2-10;
				button.y = -_game.y+3;
			}

			hudStrength.x = -game.x + 5;
			hudStrength.y = -game.y + 3;

			star1.SetXY (-_game.x+_game.width/2-star1.width/2,  -_game.y);

			star2.SetXY (-_game.x+_game.width/2-star1.width*2,  -_game.y);

			star3.SetXY (-_game.x+_game.width/2+star1.width,  -_game.y);
		}
		//=========================================================================================

		//==================BALLSTEP===================
		public void MakeStep()
		{
			enoughAnimations = false;
		    bool  skipgravity = false;

			if (!failed)
			{
				//---line-collision-check------------------
				foreach (LineSegment line in lines)
				{
					if (CheckCollision (line, false, 0.7f))
						skipgravity = true;

					if (CheckCollision (line, true, 0.7f))
						skipgravity = true;
				}

				//---sticky line collision check-----------
				foreach (LineSegment line in stickylines)
				{
					if (CheckCollision (line, false, 0.1f))
						skipgravity = true;
				
					if (CheckCollision (line, true, 0.1f))
						skipgravity = true;
				}

				//---bouncy line collision check-----------
				foreach (LineSegment line in bouncylines)
				{
					if (CheckCollision (line, false, 1.4f))
						skipgravity = true;
				
					if (CheckCollision (line, true, 1.4f))
						skipgravity = true;
				}

				//---Death line collision check-----------
				foreach (LineSegment line in deathLines)
				{
					if (CheckCollision (line, false, 0f))
						skipgravity = true;
				
					if (CheckCollision (line, true, 0f))
						skipgravity = true;
				}

				//---Restart line collision check-----------

				foreach (LineSegment line in restartLines)
				{
					if (CheckCollision (line, false, -100))
						skipgravity = true;

					if (CheckCollision (line, true, -100))
						skipgravity = true;
				}
			

				//---corner collision check----------------
				foreach (Ball sphere in corners)
					CheckSphereCollision (sphere, 0.7f);

				//---sticky corner collision check---------
				foreach (Ball sphere in stickycorners)
					CheckSphereCollision (sphere, 0.1f);

				//---bouncy corner collision-check---------
				foreach (Ball sphere in Bouncycorners)
					CheckSphereCollision (sphere, 1.4f);

				//---Death corner collision-check---------
				foreach (Ball sphere in DeathCorners)
					CheckSphereCollision (sphere, 0);

			}
			//--------makestep-------
			_ball.Step (skipgravity);
		}
		//===============================================

		//==================LINECOLLISION======================================================================
		public bool CheckCollision(LineSegment _line, bool flip, float bounciness)
		{
			Vec2 differenceVector = _ball.position.Clone ().Sub (_line.start);
			Vec2 linenormal = _line.end.Clone ().Sub (_line.start).Normal ();
			linenormal.Scale (flip?- 1:1);

			float A = differenceVector.Dot (linenormal) - _ball.radius; // ballDistance

			Vec2 differenceVector2 = _line.end.Clone ().Add (_ball.velocity);
			differenceVector2.Sub (_line.start);

			float B = differenceVector2.Dot (linenormal);

//			if (new Vec2().Set(_ball.position.Clone().Sub( _line.start)).Length() < _ball.radius)
//			{
//				Vec2 toLineCap = _ball.position.Clone().Sub(_line.start);
//				float lengthToCap = toLineCap.Length ();
//				_ball.position.Add (toLineCap.Normalize ().Scale (_ball.radius - lengthToCap));
//				_ball.velocity.Reflect (toLineCap.Normalize(), bounciness);
//				_ball.x = _ball.position.x;
//				_ball.y = _ball.position.y;
//			}
//
//			if(new Vec2().Set(_line.end.Clone().Sub(  _ball.position)).Length() < _ball.radius)
//			{
//				Vec2 toLineCap = _ball.position.Clone().Sub(_line.end);
//				float lengthToCap = toLineCap.Length ();
//				_ball.position.Add (toLineCap.Normalize ().Scale (_ball.radius - lengthToCap));
//				_ball.velocity.Reflect (toLineCap.Normalize(), bounciness);
//				_ball.x = _ball.position.x;
//				_ball.y = _ball.position.y;
//			}

			if (A > 0 && -B >= A)
			{
				differenceVector = _ball.position.Clone ().Sub (_line.start);
				float impactpoint = differenceVector.Dot ((_line.end.Clone ().Sub (_line.start)).Normalize ());
				float linelength = (_line.end.Clone ().Sub (_line.start)).Length ();

				if (!(impactpoint < 0 || impactpoint > linelength))
				{
					if (bounciness == 0)
					{
						Sound spikeHit = new Sound ("spikeHit.wav");
						spikeHit.Play ();

						dead = true;

						_ball.velocity.Set (new Vec2 (0, -10));
						_ball._acceleration.Set (new Vec2 (0, 0.2f));
						anglestep *= 15;
						failed = true;
						return false;
					}

					if (bounciness == -100)
					{
						_ball.velocity.Set (new Vec2 (0, -10));
						_ball._acceleration.Set (new Vec2 (0, 0.2f));
						anglestep *= 15;
						failed = true;
						return false;
					}

					if (bounciness == 1.4f && bouncySoundTimer <=0 && (lastBouncyObjectx != _line.start.x ||  lastBouncyObjecty != _line.start.y ))
					{
						Sound bouncyPlat = new Sound ("BouncyPlat.wav");
						bouncyPlat.Play ();
						bouncySoundTimer = 3;
						lastBouncyObjectx = _line.start.x;
						lastBouncyObjecty = _line.start.y;
					}

						
					_ball.position.Add (_ball.velocity.Clone ().Scale (-A / B));
					_ball.velocity.Reflect (_line.end.Clone ().Sub (_line.start).Normal (), bounciness);
					_ball.x = _ball.position.x;
					_ball.y = _ball.position.y;
					anglestep *= -1;


					if (!enoughAnimations &&  bounciness == 0.7f && normalSoundTimer <=0 && (lastNormalObjectx != _line.start.x ||  lastNormalObjecty != _line.start.y )) 
					{
						enoughAnimations = true;

						if (contactTimer <= 0) 
						{
							AnimationSprite contact = new AnimationSprite ("contact.png", 5, 1);
							AddChild (contact);
							contact.SetOrigin (contact.width / 2, contact.height);
							Vec2 contactPosition = new Vec2 (_ball.x, _ball.y).Sub (linenormal.Clone ().Scale (_ball.radius));
							contact.SetXY (contactPosition.x, contactPosition.y);
							contact.rotation = linenormal.GetAngleDegrees () + 90;
							eventAnimationSprites.Add (contact);
							contactTimer = 2;
						}

						Sound normPlat = new Sound ("RegularPlat.wav");
						normPlat.Play ();
						stickySoundTimer = 3;
						lastNormalObjectx = _line.start.x;
						lastNormalObjecty = _line.start.y;
					}

					return true;
				}
				else return false;

			}
			else
				return false;
		}
		//=====================================================================================================

		//==================SPHERECOLLISION==========================================================================
		public void CheckSphereCollision(Ball sphere, float bounciness)
		{
			if(new Vec2().Set(sphere.position.Clone().Sub(  _ball.position)).Length() < _ball.radius + sphere.radius)
			{
				

			
				Vec2 toSphere = _ball.position.Clone().Sub(sphere.position);
				float lengthToCap = toSphere.Length ();
				_ball.position.Add (toSphere.Normalize ().Scale (_ball.radius + sphere.radius - lengthToCap));
				_ball.velocity.Reflect (toSphere.Normalize(), bounciness);
				_ball.x = _ball.position.x;
				_ball.y = _ball.position.y;
				anglestep *= -1;

				if (bounciness == 0)
				{
					_ball.velocity.Set (new Vec2 (0, 0));
					_ball._acceleration.Set (new Vec2 (0, 0));
					anglestep = 0;
					return;
				}

				if (bounciness == 0.1f && stickySoundTimer <=0 && (lastStickyObjectx != sphere.x ||  lastStickyObjecty != sphere.y )) 
				{
					Sound stickyPlat = new Sound ("StickyPlat.wav");
					stickyPlat.Play ();
					stickySoundTimer = 3;
					lastStickyObjectx = sphere.x;
					lastStickyObjecty = sphere.y;
				}

				if (!enoughAnimations && bounciness == 0.7f && normalSoundTimer <=0 && (lastNormalObjectx != sphere.x ||  lastNormalObjecty != sphere.y )) 
				{
					enoughAnimations = true;

					if (contactTimer <= 0)
					{
						AnimationSprite contact = new AnimationSprite ("contact.png", 5, 1);
						AddChild (contact);
						contact.SetOrigin (contact.width / 2, contact.height);
						Vec2 contactPosition = new Vec2 (_ball.x, _ball.y).Add (toSphere.Normalize ().Scale (_ball.radius));
						contact.SetXY (contactPosition.x, contactPosition.y);
						contact.rotation = toSphere.GetAngleDegrees () - 90;
						eventAnimationSprites.Add (contact);
						contactTimer = 2;
					}

					Sound normPlat = new Sound ("RegularPlat.wav");
					normPlat.Play ();
					stickySoundTimer = 3;
					lastNormalObjectx = sphere.x;
					lastNormalObjecty = sphere.y;
				}
			}
		}
		//===========================================================================================================

		//===================================================CASINOINDICATOR=====================================================
		public void LaunchCasinoIndicator()
		{
			Vec2 _newVelocity = ballVelocity.Clone ().Normalize ().Scale (strength / 35);
			Vec2 trajectoryNew = new Vec2 ().Set (_ball.position);

			//--------destroy old trajectory------------------
			foreach (Sprite trajectoryPoint in trajectoryPoints)
			{
				trajectoryPoint.Destroy ();
			}

			trajectoryPoints.Clear ();
			//------------------------------------------------

			//--------draw new trajectory-----------------------------------------------------------------------------------------
			for (int i = 1; i < 12; i++)
			{
				for (int j = 1; j < 10; j++)
				{
					trajectoryNew.Add (_newVelocity);
					_newVelocity.y+=0.1f;
				}
					
				Sprite trajectoryCircle = new Sprite ("trajectory circle.png");
				trajectoryCircle.SetOrigin (trajectoryCircle.width / 2, trajectoryCircle.height / 2);
				trajectoryCircle.SetXY (trajectoryNew.x, trajectoryNew.y);
				trajectoryCircle.alpha = 1 - (float)i/20;
				trajectoryCircle.scale = 1 - (float)i/15;
				Console.WriteLine (trajectoryCircle.alpha);
				AddChild (trajectoryCircle);
				trajectoryPoints.Add (trajectoryCircle);
			}
			//---------------------------------------------------------------------------------------------------------------------

			if (IncreaseSpeed)
			{
				strength++;
				if (strength == 300)
					IncreaseSpeed = false;
			}
			else
			{
				strength--;
				if (strength == 120)
					IncreaseSpeed = true;
			}
		}
		//=========================================================================================================================

		//======================CHECK STAR COLLISION===================
		public void CheckStarCollision()
		{
			foreach (Sprite other in _ball.GetCollisions())
			{
				if (other is Star)
				{
					AnimationSprite starAnim = new AnimationSprite ("StarAnimation.png", 3, 4);
					starAnim.SetOrigin (0, starAnim.height);
					starAnim.SetXY (other.x, other.y);
					starAnim.rotation = other.rotation;
					pickedUpStars.Add (starAnim);
					AddChild (starAnim);

					other.Destroy ();
					inventoryStar = new Sprite ("Star2.png");
					AddChild (inventoryStar);
					_starList.Add (inventoryStar);
				}
			}
		}
		//==============================================================

		//===================UPDATE PROGRESS DATA===============
		public void UpdateProgressData()
		{
			switch (_whichLevel)
			{
			case 1:
				if (_progressData.Level1Stars < _starList.Count)
					_progressData.Level1Stars = _starList.Count;
				break;
			case 2:
				if (_progressData.Level2Stars < _starList.Count)
					_progressData.Level2Stars = _starList.Count;
				break;
			case 3:
				if (_progressData.Level3Stars < _starList.Count)
					_progressData.Level3Stars = _starList.Count;
				break;
			case 4:
				if (_progressData.Level4Stars < _starList.Count)
					_progressData.Level4Stars = _starList.Count;
				break;
			case 5:
				if (_progressData.Level5Stars < _starList.Count)
					_progressData.Level5Stars = _starList.Count;
				break;
			case 6:
				if (_progressData.Level6Stars < _starList.Count)
					_progressData.Level6Stars = _starList.Count;
				break;
			case 7:
				if (_progressData.Level7Stars < _starList.Count)
					_progressData.Level7Stars = _starList.Count;
				break;
			case 8:
				if (_progressData.Level8Stars < _starList.Count)
					_progressData.Level8Stars = _starList.Count;
				break;
			}
		}
		//======================================================

		//============================MOVE CAMERA=============================
		public void MoveCamera()
		{

			_game.y += ((-_ball.y + _game.height / 2) - _game.y) / 20;


			if (_game.y <= -groundy + _game.height)
			{
				_game.y = -groundy + _game.height;
			}
			else
			{
				background.y -= ((-_ball.y + _game.height / 2) - _game.y) / 40;
			}

			//--------star movement----------------------------
			foreach (Sprite star in _starList)
			{
				if(_starList.IndexOf(star)==0)
					star.SetXY (-_game.x+_game.width/2-star1.width/2,  -_game.y);
				if(_starList.IndexOf(star)==1)
					star.SetXY (-_game.x+_game.width/2-star1.width*2,  -_game.y);
				if(_starList.IndexOf(star)==2)
					star.SetXY (-_game.x+_game.width/2+star1.width,  -_game.y);
			}
			//--------button movement--------------------------
			foreach (Sprite button in buttonList) 
			{
				button.x = -_game.x + _game.width + buttonList.IndexOf (button) * 64 - button.width * 2 -10;
				button.y = -_game.y+3;
			}

			float strengthconverter =  ((strength -120)/180)*300; 

			strengthLine.end.x =-_game.x +28 ;
			strengthLine.end.y =-_game.y+299 +17;
			strengthLine.start.x = -_game.x+28;
			strengthLine.start.y = -_game.y+17+300-strengthconverter;

			hudStrength.x = -game.x + 5;
			hudStrength.y = -game.y + 3;

			star1.SetXY (-_game.x+_game.width/2-star1.width/2,  -_game.y);

			star2.SetXY (-_game.x+_game.width/2-star1.width*2,  -_game.y);

			star3.SetXY (-_game.x+_game.width/2+star1.width,  -_game.y);
		}
		//=====================================================================

		//============================CHECK RESET=========================
		public void CheckReset(bool skipR = false)
		{
			if (camerasetonthestart)
			{
				if (step >= 1)
				{
					_game.x -= _game.x / step;
					_game.y -= _game.y / step;
					background.x -= background.x / step;
					background.y -= background.y / step + 300 / step;

					//--------star movement----------------------------
					foreach (Sprite star in _starList)
					{
						if(_starList.IndexOf(star)==0)
							star.SetXY (-_game.x+_game.width/2-star1.width/2,  -_game.y);
						if(_starList.IndexOf(star)==1)
							star.SetXY (-_game.x+_game.width/2-star1.width*2,  -_game.y);
						if(_starList.IndexOf(star)==2)
							star.SetXY (-_game.x+_game.width/2+star1.width,  -_game.y);
					}
					//--------button movement--------------------------
					foreach (Sprite button in buttonList) 
					{
						button.x = -_game.x + _game.width + buttonList.IndexOf (button) * 64 - button.width * 2 -10;
						button.y = -_game.y+3;
					}

					float strengthconverter =  ((strength -120)/180)*300; 

					strengthLine.end.x =-_game.x +28 ;
					strengthLine.end.y =-_game.y+299 +17;
					strengthLine.start.x = -_game.x+28;
					strengthLine.start.y = -_game.y+17+300-strengthconverter;

					hudStrength.x = -game.x + 5;
					hudStrength.y = -game.y + 3;

					star1.SetXY (-_game.x+_game.width/2-star1.width/2,  -_game.y);

					star2.SetXY (-_game.x+_game.width/2-star1.width*2,  -_game.y);

					star3.SetXY (-_game.x+_game.width/2+star1.width,  -_game.y);
				}
				else
				{
					camerasetonthestart = false;
					allowLaunch = true;
				}
				step-=0.5f;
			}
			else if (Input.GetKeyDown (Key.R) || skipR)
			{
				if (!supertongue.eat && ballLaunched) 
				{
					if (_ball.position.x != cannonx && _ball.position.y != cannony)
					{
						step = 30;
						anglestep = 0;
						Console.WriteLine ("sds");
						allowLaunch = false;
						camerasetonthestart = true;
						dead = false;

						_ball.position.x = -1000;
						_ball.position.y = -1000;

						_ball.velocity.Set (new Vec2 (0, 0));
						_ball._acceleration.Set (new Vec2 (0, 0));

						ballLaunched = false;
						_ballHandler.OnMouseDown += onMouseDown;

						for (int i = 0; i< _starList.Count; i++) 
						{
							_starList [i].Destroy ();
						}
						_starList.Clear ();

						for (int i = 0; i< levelStars.Count; i++) 
						{
							levelStars [i].Destroy ();
						}
						levelStars.Clear ();

						foreach (Sprite starLocation in starLocations)
						{
							Star star = new Star ();
							AddChild (star);
							star.SetXY (starLocation.x, starLocation.y);
							star.Turn (starLocation.rotation);
							levelStars.Add (star);
						}

						lastStickyObjectx=0;
						lastStickyObjecty=0;
						lastNormalObjectx=0;
						lastNormalObjecty=0;
						lastBouncyObjectx=0;
						lastBouncyObjecty=0;
							
					}
				}
			}
		}
		//=================================================================

		//========UNLOCK NEXT LEVEL==========
		public void UnlockNexTLevel ()
		{
			switch (_whichLevel)
			{
			case 1:
				_progressData.Level2 = false;
				break;
			case 2:
				_progressData.Level3 = false;
				break;
			case 3:
				_progressData.Level4 = false;
				break;
			case 4:
				_progressData.Level5 = false;
				break;
			case 5:
				_progressData.Level6 = false;
				break;
			case 6:
				_progressData.Level7 = false;
				break;
			case 7:
				_progressData.Level8 = false;
				break;
			}
		}
		//===================================

		//=====GET STAR AMMOUNT======
		public int GetAmmountStars ()
		{
			return _starList.Count;
		}
		//===========================

		public void SetEatingAnimation()
		{
			if (!alreadyEating) 
			{
				Console.WriteLine("yay");
				monsterHitAnimation = new AnimationSprite ("MonsterHit SpriteSheet.png", 4, 8);
				monsterHitAnimation.scale = 1.4f;
				monsterHitAnimation.SetOrigin (0, monsterHitAnimation.height-50);
				monsterHitAnimation.SetXY (monsterAnimation.x, monsterAnimation.y);
				AddChild (monsterHitAnimation);

				monsterAnimation.Destroy ();
				alreadyEating = true;
			}
		}

		public void ShowScoreScreen()
		{
			ScoreScreen _scoreScreen = new ScoreScreen (_game, _whichLevel, _progressData);
			AddChild (_scoreScreen);
			_scoreScreen.x -= _game.x;
			_scoreScreen.y -= _game.y;
		}

		public void PressButton(int buttonnumber)
		{
			if (buttonnumber == 1) 
			{

			}
			if (buttonnumber == 2) 
			{

			}
			if (buttonnumber == 3) 
			{

			}
		}
	}
}

