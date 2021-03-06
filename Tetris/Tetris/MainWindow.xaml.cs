﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Brush blueBrush = new SolidColorBrush(Colors.DarkBlue);
        Brush blackBrush = new SolidColorBrush(Colors.Black);
        GameBoard board;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            board = new GameBoard(20, 10, this);
        }

        private void exit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            board.StopTimer();
            App.Current.Shutdown();
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = blueBrush;
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = blackBrush;
        }

        private void start_MouseUp(object sender, MouseButtonEventArgs e)
        {
            board.Start();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left)
                board.MoveToLeft();
            if (e.Key == Key.D || e.Key == Key.Right)
                board.MoveToRight();
            if (e.Key == Key.W || e.Key == Key.Up)
                board.Rotate();
            if (e.Key == Key.S || e.Key == Key.Down)
                board.FastDown();
        }
    }
}
