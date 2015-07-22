using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace HLRemoting
{
    public class MyTextRenderer
    {
        private float mp = 1.0f;

        private System.Drawing.Font _font = null;
        private Image _dummy = null;
        private Graphics _measure = null;

        public Dictionary<string, Image> EMoji = null;

        public MyTextRenderer(string fontName, int fontSize)
        {
            _font = new System.Drawing.Font(fontName, fontSize*mp);
            _dummy = new Bitmap(2560, 2560);
            _measure = Graphics.FromImage(_dummy);
            _measure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public Image[] GetGryphs(string moji)
        {
            var il = new List<Image>();
            float total = 0.0f;
            float height = 0.0f;
            for (int i = 0; i < moji.Length; i++)
            {
                string c = moji[i].ToString();
                // 絵文字指定の判定
                if (c.Equals("["))
                {
                    if (i < moji.Length - 1)
                    {
                        if (!moji[i + 1].Equals('[') && !moji[i + 1].Equals(']'))
                        {
                            for (int ii = i+1; ii < moji.Length; ii++)
                            {
                                if (moji[ii].Equals(']'))
                                {
                                    try
                                    {
                                        var name = moji.Substring(i + 1, ii - i-1);
                                        if (EMoji.ContainsKey(name))
                                        {
                                            Image ebmp = new Bitmap((int)(_font.Size*1.5), (int)(_font.Size*1.5), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                            var bg = Graphics.FromImage(ebmp);
                                            bg.DrawImage(EMoji[name], 0, (int)(_font.Size*0.5), _font.Size, _font.Size);
                                            il.Add(ebmp);
                                            i = ii ;
                                            goto label;
                                        }
                                    }catch(Exception)
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                }
                var sr = TextRenderer.MeasureText(_measure, c, _font, new Size(1024, 1024), TextFormatFlags.NoPadding);
                total += sr.Width;
                height = Math.Max(height, sr.Height);
                var bmp = new Bitmap((int)sr.Width+1, (int)sr.Height+1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillRectangle(new SolidBrush(Color.FromArgb(254, 255, 255)), -1, -1, sr.Width + 2, sr.Height + 2);
                TextRenderer.DrawText(g, c, _font, new Point(0, 0), Color.FromArgb(0, 0, 0), TextFormatFlags.NoPadding);
                bmp.MakeTransparent(Color.FromArgb(254,255,255));
                il.Add(bmp);

            label:
                var xx = i;
            }

            Image bbb = new Bitmap((int)total +1, (int)height +1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var ggg = Graphics.FromImage(bbb);
            float pos = 0.0f;
            for (int i = 0; i < moji.Length; i++)
            {
                string c = moji[i].ToString();
                var sr = TextRenderer.MeasureText(ggg, c, _font, new Size(1024, 1024), TextFormatFlags.NoPadding);
                ggg.DrawString(c, _font, Brushes.Black, pos, 0);
                pos += sr.Width;
            }

            return il.ToArray();
            //return new Image[]{bbb};
        }

        private System.Collections.Generic.Dictionary<int, Image> _imageDic = new Dictionary<int, Image>();

        public Image MakeImage(int key, string moji,int width,float spc)
        {
            if (0< key)
            {
                if (_imageDic.ContainsKey(key)) return _imageDic[key];
            }

            var targets = moji.Trim(new Char[] { '\r', '\n' });
            System.IO.StringReader sr = new System.IO.StringReader(targets);
            var lines = new List<List<Image>>();
            // 指定の幅を超えない行を作成
            for (; ; )
            {
                var buf = sr.ReadLine();
                if (null == buf) break;
                var il = GetGryphs(buf);

                var line = new List<Image>();
                if (il.Length == 0) goto nextLine;
                var ww = il[0].Width;
                line.Add(il[0]);
                for (int i = 1; i < il.Length; i++)
                {
                    if (ww + il[i].Width < width*mp)
                    {
                        line.Add(il[i]);
                        ww += il[i].Width;
                    }
                    else
                    {
                        lines.Add(line);
                        line = new List<Image>();
                        line.Add(il[i]);
                        ww = il[i].Width;
                    }
                }
            nextLine:
                lines.Add(line);
            }

            //
            Image ii = new Bitmap((int)(width*mp), (int)(_font.Size * (lines.Count + 0.5f) * spc));
            Graphics g = Graphics.FromImage(ii);
            //g.FillRectangle(System.Drawing.Brushes.LightBlue, 0, 0, ii.Width, ii.Height);
            for (int i = 0; i < lines.Count; i++)
            {
                var line=lines[i];
                var ww=0;
                for (int j=0;j<line.Count;j++)
                {
                    g.DrawImage(line[j], ww, i * _font.Size * spc);
                    ww += line[j].Width;
                }
            }
//                g.DrawString("メール本文", _font, Brushes.Black, 10, 10);
            if (0 < key)
            {
                _imageDic.Add(key,ii);
            }
            return ii;
        }
    }
}
