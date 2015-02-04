using System;
using System.Collections.Generic;
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
        private static void CloseWindow(object obj, EventArgs args)
        {
            var renderWindow = obj as RenderWindow;
            if (renderWindow != null) 
                renderWindow.Close();
        }

        public static void Main()
        {
            var window = new RenderWindow(new VideoMode(1536, 864), "SFML works!");
            window.Closed += CloseWindow;

            var renderer = new Renderer(window,16);
            while (window.IsOpen())
            {
                window.DispatchEvents();
                renderer.Render();
                window.Display();
            }
        }
    }
}
