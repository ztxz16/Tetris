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
        public int[,] block;
        public int size;

        List<Rectangle> rects = new List<Rectangle>();

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

        public void rotate()
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
                    block[i, j] = temp[size - j, i];
                }
            }
        }

        public void draw(Canvas canvas, GameBoard board, Brush fillBrush, Brush edgeBrush, int top, int left)
        {
            double heightUnit = canvas.Height / Convert.ToDouble(board.row);
            double widthUnit = canvas.Width / Convert.ToDouble(board.column);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (block[i, j] == 1)
                    {
                        Rectangle rectNow = new Rectangle();
                        rectNow.Height = heightUnit;
                        rectNow.Width = widthUnit;
                        Canvas.SetTop(rectNow, (top + i) * heightUnit);
                        Canvas.SetLeft(rectNow, (left + j) * widthUnit);
                        rects.Add(rectNow);
                        canvas.Children.Add(rectNow);
                    }
                }
            } 
        }

        public void remove(Canvas canvas)
        {
            foreach (Rectangle it in rects)
                canvas.Children.Remove(it);
            rects.Clear();
        }
    }
}
