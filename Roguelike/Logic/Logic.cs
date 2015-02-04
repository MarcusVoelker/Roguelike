using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Roguelike.World;
using SFML.Window;

namespace Roguelike.Logic
{
    public class EntityLogic
    {
        private Vector2i pos;
        public Map Map { get; set; }
        
        public event EventHandler<Vector2i> PosUpdate;
        public Vector2i Pos
        {
            get { return pos; }
            set
            {
                if (!Map.IsPassable(value.X, value.Y))
                    return;

                pos = value;
                PosUpdate(this, pos);
            }
        }
    }
}
