using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Form_Label.Form8;

namespace Form_Label
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x = int.Parse(textBox1.Text);
            int y  = int.Parse(textBox2.Text);
            label1.Location = new Point(x, y);
            
        }
        private void connectLabels()
        {

        }
        public void DrawTextFromFile(string filePath, Graphics g, Panel panel1)
        {
            try
            {
                // 从文件中读取文本内容
                string text = File.ReadAllText(filePath);

                // 创建字体和画刷
                Font font = new Font("Arial", 12);
                Brush brush = Brushes.Black;

                // 创建要绘制的区域
                RectangleF rect = new RectangleF(0, 0, panel1.Width, panel1.Height);

                // 清空面板
                panel1.Controls.Clear();

                // 创建一个 Label 控件，用于显示文本
                Label label = new Label();
                label.Text = text;
                label.Font = font;
                label.ForeColor = Color.Black;
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.TopLeft;
                label.Dock = DockStyle.Fill;

                // 添加 Label 控件到 Panel 上
                panel1.Controls.Add(label);

                // 注册 Panel 的 Paint 事件处理程序
                panel1.Paint += (sender, e) =>
                {
                    // 在 Panel 上绘制文本
                    label.DrawToBitmap(new Bitmap(panel1.Width, panel1.Height), panel1.ClientRectangle);
                };

                // 刷新 Panel
                panel1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法加载文件: {ex.Message}");
            }
        }
        private void Form8_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;


        }
        string path = "Resource\\data\\text.txt";
        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Pen pen = new Pen(Color.Green, 2);
            Point[] points = new Point[]
            {
                new Point(label1.Left, label1.Top),
                new Point((label1.Left+label2.Left)/2+10, (label1.Top+label2.Top)/2+10),
                new Point(label2.Left, label2.Top)
            };
            DrawTextFromFile(path, g, panel1);
            g.DrawCurve(pen, points,float.Parse(textBox3.Text));
            pen.Dispose();
            g.Dispose();
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
        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Font font = new Font("宋体", 12);
            Brush brush = Brushes.Black;
            float lineHeight = 1.5f; // 行间距倍数
            float y = 0; // 初始 Y 坐标
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            string[] lines = GetTxtData("text.txt");
            foreach(string line in lines)
            {
                g.DrawString(line, font, brush, new PointF(0, y));
                y += font.Height * lineHeight;
            }
            g.Dispose ();
        }
    }
}
