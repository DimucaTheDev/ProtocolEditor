using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DimucaTheDev.ProtocolEditor
{
    public partial class Main : Form
    {
        public static List<string> regs = new List<string>() { };
        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "";
            regs = Registry.ClassesRoot.GetSubKeyNames().ToList();
            Program.Log("Form Started");
            Program.Log("Searching for already created protocols...");

            int count = 0;

            //Получаем каждый ключ(или как там)
            foreach (string name in regs.ToArray())
            {
                //мб попробуем получить ключи и если есть нужные то протокол
                try
                {
                    if (!(Registry.ClassesRoot.OpenSubKey($"{name}\\shell\\open\\command") is null) 
                       && !(Registry.ClassesRoot.OpenSubKey($"{name}").GetValue("URL Protocol") is null) 
                       //&& !(Registry.ClassesRoot?.OpenSubKey($"{name}\\shell\\open\\command")?.GetValue("")?.ToString().Length > 0)
                       )
                    {
                        regs.Add(name);
                        comboBox1.Items.Add(name);
                        Program.Log(name);
                        count++;
                    }

                }
                catch (Exception ex) { Program.Log(ex.Message); }
            }

            Program.Log($"Found {count} protocols");
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e) => Program.Log($"Form Closed({e.CloseReason})");
        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length > 0 && comboBox1.Text[0] != ' ')
                linkLabel1.Text = $"{comboBox1.Text}://arg1/arg2";
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(linkLabel1.Text);
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            linkLabel1.Text = $"{comboBox1.Text}://arg1/arg2";
        }
        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(sender, e);
            if (!regs.Contains(comboBox1.Text)) { removeB.Enabled = false; createB.Enabled = true; }
            if (regs.Contains(comboBox1.Text)) { removeB.Enabled = true; createB.Enabled = false; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Executable files |*.exe;*.com;*.bat;*.cmd";
            if(dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = dialog.FileName;
            else
                MessageBox.Show("File isnt selected!");
        }
        private void createB_Click(object sender, EventArgs e)
        {
            visibleL.Visible = true;
            createB.Enabled = false;

            var registry = Registry.ClassesRoot.CreateSubKey($"{comboBox1.Text}");
            var regShell = registry.CreateSubKey("shell");
            var regOpen = regShell.CreateSubKey("open");
            var regCommand = regOpen.CreateSubKey("command");

            registry.SetValue("URL Protocol", "");
            regCommand.SetValue("",$"\"{textBox1.Text}\" \"%1\"");

            visibleL.Visible = false;
            removeB.Enabled = true;
            refresh();
        }
        private void refresh()
        {
            regs = Registry.ClassesRoot.GetSubKeyNames().ToList();
            foreach (string name in regs.ToArray())
            {
                //мб попробуем получить ключи и если есть нужные то протокол
                try
                {
                    if (!(Registry.ClassesRoot.OpenSubKey($"{name}\\shell\\open\\command") is null)
                       && !(Registry.ClassesRoot.OpenSubKey($"{name}").GetValue("URL Protocol") is null)
                       //&& !(Registry.ClassesRoot?.OpenSubKey($"{name}\\shell\\open\\command")?.GetValue("")?.ToString().Length > 0)
                       )
                    {
                        regs.Add(name);
                        comboBox1.Items.Add(name);
                        Program.Log(name);
                    }

                }
                catch (Exception ex) { Program.Log(ex.Message); }
            }
        }
        private void removeB_Click(object sender, EventArgs e)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(comboBox1.Text);
            createB.Enabled = true;
            removeB.Enabled = false;
            refresh();
        }
    }
}
