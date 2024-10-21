namespace Saper2
{
    public partial class Form1 : Form
    {
        GroupBox gb;
        RadioButton rb1;
        RadioButton rb2;
        RadioButton rb3;
        Button btnNext;
        public Form1()
        {
            InitializeComponent();
            GenerateMenu();
        }
        void GenerateMenu()
        {
            //zmienne
            int xMargin = 25;
            int yMargin = 25;
            //groupBox
            gb = new GroupBox();
            gb.Left = xMargin;
            gb.Top = yMargin;
            gb.AutoSize = true;
            gb.Text = "Wybierz poziom trudnoœci";
            gb.FlatStyle = FlatStyle.Flat;
            
            //this.Controls.Add(gb);

            //radioButtony
            //pierwszy
            rb1 = new RadioButton();
            rb1.Left = xMargin;
            rb1.Top = yMargin;
            rb1.Text = "£atwy";
            rb1.Checked = false;
            gb.Controls.Add(rb1);
            //drugi
            rb2 = new RadioButton();
            rb2.Left = xMargin;
            rb2.Top = yMargin * 2;
            rb2.Text = "Œredni";
            rb2.Checked = false;
            gb.Controls.Add(rb2);
            //trzeci
            rb3 = new RadioButton();
            rb3.Left = xMargin;
            rb3.Top = yMargin * 3;
            rb3.Text = "Trudny";
            rb3.Checked = false;
            gb.Controls.Add(rb3);

            //dodanie groupboxa do kontrolek
            this.Controls.Add(gb);

            //przycisk dalej
            btnNext = new Button();
            btnNext.Left = xMargin * 2;
            btnNext.Top = 150;
            btnNext.Text = "Dalej";
            this.Controls.Add(btnNext);
            btnNext.Click += btnNext_Click;
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            string selected = "";
            if(rb1.Checked == true)
            {
                selected = rb1.Text;
            }
            if (rb2.Checked == true)
            {
                selected = rb2.Text;
            }
            if (rb3.Checked == true)
            {
                selected = rb3.Text;
            }
            MessageBox.Show(selected);
        }
    }
}
