using GameProject1.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{

    public static class CollisionHelper
    {

        /// <summary>
        /// Detects collision between two bounding circles
        /// </summary>
        /// <param name="a">First bounding circle</param>
        /// <param name="b">Second bounding circle</param>
        /// <returns>True for collision, false otherwise</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects collision between 2 bounding rectangles
        /// </summary>
        /// <param name="a">First rectangle</param>
        /// <param name="b">Second rectangle</param>
        /// <returns>true if collision, false otherwise</returns>
        public static CollisionSide Collides(BoundingRectangle a, BoundingRectangle b)
        {
            if(!((a.Right < b.Left) || (a.Left > b.Right)))
            {
                if (a.Right > b.Left && a.Left < b.Left)
                {
                    return CollisionSide.Left;
                }
                else if (a.Left < b.Right && a.Right > b.Right)
                {
                    return CollisionSide.Right; 
                }
            }
                return CollisionSide.None;
        }

        /// <summary>
        /// Detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="c">Bounding circle</param>
        /// <param name="r">bounding rectangle</param>
        /// <returns>True for collision, false otherwise</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >=
            Math.Pow(c.Center.X - nearestX, 2) +
            Math.Pow(c.Center.Y - nearestY, 2);
        }

        public static bool Collides(BoundingRectangle r, BoundingCircle c) => Collides(c, r);
    }
}
