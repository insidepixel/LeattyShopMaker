using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace LeattyMapleShopMaker
{
    public partial class Form1 : Form
    {
        //Storing items read from item.txt
        ArrayList results = new ArrayList();
        //Data for Author
        string NPCID;
        string NPCName;
        string NPCDescription;
        string AuthorName;
        //Data for Items
        string ItemID;
        string ItemName;
        string MesoCost;
        string ItemTab;
        string Quantity;
        string MaxBuy;
        string Currency;
        string CurrCost;
        string BulletAmount;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)13)
            {
                string line;
                results.Clear();
                cbResults.Items.Clear();
                string ItemListPath = Application.StartupPath + @"\Items.txt";
                if (File.Exists(ItemListPath))
                {
                    using (StreamReader sr = new StreamReader(ItemListPath))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.ToString().ToLower().Contains(tbSearch.Text.ToLower()))
                            {
                                cbResults.Items.Add(line.ToString().Remove(line.ToString().IndexOf(char.Parse("|"))));
                                results.Add(line.ToString().Substring(line.ToString().IndexOf(char.Parse("|")), line.ToString().Length - line.ToString().IndexOf(char.Parse("|"))));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Could not find item list file.\nPut it in the same folder as this .exe.", "Unable to find text file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            ItemID = tbItemID.Text;
            ItemName = tbItemName.Text;
            MesoCost = tbMesoCost.Text;
            ItemTab = tbItemTab.Text;
            Quantity = tbQuantity.Text;
            MaxBuy = tbMaxBuy.Text;
            Currency = tbCurrency.Text;
            CurrCost = tbCurrAmount.Text;
            BulletAmount = tbBulletAmount.Text;
            if (checkBox1.Checked == true)
            {
                MesoCost = "0";
            }
            else
            {
                Currency = "0";
                CurrCost = "0";
            }
            if (tbBulletAmount.Text.Length == 0)
            {
                BulletAmount = "0";
            }
            if (ItemID.Length > 0 && ItemName.Length > 0 && MesoCost.Length > 0 && ItemTab.Length > 0 && Quantity.Length > 0 && MaxBuy.Length > 0 && Currency.Length > 0 && CurrCost.Length > 0)
            {
                dataGridView1.Rows.Add(ItemID, ItemName, MesoCost, ItemTab, Currency, CurrCost, Quantity, MaxBuy, BulletAmount);
            }
            else
            {
                MessageBox.Show("You forgot some required item fields\nAll enabled text boxes must be filled","Missing information",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void cbResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbItemName.Text = cbResults.SelectedItem.ToString();
            tbItemID.Text = results[cbResults.SelectedIndex].ToString().TrimStart(char.Parse("|"));
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            NPCID = tbNPCID.Text;
            NPCName = tbNPCName.Text;
            NPCDescription = tbNPCDesc.Text;
            AuthorName = tbAuthorName.Text;
            string newFilePath = Application.StartupPath + @"\Scripts\shop_" + NPCID + ".cs";
            if(File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }
            TextWriter newScript = File.CreateText(newFilePath);
            newScript.WriteLine("using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;");
            newScript.WriteLine("/*\n* Generated using CorruptedTNC's shop generator\n*/");
            newScript.WriteLine("namespace scripts.Shops");
            newScript.WriteLine("{");
            newScript.WriteLine("\t[Author(\"" + AuthorName + "\")]");
            newScript.WriteLine("\t[NPC(\"" + NPCName + "\",\"" + NPCDescription + "\")]");
            newScript.WriteLine("\tpublic class shop_" + NPCID + " : ShopScript");
            newScript.WriteLine("\t{");
            newScript.WriteLine("\t\tpublic override void Execute()");
            newScript.WriteLine("\t\t{");
            newScript.WriteLine("\t\t\tif (State == 0)");
            newScript.WriteLine("\t\t\t{");
            newScript.WriteLine("\t\t\t\tItemShop shop = new ItemShop();");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                newScript.WriteLine("\t\t\t\tshop.WriteItem(" + dataGridView1[0,i].Value + "," + dataGridView1[2,i].Value + "," + dataGridView1[3,i].Value + "," + dataGridView1[4,i].Value + "," + dataGridView1[5,i].Value + "," + dataGridView1[6,i].Value + "," + dataGridView1[7,i].Value + "," + dataGridView1[8,i].Value + "); //" + dataGridView1[1,i].Value);
            }
            newScript.WriteLine("\t\t\t\tthis.RunShopPacket(shop);");
            newScript.WriteLine("\t\t\t}");
            newScript.WriteLine("\t\t\telse if (State == 1)");
            newScript.WriteLine("\t\t\t{");
            newScript.WriteLine("\t\t\t\tbase.Execute();");
            newScript.WriteLine("\t\t\t}");
            newScript.WriteLine("\t\t}");
            newScript.WriteLine("\t}");
            newScript.WriteLine("}");
            newScript.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbCurrency.Enabled = checkBox1.Checked;
            tbCurrAmount.Enabled = checkBox1.Checked;
            tbMesoCost.Enabled = !checkBox1.Checked;
        }

        private void tsmiNewTable_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Should've watched the video, fill in the boxes.\nProper help window coming in later version.","Nope.",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        }

        private void tsmiCredits_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Leatty developers AloneVampire, Exalted and Yaminike - Creating the format for shop scripts.\nCorruptedTNC - Making this tool from scratch.","Credits",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}