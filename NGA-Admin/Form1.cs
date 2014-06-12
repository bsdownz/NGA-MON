using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using EveAI.Live;
using EveAI.Live.Account;
using EveAI.Live.Character;
using EveAI.Live.Corporation;
using System.Threading;
namespace NGA_Admin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int KeyID = int.Parse(recKeyID.Text);
            string vCode = recVCode.Text;
            playerMgm.addRecruit(KeyID, vCode);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = playerMgm.getRecruits();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText(playerMgm.getBasicInfo((AccountEntry)listBox1.SelectedItem));
            //listBox2.DataSource = playerMgm.retAssets();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.DataSource = null;
                listBox2.Items.Clear();
                listBox2.DataSource = playerMgm.retAssets();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
