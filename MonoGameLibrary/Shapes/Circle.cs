using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Shapes
{
    public readonly struct Circle : IEquatable<Circle>
    {
        #region Static Fields

        private static readonly Circle emptyCircle = new Circle();

        #endregion

        #region Public Fields

        public readonly int X;
        public readonly int Y;
        public readonly int Radius;

        #endregion

        #region Properties

        public readonly Point Location
        {
            get { return new Point(X, Y); }
        }

        public static Circle EmptyCircle
        {
            get { return emptyCircle; }
        }

        public readonly bool IsEmpty
        {
            get { return X == 0 && Y == 0 && Radius == 0; }
        }

        public readonly int Top
        {
            get { return Y - Radius; }
        }

        public readonly int Bottom
        {
            get { return Y + Radius; }
        }

        public readonly int Left
        {
            get { return X - Radius; }
        }

        public readonly int Right
        {
            get { return X + Radius; }
        }

        #endregion

        #region Constructor

        public Circle(int x, int y, int radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        #endregion

        #region Public Methods

        public bool IsIntersecting(Circle otherCircle)
        {
            int radiiSquared =
                (this.Radius + otherCircle.Radius) * (this.Radius + otherCircle.Radius);
            float distanceSquared = Vector2.DistanceSquared(
                this.Location.ToVector2(),
                otherCircle.Location.ToVector2()
            );
            return distanceSquared < radiiSquared;
        }

        #endregion

        #region Override Methods

        public override bool Equals(object someObject)
        {
            if (someObject is Circle otherCircle)
            {
                return this.Equals(otherCircle);
            }
            else
            {
                return false;
            }
        }

        public readonly bool Equals(Circle otherCircle)
        {
            if (
                this.X == otherCircle.X
                && this.Y == otherCircle.Y
                && this.Radius == otherCircle.Radius
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Radius);
        }

        #endregion

        #region Operator Overloads

        public static bool operator ==(Circle leftHandSide, Circle rightHandSide)
        {
            return leftHandSide.Equals(rightHandSide);
        }

        public static bool operator !=(Circle leftHandSide, Circle rightHandSide)
        {
            return !(leftHandSide.Equals(rightHandSide));
        }

        #endregion
    }
}