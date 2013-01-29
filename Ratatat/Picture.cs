//Copyright (C) 2013 <copyright holders>
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
    /// A utility class for handling pictures.
    /// </summary>
    public class Picture
    {
        public Bitmap bitmap;
        public PointF location;
        public float angle = 0f;
        public PointF offset;
        // The current frame.
        public int frameH = 0;
        public int frameW = 0;
        // The total number of frames.
        public int frameCountH;
        public int frameCountW;
        // The number of "ticks" per frame.
        public int timePerFrame;
        public int animationCounter = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Picture"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="location">The location.</param>
        /// <param name="frameCountW">The frame count W.</param>
        /// <param name="frameCountH">The frame count H.</param>
        /// <param name="timePerFrame">The time per frame.</param>
        public Picture(string fileName, PointF location, int frameCountW, int frameCountH, int timePerFrame)
        {
            this.frameCountW = frameCountW;
            this.frameCountH = frameCountH;
            this.timePerFrame = timePerFrame;
            bitmap = new Bitmap(fileName);
            this.location = location;
            offset = new PointF(bitmap.Width / frameCountW / 2f, bitmap.Height / frameCountH / 2f);
        }

        /// <summary>
        /// Draws the picture to the graphics.
        /// </summary>
        /// <param name="g">The g.</param>
        public void Draw(Graphics g)
        {
            Point drawLocation = new Point((int)(location.X - offset.X), (int)(location.Y - offset.Y));
            Matrix m = new Matrix();
            m.RotateAt(-angle, location);
            g.Transform = m;
            g.DrawImage(bitmap, new Rectangle(drawLocation.X, drawLocation.Y, bitmap.Width / this.frameCountW, bitmap.Height / this.frameCountH),
                new Rectangle(this.frameW * bitmap.Width / this.frameCountW, this.frameH * bitmap.Height / this.frameCountH, bitmap.Width / this.frameCountW, bitmap.Height / this.frameCountH), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Updates the animation of the picture.
        /// </summary>
        /// <param name="time">The time.</param>
        public void Update(int time)
        {
            animationCounter += time;
            if (animationCounter >= this.timePerFrame)
            {
                frameW++;
                if (frameW >= frameCountW)
                {
                    frameW = 0;
                    frameH++;
                    if (frameH >= frameCountH) frameH = 0;
                }
                animationCounter = 0;
            }
        }
    }
}