using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LR24
{
    public partial class addgroup : Form
    {
        private Form1 form1;

        // ~~~~~~~~~~~~~~~~~~~ КОНСТРУКТОР ФОРМЫ ~~~~~~~~~~~~~~~~~~~
        public addgroup(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            Form1.Depart(comboBox1);
        }

        // ~~~~~~~~~~~~~~~~~~~ КЕЙ ПРЕСС ~~~~~~~~~~~~~~~~~~~
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // основная проверка
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            // не может начинаться с буквы
            if (textBox1.Text.Length == 0 && char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            // не больше четырёх
            if (textBox1.Text.Length >= 4 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // ~~~~~~~~~~~~~~~~~~~ ДОБАВЛЕНИЕ ГРУППЫ ~~~~~~~~~~~~~~~~~~~
        private void button1_Click(object sender, EventArgs e)
        {
            var name = textBox1.Text;
            var depart = comboBox1.SelectedItem.ToString();

            var doc = new XDocument(
                new XElement("Group",
                    new XElement("Name", name),
                    new XElement("Department", depart),
                    new XElement("Schedule",
                        new XElement("Monday", new XElement("Subject", "Отсутствует.")),
                        new XElement("Tuesday", new XElement("Subject", "Отсутствует.")),
                        new XElement("Wednesday", new XElement("Subject", "Отсутствует.")),
                        new XElement("Thursday", new XElement("Subject", "Отсутствует.")),
                        new XElement("Friday", new XElement("Subject", "Отсутствует."))
                    )
                )
            );

            doc.Save($"{name}.xml");

            var groupsDoc = XDocument.Load("groups.xml");
            groupsDoc.Root.Add(new XElement("group",
            new XElement("name", name),
            new XElement("department", depart)));
            groupsDoc.Save("groups.xml");

            DialogResult result = MessageBox.Show("Группа создана, но у неё пока нет расписания. Хотите создать расписание для этой группы?", "Отлично!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                edit editform = new edit();
                editform.Show();
            }
            else
            {
                this.Close();
            }
            form1.InitializeForm();
        }
    }
}
