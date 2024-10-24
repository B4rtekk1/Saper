using System;
using System.Drawing;
using System.Windows.Forms;

namespace Saper2
{
    public class GameBoard : Form
    {
        private bool firstClick = true;
        private int rows;
        private int columns;
        private int bombCount;
        private int flagCount;
        private int buttonSize = 32;
        private Button[,] buttons;
        private List<int> bombPosition = new List<int>();
        private string imagePathFlag = Path.Combine(Application.StartupPath, "flaga.png");
        private string imagePathBomb = Path.Combine(Application.StartupPath, "bomba.png");


        public GameBoard(int rows, int columns, int bombCount, string difficultyLevel)
        {
            this.rows = rows;
            this.columns = columns;
            this.bombCount = bombCount;
            InitializeBoard();

            this.FormClosing += GameBoard_FormClosing;
        }
        private enum Content
        {
            Bomb,
            Flag,
            Empty,
            Number
        }

        private void InitializeBoard()
        {
            int index = 0;
            this.Text = "Saper";
            this.ClientSize = new Size(rows * buttonSize, columns * buttonSize);
            this.MaximizeBox = false;
            buttons = new Button[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    index++;
                    buttons[i, j] = new Button();
                    buttons[i, j].Size = new Size(buttonSize, buttonSize);
                    buttons[i, j].Location = new Point(j * buttonSize, i * buttonSize);
                    buttons[i, j].Click += Button_Click;
                    buttons[i, j].MouseUp += GameBoard_MouseUp;
                    buttons[i, j].FlatStyle = FlatStyle.Flat;
                    buttons[i, j].FlatAppearance.BorderSize = 0;
                    buttons[i, j].Tag = Content.Empty;
                    if (index % 2 == 0)
                    {
                        buttons[i, j].BackColor = Color.Green;
                    }
                    else
                    {
                        buttons[i, j].BackColor = Color.LightGreen;
                    }
                    this.Controls.Add(buttons[i, j]);
                }
                if (rows % 2 == 0)
                {
                    index++;
                }
            }
        }

        private void GameBoard_MouseUp(object? sender, MouseEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && e.Button == MouseButtons.Right)
            {
                if (clickedButton.Tag is Content flagged && flagged == Content.Flag)
                {
                    clickedButton.Tag = Content.Empty;
                    clickedButton.Image = null; 
                }
                else if (firstClick == false)
                {
                    clickedButton.Tag = Content.Flag;
                    clickedButton.Image = Image.FromFile(imagePathFlag);
                    clickedButton.ImageAlign = ContentAlignment.MiddleCenter;
                }
            }

        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (firstClick == true)
            {
                if (clickedButton != null)
                {
                    //clickedButton.Tag = Content.Number;
                    //MessageBox.Show(clickedButton.Tag.ToString());
                }
                firstClick = false;
                GenerateMines();
            }
            else
            {
                if (clickedButton != null && clickedButton.Tag is Content tagValue)
                {
                    if (tagValue == Content.Bomb)
                    {
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                if (buttons[i, j].Tag is Content index && index == Content.Bomb)
                                {
                                    //pokazuje bomby
                                    buttons[i, j].Image = Image.FromFile(imagePathBomb);
                                    buttons[i, j].ImageAlign = ContentAlignment.MiddleCenter;
                                }
                            }
                        }
                        MessageBox.Show("Przegrałeś", "Koniec gry");
                    }
                }
            }
        }
        private void GenerateMines()
        {
            //bomby pozycje
            Random randomBomb = new Random();
            //MessageBox.Show(imagePathBomb);
            for (int i = 0; i < this.bombCount; i++)
            {
                while (bombPosition.Count < bombCount)
                {
                    int row = randomBomb.Next(rows);
                    int col = randomBomb.Next(columns);
                    int positionToFound = row * columns + col;

                    bool exists = bombPosition.Contains(positionToFound);
                    if (!exists)
                    {
                        bombPosition.Add(positionToFound);
                    }

                }
            }
            for (int i = 0; i < bombPosition.Count; i++)
            {
                int x = bombPosition[i] % columns;
                int y = (bombPosition[i] - x) / columns;
                buttons[x, y].Tag = Content.Bomb;
                //buttons[x, y].Image = Image.FromFile(imagePathBomb);
                //buttons[x, y].ImageAlign = ContentAlignment.MiddleCenter;
                buttons[x, y].FlatAppearance.BorderSize = 0;
            }
            GenerateOtherAreas();
        }
        private void GenerateOtherAreas()
        {
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if(buttons[i, j].Tag is Content tag && tag != Content.Bomb)
                    {
                        int bombCount = TouchingBomb(i, j);
                        if (bombCount > 0)
                        {
                            buttons[i, j].Tag = Content.Number;
                            buttons[i, j].Text = bombCount.ToString();
                            buttons[i, j].Font = new Font(buttons[i, j].Font.FontFamily, 12,  FontStyle.Regular);
                            buttons[i, j].TextAlign = ContentAlignment.MiddleCenter;
                        }
                        if (buttons[i, j].Tag is Content empty && empty == Content.Empty)
                        {
                            
                            if(index % 2 == 0)
                            {
                                buttons[i, j].BackColor = Color.FromArgb(173, 101, 75);
                            }
                            else
                            {
                                buttons[i, j].BackColor = Color.FromArgb(209, 122, 90);
                            }
                            
                        }
                    }
                    index++;
                }
                if (rows % 2 == 0)
                {
                    index++;
                }
            }
        }
        private int TouchingBomb(int x, int y)
        {
            int touchingBombs = 0;
            int[,] directions = new int[,]
            {
                { 1, 1 },   
                { 1, 0 },   
                { 1, -1 },  
                { 0, -1 },  
                { -1, -1 }, 
                { -1, 0 },  
                { -1, 1 },  
                { 0, 1 }    
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                if (newX >= 0 && newX < rows && newY >= 0 && newY < columns)
                {
                    if (buttons[newX, newY].Tag is Content tag && tag == Content.Bomb)
                    {
                        touchingBombs++;
                    }
                }
            }

            return touchingBombs;
        }

        private void GameBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}