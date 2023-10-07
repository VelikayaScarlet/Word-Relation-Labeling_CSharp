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

namespace Form_Label
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            UpdateFileIDColumn("c_relations.txt");
            LoadDataToComboBox();
            LoadDataToDataGridView();
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
                    Console.WriteLine(words[i]);
                }
            }
            // 保存修改后的内容回到文件
            File.WriteAllLines("Resource\\data\\"+file, words);
        }
        private void LoadDataToDataGridView()
        {
            // 创建一个DataTable作为数据源
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Word", typeof(string));
            dataTable.Columns.Add("Color", typeof(Color));

            string path = "Resource\\data\\c_relations.txt";
            string fullPath = Path.Combine(Application.StartupPath, path);

            if (File.Exists(fullPath))
            {
                string[] lines = File.ReadAllLines(fullPath);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                    string[] parts = line.Split('\t');
                    if (parts.Length == 3)
                    {
                        int id;
                        if (int.TryParse(parts[0], out id))
                        {
                            string word = parts[1];
                            Color color = ColorTranslator.FromHtml(parts[2]);

                            DataRow newRow = dataTable.NewRow();
                            newRow["ID"] = id;
                            newRow["Word"] = word;
                            newRow["Color"] = color;
                            dataTable.Rows.Add(newRow);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("文件不存在。");
            }

            // 将数据绑定到 DataGridView
            dataGridView1.DataSource = dataTable;

            // 设置单元格中文字的颜色
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Word"].Value is string word && row.Cells["Color"].Value is Color color)
                {
                    row.Cells["Word"].Style.ForeColor = color;
                    row.Cells["Word"].Value = word; // 设置文本
                }
            }
        }
        private void LoadDataToComboBox()
        {
            // 清空 ComboBox
            comboBox1.Items.Clear();

            string path = "Resource\\data\\c_relations.txt";
            string fullPath = Path.Combine(Application.StartupPath, path);

            if (File.Exists(fullPath))
            {
                string[] lines = File.ReadAllLines(fullPath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length == 3)
                    {
                        string word = parts[1];
                        comboBox1.Items.Add(word);
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                string newWord = textBox1.Text;

                // 更新 DataGridView 显示
                var dataTable = (DataTable)dataGridView1.DataSource;

                // 检查是否已经存在相同的
                bool wordExists = dataTable.AsEnumerable().Any(row => row.Field<string>("Word") == newWord);

                if (!wordExists)
                {
                    int newRowID = dataGridView1.Rows.Count + 1;
                    // 3. 生成随机的暖色
                    Random random = new Random();
                    Color randomColor;
                    do
                    {
                        randomColor = Color.FromArgb(random.Next(128, 256), random.Next(0,128), random.Next(0,128));
                    } while (randomColor.GetBrightness() < 0.7); // 仅选择较亮的颜色
                    string hexColor = ColorTranslator.ToHtml(randomColor);

                    // 添加新词语到 DataTable
                    dataTable.Rows.Add(newRowID, newWord, randomColor);

                    // 保存新词语到文件
                    string path = "Resource\\data\\c_relations.txt";
                    string fullPath = Path.Combine(Application.StartupPath, path);
                    string newData = $"{newRowID}\t{newWord}\t{hexColor}";
                    try
                    {
                        File.AppendAllText(fullPath, newData + Environment.NewLine);
                        UpdateFileIDColumn("c_relations.txt");
                        LoadDataToComboBox();
                        LoadDataToDataGridView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"添加关系失败：{ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("关系已经存在。");
                }
            }
            else
            {
                MessageBox.Show("请填写关系。");
            }
        }

        private void Form5_Load_1(object sender, EventArgs e)
        {
            LoadDataToComboBox();
            LoadDataToDataGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedText = comboBox1.SelectedItem as string;
            string[] exist = GetTxtData("relations.txt");
            foreach (string s in exist)
            {
                if (s.Contains(selectedText))
                {
                    MessageBox.Show("该关系存在于标注中，若想删除该关系，可以使用批量删除功能");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(selectedText))
            {
                string path = "Resource\\data\\c_relations.txt";
                string fullPath = Path.Combine(Application.StartupPath, path);

                if (File.Exists(fullPath))
                {
                    // 读取文本文件的所有行
                    List<string> lines = File.ReadAllLines(fullPath).ToList();

                    // 查找并删除匹配的行
                    for (int i = lines.Count - 1; i >= 0; i--)
                    {
                        string line = lines[i];
                        string[] parts = line.Split('\t');

                        if (parts.Length >= 2 && parts[1].Trim() == selectedText.Trim())
                        {
                            lines.RemoveAt(i);
                        }
                    }

                    // 将更新后的内容保存回文件
                    File.WriteAllLines(fullPath, lines);

                    // 刷新 ComboBox 和 DataGridView
                    UpdateFileIDColumn("c_relations.txt");
                    LoadDataToComboBox();
                    LoadDataToDataGridView();
                }
                else
                {
                    MessageBox.Show("文件不存在。");
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的候选关系。");
            }
        }
    }
}
