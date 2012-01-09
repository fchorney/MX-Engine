using System;
using System.Collections.Generic;
using MX;

namespace BlockGame
{
    enum BlockType
    {
        Yellow,
        Red,
        Blue,
        Green,
        White
    }

    class Block : Sprite
    {
        public int Column;
        public int Row;
        public BlockGrid Parent;

        public Block(int width, int height)
        {
            W = width;
            H = height;
            CurrentAnimation = AnimationFactory.GenerateAnimation("Cube.png", 24, 24, 5, 40);
        }
    }
}
