using System;
using System.Windows.Forms;
using NASA_API_Example;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NASA_API_Example
{
    public partial class Form1 : Form
    {
        int count = 0;
        public Form1()
        {

            GlobalDataAsync();
            NearAsteroid();
            InitializeComponent();

        }

        private async void button1_Click(object sender, EventArgs e)
        // Script pour afficcher la text box si elle n'est pas visible et la remplir de la description
        {
            if (textBox1.Visible)
            {
                textBox1.Visible = false;
            }
            else
            {
                textBox1.Visible = true;
            }

            var d = await GetDescriptionAsync();
            textBox1.Text = d;

            // Affichage du Titre de l'image 

            var Title = await GetTitleAsync();
            label1.Text = Title;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private async void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var data = await NearAsteroid();

            string[] data_tri = data.Split(",");
            // Effacement des données puis re importation
            comboBox1.Items.Clear();
            // Ajout des différentes données
            foreach (string item in data_tri)
            {
                comboBox1.Items.Add(item);
            }


            if (comboBox1.Visible)
            {
                comboBox1.Visible = false;
            }
            else
            {
                comboBox1.Visible = true;
            }
            if (label4.Visible)
            {
                label4.Visible = false;
            }
            else
            {
                label4.Visible = true;
            }
            if (comboBox2.Visible)
            {
                comboBox2.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            count = count + 1;
            label3.Text = count.ToString();

        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            // MessageBox.Show(senderComboBox.SelectedItem.ToString());     // Get the name of the selected item

            // Appelle de la fonction description 
            var description = await Desciption_Obj(senderComboBox.SelectedItem.ToString());
            label4.Text = description.ToString();
            label4.Visible = true;
            label4.Text = label4.Text.Replace(',', '\n');

            var Info = await GetNeoInfo(senderComboBox.SelectedItem.ToString());

            comboBox2.Items.Clear();
            // Ajout des différentes données
            string[] Info_tri = Info.Split(",");

            foreach (string item in Info_tri)
            {
                comboBox2.Items.Add(item);
            }

            comboBox2.Visible = true;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
