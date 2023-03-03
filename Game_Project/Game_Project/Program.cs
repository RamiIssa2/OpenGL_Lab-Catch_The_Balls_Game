using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Timers;
using System.Collections;

namespace Game_Project
{
    class Program : GameWindow
    {

        public static string TITLE = "Game Project";

        public const int WIDTH = 450;
        public const int HEIGHT = 600;
        public const float RATIO = (float)WIDTH / (float)HEIGHT;

        public static int BackgroundTextureId;
        private static int BallTextureId;
        private static int BlockTextureId;

        public static Basket basket;
        public static ArrayList balls;
        public static ArrayList blocks;

        private static Random RandomBlock = new Random();

        private static int BlockRatio;
        private const int MaxBlockRatio = 50;


        public static Writer ScoreDisplay;
        private static Vector2 ScorePosition = new Vector2(-1.0f, 1.0f);
        private static int Score;

        public static Writer MissedBallsDisplay;
        private static Vector2 MissedBallsPosition = new Vector2(-1.0f, 0.9375f);
        private static int MissedBalls;

        public static Writer LevelDisplay;
        private static Vector2 LevelPosition = new Vector2(-0.2f, 1.0f);
        private static int LevelNumber;

        public static Writer LevelTimerDisplay;
        private static Vector2 LevelTimerPosition = new Vector2(0.75f, 1.0f);

        public static Writer PauseDisplay;
        private static Vector2 PausePosition = new Vector2(-0.825f, 0.45f);

        public static Writer PauseScoreDisplay;
        private static Vector2 PauseScorePosition = new Vector2(-0.85f, 0.15f);

        public static Writer PasueContinueDisplay;
        private static Vector2 PasueContinuePosition = new Vector2(-0.775f, -0.09375f);

        public static Stars stars;

        public static Writer ResultScoreDisplay;
        private static Vector2 ResultScorePosition = new Vector2(-0.45f, 0.25f);

        public static Writer ResultMissedBallsDisplay;
        private static Vector2 ResultMissedBallsPosition = new Vector2(-0.8f, 0f);

        public static Writer ResultContinueDisplay;
        private static Vector2 ResultContinuePosition = new Vector2(-0.775f, -0.25f);

        public static Writer ResultRetryDisplay;
        private static Vector2 ResultRetryPosition = new Vector2(-0.7f, -0.25f);

        private static System.Timers.Timer DropTimer;
        private static int DropGap = 2000;
        private const int MinDropGap = 500;

        private static System.Timers.Timer LevelTimer;
        private static int LevelTimerSeconds;
        private static int LevelTimerMinuts;

        private static bool clicked = false;
        private static bool paused;
        private static bool levelFinished;
        private static bool Win;

        public enum Direction
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        private static void SetDropTimer(int DropGap)
        {
            // Create a timer with a DropGap interval.
            DropTimer = new System.Timers.Timer(DropGap);
            // Hook up the Elapsed event for the timer. 
            DropTimer.Elapsed += OnDropTimedEvent;
            DropTimer.AutoReset = true;
            DropTimer.Enabled = true;
        }

        private static void OnDropTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (RandomBlock.Next(1, 100) <= BlockRatio)
            {
                DropBlock();
            }
            else
            {
                DropBall();
            }
        }

        private static void DropBall()
        {
            Ball ball = new Ball(BallTextureId, LevelNumber);
            balls.Add(ball);
        }

        private static void DropBlock()
        {
            Block block = new Block(BlockTextureId, LevelNumber);
            blocks.Add(block);
        }

        private static void SetLevelTimer()
        {
            // Create a timer with a one second interval.
            LevelTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            LevelTimer.Elapsed += OnLevelTimedEvent;
            LevelTimer.AutoReset = true;
            LevelTimer.Enabled = true;
        }

