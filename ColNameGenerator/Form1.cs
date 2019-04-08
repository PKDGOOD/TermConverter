using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColNameGenerator
{
    public partial class Form1 : Form
    {
        string currentPath;
        public Form1()
        {
            InitializeComponent();
            currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input;
            input = tbInput.Text;

            tbOutput.Text = Generate(input);
        }

        public string Generate(string input)
        {
            string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + currentPath + "\\DB_vocab.accdb";
            string sql=null;
            string output = null;
            string[] words = input.Split(' ');

            int i = 0;
            bool fin = false;

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                try
                {
                    while (true)
                    {
                        sql = "SELECT [Output] FROM [TableVocab] WHERE [Input]= '" + words[i] + "'";
                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        object scalarValue = cmd.ExecuteScalar();

                        if(scalarValue==null)
                        {
                            MessageBox.Show("'" + words[i] + "' 은(는) 정의되지 않은 용어 입니다.\nDB에 추가해주세요.");
                            break;
                        }
                        else
                        {

                            output += (string)scalarValue;
                            output += '_';

                            i++;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { fin = true; }
                finally
                {
                    if (output != null)
                    {
                        output = output.Remove(output.Length - 1, 1);
                    }
                }
            }
            if(fin == true)
            {

                return output;
            }
            else
            {
                return null;
            }
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Enter)
                {
                    this.button1_Click(sender, e);
                }
        }
    }
}
