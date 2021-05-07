using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace kanji_compound_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image= Kanji.getKanji(textBox1.Text,crtFont,  crtColor);
        }
        Font crtFont = new Font("Arial", 50);
        Color crtColor = Color.Black;
        private void Button2_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            crtFont = fontDialog1.Font;
            crtColor = fontDialog1.Color;
            button2.Text = $"font...({crtFont.Name},{crtFont.Size.ToString()},{crtColor.ToString()})";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] func = { "⿰", "⿱", "⿲", "⿳", "⿴", "⿵", "⿶", "⿷", "⿸", "⿹", "⿺", "⿻", "↔", "↕" };
            button2.Text = $"font...({crtFont.Name},{crtFont.Size.ToString()},{crtColor.ToString()})";
            foreach (string f in func)
            {
                listView1.Items.Add(new ListViewItem(f));
            }
            
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text += listView1.SelectedItems[0].Text;
            }
            
        }
    }

    public class Kanji
    {
        private static List<string> func = new List<string>(){ "⿰", "⿱", "⿲", "⿳", "⿴", "⿵", "⿶", "⿷", "⿸", "⿹", "⿺", "⿻" ,"↔","↕"};
        private static int[] paracount = { 2, 2, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1 };



        static public Image getKanji(string str,Font font ,Color color)
        {
            Image tbmp = new Bitmap(500,500);
            Image res = new Bitmap(500, 500);
            string[] strs = Regex.Split(str, "\r\n");
            List<Profix[]> PRES = new List<Profix[]>();
            foreach (string s in strs)
            {
                PRES.Add(getProfix(s));
            }
            
            SizeF size = getCharSize("啊", font);
            int y = 0;
            foreach (Profix[] P in PRES)
            {
                int x = 0;
                foreach (Profix p in P)
                {
                    tbmp = getImage(p, font, color, Convert.ToInt32((size.Height * 0.9 + size.Width * 0.9) / 2));
                    GraphicsUtil.CombineImage(res, tbmp, out res, x * Convert.ToInt32((size.Height * 0.9 + size.Width * 0.9) / 2), y * Convert.ToInt32((size.Height * 0.9 + size.Width * 0.9) / 2));
                    x++;
                    res.Save(x.ToString() + "." + y.ToString() + ".bmp");
                }
                y++;
            }

            return res;
        }

        static private SizeF getCharSize(string text,Font font)
        {
            return Graphics.FromImage(new Bitmap(500, 500)).MeasureString(text, font);
        }

        static private Image getImage(Profix p, Font font, Color color, int w = 100)
        {
            Image tbmp = new Bitmap(w, w);
            if (p.isStr)
            {
                using (Graphics g = Graphics.FromImage(tbmp))
                {
                    g.DrawString(p.str, font, new SolidBrush(color), 0, 0);
                    
                }
                
            }
            else
            {
                Image[] tb;
                Image res1, res2, res3;
                tb = new Image[p.paranum];
                for (int i = 0; i < p.paranum; i++)
                {
                    tb[i] = getImage(p.profixes[i], font, color, w);
                }
                switch (p.operatr)
                {
                    case "⿰":
                        GraphicsUtil.ResizeImage(tb[0], w / 2, w, out res1);
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w, out res2);
                        
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, (int)(w*0.8/2), 0);
                        break;
                    case "⿱":
                        GraphicsUtil.ResizeImage(tb[0], w , w/2, out res1);
                        GraphicsUtil.ResizeImage(tb[1], w , w/2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, 0, (int)(w * 0.8 / 2));
                        break;
                    case "⿲":
                        GraphicsUtil.ResizeImage(tb[0], w / 3, w, out res1);
                        GraphicsUtil.ResizeImage(tb[1], w / 3, w, out res2);
                        GraphicsUtil.ResizeImage(tb[2], w / 3, w, out res3);
                        GraphicsUtil.CombineImage(res1, res2, out res1, w / 3, 0);
                        GraphicsUtil.CombineImage(res1, res3, out tbmp, w * 2 / 3, 0);
                        break;
                    case "⿳":
                        GraphicsUtil.ResizeImage(tb[0], w , w / 3, out res1);
                        GraphicsUtil.ResizeImage(tb[1], w , w / 3, out res2);
                        GraphicsUtil.ResizeImage(tb[2], w , w / 3, out res3);
                        GraphicsUtil.CombineImage(res1, res2, out res1, 0, w / 3);
                        GraphicsUtil.CombineImage(res1, res3, out tbmp, 0, w * 2 / 3);
                        break;
                    case "⿴":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], (int)(w / 1.8),(int)( w /1.8) , out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, (int)(w * 1.05/ 4), (int)(w * 0.8 / 4));
                        break;
                    case "⿵":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w / 2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, w / 4, w / 4);
                        break;
                    case "⿶":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w / 2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, w / 4, w / 4);
                        break;
                    case "⿷":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w / 2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, w / 4, w / 4);
                        break;
                    case "⿸":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w / 2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, w / 4, w / 4);
                        break;
                    case "⿹":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w / 2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, w / 4, w / 4);
                        break;
                    case "⿺":
                        res1 = tb[0];
                        GraphicsUtil.ResizeImage(tb[1], w / 2, w / 2, out res2);
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, w / 4, w / 4);
                        break;
                    case "⿻":
                        res1 = tb[0];
                        res2 = tb[1];
                        GraphicsUtil.CombineImage(res1, res2, out tbmp, 0, 0);
                        break;
                    case "↔":
                        tb[0].RotateFlip(RotateFlipType.RotateNoneFlipX);
                        tbmp = tb[0];
                        break;
                    case "↕":
                        tb[0].RotateFlip(RotateFlipType.Rotate180FlipY);
                        tbmp = tb[0];
                        break;
                    default:
                        break;
                }
            }
            return tbmp;
        }

        static private Profix[] getProfix(string str)
        {
            
            List<Profix> P = new List<Profix>();
            for (int i = 0; i < str.Length; i++)
            {
                P.Add(new Profix(str.Substring(i, 1)));
            }

            while (aruka(P).Count() != 0) 
            {
                int[] opindex = aruka(P);
                List<int> rmv = new List<int>();
                for (int i = 0; i < opindex.Count(); i++)
                {
                    int ti = opindex[i];
                    int paranum =paracount[ func.IndexOf(P[ti].str)];
                    Profix tp = new Profix();
                    
                    switch (paranum)
                    {
                        case 1:
                            tp = new Profix(P[ti].str,P[ti+1]);
                            P[ti] = tp;
                            rmv.Add(ti + 1);
                            break;
                        case 2:
                            tp = new Profix(P[ti].str, P[ti + 1], P[ti + 2]);
                            P[ti] = tp;
                            rmv.Add(ti + 1);
                            rmv.Add(ti + 2);
                            break;
                        case 3:
                            tp = new Profix(P[ti].str, P[ti + 1], P[ti + 2],P[ti + 3]);
                            P[ti] = tp;
                            rmv.Add(ti + 1);
                            rmv.Add(ti + 2);
                            rmv.Add(ti + 3);
                            break;
                        default:
                            break;
                    }
                }
                for (int i = rmv.Count() - 1; i >= 0; i--)
                {
                    P.RemoveAt(rmv[i]);
                }
            }

            return P.ToArray();
        }

        static private int[] aruka(List<Profix> P)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < P.Count; i++)
            {
                if (P[i].isStr && func.IndexOf(P[i].str) != -1)   
                {
                    switch (paracount[func.IndexOf(P[i].str)])
                    {
                        case 1:
                            if (soudesuka(P[i + 1]))
                            {
                                res.Add(i);
                            }
                            break;
                        case 2:
                            if (soudesuka(P[i + 1]) && soudesuka(P[i + 2]))
                            {
                                res.Add(i);
                            }
                            break;
                        case 3:
                            if (soudesuka(P[i + 1]) && soudesuka(P[i + 2]) && soudesuka(P[i + 3]))
                            {
                                res.Add(i);
                            }
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            return res.ToArray();
        }

        static private bool soudesuka(Profix p)
        {
            if (func.IndexOf(p.str) == -1 || p.isStr == false)
            {
                return true;
            }
            return false;
        }

    }

    public class Profix
    {
        public Profix() { }
        public Profix(string str)
        {
            this.str = str;
            isStr = true;
        }
        public Profix(string op, Profix p1)
        {
            operatr = op;
            profixes[0] = p1;
            paranum = 1;
            isStr = false;
        }
        public Profix(string op, Profix p1, Profix p2)
        {
            operatr = op;
            profixes[0] = p1;
            profixes[1] = p2;
            paranum = 2;
            isStr = false;
        }
        public Profix(string op, Profix p1, Profix p2, Profix p3)
        {
            operatr = op;
            profixes[0] = p1;
            profixes[1] = p2;
            profixes[2] = p3;
            paranum = 3;
            isStr = false;
        }
        public Profix[] profixes=new Profix[3];
        public int paranum = 2;
        public string operatr = "";
        public bool isStr = true;
        public string str="";
    }
}
