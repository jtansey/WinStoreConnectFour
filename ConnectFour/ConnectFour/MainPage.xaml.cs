﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ConnectFour
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int BOARD_WIDTH = 6;
        public int BOARD_HEIGHT = 7;
        public bool firstPlayerTurn = true;
        public bool gameOver = false;
        public string firstPlayerName = "Player One";
        public string secondPlayerName = "Player Two";
        public Color firstPlayerColor = Colors.Blue;
        public Color secondPlayerColor = Colors.Red;
        public int firstPlayerScore = 0;
        public int secondPlayerScore = 0;
        //public int[,] grid = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };
        public int[,] grid = { { 0, 1, 2, 0, 1, 2 }, { 1, 0, 2, 2, 1, 0 }, { 2, 2, 1, 0, 0, 1 }, { 0, 1, 0, 2, 1, 2 }, { 0, 2, 2, 1, 0, 0 }, { 0, 2, 1, 2, 1, 0 }, { 1, 1, 2, 2, 0, 1 } };
        public MainPage()
        {
            this.InitializeComponent();
            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    Border border = new Border();
                    border.Width = 100;
                    border.Height = border.Width;
                    Canvas.SetTop(border, 100 * r);
                    Canvas.SetLeft(border, 100 * c);
                    //border.Name = generateName(c, r);
                    //border.PointerPressed += showborder;
                    border.BorderBrush = new SolidColorBrush(Colors.White);
                    border.BorderThickness = new Thickness(1, 1, 1, 1);
                    if (r == 0 && c == 0)
                        border.CornerRadius = new CornerRadius(20,0,0,0);
                    if(r == 0 && c == BOARD_WIDTH - 1)
                        border.CornerRadius = new CornerRadius(0,20,0,0);
                    if(r == BOARD_HEIGHT - 1 && c == 0)
                        border.CornerRadius = new CornerRadius(0,0,0,20);
                    if (r == BOARD_HEIGHT - 1 && c == BOARD_WIDTH - 1)
                        border.CornerRadius = new CornerRadius(0,0,20,0);

                    if (c == 0)
                        border.BorderThickness = new Thickness(2, 1, 1, 1);
                    if (c == BOARD_WIDTH - 1)
                        border.BorderThickness = new Thickness(1, 1, 2, 1);
                    if (r == BOARD_HEIGHT - 1)
                        border.BorderThickness = new Thickness(1, 1, 1, 2);
                    if (r == 0)
                        border.BorderThickness = new Thickness(1, 2, 1, 1);

                    boardCanvas.Children.Add(border);
                }
            }

            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 80;
                    ellipse.Height = ellipse.Width;
                    Canvas.SetTop(ellipse, 100 * r + 10);
                    Canvas.SetLeft(ellipse, 100 * c + 10);
                    //rect.Name = generateName(c, r);
                    //rect.PointerPressed += showRect;
                    boardCanvas.Children.Add(ellipse);
                }
            }

            for (int c = 0; c < BOARD_WIDTH; c++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 100;
                textBlock.Width = 100;
                Canvas.SetLeft(textBlock, 100 * c);
                textBlock.Text = (c + 1).ToString();
                textBlock.FontFamily = new FontFamily("Kristen ITC");
                textBlock.FontSize = 36;
                textBlock.TextAlignment = TextAlignment.Center;

                textBlock.Name = c.ToString();
                textBlock.PointerPressed += columnClicked;

                columnCanvas.Children.Add(textBlock);
            }

            columnCanvas.HorizontalAlignment = HorizontalAlignment.Center;
            columnCanvas.Margin = new Thickness(0, 125, 0, 0);
            columnCanvas.Height = 100;
            columnCanvas.Width = BOARD_WIDTH * 100;

            boardCanvas.HorizontalAlignment = HorizontalAlignment.Center;
            boardCanvas.VerticalAlignment = VerticalAlignment.Center;
            boardCanvas.Height = BOARD_HEIGHT * 100;
            boardCanvas.Width = BOARD_WIDTH * 100;
            DrawGrid();

            firstPlayerNameTextBlock.Text = firstPlayerName + " Wins:  ";
            firstPlayerScoreTextBlock.Text = firstPlayerScore.ToString();

            secondPlayerNameTextBlock.Text = secondPlayerName + " Wins:  ";
            secondPlayerScoreTextBlock.Text = secondPlayerScore.ToString();
        }

        private void DrawGrid()
        {
            int index = BOARD_HEIGHT * BOARD_WIDTH;

            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    Ellipse ellipse = boardCanvas.Children[index] as Ellipse;
                    index++;

                    ellipse.StrokeThickness = 3;
                    if (grid[r, c] == 1)
                    {
                        ellipse.Stroke = new SolidColorBrush(firstPlayerColor);
                    }
                    else if (grid[r, c] == 2)
                    {
                        ellipse.Stroke = new SolidColorBrush(secondPlayerColor);
                    }
                    else
                    {
                        ellipse.Stroke = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

        private void newGame()
        {
            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    grid[r, c] = 0;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newGame();
            DrawGrid();
        }


        private void columnClicked(object sender, PointerRoutedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            makePlay(Convert.ToInt32(textBlock.Name));
        }
        private void makePlay(int column)
        {
            testTextBlock.Text = column.ToString();

            if(grid[0,column] != 0)
            {
                testTextBlock.Text = "Invalid Move";
            }
            else
            {
                for (int r = BOARD_HEIGHT - 1; r > -1; r--)
                {
                    if (grid[r, column] == 0)
                    {
                        if(firstPlayerTurn)
                        {
                            grid[r, column] = 1;
                            
                        }
                        if (!firstPlayerTurn)
                        {
                            grid[r, column] = 2;

                        }
                        break;
                    }
                }
                firstPlayerTurn = !firstPlayerTurn;
                DrawGrid();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage), null);
        }
    }
}
