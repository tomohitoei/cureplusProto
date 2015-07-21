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
        private System.Drawing.Font _font = null;
        private Image _dummy = null;
        private Graphics _measure = null;

        public MyTextRenderer(string fontName, int fontSize)
        {
            _font = new System.Drawing.Font(fontName, fontSize);
            _dummy = new Bitmap(2560, 2560);
            _measure = Graphics.FromImage(_dummy);
            _measure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public Image[] hoge(string moji)
        {
            var il = new List<Image>();
            float total = 0.0f;
            float height = 0.0f;
            for (int i = 0; i < moji.Length; i++)
            {
                string c = moji[i].ToString();
                var sr = TextRenderer.MeasureText(_measure, c, _font, new Size(1024, 1024), TextFormatFlags.NoPadding);
                total += sr.Width;
                height = Math.Max(height, sr.Height);
                //var sr = TextRenderer.MeasureText(_measure, c, _font, new Size(1024, 1024));
                var bmp = new Bitmap((int)sr.Width+1, (int)sr.Height+1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillRectangle(new SolidBrush(Color.FromArgb(254, 255, 255)), -1, -1, sr.Width + 2, sr.Height + 2);
                TextRenderer.DrawText(g, c, _font, new Point(0, 0), Color.FromArgb(0, 0, 0), TextFormatFlags.NoPadding);
                //TextRenderer.DrawText(g, c, _font, new Point(0, 0), Color.FromArgb(0, 0, 0));
                //g.DrawRectangle(Pens.Red, 0, 0, sr.Width, sr.Height);
                bmp.MakeTransparent(Color.FromArgb(254,255,255));
                il.Add(bmp);
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
    }
}
