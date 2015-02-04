using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SFML.Graphics;
using SFML.Window;

namespace Roguelike.Rendering
{
    public class Renderer
    {
        private readonly RenderTarget mainTarget;
        private readonly RenderTexture statusTarget;
        private readonly RenderTexture textTarget;
        private readonly RenderTexture worldTarget;
        private readonly int tileSize;
        private readonly Font font = new Font("audimat.ttf");
        private int playerX = 0;
        private int playerY = 0;

        public Renderer(RenderTarget mainTarget, int tileSize)
        {
            this.mainTarget = mainTarget;
            this.tileSize = tileSize;
            statusTarget = new RenderTexture((uint)(tileSize * 20), mainTarget.Size.Y - (uint)(tileSize * 10));
            textTarget = new RenderTexture(mainTarget.Size.X, (uint)(tileSize * 10));
            worldTarget = new RenderTexture(mainTarget.Size.X - (uint)(tileSize * 20), mainTarget.Size.Y - (uint)(tileSize * 10));
        }

        private void ComposeImage()
        {
            statusTarget.Display();
            textTarget.Display();
            worldTarget.Display();

            var stSprite = new Sprite(statusTarget.Texture);
            var txSprite = new Sprite(textTarget.Texture);
            var wSprite = new Sprite(worldTarget.Texture);
            stSprite.Position = new Vector2f(mainTarget.Size.X - (uint)(tileSize * 20), 0);
            txSprite.Position = new Vector2f(0, mainTarget.Size.Y - (uint)(tileSize * 10));
            wSprite.Position = new Vector2f(0, 0);

            mainTarget.Clear();
            mainTarget.Draw(stSprite);
            mainTarget.Draw(txSprite);
            mainTarget.Draw(wSprite);
        }

        private void RenderToTile<T>(RenderTarget target, int x, int y, T sprite)
            where T : Transformable, Drawable
        {
            sprite.Position = new Vector2f(x * tileSize, y * tileSize);
            target.Draw(sprite);
        }

        private void RenderText(RenderTarget target, params string[] text)
        {
            var tilesX = target.Size.X / tileSize;
            var tilesY = target.Size.Y / tileSize;

            var horizontalBorder = new RectangleShape(new Vector2f(tileSize, tileSize)) { FillColor = Color.Green };
            for (var x = 1; x < tilesX - 1; ++x)
            {
                RenderToTile(target, x, 0, horizontalBorder);
                RenderToTile(target, x, (int)(tilesY - 1), horizontalBorder);
            }

            var verticalBorder = new RectangleShape(new Vector2f(tileSize, tileSize)) { FillColor = Color.Green };
            for (var y = 1; y < tilesY - 1; ++y)
            {
                RenderToTile(target, 0, y, verticalBorder);
                RenderToTile(target, (int)tilesX - 1, y, verticalBorder);
            }

            var texts = text.Select(s => new Text(s, font)).ToList();
            for (var i = 0; i < texts.Count; ++i)
            {
                var txt = texts[i];
                txt.CharacterSize = (uint)tileSize;
                txt.Position = new Vector2f(tileSize, (i + 1) * tileSize);
                target.Draw(txt);
            }
        }

        private void RenderCharacter(int x, int y)
        {
            var chara = new RectangleShape(new Vector2f(tileSize, tileSize)) { FillColor = Color.Red };
            RenderToTile(worldTarget, x, y, chara);
        }

        public void UpdatePlayerPosition(int dx, int dy)
        {
            playerX += dx;
            playerY += dy;
        }

        public void Render()
        {
            worldTarget.Clear();
            RenderText(statusTarget, "Text1", "Text2", "  Text3");
            RenderText(textTarget, "You started the game! Welcome.");
            RenderCharacter(playerX, playerY);
            ComposeImage();
        }
    }
}
