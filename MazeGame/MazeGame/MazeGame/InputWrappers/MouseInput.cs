using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MazeGame
{
    class MouseInput
    {
        public MouseState state;
        public Point mousePosition;

        public MouseInput() { }

        public void updateState()
        {
            state = Mouse.GetState();
        }

        public void updatePosition()
        {
            mousePosition = new Point(state.X, state.Y);
        }
    }
}