        private static void OnLevelTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (LevelTimerSeconds > 0)
            {
                LevelTimerSeconds--;
            }
            else if (LevelTimerMinuts > 0)
            {
                LevelTimerMinuts--;
                LevelTimerSeconds = 59;
            }
            if ((LevelTimerSeconds == 0) && (LevelTimerMinuts == 0))
            {
                FinishLevel(true);
            }
            LevelTimerDisplay.setContent(((LevelTimerMinuts < 10) ? "0" : "") + LevelTimerMinuts + ":" + ((LevelTimerSeconds < 10) ? "0" : "") + LevelTimerSeconds);
        }

        private static void StartLevel()
        {
            levelFinished = false;
            paused = false;
            Win = false;

            Score = 0;
            MissedBalls = 0;
            if (LevelNumber <= 50)
            {
                BlockRatio = LevelNumber;
            }
            else
            {
                BlockRatio = MaxBlockRatio;
            }

            LevelTimerSeconds = 30;
            LevelTimerMinuts = 1;

            basket = new Basket();
            balls = new ArrayList();
            blocks = new ArrayList();

            if (DropGap - 20 * (LevelNumber - 1) >= MinDropGap)
            {
                SetDropTimer(DropGap - 20 * (LevelNumber - 1));
            }
            else
            {
                SetDropTimer(MinDropGap);
            }
            SetLevelTimer();

            ScoreDisplay.setContent("Score: " + Score);
            MissedBallsDisplay.setContent("Missed Balls: " + MissedBalls);
            LevelDisplay.setContent("Level " + LevelNumber);
            LevelTimerDisplay.setContent(((LevelTimerMinuts < 10) ? "0" : "") + LevelTimerMinuts + ":" + ((LevelTimerSeconds < 10) ? "0" : "") + LevelTimerSeconds);
        }

        private static void FinishLevel(bool tempWin)
        {
            levelFinished = true;
            DropTimer.Stop();
            LevelTimer.Stop();
            Win = tempWin;
            LevelNumber += Win ? 1 : 0;
            int status;
            if (!Win)
            {
                status = 3;
            }
            else if (MissedBalls <= 0)
            {
                status = 0;
            }
            else if (MissedBalls <= 5)
            {
                status = 1;
            }
            else if (MissedBalls <= 9)
            {
                status = 2;
            }
            else
            {
                status = 3;
            }
            stars.setStatus(status);
            ResultScoreDisplay.setContent("Score: " + Score);
            ResultMissedBallsDisplay.setContent("Missed Balls: " + MissedBalls);
        }

        public Program() : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE)
        {
            ScoreDisplay = new Writer(ScorePosition);

            MissedBallsDisplay = new Writer(MissedBallsPosition);

            LevelDisplay = new Writer(LevelPosition);

            PauseDisplay = new Writer(PausePosition, 3);
            PauseDisplay.setContent("Game Paused");

            PauseScoreDisplay = new Writer(PauseScorePosition, 2);


            PasueContinueDisplay = new Writer(PasueContinuePosition);
            PasueContinueDisplay.setContent("Press the space bar to continue");

            stars = new Stars();

            ResultScoreDisplay = new Writer(ResultScorePosition, 2);

            ResultMissedBallsDisplay = new Writer(ResultMissedBallsPosition, 2);

            ResultContinueDisplay = new Writer(ResultContinuePosition);
            ResultContinueDisplay.setContent("Press the space bar to continue");

            ResultRetryDisplay = new Writer(ResultRetryPosition);
            ResultRetryDisplay.setContent("Press the space bar to retry");

            LevelTimerDisplay = new Writer(LevelTimerPosition);

            LevelNumber = 1;

            StartLevel();
        }


        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            GL.Enable(EnableCap.Texture2D);
            BackgroundTextureId = Utilities.LoadTexture(@"Images\Background.png");
            BallTextureId = Utilities.LoadTexture(@"Images\Ball.png");
            BlockTextureId = Utilities.LoadTexture(@"Images\Block.png");
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[Key.Escape])
            {
                this.Exit();
            }
            else if (Keyboard[Key.Right])
            {
                if (!paused && !levelFinished)
                {
                    if (Keyboard[Key.Up])
                    {
                        basket.MoveBasket(Direction.UP);
                    }
                    else if (Keyboard[Key.Down])
                    {
                        basket.MoveBasket(Direction.DOWN);
                    }
                    basket.MoveBasket(Direction.RIGHT);
                }
            }
            else if (Keyboard[Key.Left])
            {
                if (!paused && !levelFinished)
                {
                    if (Keyboard[Key.Up])
                    {
                        basket.MoveBasket(Direction.UP);
                    }
                    else if (Keyboard[Key.Down])
                    {
                        basket.MoveBasket(Direction.DOWN);
                    }
                    basket.MoveBasket(Direction.LEFT);
                }
            }
            else if (Keyboard[Key.Up])
            {
                if (!paused && !levelFinished)
                {
                    if (Keyboard[Key.Right])
                    {
                        basket.MoveBasket(Direction.RIGHT);
                    }
                    else if (Keyboard[Key.Left])
                    {
                        basket.MoveBasket(Direction.LEFT);
                    }
                    basket.MoveBasket(Direction.UP);
                }
            }
            else if (Keyboard[Key.Down])
            {
                if (!paused && !levelFinished)
                {
                    if (Keyboard[Key.Right])
                    {
                        basket.MoveBasket(Direction.RIGHT);
                    }
                    else if (Keyboard[Key.Left])
                    {
                        basket.MoveBasket(Direction.LEFT);
                    }
                    basket.MoveBasket(Direction.DOWN);
                }
            }
            else if (Keyboard[Key.Space])
            {
                if (!clicked)
                {
                    if (levelFinished)
                    {
                        StartLevel();
                    }
                    else if (paused)
                    {
                        DropTimer.Start();
                        LevelTimer.Start();
                        paused = false;
                    }
                    else
                    {
                        DropTimer.Stop();
                        LevelTimer.Stop();
                        paused = true;
                        PauseScoreDisplay.setContent("Current Score: " + Score);
                    }
                    clicked = true;
                }
            }
            else
            {
                clicked = false;
            }

            foreach (Ball ball in balls)
            {
                if (!paused && !levelFinished)
                {
                    if (ball.position.Y - Ball.HEIGHT > -1)
                    {
                        bool Condition3 = (ball.position.X + Ball.WIDTH > basket.position.X) && (ball.position.X < basket.position.X + Basket.WIDTH);
                        bool Condition4 = (ball.position.Y - Ball.HEIGHT < basket.position.Y);
                        if (Condition3 && Condition4 && ball.Catchable)
                        {
                            balls.Remove(ball);
                            ball.Dispose();
                            if (ball.Special)
                            {
                                Score += 5;
                            }
                            else
                            {
                                Score++;
                            }
                            ScoreDisplay.setContent("Score: " + Score);
                            break;
                        }
                        else 
                        {
                            if (!Condition3 && Condition4 && ball.Catchable)
                            {
                                ball.Catchable = false;
                            }
                            ball.DropBall();
                        }
                    }
                    else
                    {
                        balls.Remove(ball);
                        ball.Dispose();
                        MissedBalls++;
                        MissedBallsDisplay.setContent("Missed Balls: " + MissedBalls);
                        if (MissedBalls == 10)
                        {
                            FinishLevel(false);
                        }
                        break;
                    }
                }
            }

            foreach (Block block in blocks)
            {
                if (!paused && !levelFinished)
                {
                    if (block.position.Y - Block.HEIGHT > -1)
                    {
                        bool Condition3 = (block.position.X + Block.WIDTH > basket.position.X) && (block.position.X < basket.position.X + Basket.WIDTH);
                        bool Condition4 = (block.position.Y - Block.HEIGHT < basket.position.Y);
                        if (Condition3 && Condition4 && block.Catchable)
                        {
                            balls.Remove(block);
                            block.Dispose();
                            FinishLevel(false);
                            break;
                        }
                        else
                        {
                            if (!Condition3 && Condition4 && block.Catchable)
                            {
                                block.Catchable = false;
                            }
                            block.DropBall();
                        }
                    }
                    else
                    {
                        blocks.Remove(block);
                        block.Dispose();
                        break;
                    }
                }
            }
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.White);

            GL.BindTexture(TextureTarget.Texture2D, BackgroundTextureId);
            GL.LoadIdentity();
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(new Vector2(0f, 0f));
            GL.Vertex2(-1, 1);
            GL.TexCoord2(new Vector2(1f, 0f));
            GL.Vertex2(1, 1);
            GL.TexCoord2(new Vector2(1f, 1f));
            GL.Vertex2(1, -1);
            GL.TexCoord2(new Vector2(0f, 1f));
            GL.Vertex2(-1, -1);
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);

            foreach (Ball ball in balls)
            {
                ball.DrawBall();
            }

            foreach (Block block in blocks)
            {
                block.DrawBlock();
            }

            basket.DrawBasket();

            ScoreDisplay.DrawContent();

            MissedBallsDisplay.DrawContent();

            LevelDisplay.DrawContent();

            LevelTimerDisplay.DrawContent();

            if (paused)
            {
                PauseDisplay.DrawContent();
                PauseScoreDisplay.DrawContent();
                PasueContinueDisplay.DrawContent();
            }

            if (levelFinished)
            {
                stars.DrawStars();
                ResultScoreDisplay.DrawContent();
                ResultMissedBallsDisplay.DrawContent();
                if (Win)
                {
                    ResultContinueDisplay.DrawContent();
                }
                else
                {
                    ResultRetryDisplay.DrawContent();
                }
            }

            SwapBuffers();
        }


        static void Main(string[] args)
        {
            Program myGameWin = new Program();
            myGameWin.Run();

        }



    }
}
