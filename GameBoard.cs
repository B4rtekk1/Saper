using System;
using System.Drawing;
using System.Windows.Forms;

namespace Saper2
{
    public class GameBoard : Form
    {
        private int rows;
        private int columns;
        private int bombCount;
        private int buttonSize;
        private Button[,] buttons;


        public GameBoard(int rows, int columns, int bombCount, string difficultyLevel)
        {
            this.rows = rows;
            this.columns = columns;
            this.bombCount = bombCount;
            if(difficultyLevel == "Easy")
            {
                buttonSize = 64;
            }
            else if(difficultyLevel == "Medium")
            {
                buttonSize = 48;
            }
            else
            {
                buttonSize = 40;
            }
            InitializeBoard();

            this.FormClosed += GameBoard_FormClosed;
        }

        private void InitializeBoard()
        {
            this.Text = "Saper";
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MaximizeBox = false;
            buttons = new Button[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Size = new Size(buttonSize, buttonSize); // Zmniejszenie rozmiaru przycisku do 32x32
                    buttons[i, j].Location = new Point(j * buttonSize, i * buttonSize); // Ustawienie nowej lokalizacji na podstawie zmniejszonego rozmiaru
                    buttons[i, j].Click += Button_Click;
                    buttons[i, j].FlatStyle = FlatStyle.Flat;
                    buttons[i, j].FlatAppearance.BorderSize = 0;
                    buttons[i, j].BackColor = Color.Gray;
                    buttons[i, j].Paint += Button_Paint;
                    this.Controls.Add(buttons[i, j]);
                }
            }
        }

        private void Button_Paint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            int cellSize = buttonSize / 2; // Ustal rozmiar pojedynczej kratki (zmniejszony do 16, aby pasował do zmniejszonego przycisku)
            for (int x = 0; x < button.Width; x += cellSize)
            {
                for (int y = 0; y < button.Height; y += cellSize)
                {
                    Color color = ((x / cellSize) + (y / cellSize)) % 2 == 0 ? Color.MediumSpringGreen : Color.Green;
                    using (Brush brush = new SolidBrush(color))
                    {
                        e.Graphics.FillRectangle(brush, x, y, cellSize, cellSize);
                    }
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            MessageBox.Show("Button clicked!");
        }

        private void GameBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
