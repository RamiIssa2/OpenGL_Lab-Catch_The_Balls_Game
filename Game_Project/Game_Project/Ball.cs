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

namespace Game_Project
{
    class Ball : IDisposable
    {
        public const float WIDTH = 0.15f;
        public const float HEIGHT = 0.15f;

        private float DropSpeed = 0.01f;
        private static float MaxDropSpeed = 0.05f;
        private static float RotateSpeed = 5.0f;

        private static Random RandomPosition = new Random();
        private static Random RandomSpecialBall = new Random();

        private static int SpecialNumber = 10;
        public bool Special;
        public bool Catchable;

        private int TextureId;
        private float angle;

        public Vector2 position;

        public Ball(int TextureId, int LevelNumber)
        {
            int TempRandPos = RandomPosition.Next(-100, 85);
            float temp = (float)TempRandPos / (float)100;
            this.position = new Vector2(temp, 1.0f + HEIGHT);
            this.TextureId = TextureId;
            this.angle = 0;
            if (DropSpeed + 0.002f * (LevelNumber - 1) < MaxDropSpeed)
            {
                this.DropSpeed += 0.002f * (LevelNumber - 1);
            }
            else
            {
                this.DropSpeed = MaxDropSpeed;
            }
            int TempSpecialNum = RandomSpecialBall.Next(1, 100);
            if (TempSpecialNum <= SpecialNumber)
            {
                this.Special = true;
            }
            else
            {
                this.Special = false;
            }
            this.Catchable = true;
        }

        public void DrawBall()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.TextureId);
            GL.LoadIdentity();
            GL.Translate(this.position.X, this.position.Y, 0);
            GL.Translate(WIDTH / 2, -HEIGHT / 2, 0);
            GL.Rotate(this.angle, Vector3.UnitZ);
            GL.Translate(-WIDTH / 2, HEIGHT / 2, 0);
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(new Vector2(0f, 0f));
            if (this.Special)
            {
                GL.Color3(Color.Red);
            }
            GL.Vertex2(new Vector2(0, 0));
            GL.TexCoord2(new Vector2(1f, 0f));
            if (this.Special)
            {
                GL.Color3(Color.Blue);
            }
            GL.Vertex2(new Vector2(WIDTH, 0));
            GL.TexCoord2(new Vector2(1f, 1f));
            if (this.Special)
            {
                GL.Color3(Color.Green);
            }
            GL.Vertex2(new Vector2(WIDTH, -HEIGHT));
            GL.TexCoord2(new Vector2(0f, 1f));
            if (this.Special)
            {
                GL.Color3(Color.Yellow);
            }
            GL.Vertex2(new Vector2(0, -HEIGHT));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void DropBall()
        {
            if (this.position.Y - HEIGHT > -1)
            {
                this.position -= new Vector2(0, DropSpeed);
                this.angle -= RotateSpeed;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
