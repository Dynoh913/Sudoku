using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SDK
{
    public partial class Form1 : Form
    {
        public int n = 9;
        public Button[][] b;
        public int sz = 45;
        struct current
        {
            public int x;
            public int y;
        }
        current acurrent = new current();
        //Boolean start = false;
        public Form1()
        {
            InitializeComponent();
        }



        #region[FormLoad]
        private void Form1_Load(object sender, EventArgs e)
        {
            createSudoku();
            acurrent.x = -1;
            acurrent.y = -1;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            buttonInput_KeyDown(sender, e);

        }
        #endregion
        #region[createSDK]
        public void createSudoku()
        {
            b = new Button[n][];
            for (int i = 0; i < n; i++)
            {
                b[i] = new Button[n];
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    b[i][j] = new Button();
                    b[i][j].Size = new Size(sz, sz);
                    if (i < 3 && j < 3)
                    {
                        b[i][j].BackColor = Color.FromName("Cornsilk");
                    }
                    else if (i >= 3 && i < 6 && j >= 3 && j < 6)
                    {
                        b[i][j].BackColor = Color.FromName("Cornsilk");
                    }
                    else if (i >= 6 && i < 9 && j >= 6 && j < 9)
                    {
                        b[i][j].BackColor = Color.FromName("Cornsilk");
                    }
                    else if (i >= 6 && i < 9 && j < 3)
                    {
                        b[i][j].BackColor = Color.FromName("Cornsilk");
                    }
                    else if (i >= 6 && i < 9 && j >= 6)
                    {
                        b[i][j].BackColor = Color.FromName("Cornsilk");
                    }
                    else if (i < 3 && j >= 6 && j < 9)
                    {
                        b[i][j].BackColor = Color.FromName("Cornsilk");
                    }
                    else
                    {
                        b[i][j].BackColor = Color.White;
                    }
                    b[i][j].Text = " ";
                    b[i][j].ForeColor = Color.FromName("red");
                    b[i][j].Location = new Point(i * sz + sz, j * sz + sz);
                    b[i][j].Click += new EventHandler(button_Click);
                    groupBox1.Controls.Add(b[i][j]);
                }
            }
        }
        #endregion
        #region[buttonClick]
        private void buttonInput_Click(object sender, EventArgs e)
        {
            int x = acurrent.x;
            int y = acurrent.y;
            if (x >= 0 && y >= 0 && x <= 8 && y <= 8)
            {
                Button bt = (Button)sender;
                if (bt.Text != "0")
                {
                    b[x][y].Text = bt.Text;
                }
                else
                {
                    b[x][y].Text = " ";
                }
                acurrent.x = -1;
                acurrent.y = -1;
            }
        }
        private void buttonInput_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            int x = acurrent.x;
            int y = acurrent.y;
            if (x >= 0 && y >= 0 && x <= 8 && y <= 8)
            {
                string text = e.KeyData.ToString();
                int result = checkKeyData(text);
                if (result >= 0 && result <= 9)
                {
                    if (result != 0)
                    {
                        b[x][y].Text = result.ToString();
                    }
                    else
                    {
                        b[x][y].Text = " ";
                    }
                    acurrent.x = -1;
                    acurrent.y = -1;
                }
            }
        }
        
        private void button_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            Button bt = (Button)sender;
            //bt.Text = (int.Parse(bt.Text) + 1).ToString();
            int x = int.Parse(bt.Location.X.ToString());
            x = x / 45 - 1;
            int y = int.Parse(bt.Location.Y.ToString());
            y = y / 45 - 1;
            acurrent.x = x;
            acurrent.y = y;

        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            acurrent.x = -1;
            acurrent.y = -1;
            Import();
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            acurrent.x = -1;
            acurrent.y = -1;
            Clear();
        }
        private void buttonExport_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            acurrent.x = -1;
            acurrent.y = -1;
            Export();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region[Solve]
        private void Solve_Click(object sender, EventArgs e)
        {
            acurrent.x = -1;
            acurrent.y = -1;
            if (checkInput() == false)
            {
                MessageBox.Show("Vui lòng kiểm tra lại đề đã nhập!");

            }
            else
            {
                //start = true;
                int[,] arr = new int[9, 9];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (b[i][j].Text != " ")
                        {
                            b[i][j].ForeColor = Color.FromName("Black");

                        }
                        if (b[i][j].Text != " ")
                        {
                            arr[i, j] = int.Parse(b[i][j].Text);
                        }
                        else
                        {
                            arr[i, j] = 0;
                        }
                        b[i][j].Click -= new EventHandler(button_Click);
                    }
                }
                int[,] arr2 = arr;
                if (solve(arr, 0, 0))
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (arr[i, j].ToString() != "0")
                            {
                                b[i][j].Text = arr[i, j].ToString();
                            }
                            else
                            {
                                b[i][j].Text = " ";
                            }
                        }
                    }
                }
                Solve.Enabled = false;
            }
        }
        public static Boolean solve(int[,] arr, int row, int col)
        {
            if (row < 9 && col < 9)
            {
                if (arr[row, col] != 0)
                {
                    if ((col + 1) < 9) return solve(arr, row, col + 1);
                    else if ((row + 1) < 9) return solve(arr, row + 1, 0);
                    else return true;
                }
                else
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        if (IsAvailable(arr, row, col, i + 1))
                        {
                            arr[row, col] = i + 1;

                            if ((col + 1) < 9)
                            {
                                if (solve(arr, row, col + 1)) return true;
                                else arr[row, col] = 0;
                            }
                            else if ((row + 1) < 9)
                            {
                                if (solve(arr, row + 1, 0)) return true;
                                else arr[row, col] = 0;
                            }
                            else return true;
                        }
                    }
                }

                return false;
            }
            else return true;
        }
        #endregion
        #region[CheckAvailabe]
        private static bool IsAvailable(int[,] puzzle, int row, int col, int num)
        {
            int rowStart = (row / 3) * 3;
            int colStart = (col / 3) * 3;

            for (int i = 0; i < 9; ++i)
            {
                if (puzzle[row, i] == num) return false;
                if (puzzle[i, col] == num) return false;
                if (puzzle[rowStart + (i % 3), colStart + (i / 3)] == num) return false;
            }

            return true;
        }
        #endregion
        #region[Checkinput]
        public Boolean checkInput() //check đề nhập vào có trùng cột, hàng, trong ô hay không.
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (b[i][j].Text != " ")
                    {
                        for (int k = i + 1; k < 9; ++k)  // kiem tra theo hang
                        {
                            if (b[k][j].Text != " ")
                            {
                                if (int.Parse(b[i][j].Text) == int.Parse(b[k][j].Text))
                                    return false;
                            }
                        }
                        for (int k = 0; k < i; ++k)  // kiem tra theo hang
                        {
                            if (b[k][j].Text != " ")
                            {
                                if (int.Parse(b[i][j].Text) == int.Parse(b[k][j].Text))
                                    return false;
                            }
                        }
                        for (int k = j + 1; k < 9; ++k)  // kiem tra theo hang
                        {
                            if (b[i][k].Text != " ")
                            {
                                if (int.Parse(b[i][j].Text) == int.Parse(b[i][k].Text))
                                    return false;
                            }
                        }
                        for (int k = 0; k < j; ++k)  // kiem tra theo hang
                        {
                            if (b[i][k].Text != " ")
                            {
                                if (int.Parse(b[i][j].Text) == int.Parse(b[i][k].Text))
                                    return false;
                            }
                        }
                        int boxRowOffset = (i / 3) * 3;
                        int boxColOffset = (j / 3) * 3;

                        for (int k = 0; k < 3; ++k) //kiem tra trong 9 ô nhỏ
                            for (int m = 0; m < 3; ++m)
                                if ((boxRowOffset + k) != i && boxColOffset + m != j)
                                {
                                    if (b[boxRowOffset + k][boxColOffset + m].Text != " ")
                                    {
                                        if (int.Parse(b[i][j].Text) == int.Parse(b[boxRowOffset + k][boxColOffset + m].Text))
                                            return false;
                                    }
                                }
                    }
                }
            }
            return true;
        }
        #endregion
        #region[Import]     
        private void Import()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "txt file|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                Clear();
                string filename = op.FileName;
                label1.Text = "";
                string[] filelines = File.ReadAllLines(filename);
                if (filelines.Length == 9)
                {
                    for (int j = 0; j < filelines.Length; j++)
                    {
                        string[] splitLines = filelines[j].Split(' ');
                        if (splitLines.Length == 9)
                        {
                            for (int i = 0; i < splitLines.Length; i++)
                            {
                                if (int.Parse(splitLines[i]) >= 0 && int.Parse(splitLines[i]) <= 9)
                                {
                                    if (int.Parse(splitLines[i]) == 0)
                                    {
                                        b[i][j].Text = " ";
                                        b[i][j].ForeColor = Color.FromName("red");
                                    }
                                    else
                                    {
                                        b[i][j].Text = splitLines[i];
                                        b[i][j].ForeColor = Color.FromName("black");
                                        b[i][j].Click -= new EventHandler(button_Click);
                                    }
                                }
                                else
                                {
                                    label1.Text = "Error Reading File";
                                    Clear();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            label1.Text = "Error Reading File";
                            Clear();
                            return;
                        }
                    }
                }
                else
                {
                    label1.Text = "Error Reading File";
                    Clear();
                    return;
                }
            }
            else
            {

            }
        }
        #endregion
        #region[Reset]
        private void Clear()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    b[i][j].Text = " ";
                    b[i][j].ForeColor = Color.FromName("red");
                    b[i][j].Click -= new EventHandler(button_Click);
                    b[i][j].Click += new EventHandler(button_Click);
                }
            }
            Solve.Enabled = true;
        }
        #endregion
        #region[Export]
        private void Export()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;
                string[] contents = new string[9];
                for (int j = 0; j < 9; j++)
                {
                    contents[j] = "";
                    for (int i = 0; i < 9; i++)
                    {
                        string anum = "";
                        if (b[i][j].Text == " ")
                        {
                            anum = "0";
                        }
                        else
                        {
                            anum = b[i][j].Text;
                        }
                        if (i < 8)
                        {
                            contents[j] += anum + " ";
                        }
                        else
                        {
                            contents[j] += anum;
                        }
                    }
                }
                File.WriteAllLines(filename, contents);
            }
        }
        #endregion
        

        
        private int checkKeyData(string data)
        {
            switch (data)
            {
                case "D1":
                case "NumPad1": return 1;
                case "D2":
                case "NumPad2": return 2;
                case "D3":
                case "NumPad3": return 3;
                case "D4":
                case "NumPad4": return 4;
                case "D5":
                case "NumPad5": return 5;
                case "D6":
                case "NumPad6": return 6;
                case "D7":
                case "NumPad7": return 7;
                case "D8":
                case "NumPad8": return 8;
                case "D9":
                case "NumPad9": return 9;
                case "D0":
                case "NumPad0":
                case "Back":
                case "Delete": return 0;
                default: return -1;
            }
        }
        


    }
}


