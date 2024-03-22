using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LR24
{
    public partial class edit : Form
    {
        private class GroupInfo
        {
            public string Name;
            public string Department;
        }

        // ~~~~~~~~~~~~~~~~~~~ ЗАГРУЗКА ВРЕМЕНИ ~~~~~~~~~~~~~~~~~~~
        private void LoadTimeColumn()
        {
            try
            {
                XDocument timeDoc = XDocument.Load("time.xml");

                // создание столбца
                DataGridViewTextBoxColumn timeColumn = new DataGridViewTextBoxColumn();
                timeColumn.HeaderText = "Время";

                // доб. столбца
                dataGridView1.Columns.Add(timeColumn);

                // загрузка времени в столбец
                foreach (var timeElement in timeDoc.Root.Elements())
                {
                    string time = timeElement.Value;
                    dataGridView1.Rows.Add(time);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке времени: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ~~~~~~~~~~~~~~~~~~~ СЛОВАРЬ ДНЕЙ ~~~~~~~~~~~~~~~~~~~
        private readonly Dictionary<string, string> DayNames = new Dictionary<string, string>()
        {
            { "Monday", "Понедельник" },
            { "Tuesday", "Вторник" },
            { "Wednesday", "Среда" },
            { "Thursday", "Четверг" },
            { "Friday", "Пятница" }
        };

        public edit()
        {
            InitializeComponent();
            LoadData();
            LoadTimeColumn();
        }

        // ~~~~~~~~~~~~~~~~~~~ ЗАГРУЗКА ГРУПП В ЛИСТБОКС ~~~~~~~~~~~~~~~~~~~
        private void LoadData()
        {
            string xmlFilePath = "groups.xml";

            try
            {
                List<GroupInfo> groups = new List<GroupInfo>();

                XDocument xmlDoc = XDocument.Load(xmlFilePath);

                var groupNodes = xmlDoc.Descendants("group");

                foreach (var groupNode in groupNodes)
                {
                    string groupName = groupNode.Element("name").Value;
                    string department = groupNode.Element("department").Value;

                    groups.Add(new GroupInfo { Name = groupName, Department = department });
                }

                listBox1.Items.Clear();

                var sortedGroups = groups.OrderBy(g => GetFirstDigit(g.Name)).ThenBy(g => g.Department);

                foreach (var group in sortedGroups)
                {
                    listBox1.Items.Add(group.Name);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл XML не найден. Проверьте путь к файлу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ~~~~~~~~~~~~~~~~~~~ ПЕРВАЯ ЦИФРА КУРСА ~~~~~~~~~~~~~~~~~~~
        private int GetFirstDigit(string str)
        {
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                {
                    return int.Parse(c.ToString());
                }
            }
            return -1; // если в названии группы нет цифр
        }

        // ~~~~~~~~~~~~~~~~~~~ ЗАГРУЗКА ИМЕЮЩЕГОСЯ РАСПИСАНИЯ ГРУППЫ ~~~~~~~~~~~~~~~~~~~
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGroupName = listBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedGroupName))
            {
                try
                {
                    // загружаем файлик
                    XDocument groupDoc = XDocument.Load($"{selectedGroupName}.xml");

                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    LoadTimeColumn();

                    // добавление колонок дней
                    foreach (var dayPair in DayNames)
                    {
                        string dayName = dayPair.Key;
                        string russianDayName = dayPair.Value;

                        dataGridView1.Columns.Add(dayName, russianDayName);
                    }

                    // загрузка предметов в словарь
                    foreach (var dayPair in DayNames)
                    {
                        string dayName = dayPair.Key;
                        // заходим в Schedule, потом в дочерние элементы Subject. ? делает проверку на пустоту. селект добавляет значение в список
                        var subjects = groupDoc.Root.Element("Schedule").Element(dayName)?.Elements("Subject").Select(subject => subject.Value).ToArray();

                        // загрузка предметов в строки таблицы
                        if (subjects != null)
                        {
                            for (int i = 0; i < subjects.Length; i++)
                            {
                                if (dataGridView1.Rows.Count <= i)
                                    dataGridView1.Rows.Add();

                                dataGridView1.Rows[i].Cells[dayName].Value = subjects[i];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке расписания для группы {selectedGroupName}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите группу из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ~~~~~~~~~~~~~~~~~~~ СОХРАНЕНИЕ РАСПИСАНИЯ ~~~~~~~~~~~~~~~~~~~
        private void button1_Click(object sender, EventArgs e)
        {
            string selectedGroupName = listBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedGroupName))
            {
                try
                {
                    XDocument groupDoc = XDocument.Load($"{selectedGroupName}.xml");

                    // название столбцов и загрузка предметов по дням недели
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        string dayName = column.Name;
                        // список строк со значениями каждого столбца
                        var subjects = dataGridView1.Rows.Cast<DataGridViewRow>().Select(row => row.Cells[column.Index].Value?.ToString() ?? string.Empty).ToList();

                        // проверка дня на пустоту
                        if (string.IsNullOrEmpty(dayName))
                        {
                            dayName = "EmptyDay";
                        }

                        // удаление существующих предметов
                        var existingSubjects = groupDoc.Root.Element("Schedule").Element(dayName)?.Elements("Subject");
                        existingSubjects?.Remove();

                        // добавление новых предметов
                        foreach (var subject in subjects)
                        {
                            groupDoc.Root.Element("Schedule").Element(dayName)?.Add(new XElement("Subject", subject));
                        }
                    }

                    groupDoc.Save($"{selectedGroupName}.xml");
                    MessageBox.Show("Изменения в расписании сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений в расписании для группы {selectedGroupName}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите группу из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
