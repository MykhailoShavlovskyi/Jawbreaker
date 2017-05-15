using System;
using GXPEngine;

namespace GXPEngine
{
	public class Vec2 
	{
		public static Vec2 zero 
		{
			get 
			{
				return new Vec2(0,0); 
			}
		}

		public float x = 0;
		public float y = 0;

		public Vec2 (float pX = 0, float pY = 0)
		{
			x = pX;
			y = pY;
		}

		public override string ToString ()
		{
			return String.Format ("({0}, {1})", x, y);
		}

		public Vec2 Add (Vec2 other)
		{
			x += other.x;
			y += other.y;
			return this;
		}

		public Vec2 Sub (Vec2 other)
		{
			x -= other.x;
			y -= other.y;
			return this;
		}

		public Vec2 Scale(float k)
		{
			x *= k;
			y *= k;
			return this;
		}

		public float Length()
		{
			float length = (float)Math.Sqrt ((x * x) + (y * y));
			return length;
		}

		public Vec2 Normalize()
		{
			float staticLength = Length ();
			y /= staticLength;
			x /= staticLength;
			return this;
		}

		public Vec2 Clone()
		{
			return new Vec2 (x, y);
		}

		public Vec2 Set(float setx, float sety)
		{
			x = setx;
			y = sety;
			return this;
		}

		public Vec2 Set(Vec2 setvec)
		{
			x = setvec.x;
			y = setvec.y;
			return this;
		}
		////////////////////////////////

		static float Deg2Rad(float angle)
		{
			return (float)(Math.PI / 180) * angle;
		}

		static float Rad2Deg(float angle)
		{
			return (float)(180 / Math.PI) * angle;
		}

		static Vec2 GetUnitVectorDegrees(float angle)
		{
			Vec2 unitVector = new Vec2 ((float)Math.Cos (angle), (float)Math.Sin (angle));
			return unitVector;
		}

		static Vec2 GetUnitVectorRadians(float angle)
		{
			return GetUnitVectorDegrees(Deg2Rad (angle));
		}

		public Vec2 RandomUnitVector()
		{
			Random random = new Random();
			float angle = random.Next(0, 360);
			angle = Deg2Rad (angle);
			Vec2 unitVector = new Vec2 ((float)Math.Cos (angle), (float)Math.Sin (angle));
			return unitVector;
		}
		////////////////////////////////

		public void SetAngleDegrees(float angle)
		{
			float oldLength = Length();
			Set((float)(oldLength  * Math.Cos (Deg2Rad(angle))),(float)(oldLength * Math.Sin (Deg2Rad(angle))));		
		}

		public void SetAngleRadians(float angle)
		{

			float oldLength = Length();
			Set((float)(oldLength  * Math.Cos (angle)),(float)(oldLength * Math.Sin (angle)));		
		}

		public float GetAngleRadians()
		{
			float oldLength = Length();
			float angle = (float)Math.Acos (x / oldLength);
			if (y >= 0)
				return angle;
			else
				return (float)Math.PI * 2 - angle;
		}

		public float GetAngleDegrees()
		{
			float oldLength = Length();
			float angle = (float)Math.Acos (x / oldLength);
			if (y >= 0)
				return Rad2Deg (angle);
			else
				return Rad2Deg ((float)Math.PI*2 - angle);
		}

		public void RotateDegrees(float angle)
		{
			Set(x * (float)Math.Cos (Deg2Rad (angle)) - y * (float)Math.Sin (Deg2Rad (angle)), x *  (float)Math.Sin (Deg2Rad (angle)) + y * (float)Math.Cos (Deg2Rad (angle)));
		}

		public void RotateRadians(float angle)
		{
			Set(x * (float)Math.Cos (angle) - y * (float)Math.Sin (angle), x *  (float)Math.Sin (angle) + y * (float)Math.Cos (angle));
		}

		public void RotateAroundRadians(float pointx, float pointy, float angle)
		{
			Set((x - pointx) * (float)Math.Cos (angle) - (y - pointy) * (float)Math.Sin (angle) + pointx, (x - pointx) *  (float)Math.Sin (angle) + (y - pointy) * (float)Math.Cos (angle)+ pointy);
		}

		public void RotateAroundDegrees(float pointx, float pointy, float angle)
		{
			angle = Deg2Rad (angle);
			Set((x - pointx) * (float)Math.Cos (angle) - (y - pointy) * (float)Math.Sin (angle) + pointx, (x - pointx) *  (float)Math.Sin (angle) + (y - pointy) * (float)Math.Cos (angle)+ pointy);
		}

		////////////////////////////////
		public Vec2 Normal()
		{
			return new Vec2 (-y, x).Normalize();
		}

		public float Dot(Vec2 vec)
		{
			return (x * vec.x + y * vec.y);
		}

		public Vec2 Reflect(Vec2 pNormal, float pBounciness = 1)
		{
			float helper = this.Dot (pNormal) * (1 + pBounciness );
			pNormal.Scale(helper);
			return this.Sub(pNormal);
		}

		////////////////////////////////
		public float GetAngleBetween (Vec2 vec)
		{
			return((float) (Math.Acos (    (x * vec.x + y * vec.y) / (Math.Sqrt (x * x + y * y) * Math.Sqrt (vec.x * vec.x + vec.y * vec.y))     )       ));
		}
	}
}

