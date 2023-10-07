using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form_Label
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private string[] GetTxtData(string filename)
        {
            string path = "Resource\\data\\" + filename;
            string fullPath = Path.Combine(Application.StartupPath, path);
            if (File.Exists(fullPath))
            {
                string[] txt = File.ReadAllLines(fullPath);
                return txt;
            }
            else
            {
                MessageBox.Show("文件不存在。");
                return new string[0];
            }
        }
        private void UpdateFileIDColumn(string file)
        {
            string[] words = GetTxtData(file);
            for (int i = 0; i < words.Length; i++)
            {
                string[] parts = words[i].Split('\t');
                if (parts.Length >= 1)
                {
                    parts[0] = (i + 1).ToString(); // 修改第一列为行数
                    words[i] = string.Join("\t", parts);
                }
            }
            // 保存修改后的内容回到文件
            File.WriteAllLines("Resource\\data\\"+file, words);
        }
        private void InitForm6()
        {
            // 创建一个DataTable来存储数据
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("词语", typeof(string));
            dataTable.Columns.Add("标注", typeof(string));
            dataTable.Columns.Add("起始", typeof(int));
            dataTable.Columns.Add("结束", typeof(int));
            dataTable.Columns.Add("颜色", typeof(Color));

            // 读取文件并将数据添加到DataTable中
            string[] lines = GetTxtData("words.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                if (parts.Length == 6)
                {
                    int id;
                    if (int.TryParse(parts[0], out id))
                    {
                        string word = parts[1];
                        string label = parts[2];
                        int start = int.Parse(parts[3]);
                        int end = int.Parse(parts[4]);
                        Color color = ColorTranslator.FromHtml(parts[5]);

                        dataTable.Rows.Add(id, word, label, start, end, color);
                    }
                }
            }
            // 将DataTable绑定到DataGridView
            dataGridView1.DataSource = dataTable;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["词语"].Value is string word && row.Cells["颜色"].Value is Color color)
                {
                    row.Cells["词语"].Style.ForeColor = color;
                    row.Cells["词语"].Value = word; // 设置文本
                }
            }
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            InitForm6();
            LoadDataToComboBox("c_words.txt");
        }
        private void LoadDataToComboBox(string p)
        {
            // 清空 ComboBox
            comboBox1.Items.Clear();

            string path = "Resource\\data\\" + p;
            string fullPath = Path.Combine(Application.StartupPath, path);

            if (File.Exists(fullPath))
            {
                string[] lines = File.ReadAllLines(fullPath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t');
                    string word = parts[1];
                    comboBox1.Items.Add(word);
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFileIDColumn("words.txt");
            //MessageBox.Show("你正在删除");
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                int index = selectedRow.Index + 1;
                string[] lines = GetTxtData("words.txt");
                List<string> list =new List<string>();
                foreach (string line in lines)
                {
                    string[] l = line.Split('\t');
                    if (index+"" != l[0])
                    {
                        list.Add(line);
                    }
                }
                string[] result = list.ToArray();
                for(int i = 0; i < result.Length; i++)
                {
                    File.WriteAllLines("Resource\\data\\words.txt", result);
                    UpdateFileIDColumn("words.txt");
                    InitForm6();

                }
                MessageBox.Show("已删除");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] words = GetTxtData("words.txt");
            string dw = comboBox1.Text;
            int cnt = 0;
            int index = 0;
            foreach(string word in words)
            {
                if(!word.Contains(dw)) cnt++;
            }
            if (cnt == words.Length)//全部都需要保留，即没有想要删除的词
            {
                MessageBox.Show("没有需要删除的词");
            }
            else
            {
                string[] newWords = new string[cnt];
                foreach (string word in words)
                {
                    if (!word.Contains(dw)) newWords[index++] = word;
                }
                File.WriteAllLines("Resource\\data\\words.txt", newWords);
                UpdateFileIDColumn("words.txt");
                InitForm6();
            }
        }
    }
}
