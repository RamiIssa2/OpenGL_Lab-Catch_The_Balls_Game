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
    class Block : IDisposable
    {
        public const float WIDTH = 0.5f;
        public const float HEIGHT = WIDTH / 6;

        private float DropSpeed = 0.01f;
        private static float MaxDropSpeed = 0.05f;

        private static Random RandomPosition = new Random();

        private int TextureId;
        public bool Catchable;

        public Vector2 position;

        public Block(int TextureId, int LevelNumber)
        {
            int TempRandPos = RandomPosition.Next(-100, 50);
            float temp = (float)TempRandPos / (float)100;
            this.position = new Vector2(temp, 1.0f + HEIGHT);
            this.TextureId = TextureId;
            if (DropSpeed + 0.002f * (LevelNumber - 1) < MaxDropSpeed)
            {
                this.DropSpeed += 0.002f * (LevelNumber - 1);
            }
            else
            {
                this.DropSpeed = MaxDropSpeed;
            }
            this.Catchable = true;
        }

        public void DrawBlock()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.TextureId);
            GL.LoadIdentity();
            GL.Translate(this.position.X, this.position.Y, 0);
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(new Vector2(0f, 0f));
            GL.Vertex2(new Vector2(0, 0));
            GL.TexCoord2(new Vector2(1f, 0f));
            GL.Vertex2(new Vector2(WIDTH, 0));
            GL.TexCoord2(new Vector2(1f, 1f));
            GL.Vertex2(new Vector2(WIDTH, -HEIGHT));
            GL.TexCoord2(new Vector2(0f, 1f));
            GL.Vertex2(new Vector2(0, -HEIGHT));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void DropBall()
        {
            if (this.position.Y - HEIGHT > -1)
            {
                this.position -= new Vector2(0, DropSpeed);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
