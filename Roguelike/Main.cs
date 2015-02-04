using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roguelike.Logic;
using Roguelike.Rendering;
using Roguelike.World;
using SFML.Graphics;
using SFML.Window;

namespace Roguelike
{
    static class MainClass
    {
        private static EntityLogic playerLogic = new EntityLogic();
        private static Renderer _renderer;
        private static void CloseWindow(object obj, EventArgs args)
        {
            var renderWindow = obj as RenderWindow;
            if (renderWindow != null) 
                renderWindow.Close();
        }

        private static void KeyPressed(object obj, EventArgs args)
        {
            var key = args as KeyEventArgs;
            Debug.Assert(key != null, "Broken Keypress Event!");
            if (key.Code == Keyboard.Key.H)
            {
                playerLogic.Pos += new Vector2i(-1, 0);
                return;
            }
            if (key.Code == Keyboard.Key.L)
            {
                playerLogic.Pos += new Vector2i(1, 0);
                return;
            }
            if (key.Code == Keyboard.Key.K)
            {
                playerLogic.Pos += new Vector2i(0, -1);
                return;
            }
            if (key.Code == Keyboard.Key.J)
            {
                playerLogic.Pos += new Vector2i(0, 1);
                return;
            }
            if (key.Code == Keyboard.Key.Y)
            {
                playerLogic.Pos += new Vector2i(-1, -1);
                return;
            }
            if (key.Code == Keyboard.Key.U)
            {
                playerLogic.Pos += new Vector2i(1, -1);
                return;
            }
            if (key.Code == Keyboard.Key.B)
            {
                playerLogic.Pos += new Vector2i(-1, 1);
                return;
            }
            if (key.Code == Keyboard.Key.N)
            {
                playerLogic.Pos += new Vector2i(1, 1);
                return;
            }
        }

        public static void Main()
        {
            var window = new RenderWindow(new VideoMode(1536, 864), "SFML works!");
            window.Closed += CloseWindow;
            window.KeyPressed += KeyPressed;

            _renderer = new Renderer(window,16);
            playerLogic.PosUpdate += _renderer.UpdatePlayerPosition;
            var rnd = new Random();
            var map = new List<bool>();
            for (int i = 0; i < 10000; ++i)
                map.Add(rnd.Next() % 4 != 1);

            _renderer.Map = new Map(100,100,map);
            playerLogic.Map = _renderer.Map;
            while (window.IsOpen())
            {
                window.DispatchEvents();
                _renderer.Render();
                window.Display();
            }
        }
    }
}
