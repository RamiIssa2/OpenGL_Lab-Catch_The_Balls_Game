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
    class Writer
    {
        private const float WIDTH = 0.05f;
        private const float HEIGHT = 1.25f * WIDTH;

        private static int LettersTextureId = Utilities.LoadTexture(@"Images\Letters.png");
        private static int NumbersTextureId = Utilities.LoadTexture(@"Images\Numbers.png");
        private static int ColonTextureId = Utilities.LoadTexture(@"Images\Colon.png");

        private static Vector2[] LettersTextureIndexes =
        {
            new Vector2(0f,0f),
            new Vector2(0.125f,0f),
            new Vector2(0.25f,0f),
            new Vector2(0.375f,0f),
            new Vector2(0.5f,0f),
            new Vector2(0.625f,0f),
            new Vector2(0.75f,0f),
            new Vector2(0.875f,0f),
            new Vector2(0f,0.25f),
            new Vector2(0.125f,0.25f),
            new Vector2(0.25f,0.25f),
            new Vector2(0.375f,0.25f),
            new Vector2(0.5f,0.25f),
            new Vector2(0.625f,0.25f),
            new Vector2(0.75f,0.25f),
            new Vector2(0.875f,0.25f),
            new Vector2(0f,0.5f),
            new Vector2(0.125f,0.5f),
            new Vector2(0.25f,0.5f),
            new Vector2(0.375f,0.5f),
            new Vector2(0.5f,0.5f),
            new Vector2(0.625f,0.5f),
            new Vector2(0.75f,0.5f),
            new Vector2(0.875f,0.5f),
            new Vector2(0f,0.75f),
            new Vector2(0.125f,0.75f)
        };

        private static Vector2[] NumbersTextureIndexes =
        {
            new Vector2(0f,0f),
            new Vector2(0.2f,0f),
            new Vector2(0.4f,0f),
            new Vector2(0.6f,0f),
            new Vector2(0.8f,0f),
            new Vector2(0f,0.5f),
            new Vector2(0.2f,0.5f),
            new Vector2(0.4f,0.5f),
            new Vector2(0.6f,0.5f),
            new Vector2(0.8f,0.5f)
        };

        private int FontSize = 1;
        private string Content = "";
        public Vector2 position;

        public Writer(Vector2 position, int FontSize = 1)
        {
            this.position = position;
            this.FontSize = FontSize;
        }

        public void setContent(string Content)
        {
            this.Content = Content;
        }

        public void DrawContent()
        {
            for (int i=0; i<Content.Length; i++)
            {
                int characterIndex = Content[i] - 'A';
                if ((characterIndex >= 0) && (characterIndex <= 25))
                {
                    DrawCharacter(i, characterIndex);
                }
                else
                {
                    characterIndex = Content[i] -'a';
                    if ((characterIndex >= 0) && (characterIndex <= 25))
                    {
                        DrawCharacter(i, characterIndex);
                    }
                    else
                    {
                        characterIndex = Content[i] - '0';
                        if ((characterIndex >= 0) && (characterIndex <= 9))
                        {
                            DrawNumber(i, characterIndex);
                        }
                        else if (Content[i] == ':')
                        {
                            DrawColon(i);
                        }
                    }
                }
            }
        }

        private void DrawCharacter(int orderNumber, int characterIndex)
        {
            GL.BindTexture(TextureTarget.Texture2D, LettersTextureId);
            GL.LoadIdentity();
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(LettersTextureIndexes[characterIndex] + new Vector2(0f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * orderNumber, 0));
            GL.TexCoord2(LettersTextureIndexes[characterIndex] + new Vector2(0.125f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * (orderNumber + 1), 0));
            GL.TexCoord2(LettersTextureIndexes[characterIndex] + new Vector2(0.125f, 0.25f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * (orderNumber + 1), -HEIGHT * FontSize));
            GL.TexCoord2(LettersTextureIndexes[characterIndex] + new Vector2(0f, 0.25f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * orderNumber, -HEIGHT * FontSize));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void DrawNumber(int orderNumber, int numberIndex)
        {
            GL.BindTexture(TextureTarget.Texture2D, NumbersTextureId);
            GL.LoadIdentity();
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(NumbersTextureIndexes[numberIndex] + new Vector2(0f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * orderNumber, 0));
            GL.TexCoord2(NumbersTextureIndexes[numberIndex] + new Vector2(0.2f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * (orderNumber + 1), 0));
            GL.TexCoord2(NumbersTextureIndexes[numberIndex] + new Vector2(0.2f, 0.5f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * (orderNumber + 1), -HEIGHT * FontSize));
            GL.TexCoord2(NumbersTextureIndexes[numberIndex] + new Vector2(0f, 0.5f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * orderNumber, -HEIGHT * FontSize));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void DrawColon(int orderNumber)
        {
            GL.BindTexture(TextureTarget.Texture2D, ColonTextureId);
            GL.LoadIdentity();
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(new Vector2(0f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * orderNumber, 0));
            GL.TexCoord2(new Vector2(1.0f, 0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * (orderNumber + 1), 0));
            GL.TexCoord2(new Vector2(1.0f, 1.0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * (orderNumber + 1), -HEIGHT * FontSize));
            GL.TexCoord2(new Vector2(0f, 1.0f));
            GL.Vertex2(this.position + new Vector2(WIDTH * FontSize * orderNumber, -HEIGHT * FontSize));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
