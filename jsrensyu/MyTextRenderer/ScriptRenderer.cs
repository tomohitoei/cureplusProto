using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MyTextRenderer
{
    namespace MojiRendererLib
    {
        public class ScriptRenderer
        {
            private System.Drawing.Font _font = new System.Drawing.Font("メイリオ", 64);
            private Image _dummy = null;
            private Graphics _measure = null;

            public ScriptRenderer()
            {
                _dummy = new Bitmap(256, 256);
                _measure = Graphics.FromImage(_dummy);
                _measure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }

            public Image[] hoge(string moji)
            {
                var il = new List<Image>();
                for (int i = 0; i < moji.Length; i++)
                {
                    string c = moji[i].ToString();
                    var sr = TextRenderer.MeasureText(_measure, c, _font, new Size(1024, 1024), TextFormatFlags.NoPadding);
                    var bmp = new Bitmap((int)sr.Width, (int)sr.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    var g = Graphics.FromImage(bmp);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    TextRenderer.DrawText(g, c, _font, new Point(0, 0), Color.FromArgb(0, 0, 0), TextFormatFlags.NoPadding);
                    var sr2 = g.MeasureString(moji, _font);
                    bmp.MakeTransparent();
                    il.Add(bmp);
                }
                return il.ToArray();
            }
        }
    }

}
