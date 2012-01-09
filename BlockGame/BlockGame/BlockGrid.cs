using System;
using System.Collections.Generic;
using MX;

namespace BlockGame
{

    class BlockGrid
    {
        int Width;
        int Height;
        int Rows;
        int Cols;

        List<Block> Blocks;
        int[,] Grid;

        public BlockGrid(int width, int height, int cols, int rows)
        {
            this.Width = width;
            this.Height = height;
            this.Cols = cols;
            this.Rows = rows;

            Grid = new int[cols, rows];
            Blocks = new List<Block>();
        }


        public void DropBlock(int col, Block block)
        {
            block.Parent = this;
            block.Column = col;
            block.X = Width * col; //move to Column property

            Blocks.Add(block);
        }

    }

}
