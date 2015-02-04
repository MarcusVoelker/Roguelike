using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roguelike.Rendering;
using SFML.Graphics;
using SFML.Window;

namespace Roguelike
{
    static class MainClass
    {
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
            if (key.Code == Keyboard.Key.Left)
            {
                _renderer.UpdatePlayerPosition(-1, 0);
                return;
            }
            if (key.Code == Keyboard.Key.Right)
            {
                _renderer.UpdatePlayerPosition(1, 0);
                return;
            }
            if (key.Code == Keyboard.Key.Up)
            {
                _renderer.UpdatePlayerPosition(0, -1);
                return;
            }
            if (key.Code == Keyboard.Key.Down)
            {
                _renderer.UpdatePlayerPosition(0, 1);
                return;
            }
        }

        public static void Main()
        {
            var window = new RenderWindow(new VideoMode(1536, 864), "SFML works!");
            window.Closed += CloseWindow;
            window.KeyPressed += KeyPressed;

            _renderer = new Renderer(window,16);
            while (window.IsOpen())
            {
                window.DispatchEvents();
                _renderer.Render();
                window.Display();
            }
        }
    }
}
