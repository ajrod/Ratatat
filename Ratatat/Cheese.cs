//Author: Alex Rodrigues
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ratatat
{
    /// <summary>
    /// The cheese in the rat trials. The rats want this!
    /// </summary>
    public class Cheese
    {
        /// <summary>
        /// The location of the cheese on MainForm.
        /// </summary>
        public PointF location;
        /// <summary>
        /// The picture of the cheese to be drawn to the screen.
        /// </summary>
        public Picture pic;
        /// <summary>
        /// The picture's angle in degrees. Default is zero (facing right)
        /// </summary>
        public float facingAngle = 0;
        /// <summary>
        /// The radius of the cheese to be used for radial collision detection.
        /// </summary>
        public float radius;
        /// <summary>
        /// The velocity of the cheese. This will only ever be one wall unit in the specified direction.
        /// </summary>
        public PointF velocity;
        /// <summary>
        /// Determines if the picture needs to be updated or not.
        /// This is toggled only when the cheese is moved.
        /// </summary>
        public bool change = true;
        /// <summary>
        /// The width of the cheese.
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the cheese.
        /// </summary>
        public int height;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cheese"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="frameW">The frame W.</param>
        /// <param name="frameH">The frame H.</param>
        /// <param name="frameDelay">The frame delay.</param>
        public Cheese(PointF location, int frameW, int frameH, int frameDelay)
        {
            radius = (int)(0.35 * MainForm.LEN);
            pic = new Picture("pics/Cheese-icon.png", location, frameW, frameH, frameDelay);
            width = pic.bitmap.Width;
            height = pic.bitmap.Height;
            this.location = location;
        }

        /// <summary>
        /// Draws the cheese to graphics g.
        /// </summary>
        /// <param name="g">The g.</param>
        public virtual void Draw(Graphics g)
        {
            pic.angle = facingAngle;
            pic.location.X = this.location.X;
            pic.location.Y = this.location.Y;
            pic.Draw(g);
        }

        /// <summary>
        /// Listens to which keys are being pressed down.
        /// Left, Right, Up and Down move the cheeses location.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        public void KeyDown(Object sender, KeyEventArgs e)
        {
            velocity.X = 0;
            velocity.Y = 0;
            change = true;
            if (e.KeyCode == Keys.Left)
            {
                velocity.X = -MainForm.LEN;
            }
            else if (e.KeyCode == Keys.Right)
            {
                velocity.X = MainForm.LEN;
            }
            else if (e.KeyCode == Keys.Up)
            {
                velocity.Y = -MainForm.LEN;
            }
            else if (e.KeyCode == Keys.Down)
            {
                velocity.Y = MainForm.LEN;
            }
            location.X += velocity.X;
            location.Y += velocity.Y;
        }

        /// <summary>
        /// Updates the cheeses picture if it has been moved.
        /// </summary>
        /// <param name="time">The time.</param>
        public virtual void Update(int time)
        {
            if (change)
            {
                pic.Update(time);
                change = false;
            }
        }
    }
}