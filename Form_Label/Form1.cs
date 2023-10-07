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
using System.Windows.Forms.VisualStyles;
using System.Xml;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Form_Label
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool quick_anno = false;
        string candidate_label = string.Empty;
        private string[] GetTxtData(string filename)
        {
            string path = "Resource\\data\\"+filename;
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
        private void LoadTextData()
        {
            string path = "Resource\\data\\text.txt";
            string fullPath = Path.Combine(Application.StartupPath, path);
            if (File.Exists(fullPath))
            {
                string fileContent = File.ReadAllText(fullPath);
                richTextBox1.Text = fileContent;
            }
            else MessageBox.Show("文件在" + path + "不存在。");
        }
        private void LoadDataToWordsComboBox()
        {
            // 清空 ComboBox
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            string[] c_words = GetTxtData("c_words.txt");
            string[] c_relations = GetTxtData("c_relations.txt");

            foreach (string line in c_words)
            {
                string[] parts = line.Split('\t');
                if (parts.Length == 3)
                {
                    string word = parts[1];
                    comboBox1.Items.Add(word);
                }
            }
            foreach (string line in c_relations)
            {
                string[] parts = line.Split('\t');
                if (parts.Length == 3)
                {
                    string word = parts[1];
                    comboBox2.Items.Add(word);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            LoadDataToWordsComboBox();
            LoadTextData();
            LoadWordsDataToTextColor();
            ClearForm1();
            Labels_Clicks();
        }
        private void LoadWordsDataToTextColor()
        {
            string[] lines = GetTxtData("words.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');

                if (parts.Length == 6)
                {
                    int start = int.Parse(parts[3]);
                    int end = int.Parse(parts[4]);
                    string colorHex = parts[5];

                    if (start < 0 || end < start || colorHex.Length != 7 || !colorHex.StartsWith("#"))
                    {
                        MessageBox.Show("无效的数据行。");
                        return;
                    }

                    Color color = ColorTranslator.FromHtml(colorHex);

                    richTextBox1.Select(start, end - start);
                    richTextBox1.SelectionColor = color;
                }
            }
            //string[] rs = GetTxtData("relations.txt");
            //foreach(string line in rs)
            //{
            //    string[] parts = line.Split('\t');
            //    if (parts.Length == 7)
            //    {
            //        int len1 = parts[2].Length;
            //        int start1 = int.Parse(parts[3]);
            //        int len2 = parts[4].Length;
            //        int start2 = int.Parse(parts[5]);
            //        string colorHex = parts[6];
            //        Color color = ColorTranslator.FromHtml(colorHex);

            //        richTextBox1.Select(start1, start1+len1);
            //        richTextBox1.Select(start2, start2 + len2);
            //        //在这里帮我补充加特定颜色下划线的代码
            //        richTextBox1.Select(start1, len1);
            //        richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);

            //        // 选择第二个文本范围并设置颜色和下划线
            //        richTextBox1.Select(start2, len2);
            //        richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
            //    }
            //}
        }

        private void 联系作者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void 查看使用说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void 导入文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filepath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "请选择要处理的文件";//窗口title
            ofd.Filter = "文本文件（*.txt）|*.txt";//筛选文本文件
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                string fileContent = File.ReadAllText(filepath);
                // 将文件内容赋给label1
                richTextBox1.Text = fileContent;
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void richTextBox1_SelectionChanged_1(object sender, EventArgs e)
        {
            //string selectedText = richTextBox1.SelectedText;
            //int start = richTextBox1.SelectionStart; // 获取选定文本的起始位置
            //int length = richTextBox1.SelectionLength; // 获取选定文本的长度
            //if (length != 0)
            //{
            //    // 在控制台输出颜色信息
            //    Color selectedColor = richTextBox1.SelectionColor;
            //    Console.WriteLine($"选中文本的字体颜色为: {selectedColor}");
            //    if(selectedColor == Color.Black)
            //    {
            //        textBox1.Text = selectedText;
            //        textBox2.Text = start + "";
            //        textBox3.Text = start + length + "";
            //    }
            //    else
            //    {
            //        if(textBox5.Text==string.Empty)
            //        {
            //            textBox5.Text = selectedText;
            //            textBox4.Text = start+"";
            //        }
            //        else if(textBox5.Text != string.Empty&&textBox7.Text==string.Empty)
            //        {
            //            textBox7.Text = selectedText;
            //            textBox6.Text = start + "";
            //        }
            //    }
            //}
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            string selectedText = richTextBox1.SelectedText;
            int start = richTextBox1.SelectionStart; // 获取选定文本的起始位置
            int length = richTextBox1.SelectionLength; // 获取选定文本的长度
            if (length != 0)
            {
                // 在控制台输出颜色信息
                Color selectedColor = richTextBox1.SelectionColor;
                Console.WriteLine($"选中文本的字体颜色为: {selectedColor}");
                if (selectedColor == Color.Black)
                {
                    textBox1.Text = selectedText;
                    textBox2.Text = start + "";
                    textBox3.Text = start + length + "";
                    textBox4.Text = string.Empty;
                    textBox5.Text = string.Empty;
                    textBox6.Text = string.Empty;
                    textBox7.Text = string.Empty;
                    if (quick_anno) LoadDataToBottomBar("word");
                }
                else
                {
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    textBox3.Text = string.Empty;
                    if (textBox5.Text == string.Empty&&selectedColor!=Color.Empty)
                    {
                        textBox5.Text = selectedText;
                        textBox4.Text = start + "";
                    }
                    else if (textBox5.Text != string.Empty && textBox7.Text == string.Empty && selectedColor != Color.Empty)
                    {
                        if (""+start!=textBox4.Text)
                        {
                            textBox7.Text = selectedText;
                            textBox6.Text = start + "";
                            if(quick_anno) LoadDataToBottomBar("relation");
                        }
                    }
                }
            }
        }

        private void 候选词语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void 候选关系ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }

        private void 已标词语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.ShowDialog();
        }

        private void 已标关系ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string word = textBox1.Text; //词
            string property = comboBox1.SelectedItem?.ToString(); //属性

            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(property))
            {
                MessageBox.Show("请填写词语和属性。");
                return;
            }

            // 输出选定文本的起始位置和长度

            string cWordsFilePath = "Resource\\data\\c_words.txt";
            string wordsFilePath = "Resource\\data\\words.txt";
            wordsFilePath = Path.Combine(Application.StartupPath, wordsFilePath);
            int rowCount = File.ReadLines(wordsFilePath).Count();
            // 读取 c_words.txt 文件
            string[] cWordsLines = File.ReadAllLines(Path.Combine(Application.StartupPath, cWordsFilePath));
            string color = null; // 用于存储颜色
            foreach (string line in cWordsLines)
            {
                //候选词
                string[] parts = line.Split('\t');
                if (parts.Length == 3 && parts[1] == property)
                {
                    color = parts[2];
                    break;
                }
            }
            if (color == null)
            {
                MessageBox.Show("未找到匹配的词语。");
                return;
            }
            string[] words = GetTxtData("words.txt");
            foreach(string line in words)
            {
                string[] ws = line.Split('\t');
                if (ws.Length > 3)
                {
                    if (ws[4] == textBox3.Text && ws[3] == textBox2.Text)
                    {
                        MessageBox.Show("该词已存在");
                        return;
                    }

                }
            }
            // 创建并写入元组到 words.txt
            UpdateFileIDColumn("words.txt");
            string newData = $"{rowCount + 1}\t{word}\t{property}\t{textBox2.Text}\t{textBox3.Text}\t{color}";
            File.AppendAllText(wordsFilePath, newData + Environment.NewLine);
            MessageBox.Show("添加" + word + "成功");
            LoadData();
        }
        private void ClearForm1()
        {
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            textBox7.Text = string.Empty;
            comboBox1.Text = string.Empty;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm1();
            comboBox1.Text=string.Empty; 
            comboBox2.Text=string.Empty;
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

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
            Graphics g = panel4.CreateGraphics();
            DrawTextFromFile("Resource\\data\\text.txt",g , panel4);
            Console.WriteLine("clicked button4");
        }

        private void 导出关系JSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[\n");
            string[] lines = GetTxtData("relations.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split('\t');
                if (parts.Length == 7) // 注意这里修改为 7，因为有 7 个字段
                {
                    int id = int.Parse(parts[0]);
                    string relationship = parts[1];
                    string word1 = parts[2];
                    int start1 = int.Parse(parts[3]);
                    string word2 = parts[4];
                    int start2 = int.Parse(parts[5]);
                    string color = parts[6];

                    jsonBuilder.Append("{\n");
                    jsonBuilder.AppendFormat("\"id\": {0},\n", id);
                    jsonBuilder.AppendFormat("\"relationship\": \"{0}\",\n", relationship);
                    jsonBuilder.AppendFormat("\"word1\": \"{0}\",\n", word1);
                    jsonBuilder.AppendFormat("\"start1\": {0},\n", start1);
                    jsonBuilder.AppendFormat("\"word2\": \"{0}\",\n", word2);
                    jsonBuilder.AppendFormat("\"start2\": {0},\n", start2);
                    jsonBuilder.AppendFormat("\"color\": \"{0}\"\n", color);
                    jsonBuilder.Append("}");

                    if (i != lines.Length - 1)
                    {
                        jsonBuilder.Append(",\n");
                    }
                }
            }
            jsonBuilder.Append("\n]");

            // 将 JSON 数据写入文件
            File.WriteAllText("Relations.json", jsonBuilder.ToString());

            MessageBox.Show("已导出");
        }
        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[\n");
            string[] lines = GetTxtData("words.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split('\t');
                if (parts.Length == 6)
                {
                    int id = int.Parse(parts[0]);
                    string word = parts[1];
                    string label = parts[2];
                    int start = int.Parse(parts[3]);
                    int end = int.Parse(parts[4]);
                    string color = parts[5];

                    jsonBuilder.Append("{\n");
                    jsonBuilder.AppendFormat("\"id\": {0},\n", id);
                    jsonBuilder.AppendFormat("\"word\": \"{0}\",\n", word);
                    jsonBuilder.AppendFormat("\"label\": \"{0}\",\n", label);
                    jsonBuilder.AppendFormat("\"start\": {0},\n", start);
                    jsonBuilder.AppendFormat("\"end\": {0},\n", end);
                    jsonBuilder.AppendFormat("\"color\": \"{0}\"\n", color);
                    jsonBuilder.Append("}");

                    if (i != lines.Length - 1)
                    {
                        jsonBuilder.Append(",\n");
                    }
                }
            }
            jsonBuilder.Append("\n]");
            //Console.WriteLine(jsonBuilder.ToString());
            File.WriteAllText("Labels.json", jsonBuilder.ToString());
            MessageBox.Show("已导出");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string word1 = textBox5.Text; //词
            string word2 = textBox7.Text;
            string relation = comboBox2.SelectedItem?.ToString(); //属性

            if (string.IsNullOrEmpty(word1)|| string.IsNullOrEmpty(word2) || string.IsNullOrEmpty(relation))
            {
                MessageBox.Show("请填写词语和属性。");
                return;
            }

            // 输出选定文本的起始位置和长度

            string crelationsFilePath = "Resource\\data\\c_relations.txt";
            string relationsFilePath = "Resource\\data\\relations.txt";
            relationsFilePath = Path.Combine(Application.StartupPath, relationsFilePath);
            int rowCount = File.ReadLines(relationsFilePath).Count();
            string[] crelationsLines = File.ReadAllLines(Path.Combine(Application.StartupPath, crelationsFilePath));
            string color = null; // 用于存储颜色

            foreach (string line in crelationsLines)
            {
                //候选词
                string[] parts = line.Split('\t');
                if (parts[1] == relation)
                {
                    color = parts[2];
                    break;
                }
            }
            if (color == null)
            {
                MessageBox.Show("未找到匹配的关系。");
                return;
            }

            string[] relations = GetTxtData("relations.txt");
            foreach (string line in relations)
            {
                string[] s = line.Split('\t');
                if (s.Length > 3)
                {
                    if (s[2] == textBox5.Text && s[4] == textBox7.Text && s[3] == textBox4.Text && s[5]==textBox6.Text)
                    {
                        MessageBox.Show("该关系已存在");
                        return;
                    }

                }
            }
            UpdateFileIDColumn("relations.txt");
            string newData = $"{rowCount + 1}\t{relation}\t{word1}\t{textBox4.Text}\t{word2}\t{textBox6.Text}\t{color}";
            File.AppendAllText(relationsFilePath, newData + Environment.NewLine);
            MessageBox.Show("添加" + relation + "成功");
            LoadData();
        }

        private void LoadDataToBottomBar(string type)
        {
            string[] result = new string[14];
            string[] lines = GetTxtData("c_"+type+"s.txt");
            int maxlen = lines.Length - 1;
            for (int i = 0; i < 14; i++)
            {
                if (i <= maxlen)
                {
                    string[] ls = lines[i].Split('\t');
                    result[i] = ls[1];
                }
                else result[i] = "";
            }
            //这个时候就获得了ls, 里面装的都是空字符或者需要显示的词
            var labels = new List<Label> { label7, label8, label9,
            label10, label11, label12,
            label13, label14, label15,
            label16, label17, label18, label19, label20};
            int index = 0;
            foreach (Label lb in labels)
            {
                lb.Text = result[index++];
            }
        }

        private void 启用快捷标注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!quick_anno) quick_anno = true;
            else quick_anno = false;
            Console.WriteLine(quick_anno);
        }
        private void Label_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            if (clickedLabel != null && !string.IsNullOrEmpty(clickedLabel.Text) && quick_anno)
            {
                candidate_label = clickedLabel.Text;
                if (textBox1.Text != string.Empty) comboBox1.Text = candidate_label;
                else if (textBox5.Text != string.Empty && textBox7.Text != string.Empty) comboBox2.Text = candidate_label;
                // 在这里执行其他需要的操作
            }
            if (textBox1.Text != string.Empty)
            {
                button1.PerformClick();
            }
            else if (textBox5.Text != string.Empty && textBox7.Text != string.Empty)
            {
                button2.PerformClick();
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void Labels_Clicks()
        {
            label7.Click += Label_Click;
            label8.Click += Label_Click;
            label9.Click += Label_Click;
            label10.Click += Label_Click;
            label11.Click += Label_Click;
            label12.Click += Label_Click;
            label13.Click += Label_Click;
            label14.Click += Label_Click;
            label15.Click += Label_Click;
            label16.Click += Label_Click;
            label17.Click += Label_Click;
            label18.Click += Label_Click;
            label19.Click += Label_Click;
            label20.Click += Label_Click;
        }
        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
        }

        private void label18_Click(object sender, EventArgs e)
        {
        }

        private void label19_Click(object sender, EventArgs e)
        {
        }

        private void label20_Click(object sender, EventArgs e)
        {
        }

        private void 功能测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.ShowDialog();
        }
        public void DrawTextFromFile(string filePath, Graphics g, Panel panel1)
        {
            g = panel1.CreateGraphics();
            Font font = new Font("宋体", 12);
            Brush brush = Brushes.Black;
            float lineHeight = 1.5f; // 行间距倍数
            float y = 0; // 初始 Y 坐标
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            string[] lines = GetTxtData("text.txt");
            foreach (string line in lines)
            {
                g.DrawString(line, font, brush, new PointF(0, y));
                y += font.Height * lineHeight;
            }
            g.Dispose();
        }

        private int FindWordIndex(string word, int start)
        {
            int index = 0;
            int end = word.Length + start;
            string text = File.ReadAllText("Resource\\data\\text.txt");
            int currentIndex = -1;
            while ((currentIndex = text.IndexOf(word, currentIndex + 1)) >= 0)
            {
                if (currentIndex <= start)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            return index;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            button4.PerformClick();
        }
    }
}
