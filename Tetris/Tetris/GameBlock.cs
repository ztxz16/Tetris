using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    class GameBlock
    {
        static Random random = new Random();

        static public int[] sizes = {3 };
        static public string[] fills = {"010111000" };

        public int[,] block;
        public int size;
        
        List<Rectangle> rects = new List<Rectangle>();

        static public GameBlock NewBlock()
        {
            int id = random.Next() % sizes.Count();
            return (new GameBlock(sizes[id], fills[id]));
        }

        public GameBlock(int _size, string fill)
        {
            size = _size;
            block = new int[size, size];
            int start = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    block[i, j] = Convert.ToInt32(fill[start++]) - 48;
                }
            }
        }

        public void Rotate()
        {
            int[,] temp = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp[i, j] = block[i, j];
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    block[i, j] = temp[j, size - 1 - i];
                }
            }
        }

        public void Draw(Canvas canvas, GameBoard board, Brush fillBrush, Brush edgeBrush, int top, int left)
        {
            double heightUnit = canvas.ActualHeight / Convert.ToDouble(board.row);
            double widthUnit = canvas.ActualWidth / Convert.ToDouble(board.column);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (block[i, j] == 1)
                    {
                        Rectangle rectNow = new Rectangle();
                        rectNow.Height = heightUnit;
                        rectNow.Width = widthUnit;
                        rectNow.Fill = fillBrush;
                        rectNow.Stroke = edgeBrush;
                        Canvas.SetTop(rectNow, (top + i) * heightUnit);
                        Canvas.SetLeft(rectNow, (left + j) * widthUnit);
                        rects.Add(rectNow);
                        canvas.Children.Add(rectNow);
                    }
                }
            } 
        }

        public void Remove(Canvas canvas)
        {
            foreach (Rectangle it in rects)
                canvas.Children.Remove(it);
            rects.Clear();
        }
    }
}
