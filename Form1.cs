using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LR24
{
    public partial class Form1 : Form
    {
        string sel_day = "Понедельник";
        string sel_dep;

        public Form1()
        {
            InitializeComponent();
            button1.PerformClick();
            InitializeForm();
        }

        // ~~~~~~~~~~~~~~~~~~~ ОБНОВЛЕНИЕ ФОРМЫ ~~~~~~~~~~~~~~~~~~~
        public void InitializeForm()
        {
            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn timeColumn = new DataGridViewTextBoxColumn();
            timeColumn.HeaderText = "Время";
            timeColumn.DataPropertyName = "Время";
            dataGridView1.Columns.Add(timeColumn);

            DataTable timeTable = time();
            dataGridView1.DataSource = timeTable;

            comboBox2.Items.Clear();
            Depart(comboBox2);

            comboBox2.SelectedIndex = 0;

            sel_dep = comboBox2.SelectedItem?.ToString();

            LoadGroups();
        }

        // ~~~~~~~~~~~~~~~~~~~ СЛОВАРЬ ДНИ НЕДЕЛИ ~~~~~~~~~~~~~~~~~~~
        private Dictionary<string, string> russianToEnglishDays = new Dictionary<string, string>
        {
            { "Понедельник", "Monday" },
            { "Вторник", "Tuesday" },
            { "Среда", "Wednesday" },
            { "Четверг", "Thursday" },
            { "Пятница", "Friday" },
            { "Суббота", "Saturday" },
            { "Воскресенье", "Sunday" }
        };

        // ~~~~~~~~~~~~~~~~~~~ МЕНЮ ПУНКТЫ ~~~~~~~~~~~~~~~~~~~
        private void добавитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addgroup add = new addgroup(this);
            add.Show();
            InitializeForm();
        }
        private void удалитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete delform = new delete();
            delform.Show();
            InitializeForm();
        }
        private void изменитьРасписаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit editform = new edit();
            editform.Show();
            InitializeForm();
        }

        // ~~~~~~~~~~~~~~~~~~~ КОМБОБОКС ОТДЕЛЕНИЯ ~~~~~~~~~~~~~~~~~~~
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sel_dep = comboBox2.SelectedItem.ToString();
            LoadGroups();
        }

        // ~~~~~~~~~~~~~~~~~~~ ЗАГРУЗКА ГРУПП ~~~~~~~~~~~~~~~~~~~
        private void LoadGroups()
        {
            dataGridView1.Columns.Clear();

            XDocument groupsDoc = XDocument.Load("groups.xml");

            string selectedDay = russianToEnglishDays[sel_day];

            DataTable dt = new DataTable();

            dt.Columns.Add("Время");

            var filteredGroups = groupsDoc.Root.Elements("group");
            if (!string.IsNullOrEmpty(sel_dep))
            {
                filteredGroups = filteredGroups.Where(group => group.Element("department").Value == sel_dep);
            }

            var sortedGroups = filteredGroups.OrderBy(group => group.Element("name").Value);

            foreach (var groupElement in sortedGroups)
            {
                string groupName = groupElement.Element("name").Value;
                dt.Columns.Add(groupName);
            }

            DataTable timeTable = time();
            foreach (DataRow row in timeTable.Rows)
            {
                dt.Rows.Add(row.ItemArray);
            }

            foreach (var groupElement in filteredGroups)
            {
                string groupName = groupElement.Element("name").Value;
                XDocument groupScheduleDoc = XDocument.Load($"{groupName}.xml");

                var subjects = groupScheduleDoc.Root.Element("Schedule").Element(selectedDay)?.Elements("Subject").Select(subject => subject.Value).ToList();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (subjects != null && i < subjects.Count)
                    {
                        dt.Rows[i][groupName] = subjects[i];
                    }
                    else
                    {
                        dt.Rows[i][groupName] = string.Empty;
                    }
                }
            }

            dataGridView1.DataSource = dt;
        }

        // ~~~~~~~~~~~~~~~~~~~ ЗАГРУЗКА ВРЕМЕНИ ЗАНЯТИЙ ~~~~~~~~~~~~~~~~~~~
        private DataTable time()
        {
            XDocument loadedData = XDocument.Load("time.xml");

            DataTable dt = new DataTable();
            dt.Columns.Add("Время");

            foreach (XElement xelement in loadedData.Element("root").Elements())
            {
                DataRow row = dt.NewRow();
                row["Время"] = xelement.Value;
                dt.Rows.Add(row);
            }

            return dt;
        }

        // ~~~~~~~~~~~~~~~~~~~ ОТДЕЛЕНИЯ ЗАГРУЗКА ФАЙЛА ~~~~~~~~~~~~~~~~~~~
        public static void Depart(System.Windows.Forms.ComboBox comboBox)
        {
            try
            {
                XDocument loadedData = XDocument.Load("depart.xml");
                foreach (XElement xelement in loadedData.Element("root").Elements())
                {
                    comboBox.Items.Add(xelement.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка отделений: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ~~~~~~~~~~~~~~~~~~~ КНОПКИ ДНЕЙ ~~~~~~~~~~~~~~~~~~~
        private void button1_Click(object sender, EventArgs e)
        {
            sel_day = "Понедельник";
            button1.BackColor = Color.MistyRose;
            button2.BackColor = Color.MintCream;
            button3.BackColor = Color.MintCream;
            button4.BackColor = Color.MintCream;
            button5.BackColor = Color.MintCream;
            LoadGroups();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            sel_day = "Вторник";
            button1.BackColor = Color.MintCream;
            button2.BackColor = Color.MistyRose;
            button3.BackColor = Color.MintCream;
            button4.BackColor = Color.MintCream;
            button5.BackColor = Color.MintCream;
            LoadGroups();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            sel_day = "Среда";
            button1.BackColor = Color.MintCream;
            button2.BackColor = Color.MintCream;
            button3.BackColor = Color.MistyRose;
            button4.BackColor = Color.MintCream;
            button5.BackColor = Color.MintCream;
            LoadGroups();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            sel_day = "Четверг";
            button1.BackColor = Color.MintCream;
            button2.BackColor = Color.MintCream;
            button3.BackColor = Color.MintCream;
            button4.BackColor = Color.MistyRose;
            button5.BackColor = Color.MintCream;
            LoadGroups();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            sel_day = "Пятница";
            button1.BackColor = Color.MintCream;
            button2.BackColor = Color.MintCream;
            button3.BackColor = Color.MintCream;
            button4.BackColor = Color.MintCream;
            button5.BackColor = Color.MistyRose;
            LoadGroups();
        }

        // ~~~~~~~~~~~~~~~~~~~ АДАПТИВ ~~~~~~~~~~~~~~~~~~~
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Font small = new Font("Comic Sans MS", 11, FontStyle.Regular);
            Font big = new Font("Comic Sans MS", 12, FontStyle.Regular);
            if (this.Width < 700)
            {
                button1.Text = "Пн";
                button2.Text = "Вт";
                button3.Text = "Ср";
                button4.Text = "Чт";
                button5.Text = "Пт";
                this.Font = small;
            }
            else
            {
                button1.Text = "Понедельник";
                button2.Text = "Вторник";
                button3.Text = "Среда";
                button4.Text = "Четверг";
                button5.Text = "Пятница";
                this.Font = big;
            }
        }

        // ~~~~~~~~~~~~~~~~~~~ ФОН ~~~~~~~~~~~~~~~~~~~
        private void background_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 6);

            switch (randomNumber)
            {
                case 1:
                    this.BackColor = Color.PaleTurquoise;
                    break;
                case 2:
                    this.BackColor = Color.SkyBlue;
                    break;
                case 3:
                    this.BackColor = Color.LavenderBlush;
                    break;
                case 4:
                    this.BackColor = Color.Honeydew;
                    break;
                case 5:
                    this.BackColor = Color.OldLace;
                    break;
                default:
                    this.BackColor = Color.PaleTurquoise;
                    break;
            }
        }
    }
}
