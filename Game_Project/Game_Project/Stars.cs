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
    class Stars
    {
        private const float WIDTH = 1.5f;
        private const float HEIGHT = 0.3125f * WIDTH;

        private static int TextureId = Utilities.LoadTexture(@"Images\Stars.png");

        private int status;
        public Vector2 position;

        public Stars()
        {
            this.position = new Vector2(-WIDTH / 2, 0.8f);
        }

        public void setStatus(int status)
        {
            this.status = status;
        }

        public void DrawStars()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.LoadIdentity();
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(new Vector2(0f, 0.25f * status));
            GL.Vertex2(this.position);
            GL.TexCoord2(new Vector2(1f, 0.25f * status));
            GL.Vertex2(this.position + new Vector2(WIDTH, 0));
            GL.TexCoord2(new Vector2(1f, 0.25f * (status + 1)));
            GL.Vertex2(this.position + new Vector2(WIDTH, -HEIGHT));
            GL.TexCoord2(new Vector2(0f, 0.25f * (status + 1)));
            GL.Vertex2(this.position + new Vector2(0, -HEIGHT));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
