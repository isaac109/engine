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
    class KeyInput
    {
        public KeyboardState oldState; 
        public KeyInput(){}
        public void updateState()
        {
            oldState = Keyboard.GetState();
        }
    }
}
