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

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ratatat
{
    /// <summary>
    /// The poor rat to be trial tested.
    /// </summary>
    public class Rat
    {

        /// <summary>
        /// The location of the rat.
        /// </summary>
        public PointF location;
        /// <summary>
        /// The velocity of the rat.
        /// </summary>
        public PointF velocity;
        /// <summary>
        /// The picture of the rat.
        /// </summary>
        public Picture pic;
        /// <summary>
        /// The angle the rat is facing.
        /// </summary>
        public float facingAngle = 0;
        /// <summary>
        /// The radius of the rat used for collision detection.
        /// </summary>
        public float radius;
        /// <summary>
        /// The fitness score of the rat.
        /// </summary>
        public double score;
        /// <summary>
        /// The coded genes of the rat.
        /// </summary>
        public String code;
        /// <summary>
        /// The decoded genes of the rat.
        /// </summary>
        public String decoded;
        /// <summary>
        /// True iff the rat has finished its trial.
        /// </summary>
        public bool finished;
        /// <summary>
        /// The current chromosome rat is using to move.
        /// </summary>
        public int currentChrom;
        /// <summary>
        /// The number of steps used by the rat. Rats have a finite number of steps they can
        /// use before the trial ends.
        /// </summary>
        public int steps = 0;
        /// <summary>
        /// How many steps are used per movement.
        /// </summary>
        public int steps_per_movement;
        /// <summary>
        /// The width of the rats picture.
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the rats picture.
        /// </summary>
        public int height;
        /// <summary>
        /// The number of times the rat has ran into a wall. Used to penalize the rat.
        /// </summary>
        public int hitWall;
        /// <summary>
        /// The number of times the rat has stepped on a tile that it has already used. This counter
        /// can be used to penalize the rats fitness to promote efficiency.
        /// </summary>
        public int usedPenalty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rat"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="frameW">The frame W.</param>
        /// <param name="frameH">The frame H.</param>
        /// <param name="frameDelay">The frame delay.</param>
        public Rat(PointF location, int frameW, int frameH, int frameDelay)
        {
            usedPenalty = 0;
            hitWall = 0;
            if (MainForm.LEN <= 50)
            {
                steps_per_movement = 4;
            }
            else
            {
                steps_per_movement = 8;
            }
            score = 0;
            finished = false;
            steps = 0;
            currentChrom = 0;
            radius = (int)(0.35 * MainForm.LEN);
            pic = new Picture("pics/Rat.gif", location, frameW, frameH, frameDelay);
            width = pic.bitmap.Width;
            height = pic.bitmap.Height;
            this.location = location;
        }

        /// <summary>
        /// Determines whether the rat is touching the cheese.
        /// </summary>
        /// <param name="cheese">The cheese.</param>
        /// <returns>
        ///   <c>true</c> if the rat is touching the cheese; otherwise, <c>false</c>.
        /// </returns>
        public bool isTouching(Cheese cheese)
        {
            double distance = Math.Sqrt(
                (this.location.X - cheese.location.X) * (this.location.X - cheese.location.X)
                + (this.location.Y - cheese.location.Y) * (this.location.Y - cheese.location.Y));

            return distance < this.radius;
        }

        /// <summary>
        /// Draws the rat to graphics g.
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
        /// Updates many things, such as the rat's current fitness score, velocity,
        /// facing angle among other things.
        /// </summary>
        /// <param name="time">The time.</param>
        public virtual void Update(int time, int rat_numb)
        {
            int distanceModifier = 2;
            double xDiff = Math.Pow(Math.Abs(this.location.X - MainForm.cheese.location.X) / MainForm.LEN, distanceModifier);
            double yDiff = Math.Pow(Math.Abs(this.location.Y - MainForm.cheese.location.Y) / MainForm.LEN, distanceModifier);
            //calculates the rats fitness score
            score = 100 / (xDiff + yDiff + 1 + this.hitWall + (this.usedPenalty * 2));

            if (this.isTouching(MainForm.cheese))
            {
                //gives a bonus to the rat proportional to how efficiently it
                //found the cheese
                score += (MainForm.NUMB_STEPS - currentChrom) + 10;
                this.finished = true;
                return;
            }

            //user is viewing rat trials; make rats gradually go from tile to tile so they can watch
            if (MainForm.DRAW_GAME)
            {
                checkWalls();
                MoveRat(steps_per_movement);
                steps += 1;
                if (steps >= steps_per_movement)
                {
                    centerRat();
                    int row = (int)this.location.Y / MainForm.LEN;
                    int col = (int)this.location.X / MainForm.LEN;
                    PointF touchPoint = new PointF();
                    if (!MainForm.walls[row, col].Visible && isTouchingWall(MainForm.walls[row, col], ref touchPoint))
                    {
                        if (MainForm.walls[row, col].used[rat_numb])
                        {
                            usedPenalty += 1;
                        }
                        else
                        {
                            MainForm.walls[row, col].used[rat_numb] = true;
                        }
                    }
                    steps = 0;
                    ++currentChrom;
                }
            }
            else //simulate rat movements; rats instantly go from tile to tile
            {
                
                centerRat();
                MoveRat(1);
                int row = (int)this.location.Y / MainForm.LEN;
                int col = (int)this.location.X / MainForm.LEN;
                PointF touchPoint = new PointF();
                if (MainForm.walls[row, col].Visible && isTouchingWall(MainForm.walls[row, col], ref touchPoint))
                {
                    this.hitWall += 1;
                    this.location.X -= velocity.X;
                    this.location.Y -= velocity.Y;
                }
                else if (!MainForm.walls[row, col].Visible && isTouchingWall(MainForm.walls[row, col], ref touchPoint))
                {
                    if (MainForm.walls[row, col].used[rat_numb])
                    {
                        usedPenalty += 1;
                    }
                    else
                    {
                        MainForm.walls[row, col].used[rat_numb] = true;
                    }
                }
                ++currentChrom;
            }

            if (currentChrom >= MainForm.NUMB_STEPS)
            {
                finished = true;
            }
            if (velocity.X != 0 && velocity.Y != 0)
            {
                pic.Update(time);
            }
        }

        /// <summary>
        /// Determines whether the rat is touching the wall, w and sets the collision point if it is.
        /// </summary>
        /// <param name="wall">The wall to be checked.</param>
        /// <param name="touchPoint">The touching point if the wall is touching the rat.</param>
        /// <returns>
        ///   <c>true</c> if [is touching wall]; otherwise, <c>false</c>.
        /// </returns>
        public bool isTouchingWall(Wall wall, ref PointF touchPoint)
        {
            PointF nearestPoint = wall.PointNearestTo(this.location);
            float distance = (float)Math.Sqrt((nearestPoint.X - location.X) * (nearestPoint.X - location.X) + (nearestPoint.Y - location.Y) * (nearestPoint.Y - location.Y));
            if (distance < this.radius)
            {
                touchPoint = nearestPoint;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pushes this rat from the point p by a distance proportional to the radius + 1 in the
        /// opposition direction of it's current velocity.
        /// </summary>
        /// <param name="p">The point to be pushed from.</param>
        public void PushFrom(PointF p)
        {
            // Calculate the actualDistance between the points p and this rats
            //location, using euclidean distance.
            float actualDistance = (float)Math.Sqrt((p.X - this.location.X) * (p.X - this.location.X) + (p.Y - this.location.Y) * (p.Y - this.location.Y));

            if (actualDistance == 0)
            {
                return;
            }
            float desiredDistance = this.radius + 1;
            float proportion = desiredDistance / actualDistance;
            PointF move = new PointF(this.location.X - p.X, this.location.Y - p.Y);
            // Move this object away from p
            move.X *= proportion;
            move.Y *= proportion;

            this.location.X = p.X + move.X;
            this.location.Y = p.Y + move.Y;
        }

        /// <summary>
        /// Centers the rat onto the current tile.
        /// </summary>
        private void centerRat()
        {
            this.location.X = ((int)this.location.X / MainForm.LEN) * MainForm.LEN + MainForm.LEN / 2;
            this.location.Y = ((int)this.location.Y / MainForm.LEN) * MainForm.LEN + MainForm.LEN / 2;
        }

        /// <summary>
        /// Moves the rat using its current chromosome.
        /// </summary>
        /// <param name="cut">The cut.</param>
        private void MoveRat(int cut)
        {
            int direction;
            int.TryParse(decoded[currentChrom].ToString(), out direction);
            if (direction == MainForm.RIGHT)
            {
                velocity.X = MainForm.LEN / cut;
                velocity.Y = 0;
                facingAngle = 0;
            }
            else if (direction == MainForm.LEFT)
            {
                velocity.X = -MainForm.LEN / cut;
                velocity.Y = 0;
                facingAngle = 180;
            }
            else if (direction == MainForm.UP)
            {
                velocity.X = 0;
                velocity.Y = -MainForm.LEN / cut;
                facingAngle = 90;
            }
            else if (direction == MainForm.DOWN)
            {
                velocity.X = 0;
                velocity.Y = MainForm.LEN / cut;
                facingAngle = 270;
            }

            location.X += velocity.X;
            location.Y += velocity.Y;
        }

        /// <summary>
        /// Checks if the rat is touching any of the adjacent walls.
        /// </summary>
        private void checkWalls()
        {
            int row = (int)this.location.Y / MainForm.LEN;
            int col = (int)this.location.X / MainForm.LEN;

            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    if (_Checkwalls(row + r, col + c))
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if this rat is touching the wall and penalizes it.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        private bool _Checkwalls(int row, int col)
        {
            if (row < 0 || col < 0 || row >= (MainForm.RES_HEIGHT / MainForm.LEN) || col >= (MainForm.RES_WIDTH / MainForm.LEN))
            {
                return false;
            }

            PointF touchPoint = new PointF();
            if (MainForm.walls[row, col].Visible && isTouchingWall(MainForm.walls[row, col], ref touchPoint))
            {
                this.hitWall += 1;
                steps = steps_per_movement;
                PushFrom(touchPoint);
                centerRat();
                return true;
            }
            return false;
        }
    }
}