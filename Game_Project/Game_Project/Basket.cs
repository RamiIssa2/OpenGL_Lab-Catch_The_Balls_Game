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
    class Basket
    {
        public const float WIDTH = 0.6f;
        public const float HEIGHT = 0.3f;

        private static float speed = 0.05f;

        private static int TextureId = Utilities.LoadTexture(@"Images\Basket.png");

        public Vector2 position;

        public Basket()
        {
            this.position = new Vector2(-WIDTH / 2, -0.5f);
        }

        public void DrawBasket ()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.LoadIdentity();
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(new Vector2(0f, 0f));
            GL.Vertex2(this.position);
            GL.TexCoord2(new Vector2(1f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH, 0));
            GL.TexCoord2(new Vector2(1f, 1f));
            GL.Vertex2(this.position + new Vector2(WIDTH, -HEIGHT));
            GL.TexCoord2(new Vector2(0f, 1f));
            GL.Vertex2(this.position + new Vector2(0, -HEIGHT));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void MoveBasket(Program.Direction direction)
        {
            if ((direction == Program.Direction.RIGHT) && (this.position.X + WIDTH < 1))
            {
                this.position += new Vector2(speed, 0);
            }
            else if ((direction == Program.Direction.LEFT) && (this.position.X > -1))
            {
                this.position -= new Vector2(speed, 0);
            }
            else if((direction == Program.Direction.UP) && (this.position.Y < -0.2f))
            {
                this.position += new Vector2(0, speed);
            }
            else if ((direction == Program.Direction.DOWN) && (this.position.Y - HEIGHT > -1))
            {
                this.position -= new Vector2(0, speed);
            }
        }
    }
}
