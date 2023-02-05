using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace NASA_API_Example
{
    partial class Form1
    {
        private const string Start_date = "2023-01-25";
        private const string End_date = "2023-01-25";
        private const string API_Neo = "https://api.nasa.gov/neo/rest/v1/feed?start_date=" + Start_date + "&end_date=" + End_date + "&api_key=Gx3vz79DKsHOFc0Ifii1ziFJ6M5Hdi2U7tr7JVAc";

        private const string DATA_API = "https://api.nasa.gov/planetary/apod?api_key=Gx3vz79DKsHOFc0Ifii1ziFJ6M5Hdi2U7tr7JVAc";

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        /// 

        async Task<string> GetNeoInfo(string ID_NEO)
        {
            string alldate = "";

            // Requete à NEO feed pour avoir l'id
            var Date = "";
            string NEO_miss_position = "";
            string NEO_ids_split = "";

            Console.WriteLine(API_Neo);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("near_earth_objects");

                string NEO_all = AllDatasplitted[1];
                // Code to get the id of the asteroid

                string[] Neo_ids_not_split = NEO_all.Split("neo_reference_id");

                NEO_ids_split = Neo_ids_not_split[1];
                NEO_ids_split = NEO_ids_split.Substring(3, 7);   // Work
            }


            // Utilisation de l'id sur NEO - lookup
            string New_API_KEY = "https://api.nasa.gov/neo/rest/v1/neo/" + NEO_ids_split + "?api_key=Gx3vz79DKsHOFc0Ifii1ziFJ6M5Hdi2U7tr7JVAc";


            HttpClient client2 = new HttpClient();
            HttpResponseMessage response2 = await client.GetAsync(New_API_KEY);
            if (response2.IsSuccessStatusCode)
            {
                string content = await response2.Content.ReadAsStringAsync();
                string patternDate = "\"close_approach_date\":\"(.*?)\",";
                MatchCollection dateMatch = Regex.Matches(content, patternDate);
                for (int i = 0; i < dateMatch.Count(); i++)
                {
                    string date = dateMatch[i].Groups[1].Value;
                    alldate = alldate + "," + date;
                }


            }
            return alldate;
        }

        async Task<string> NearAsteroid()
        {
            string Result = "";

            string[] Neo_name2 = new string[] { "" };
            Console.WriteLine(API_Neo);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                for (int i = 1; i < 22; i++)
                {
                    string AllData = await response.Content.ReadAsStringAsync();
                    string[] AllDatasplitted = AllData.Split("near_earth_objects");

                    string NEO_all = AllDatasplitted[1];

                    string[] NEO_object = NEO_all.Split("link");
                    string Neo_names = NEO_object[i];


                    // Code to get asteroid name
                    string[] Neo_name_tab = Neo_names.Split("name");
                    string Neo_name = Neo_name_tab[1];
                    Neo_name = Neo_name.Substring(3);

                    Neo_name2 = Neo_name.Split("\"");

                    Result = Result + Neo_name2[0] + ",";

                }
            }
            return Result;
        }

        async Task<string> Desciption_Obj(string Object_Name)
        {
            var Date = "";
            string NEO_miss_position = "";

            Console.WriteLine(API_Neo);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("near_earth_objects");

                string NEO_all = AllDatasplitted[1];


                // Information à partir du nom des asteroides
                string[] NEO_object = NEO_all.Split(Object_Name);


                // Code to get the id of the asteroid

                string[] Neo_ids_not_split = NEO_all.Split("neo_reference_id");

                string NEO_ids_split = Neo_ids_not_split[1];
                NEO_ids_split = NEO_ids_split.Substring(3, 7);
                //Date = await GetNeoInfo(NEO_ids_split);


                //
                string NEO_object_names_data = NEO_object[1];// voir distance des asteroide quand ils ont été au plus proche de la terre ( dans miss_distance )
                string[] NEO_object_name_Tab = NEO_object_names_data.Split("miss_distance");
                string NEO_object_name = NEO_object_name_Tab[1];
                string[] NEO_object_miss_position = NEO_object_name.Split("orbiting_body");
                NEO_miss_position = NEO_object_miss_position[0];
                NEO_miss_position = "Miss distance of the asteroide : \n " + NEO_miss_position.Substring(4);


            }
            // Reprendre Code pour parcourir list jusqu'au bon chiffre puis 
            // Se servir du name de l'objet ?
            return NEO_miss_position;
        }

        async Task<string> GlobalDataAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(DATA_API);
            var data = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string[] splitted = content.Split("url");
                string url = splitted[2];

                url = url.Substring(3, url.Length - 6);
                Console.WriteLine("Lien de l'image " + url);
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {

                    byte[] imageData = await resp.Content.ReadAsByteArrayAsync();
                    using (var stream = new MemoryStream(imageData))
                    {
                        var image = Image.FromStream(stream);
                        this.BackgroundImage = new Bitmap(image);
                        this.BackgroundImageLayout = ImageLayout.Zoom;

                    }
                }

                data = await response.Content.ReadAsStringAsync();

            }

            return data;
        }


        private async Task<string> GetDescriptionAsync()
        {
            string[] Description = new string[] { };
            string Description2 = "";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(DATA_API);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string[] splitted = content.Split("explanation");
                Description2 = splitted[1];

                Description2 = Description2.Substring(3);
                Description = Description2.Split("hdurl");





            }
            return Description[0];
        }
        private async Task<string> GetTitleAsync()
        {
            string Title = "";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(DATA_API);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string[] splitted = content.Split("title");

                Title = splitted[1];

                Title = Title.Substring(3, Title.Length - 5);
                string[] tokens = Title.Split('"');
                Title = "Name of the picture : " + tokens[0];

            }
            return Title;
        }
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Help;
            this.button1.Location = new System.Drawing.Point(12, 596);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 145);
            this.button1.TabIndex = 0;
            this.button1.Text = "Voir Description";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.UseWaitCursor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(943, 22);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(480, 503);
            this.textBox1.TabIndex = 1;
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(505, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Title of the picture:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Asteroide ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(221, 598);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 141);
            this.button2.TabIndex = 5;
            this.button2.Text = "Voir asteroide";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1066, 686);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(198, 29);
            this.button3.TabIndex = 6;
            this.button3.Text = "What if you click 625 times ?";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(1264, 690);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 20);
            this.label3.TabIndex = 7;
            this.label3.UseWaitCursor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "test",
            "test2"});
            this.comboBox1.Location = new System.Drawing.Point(49, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(151, 28);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.Text = "Asteroide";
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(12, 376);
            this.label4.MaximumSize = new System.Drawing.Size(300, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(213, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Miss distance of the asteroide :";
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(206, 28);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(274, 28);
            this.comboBox2.TabIndex = 10;
            this.comboBox2.Text = "Asteroide last and futur pattern Date";
            this.comboBox2.Visible = false;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "NasaApi";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Button button2;
        private Button button3;
        private Label label3;
        private ComboBox comboBox1;
        private Label label4;
        private ComboBox comboBox2;
    }
}