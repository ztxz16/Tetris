using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    class GameBoard
    {
        static public int initInterval = 100;

        static public Brush fillBrush1 = Brushes.Blue;
        static public Brush fillBrush2 = Brushes.AliceBlue;
        static public Brush edgeBrush1 = Brushes.Blue;
        static public Brush edgeBrush2 = Brushes.AliceBlue;

        private MainWindow mainWindow;

        public int score;
        public int[,] board;
        public int row, column, top, left;
        public GameBlock nextBlock, nowBlock;
        Random random = new Random();

        Timer timer = new Timer(initInterval);

        List<Rectangle> rects = new List<Rectangle>();

        public void StopTimer()
        {
            timer.Stop();
        }

        private void Remove()
        {
            foreach (Rectangle it in rects)
                mainWindow.gameCanvas.Children.Remove(it);
            rects.Clear();
        }

        private void Draw()
        {
            double heightUnit = mainWindow.gameCanvas.ActualHeight / Convert.ToDouble(row);
            double widthUnit = mainWindow.gameCanvas.ActualWidth / Convert.ToDouble(column);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (board[i, j] == 1)
                    {
                        Rectangle rectNow = new Rectangle();
                        rectNow.Height = heightUnit;
                        rectNow.Width = widthUnit;
                        rectNow.Fill = fillBrush2;
                        rectNow.Stroke = edgeBrush2;
                        Canvas.SetTop(rectNow, i * heightUnit);
                        Canvas.SetLeft(rectNow, j * widthUnit);
                        rects.Add(rectNow);
                        mainWindow.gameCanvas.Children.Add(rectNow);
                    }
                }
            }
        }

        private void Clear()
        {
            Remove();
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
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            mainWindow.Dispatcher.Invoke(new Action(() =>
            {
                if (MoveDown())
                    return;
                else
                {
                    //下落完成
                    Insert(nowBlock, top, left);

                    nowBlock = nextBlock;
                    top = 0;
                    left = column / 2 - nowBlock.size / 2;

                    nextBlock = GameBlock.NewBlock();

                    UpdateBoard();
                    
                    if (!CanInsert(nowBlock, top, left))
                    {
                        timer.Stop();
                        MessageBox.Show("游戏结束");
                        nowBlock = null;
                        Clear();
                    } else
                    {
                        nowBlock.Draw(mainWindow.gameCanvas, this, fillBrush1, edgeBrush1, top, left);
                    }
                }
            }));
        }

        /// <summary>
        /// 刷新board
        /// </summary>
        public void UpdateBoard()
        {
            Draw();
        }
        /// <summary>
        /// 开始游戏
        /// </summary>
        public void Start()
        {
            timer.Stop();
            if (nowBlock != null)
                nowBlock.Remove(mainWindow.gameCanvas);
            random = new Random();
            Clear();

            nowBlock = GameBlock.NewBlock();
            nextBlock = GameBlock.NewBlock();

            top = 0;
            left = column / 2 - nowBlock.size / 2;

            nowBlock.Draw(mainWindow.gameCanvas, this, fillBrush1, edgeBrush1, top, left);

            score = 0;
            mainWindow.highScore.Text = "最高分:\n" + 1000.ToString();
            mainWindow.score.Text = "得分:\n" + score.ToString();

            timer.Interval = initInterval;
            timer.Start();
        }

        /// <summary>
        /// 判断是否能在top, left位置插入一个block
        /// </summary>
        public bool CanInsert(GameBlock block, int top, int left)
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
        public void Insert(GameBlock block, int top, int left)
        {
            for (int i = 0; i < block.size; i++)
            {
                for (int j = 0; j < block.size; j++)
                {
                    if (block.block[i, j] == 1)
                        board[top + i, left + j] = 1;
                }
            }

            block.Remove(mainWindow.gameCanvas);
        }

        /// <summary>
        /// 把nowBlock左移
        /// </summary>
        public bool MoveToLeft()
        {
            lock (nowBlock)
            {
                if (CanInsert(nowBlock, top, left - 1))
                {
                    left--;
                    nowBlock.Remove(mainWindow.gameCanvas);
                    nowBlock.Draw(mainWindow.gameCanvas, this, fillBrush1, edgeBrush1, top, left);
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 把nowBlock右移
        /// </summary>
        public bool MoveToRight()
        {
            lock (nowBlock)
            {
                if (CanInsert(nowBlock, top, left + 1))
                {
                    left++;
                    nowBlock.Remove(mainWindow.gameCanvas);
                    nowBlock.Draw(mainWindow.gameCanvas, this, fillBrush1, edgeBrush1, top, left);
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 把nowBlock下移
        /// </summary>

        public bool MoveDown()
        {
            lock (nowBlock)
            {
                if (CanInsert(nowBlock, top + 1, left))
                {
                    top++;
                    nowBlock.Remove(mainWindow.gameCanvas);
                    nowBlock.Draw(mainWindow.gameCanvas, this, fillBrush1, edgeBrush1, top, left);
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 把nowBlock翻转
        /// </summary>
        public bool Rotate()
        {
            lock (nowBlock)
            {
                nowBlock.Rotate();
                if (CanInsert(nowBlock, top, left))
                {
                    nowBlock.Remove(mainWindow.gameCanvas);
                    nowBlock.Draw(mainWindow.gameCanvas, this, fillBrush1, edgeBrush1, top, left);
                    return true;
                }
                else
                {
                    nowBlock.Rotate();
                    nowBlock.Rotate();
                    nowBlock.Rotate();
                    return false;
                }
            }
        }
    }
}
