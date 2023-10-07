using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Form_Label
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
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
        private void InitForm7()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("关系", typeof(string));
            dataTable.Columns.Add("词1", typeof(string));
            dataTable.Columns.Add("索引1", typeof(int));
            dataTable.Columns.Add("词2", typeof(string));
            dataTable.Columns.Add("索引2", typeof(int));
            dataTable.Columns.Add("颜色", typeof(Color));

            string[] lines = GetTxtData("relations.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                int id;
                if (int.TryParse(parts[0], out id))
                {
                    string r = parts[1];
                    string word1 = parts[2];
                    string word2 = parts[4];
                    int i1 = int.Parse(parts[3]);
                    int i2 = int.Parse(parts[5]);
                    Color color = ColorTranslator.FromHtml(parts[6]);
                    DataRow newRow = dataTable.NewRow();
                    newRow["ID"] = id;
                    newRow["关系"] = r;
                    newRow["词1"]=word1;
                    newRow["索引1"] = i1;
                    newRow["词2"] = word2;
                    newRow["索引2"] = i2;
                    newRow["颜色"] = color;
                    dataTable.Rows.Add(newRow);
                }
            }
            // 将DataTable绑定到DataGridView
            dataGridView1.DataSource = dataTable;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["关系"].Value is string word && row.Cells["颜色"].Value is Color color)
                {
                    row.Cells["关系"].Style.ForeColor = color;
                    row.Cells["关系"].Value = word; // 设置文本
                }
            }
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            InitForm7();
            LoadDataToComboBox("c_relations.txt");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFileIDColumn("relations.txt");
            //MessageBox.Show("你正在删除");
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                int index = selectedRow.Index + 1;
                string[] lines = GetTxtData("relations.txt");
                List<string> list = new List<string>();
                foreach (string line in lines)
                {
                    string[] l = line.Split('\t');
                    if (index + "" != l[0])
                    {
                        list.Add(line);
                    }
                }
                string[] result = list.ToArray();
                for (int i = 0; i < result.Length; i++)
                {
                    File.WriteAllLines("Resource\\data\\relations.txt", result);
                    UpdateFileIDColumn("relations.txt");
                    InitForm7();

                }
                MessageBox.Show("已删除");
            }
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
        private void button1_Click(object sender, EventArgs e)
        {
            //直接从words那里抄过来的
            string[] words = GetTxtData("relations.txt");
            string dw = comboBox1.Text;
            int cnt = 0;
            int index = 0;
            foreach (string word in words)
            {
                if (!word.Contains(dw)) cnt++;
            }
            if (cnt == words.Length)//全部都需要保留，即没有想要删除的词
            {
                MessageBox.Show("没有需要删除的关系");
            }
            else
            {
                string[] newWords = new string[cnt];
                foreach (string word in words)
                {
                    if (!word.Contains(dw)) newWords[index++] = word;
                }
                File.WriteAllLines("Resource\\data\\relations.txt", newWords);
                UpdateFileIDColumn("relations.txt");
                InitForm7();
            }
        }
    }
}
