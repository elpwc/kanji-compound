using System;
using System.Drawing;

namespace kanji_compound_test
{
    /// <summary>
    /// 图像操作
    /// </summary>
    public static class GraphicsUtil
    {
        /// <summary>
        /// 释放Bitmap内存
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private static Bitmap bmp;
        /// <summary>
        /// 画一个点
        /// </summary>
        /// <param name="g">要绘制的Graphics</param>
        /// <param name="c">颜色</param>
        /// <param name="site">点的位置~</param>
        /// <param name="width">宽度</param>
        /// <param name="type">类型：0 圆  1 方</param>
        public static void DrawDot(Graphics g, Color c, Point site, int width, int type = 0, bool border = false)
        {
            if (type == 0)
            {
                Rectangle t = new Rectangle(site.X - width / 2, site.Y - width / 2, width, width);
                g.FillEllipse(new SolidBrush(c), t);
                if (border)
                {
                    g.DrawEllipse(new Pen(Color.Black, 1), t);
                }

            }
            else if (type == 1)
            {
                Rectangle t = new Rectangle(site.X - width / 2,site.Y - width / 2, width, width);
                g.FillRectangle(new SolidBrush(c), t);
                if (border)
                {
                    g.DrawRectangle(new Pen(Color.Black, 1), t);
                }
            }
        }




        /// <summary>  
        /// 合并图片  
        /// </summary>  
        /// <param name="imgBack"></param>  
        /// <param name="img"></param>  
        /// <returns></returns>  
        public static void CombineImage(Image imgBack, Image img, out Image res, int xDeviation = 0, int yDeviation = 0)
        {
            bmp = new Bitmap(img.Width + xDeviation < imgBack.Width ? imgBack.Width : img.Width + xDeviation, img.Height + yDeviation < imgBack.Height ? imgBack.Height : img.Height + yDeviation);
            IntPtr h_bmp = bmp.GetHbitmap();
            {
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Transparent);
                g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
                g.DrawImage(img, xDeviation, yDeviation, img.Width, img.Height);
                g.Dispose();
                res = bmp;
                DeleteObject(h_bmp);
            }
        }



        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="b">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static void ResizeImage(Image b, int newW, int newH, out Image res, int mode = 0)
        {
            bmp = new Bitmap(newW, newH);
            IntPtr h_bmp = bmp.GetHbitmap();
            {
                Graphics g = Graphics.FromImage(bmp);

                // 插值算法的质量  
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(b, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
                g.Dispose();
                res = bmp;
                DeleteObject(h_bmp);
            }

        }



        /// <summary>
        /// 截取图片
        /// </summary>
        /// <param name="formerImage">来源图片</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public static void CaptureImage(Image formerImage, int offsetX, int offsetY, int width, int height, out Image res)
        {
            bmp = new Bitmap(width, height);
            IntPtr h_bmp = bmp.GetHbitmap();
            {
                Graphics graphic = Graphics.FromImage(bmp);
                graphic.DrawImage(formerImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
                graphic.Dispose();
                res = bmp;
                DeleteObject(h_bmp);
            }

        }

        /// <summary>
        /// 截取图片
        /// </summary>
        /// <param name="formerImage">来源图片</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public static void CaptureImage(Image formerImage, Graphics g, int offsetX, int offsetY, int width, int height)
        {
            g.DrawImage(formerImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
        }

    }


}
