using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class GameBoard
    {
        public int[,] board;
        public int row, column;

        GameBoard(int _row, int _column)
        {
            row = _row;
            column = _column;
            board = new int[row, column];
        }

        /// <summary>
        /// 判断是否能在top, left位置插入一个block
        /// </summary>
        bool canInsert(GameBlock block, int top, int left)
        {
            for (int i = 0; i < block.size; i++)
            {
                for (int j = 0; j < block.size; j++)
                {
                    if (block.block[i, j] == 0)
                        continue;
                    if (top + i < 0 || top + i >= row || left + j < 0 || left + j >= column)
                        return false;
                    if (board[top + i, left + j] == 1)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 在top, left位置插入一个block
        /// </summary>
        void insert(GameBlock block, int top, int left)
        {
            for (int i = 0; i < block.size; i++)
            {
                for (int j = 0; j < block.size; j++)
                {
                    board[top + i, left + j] = 1;
                }
            }
        }
    }
}
