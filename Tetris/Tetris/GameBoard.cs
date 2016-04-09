using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace Tetris
{
    class GameBoard
    {
        private MainWindow mainWindow;

        public int[,] board;
        public int row, column, top, left;
        public GameBlock nextBlock, nowBlock;
        public List<GameBlock> blocks = new List<GameBlock>();
        Random random = new Random();

        private void Clear()
        {
            foreach (GameBlock it in blocks)
                it.Remove(mainWindow.gameCanvas);
            blocks.Clear();

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    board[i, j] = 0;
        }

        public GameBoard(int _row, int _column, MainWindow _mainWindow)
        {
            row = _row;
            column = _column;
            board = new int[row, column];
            mainWindow = _mainWindow;
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void Start()
        {
            random = new Random();
            Clear();

            nowBlock = GameBlock.NewBlock(random.Next() % GameBlock.sizes.Count());
            nextBlock = GameBlock.NewBlock(random.Next() % GameBlock.sizes.Count());

            top = 0;
            left = column / 2 - nowBlock.size / 2;

            nowBlock.Draw(mainWindow.gameCanvas, this, Brushes.Blue, Brushes.Blue, top, left);
        }

        /// <summary>
        /// 判断是否能在top, left位置插入一个block
        /// </summary>
        bool CanInsert(GameBlock block, int top, int left)
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
        void Insert(GameBlock block, int top, int left)
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
