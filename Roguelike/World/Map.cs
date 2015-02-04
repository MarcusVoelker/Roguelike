using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.World
{
    public class Map
    {
        private readonly int sizeX;
        private readonly int sizeY;
        private readonly List<bool> passable;

        public int SizeX
        {
            get { return sizeX; }
        }

        public int SizeY
        {
            get { return sizeY; }
        }

        public Map(int sizeX, int sizeY, List<bool> passable)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.passable = passable;
        }

        public bool IsPassable(int x, int y)
        {
            if (x < 0 || y < 0 || x >= sizeX || y >= sizeY)
                return false;
            return passable[x + sizeX*y];
        }
    }
}
