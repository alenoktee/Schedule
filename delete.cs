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

namespace LR24
{
    public partial class delete : Form
    {
        private class GroupInfo
        {
            public string Name;
            public string Department;
        }

        public delete()
        {
            InitializeComponent();
            LoadData();
        }

        // ~~~~~~~~~~~~~~~~~~~ ЗАГРУЗКА СПИСКА ГРУПП ~~~~~~~~~~~~~~~~~~~
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

        // ~~~~~~~~~~~~~~~~~~~ ФУНКЦИЯ ДЛЯ ОПРЕДЕЛЕНИЯ КУРСА ~~~~~~~~~~~~~~~~~~~
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

        // ~~~~~~~~~~~~~~~~~~~ УДАЛЕНИЕ ~~~~~~~~~~~~~~~~~~~
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedGroupName = listBox1.SelectedItem.ToString();

                try
                {
                    // удаление файла группы
                    File.Delete($"{selectedGroupName}.xml");

                    // удаление из гроупс
                    XDocument xmlDoc = XDocument.Load("groups.xml");
                    xmlDoc.Root.Elements("group").Where(group => group.Element("name").Value == selectedGroupName).Remove();
                    xmlDoc.Save("groups.xml");

                    MessageBox.Show($"Группа '{selectedGroupName}' успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // обновление
                    listBox1.Items.Clear();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении группы '{selectedGroupName}': {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите группу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
