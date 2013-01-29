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

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ratatat
{
    /// <summary>
    /// The mainform of the program. This is the window everything is being drawn too.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The programs resolution width.
        /// </summary>
        public const int RES_WIDTH = 800;
        /// <summary>
        /// The programs resolution height.
        /// </summary>
        public const int RES_HEIGHT = 600;
        /// <summary>
        /// A static picture of a wall. Every wall on the screen is a clone of this.
        /// </summary>
        public static Bitmap wall_pic;
        /// <summary>
        /// All the walls in the game in a 2D array representing the grid over mainform.
        /// </summary>
        public static Wall[,] walls;
        /// <summary>
        /// The cheese, the rats goal to obtaining the highest fitness score.
        /// </summary>
        public static Cheese cheese;
        /// <summary>
        /// What the user sees. Everything is drawn onto this.
        /// </summary>
        private Graphics onScreenGraphics;
        private Bitmap screen;
        private Graphics windowsGraphics;

        /// <summary>
        /// The engine for the Genetic Algorithm.
        /// </summary>
        public GeneticAlgorithm GA_ENGINE;
        /// <summary>
        /// The start position of each trial rat.
        /// </summary>
        public static Point START_POSITION;

        /// <summary>
        /// The length of the squares of each wall. This also determines how many individual
        /// walls will be created. They all must fit into a 800x600 grid so a small number will create
        /// more walls.
        /// </summary>
        public const int LEN = 40; //must be an even number

        /// <summary>
        /// Toggle drawing off if your away from keyboard by pressing D.
        /// This increases CPU performance significantly.
        /// </summary>
        public static bool DRAW_GAME = true;

        /// <summary>
        /// True iff the labels are visible.
        /// </summary>
        public static bool SEE_LABELS = true;

        /// <summary>
        /// Hides all visuals of the program.
        /// </summary>
        public static bool HIDE_GAME = false;

        //all variables that define how the genetic algorithm will behave
        #region GA_ATTRIBUTES

        /// <summary>
        /// The mutation rate of the genetic algorithm. This is how often genes will be mutated
        /// in new born offspring.
        /// </summary>
        public const double mutationRate = 0.02;
        /// <summary>
        /// The crossover rate of the genetic algorithm. This is how often two parents
        /// will create new offspring.
        /// </summary>
        public const double crossoverRate = 0.7;
        /// <summary>
        /// The percentage of the elite population. These individuals are 
        /// guaranteed to survive to the next generation.
        /// </summary>
        public const double populationElitism = 0.01;
       
        //TABLE OF VALUES FOR GENETIC CODING
        public const int UP = 0;    //00
        public const int RIGHT = 1; //01
        public const int DOWN = 2;  //10
        public const int LEFT = 3;  //11

        /// <summary>
        /// The number of bits per chromosome.
        /// </summary>
        public const int BITS_PER_CHROM = 2;
        /// <summary>
        /// The total number of steps the rat has to take to complete a trial test.
        /// </summary>
        public const int NUMB_STEPS = 100 * (int)(60 / LEN);
        /// <summary>
        /// The population size for testing and  breeding.
        /// </summary>
        public const int POPULATION = 800 *(int)( 40 / LEN);

        /// <summary>
        /// The number of rats that can be tested at a time.
        /// </summary>
        public static int current_rats_per_trial = 2;
        #endregion

        /// <summary>
        /// If true, rats tests become simulated.
        /// </summary>
        public bool switch_mode = false;

        /// <summary>
        /// Boolean value for if the game is in a paused state or not. Default is true.
        /// </summary>
        public bool PAUSE;
        /// <summary>
        /// Avoids an unintentional flickering of pausing on and off due to low timer tick interval.
        /// By creating a delay after each toggle of the pause state.
        /// </summary>
        public int DELAY_PAUSE;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            wall_pic = new Bitmap("pics/" + "Green" + "Box.png");
            InitializeComponent();
            this.Width = RES_WIDTH + 325;
            this.Height = RES_HEIGHT + 40;

            PopLabel.Text = "Population: " + POPULATION.ToString();
            mutationLabel.Text = "Mutation Rate: " + mutationRate;
            crossoverLabel.Text = "Crossover Rate: " + crossoverRate.ToString();
            eliteLabel.Text = "Elitism (%): " + populationElitism.ToString();

            START_POSITION = new Point(LEN + LEN / 2, LEN + LEN / 2);
            walls = new Wall[RES_HEIGHT / LEN + 1, RES_WIDTH / LEN + 1];
            stepThroughWalls((int i, int j) => { walls[j, i] = new Wall(i * LEN, j * LEN, LEN, LEN); });
            GA_ENGINE = new GeneticAlgorithm(BITS_PER_CHROM, NUMB_STEPS, POPULATION, crossoverRate, mutationRate, populationElitism);
            PAUSE = true;
            DELAY_PAUSE = 0;
            cheese = new Cheese(new Point(), 1, 1, 1);
            cheese.location.X = LEN * ((RES_WIDTH / LEN) / 2) + LEN / 2;
            cheese.location.Y = LEN / 2;
            windowsGraphics = this.CreateGraphics();
            screen = new Bitmap(this.Width, this.Height);
            onScreenGraphics = Graphics.FromImage(screen);
            this.Paint += new PaintEventHandler(DrawGame);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(cheese.KeyDown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(MainFormKeyDown);
            //this.KeyUp += new System.Windows.Forms.KeyEventHandler(cheese.KeyUp);
        }

        /// <summary>
        /// Draws the entire game to the onscreen graphics.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        public void DrawGame(Object sender, PaintEventArgs e)
        {
            if (DRAW_GAME)
            {
                onScreenGraphics.Clear(Color.Black);
                stepThroughWalls((int i, int j) => { if (walls[j, i].Visible) { walls[j, i].Draw(onScreenGraphics); } });
                foreach (Rat rat in GA_ENGINE.CurrentRats()) rat.Draw(onScreenGraphics);
                cheese.Draw(onScreenGraphics);
                windowsGraphics.DrawImage(screen, new Point(0, 0));
            }
        }

        /// <summary>
        /// Handles the Tick event of the GameTimer control. This is the update engine for the program.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timer_tick(object sender, EventArgs e)
        {
            toggleGame();
            cheese.Update(GameTimer.Interval);
            if (!PAUSE)
            {
                int ct = 0;
                while (ct < GA_ENGINE.CurrentRats().Length)
                {
                    if (!GA_ENGINE.CurrentRats()[ct].finished)
                    {
                        GA_ENGINE.CurrentRats()[ct].Update(GameTimer.Interval, ct);
                    }
                    ++ct;
                }
                if (finished(GA_ENGINE.CurrentRats()))
                {
                    double score;
                    if ((score = getMaxScore(GA_ENGINE.CurrentRats())) > GA_ENGINE.bestScoreSeen)
                    {
                        GA_ENGINE.bestScoreSeen = score;
                    }

                    if (GA_ENGINE.Next())
                    {
                        setGameMode();
                        avgFitness.Text = "Avg Fitness: " + (GA_ENGINE.generationScore / GA_ENGINE.POPULATION_SIZE).ToString();
                    }
                    stepThroughWalls((int i, int j) => { walls[j, i].used = new bool[MainForm.current_rats_per_trial]; });
                    topFitness.Text = "Best Fitness: " + GA_ENGINE.bestScoreSeen.ToString();
                }
            }

            updateLabelsText();
            if (DRAW_GAME)
            {
                OnPaint(new PaintEventArgs(windowsGraphics, new Rectangle(0, 0, this.Width, this.Height)));
            }
        }

        /// <summary>
        /// Updates the text for most labels.
        /// </summary>
        public void updateLabelsText()
        {

            if (SEE_LABELS)
            {
                currentRat.Text = "Rat: " + (!switch_mode ? GA_ENGINE.currentRat.ToString() : "Simulating...");
                currentGeneration.Text = "Generation: " + GA_ENGINE.generation.ToString();

                pauseLabel.Text = "Paused: " + PAUSE.ToString();
                if (DRAW_GAME)
                {
                    currentFitness.Text = "Fitness: " + (!switch_mode ? getMaxScore(GA_ENGINE.CurrentRats()).ToString() : "Simulating...");
                }
            }
        }
        /// <summary>
        /// Checks if the user is placing or destroying walls.
        /// Right mouse click creates walls, left mouse click destroys walls.
        /// The game must be paused and the mouse must be on a valid location (on the mainform).
        /// </summary>
        private void checkWallPlacement()
        {
            //Check if mouse position is on mainform grid of 800x600
            if (MousePosition.X >= this.Location.X && MousePosition.X <= this.Location.X + RES_WIDTH
      && MousePosition.Y >= this.Location.Y + 38 / 2 && MousePosition.Y <= this.Location.Y + RES_HEIGHT)
            {
                int row, col;

                //calculate column and row position to determine which wall is being manipulated
                col = (int)(MainForm.MousePosition.X - LEN / 4 - this.Location.X) / LEN;
                row = (int)(MousePosition.Y - LEN / 4 - 38 / 2 - this.Location.Y) / LEN;

                //destroy wall if left mouse button is being clicked
                if (MainForm.MouseButtons == MouseButtons.Left)
                {
                    walls[row, col].Visible = false;
                }
                //create wall if right mouse button is clicked
                else if (MainForm.MouseButtons == MouseButtons.Right)
                {
                    walls[row, col].Visible = true;
                }
            }
        }

        /// <summary>
        /// See Step through walls, this essentially allows for passing of a function as a parameter.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        public delegate void Myfunction(int i, int j);

        /// <summary>
        /// Step through each wall and call f.
        /// </summary>
        /// <param name="f">The f is the function can do almost anything based on the
        /// position in the array.</param>
        public static void stepThroughWalls(Myfunction f)
        {
            int i, j;
            i = j = 0;
            while (j < RES_HEIGHT / LEN)
            {
                while (i < RES_WIDTH / LEN)
                {
                    //this function could do anything but i and j determine which wall we are at in the array
                    //so this is mostly used for checking or setting some property of the walls.
                    f(i, j);
                    ++i;
                }
                i = 0;
                ++j;
            }
        }

        /// <summary>
        /// Toggles the game from a paused state to an unpaused state. This can be done with
        /// the mouses middle click button or by pressing p. If the game is paused it also checks for wall editing.
        /// </summary>
        private void toggleGame()
        {
            //if game is paused check for editing.
            if (PAUSE) checkWallPlacement();

            if (DELAY_PAUSE > 0) DELAY_PAUSE -= 5;

            //Toggle pause button
            if (MainForm.MouseButtons == MouseButtons.Middle)
            {
                if (DELAY_PAUSE <= 0)
                {
                    DELAY_PAUSE = 50;
                    PAUSE = !PAUSE;
                }
            }
        }

        /// <summary>
        /// Handles when a user press down a key.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        public void MainFormKeyDown(Object sender, KeyEventArgs e)
        {
            //toggles pause
            if (e.KeyCode == Keys.P)
            {
                PAUSE = !PAUSE;
            }

            //toggles simulate rat trials
            if (e.KeyCode == Keys.Q)
            {
                if (!PAUSE)
                {
                    switch_mode = true;
                    if (DRAW_GAME)
                    {
                        GA_ENGINE.RetroPopulation();
                        GA_ENGINE.currentRat = 0;
                        setGameMode();
                        stepThroughWalls((int i, int j) => { walls[j, i].used = new bool[MainForm.current_rats_per_trial]; });
                    }
                }
            }
            //toggles label's visibility
            if (e.KeyCode == Keys.L)
            {
                SEE_LABELS = !SEE_LABELS;
                showLabels(SEE_LABELS);
            }

            if (e.KeyCode == Keys.V && PAUSE)
            {
                stepThroughWalls((int i, int j) => { walls[j, i].Visible = false; });
            }
        }

        /// <summary>
        /// Sets the visibility for all the labels.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show all labels].</param>
        private void showLabels(bool show)
        {
            currentFitness.Visible = show;
            currentGeneration.Visible = show;
            currentRat.Visible = show;
            PopLabel.Visible = show;
            avgFitness.Visible = show;
            topFitness.Visible = show;
            crossoverLabel.Visible = show;
            mutationLabel.Visible = show;
            eliteLabel.Visible = show;
            pauseLabel.Visible = show;
            instructionsLabel.Visible = show;
        }
        /// <summary>
        /// Return true if all rats are finished.
        /// </summary>
        /// <param name="rats">The rats.</param>
        /// <returns></returns>
        public bool finished(Rat[] rats)
        {
            foreach (Rat rat in rats)
            {
                if (!rat.finished)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the max score of the rats.
        /// </summary>
        /// <param name="rats">The rats.</param>
        /// <returns></returns>
        public double getMaxScore(Rat[] rats)
        {
            double max = 0;
            foreach (Rat rat in rats)
            {
                if (rat.score > max)
                {
                    max = rat.score;
                }
            }

            return max;
        }

        /// <summary>
        /// Toggles the game mode between simulating and viewing the rat trials.
        /// </summary>
        private void setGameMode()
        {
            if (switch_mode)
            {
                switch_mode = false;
                DRAW_GAME = !DRAW_GAME;

                if (DRAW_GAME)
                {
                    current_rats_per_trial = 2;
                }
                else
                {
                    current_rats_per_trial = GA_ENGINE.POPULATION_SIZE;
                }
            }
        }
    }
}