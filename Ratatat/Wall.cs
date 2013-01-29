//Copyright (C) 2013 Alex Rodrigues
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this 
//software and associated documentation files (the "Software"), to deal in the Software without 
//restriction, including without limitation the rights to use, copy, modify, merge, publish, 
//distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom 
//the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or 
//substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
//ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
//SOFTWARE.

//Author: Alex Rodrigues

namespace Ratatat
{
    using System.Drawing;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// A immobile wall, an obstacle for the rats in trial tests.
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// Where the left of the wall resides on Mainform.
        /// </summary>
        public int left;
        /// <summary>
        /// Where the top of the wall resides on Mainform.
        /// </summary>
        public int top;
        /// <summary>
        /// The width of the wall in pixels.
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the wall in pixels.
        /// </summary>
        public int height;
        /// <summary>
        /// The visible state of the wall. If true the wall is drawn to the screen.
        /// </summary>
        public bool Visible;
        /// <summary>
        /// The image of the wall. Clones from Mainform wall_pic.
        /// </summary>
        Bitmap image;

        /// <summary>
        /// True iff the rat has used this location before.
        /// This is specifically for walls that aren't visible and for influencing the rat's score
        /// by penalizing repetitive moves.
        /// </summary>
        public bool[] used;

        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Wall(int left, int top, int width, int height)
        {
            Visible = true;
            used = new bool[MainForm.current_rats_per_trial];
            image = (Bitmap)MainForm.wall_pic.Clone();
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Draws the wall to graphics g.
        /// </summary>
        /// <param name="g">The g.</param>
        public void Draw(Graphics g)
        {
            g.Transform = new Matrix();
            g.DrawImage(image, new RectangleF(left, top, width, height));
        }

        /// <summary>
        /// Returns the point of this wall that is closest to point P.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public PointF PointNearestTo(PointF p)
        {
            // Initialize a new PointF called nearestPoint.
            PointF nearestPoint = new PointF();

            //Check IF the left edge of this wall is to the right of the point "p" if it
            //is, then the nearestPoint's X coordinate must be the left edge of this wall.
            if (p.X < this.left)
            {
                nearestPoint.X = this.left;
            }
            //Else IF the right edge of this wall is to the left of the point "p"
            //the nearestPoint's X coordinatre must be the right edge of this wall

            else if (p.X > this.left + this.width)
            {
                nearestPoint.X = this.left + this.width;
            }
            //If it's not to the left, and it's not to the right, it MUST be inside the
            //wall. So we'll set the nearestPoint's X coordinate equal to p's X coordinate.
            else
            {
                nearestPoint.X = p.X;
            }

            if (p.Y < this.top)
            {
                nearestPoint.Y = this.top;
            }
            else if (p.Y > this.top + this.height)
            {
                nearestPoint.Y = this.top + this.height;
            }
            else
            {
                nearestPoint.Y = p.Y;
            }

            return nearestPoint;
        }
    }
}