using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKY
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            //abb
            Grammar.Columns.Add("1", "1");
            Grammar.Columns.Add("2", "-");
            Grammar.Columns.Add("3", "2");
            Grammar.Columns[0].Width = 25;
            Grammar.Columns[1].Width = 25;
            Grammar.Columns[2].Width = 25;
            Grammar.Rows.Add("S", "->", "AB");
            Grammar.Rows.Add("A", "->", "CD");
            Grammar.Rows.Add("A", "->", "CF");
            //Grammar.Rows.Add("A", "->", "AD");
            Grammar.Rows.Add("B", "->", "c");
            Grammar.Rows.Add("B", "->", "EB");
            Grammar.Rows.Add("C", "->", "a");
            Grammar.Rows.Add("D", "->", "b");
            Grammar.Rows.Add("E", "->", "c");
            Grammar.Rows.Add("F", "->", "AD");
            //Grammar.Rows.Add("F", "->", "DD");
            //Grammar.Rows.Add("K", "->", "CF");
        }
        public int kol = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            kol++;
            if (kol>1)
            {
                Form1_Load(sender, e);
            }
            if (textBox1.Text != "")
            {
                string input = textBox1.Text;
                string[][] matrix = new string[input.Length][];
                int k = 0;
                for (int i = input.Length; i > 0; --i)
                {
                    matrix[k] = new string[i];
                    for (int j = 0; j < matrix[k].Length; ++j)
                    {
                        matrix[k][j] = "";
                    }
                    k++;
                }
                for (int i = 0; i < input.Length; ++i)
                {
                    dataGridViewResult.Columns.Add($"{i + 1}", input[i].ToString());
                    dataGridViewResult.Columns[i].Width = 50;
                    dataGridViewResult.Rows.Add();
                }
                MainAlgorythm(ref matrix, input, this);
                k = 0;
                for (int i = 0; i < matrix.Length; ++i)
                {
                    for (int j = 0; j < matrix[k].Length; ++j)
                    {
                        dataGridViewResult[j, i].Value = matrix[k][j];
                    }
                    k++;
                }
                for (int i = 0; i < matrix.Length; ++i)
                {
                    for (int j = 0; j < matrix.Length; ++j)
                    {
                        if (dataGridViewResult[j, i].Value == null)
                        {
                            dataGridViewResult[j, i].Style.BackColor=Color.Transparent;
                        }

                    }
                }
                Regex r = new Regex("S");
                if (r.IsMatch(dataGridViewResult[0, input.Length-1].Value.ToString()))
                {
                    labelAns.Text = "В заданной контекстно-свободной грамматике \n можно вывести заданную строку";
                }
                else
                {
                    labelAns.Text = "В заданной контекстно-свободной грамматике \n нельзя вывести заданную строку";
                }
            }
            else
            {
                MessageBox.Show("Введите входящую строку");
            }
        }

        public static void MainAlgorythm(ref string[][] matrix, string input, Form1 f)
        {
            int k = 0;
            int x1, y1, x2, y2;
            for (int i = 0; i < matrix.Length; i++)
            {
                x1 = 0;
                x2 = i - 1;
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    y1 = j;
                    y2 = j + 1;
                    if (x2 <= -1)
                    {
                        x1 = 0;
                        x2 = i - 1;
                    }
                    if (i == 0)
                    {
                        matrix[k][j] = FirstLine(input[j].ToString(), f);
                    }
                    else
                    {
                        while (x1 != i)
                        {
                            Change(ref matrix[k][j], matrix[x1][y1], matrix[x2][y2], f);
                            x1++;
                            x2--;
                            y2++;
                        }
                    }
                }
                k++;
            }
        }

        public static string FirstLine(string First, Form1 f)
        {
            string ans = "";
            string g;
            string[] Terminals = First.Split(',');
            for (int i = 0; i < Terminals.Length; ++i)
            {
                for (int j = 0; j < f.Grammar.Rows.Count - 1; ++j)
                {
                    g = f.Grammar[2, j].Value.ToString();
                    if (g == Terminals[i])
                    {
                        if (ans.Length > 0)
                        {
                            ans += ",";
                        }
                        ans += f.Grammar[0, j].Value.ToString();
                    }
                }
            }
            return ans;
        }

        public static void Change(ref string before, string First, string Sec, Form1 f)
        {
            string symb;
            string[] Terminals1 = First.Split(',');
            string[] Terminals2 = Sec.Split(',');
            foreach (var t1 in Terminals1)
            {
                foreach (var t2 in Terminals2)
                {
                    symb = t1 + t2;
                    for (int j = 0; j < f.Grammar.Rows.Count - 1; ++j)
                    { 
                        if (symb == f.Grammar[2, j].Value.ToString())
                        {
                            if (before.Length > 0)
                            {
                                before += ",";
                            }
                            before += f.Grammar[0, j].Value.ToString();
                        }
                    }
                }
            }
        }
    }
}
