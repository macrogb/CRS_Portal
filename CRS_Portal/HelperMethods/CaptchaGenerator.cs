using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.HelperMethods
{
    public class CaptchaGenerator
    {
        public string captchaString { get; private set; }
        public string capImage { get; private set; }

        public void GenerateCaptcha()
        {
            try
            {
                int x, y;
                string strKey = null;
                Random rnd = new Random();

                strKey = GenerateString(5);

                captchaString = strKey;
                Font font = new Font("Bookman Old Style", (float)rnd.Next(18, 24), FontStyle.Italic);
                Bitmap bitmap = new Bitmap(250, 50);
                Graphics gr = Graphics.FromImage(bitmap);
                //Color black = Color.Black;
                //Color line = Color.FromArgb(165, 68, 68);
                Color line = Color.FromArgb(31, 29, 29);
                SolidBrush brush = new SolidBrush(line);

                Color bgcolor = Color.FromArgb(0, 170, 185);
                SolidBrush bgbrush = new SolidBrush(bgcolor);

                gr.FillRectangle(bgbrush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                for (x = 0; x < bitmap.Width; x++)
                    for (y = 0; y < bitmap.Height; y++)
                        if (rnd.Next(3) == 1)
                            bitmap.SetPixel(x, y, Color.FromArgb(216, 216, 216));

                gr.DrawString(strKey, font, brush, (float)rnd.Next(120), (float)rnd.Next(20));
                gr.Dispose();

                MemoryStream s = new MemoryStream();
                bitmap.Save(s, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = s.ToArray();
                capImage = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Captcha Generation failed, Please retry", ex);
            }

        }

        public static string GenerateString(int length)
        {
            string validatingText = String.Empty;
            Int32 seed = new Int32();
            Random rnd = new Random();

            for (int a = 0; a < length; a++)
            {
                do
                {
                    seed = rnd.Next(0, 61);
                }
                while (seed == 0 || seed == 1 || seed == 18 || seed == 24 || seed == 44 || seed == 50);
                if (seed < 10)
                    seed += 48;
                else if (seed < 36)
                    seed += 55;
                else
                    seed += 61;
                char chr = (char)seed;
                validatingText += chr.ToString();
            }
            return validatingText;
        }
    }
}
