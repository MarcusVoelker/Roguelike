using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Roguelike.World;
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
        private const int statusWidth = 20;
        private const int textHeight = 10;

        public Renderer(RenderTarget mainTarget, int tileSize)
        {
            this.mainTarget = mainTarget;
            this.tileSize = tileSize;
            statusTarget = new RenderTexture((uint)(statusWidth * tileSize), mainTarget.Size.Y - (uint)(tileSize * 10));
            textTarget = new RenderTexture(mainTarget.Size.X, (uint)(tileSize * 10));
            worldTarget = new RenderTexture(mainTarget.Size.X - (uint)(tileSize * statusWidth), mainTarget.Size.Y - (uint)(tileSize * 10));
            Map = null;
        }

        public Map Map { get; set; }

        private void ComposeImage()
        {
            statusTarget.Display();
            textTarget.Display();
            worldTarget.Display();

            var stSprite = new Sprite(statusTarget.Texture);
            var txSprite = new Sprite(textTarget.Texture);
            var wSprite = new Sprite(worldTarget.Texture);
            stSprite.Position = new Vector2f(mainTarget.Size.X - (uint)(tileSize * statusWidth), 0);
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
            var adj = CameraAdjustment();
            var chara = new RectangleShape(new Vector2f(tileSize, tileSize)) { FillColor = Color.Red };
            RenderToTile(worldTarget, x - adj.X, y - adj.Y, chara);
        }

        public void UpdatePlayerPosition(object sender, Vector2i pos)
        {
            playerX = pos.X;
            playerY = pos.Y;
        }

        private Vector2i CameraAdjustment()
        {
            var tilesX = worldTarget.Size.X / tileSize;
            var tilesY = worldTarget.Size.Y / tileSize;
            return new Vector2i(
                Math.Min(Math.Max(0, (int)(playerX - tilesX / 2)), (int)(Map.SizeX - tilesX)),
                Math.Min(Math.Max(0, (int)(playerY - tilesY / 2)), (int)(Map.SizeY - tilesY))
                );
        }
        private void RenderMap()
        {
            var passableTile = new RectangleShape(new Vector2f(tileSize, tileSize)) { FillColor = Color.Blue };
            var impassableTile = new RectangleShape(new Vector2f(tileSize, tileSize)) { FillColor = Color.Cyan };

            var adj = CameraAdjustment();
            for (int y = 0; y < Map.SizeY; ++y)
                for (int x = 0; x < Map.SizeX; ++x)
                {
                    if (Map.IsPassable(x, y))
                        RenderToTile(worldTarget, x - adj.X, y - adj.Y, passableTile);
                    else
                        RenderToTile(worldTarget, x - adj.X, y - adj.Y, impassableTile);
                }
        }

        public void Render()
        {
            worldTarget.Clear();
            RenderText(statusTarget, "Text1", "Text2", "  Text3");
            RenderText(textTarget, "You started the game! Welcome.");
            RenderMap();

            RenderCharacter(playerX, playerY);
            ComposeImage();
        }
    }
}
