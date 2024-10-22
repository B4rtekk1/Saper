using System.IO;

namespace Saper2
{
    public partial class Form1 : Form
    {
        GroupBox? gb;
        RadioButton? rb1;
        RadioButton? rb2;
        RadioButton? rb3;
        Button? btnNext;
        Panel? panel;
        Label? infoLabel;
        PictureBox? imageBox;
        PictureBox? imageBoxFlag;
        Label? bombsLabel;
        Label? flagLabel;
        Label? rulesLabel;
        public Form1()
        {
            InitializeComponent();
            GenerateMenu();
        }
        public enum DifficultyLevel
        {
            Easy,
            Medium,
            Hard
        }
        private DifficultyLevel currentDifficulty;
        void GenerateMenu()
        {
            currentDifficulty = DifficultyLevel.Medium;
            this.MaximizeBox = false;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "Menu";
            //zmienne
            int xMargin = 25;
            int yMargin = 25;
            //groupBox
            gb = new GroupBox();
            gb.Left = xMargin;
            gb.Top = yMargin;
            gb.AutoSize = true;
            gb.Text = "Choose difficulty";
            gb.FlatStyle = FlatStyle.Flat;
            
            //this.Controls.Add(gb);

            //radioButtony
            //pierwszy
            rb1 = new RadioButton();
            rb1.Left = xMargin;
            rb1.Top = yMargin;
            rb1.Text = "Easy";
            rb1.Checked = false;
            gb.Controls.Add(rb1);
            rb1.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            //drugi
            rb2 = new RadioButton();
            rb2.Left = xMargin;
            rb2.Top = yMargin * 2;
            rb2.Text = "Medium";
            rb2.Checked = true;
            gb.Controls.Add(rb2);
            rb2.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            //trzeci
            rb3 = new RadioButton();
            rb3.Left = xMargin;
            rb3.Top = yMargin * 3;
            rb3.Text = "Hard";
            rb3.Checked = false;
            gb.Controls.Add(rb3);
            rb3.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            //dodanie groupboxa do kontrolek
            this.Controls.Add(gb);
            

            //przycisk dalej
            btnNext = new Button();
            btnNext.Left = 90;
            btnNext.Top = 150;
            btnNext.Text = "Next";
            btnNext.AutoSize = true;
            this.Controls.Add(btnNext);
            btnNext.Click += btnNext_Click;

            //panel zawierajacy zdjcie oraz opis
            panel = new Panel();
            panel.Left = 250;
            panel.Top = yMargin + 10;
            panel.AutoSize = true;

            //scie¿ka
            string imagePathBomb = Path.Combine(Application.StartupPath, "bomba.png");
            string imagePathFlag = Path.Combine(Application.StartupPath, "flaga.png");
            if (!File.Exists(imagePathBomb))
            {
                MessageBox.Show($"File not found: {imagePathBomb}");
                Application.Exit();
            }
            if (!File.Exists(imagePathFlag))
            {
                MessageBox.Show($"File not found: {imagePathFlag}");
                Application.Exit();
            }


            //zdjecia
            imageBox = new PictureBox();
            imageBox.Image = Image.FromFile(imagePathBomb);
            imageBox.SizeMode = PictureBoxSizeMode.StretchImage; // Ustaw rozci¹ganie obrazu
            imageBox.Size = new Size(32, 32); // Ustaw wymiary
            imageBox.Top = 0;
            panel.Controls.Add(imageBox);
            
            imageBoxFlag = new PictureBox();
            imageBoxFlag.Image = Image.FromFile(imagePathFlag);
            imageBoxFlag.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBoxFlag.Size = new Size(32, 32);
            imageBoxFlag.Top = imageBox.Bottom + 32;
            panel.Controls.Add(imageBoxFlag);


            //opis
            bombsLabel = new Label();
            bombsLabel.Text = "x 40";
            bombsLabel.Font = new Font(bombsLabel.Font.FontFamily, 12, FontStyle.Regular);
            bombsLabel.AutoSize = true;
            bombsLabel.Top = 0;
            bombsLabel.Left = imageBox.Left + 40;
            panel.Controls.Add(bombsLabel);

            flagLabel = new Label();
            flagLabel.Text = "x 40";
            flagLabel.Font = new Font(bombsLabel.Font.FontFamily, 12, FontStyle.Regular);
            flagLabel.AutoSize = true;
            flagLabel.Top = imageBoxFlag.Top;
            flagLabel.Left = bombsLabel.Left;
            panel.Controls.Add(flagLabel);

            rulesLabel = new Label();
            rulesLabel.Text = "Game board 16x16";
            rulesLabel.Font = new Font(rulesLabel.Font.ToString(), 12, FontStyle.Regular);
            rulesLabel.AutoSize = true;
            rulesLabel.Top = bombsLabel.Top + 4;
            rulesLabel.Left = flagLabel.Left + 64;
            panel.Controls.Add(rulesLabel);

            //dodanie do formularza
            this.Controls.Add(panel);

            //pozoim trudnosci nie jest w panelu test
            infoLabel = new Label();
            infoLabel.Left = xMargin;
            infoLabel.Top = 200;
            infoLabel.Width = 100;
            infoLabel.Height = 100;
            infoLabel.AutoSize = true;
           // infoLabel.Text = "Poziom trudnoœci: œredni";
            this.Controls.Add(infoLabel);
        }

        //zmiana poziomu trudnoœci
        private void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButtonSelected && radioButtonSelected.Checked)
            {
                // Update the current difficulty level based on the selected radio button
                currentDifficulty = radioButtonSelected.Text switch
                {
                    "Easy" => DifficultyLevel.Easy,
                    "Medium" => DifficultyLevel.Medium,
                    "Hard" => DifficultyLevel.Hard,
                    _ => currentDifficulty
                };
            }
            if (bombsLabel != null && flagLabel != null && rulesLabel != null)
            {

                switch (currentDifficulty)
                {
                    case DifficultyLevel.Easy:
                        bombsLabel.Text = "x 16";
                        flagLabel.Text = "x 16";
                        rulesLabel.Text = "Game board 9x9";
                        break;
                    case DifficultyLevel.Medium:
                        bombsLabel.Text = "x 40";
                        flagLabel.Text = "x 40";
                        rulesLabel.Text = "Game board 16x16";
                        break;
                    case DifficultyLevel.Hard:
                        bombsLabel.Text = "x 99";
                        flagLabel.Text = "x 99";
                        rulesLabel.Text = "30x30";
                        break;
                }
            }
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            int rows = 0;
            int columns = 0;
            int bombs = 0;

            // Ustal liczbê wierszy, kolumn i bomb na podstawie wybranego poziomu trudnoœci
            switch (currentDifficulty)
            {
                case DifficultyLevel.Easy:
                    rows = 9;
                    columns = 9;
                    bombs = 16;
                    break;
                case DifficultyLevel.Medium:
                    rows = 16;
                    columns = 16;
                    bombs = 40;
                    break;
                case DifficultyLevel.Hard:
                    rows = 30;
                    columns = 30;
                    bombs = 99;
                    break;
            }

            // Otwórz now¹ planszê
            MessageBox.Show(currentDifficulty.ToString() + rows + columns + bombs);
            GameBoard gameBoard = new GameBoard(rows, columns, bombs, currentDifficulty.ToString());
            gameBoard.Show();
            this.Hide();

        }
    }
}
