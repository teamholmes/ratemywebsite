using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text.RegularExpressions;

namespace OP.General.Captcha
{
    /// <summary>
    /// Summary description for CaptchaImage.
    /// </summary>
    public class CaptchaImage
    {

        public string CaptchaForegroundTextColour = "#ffffff";

        public string CaptchBackgroundColour = "##1b4180"; 

        // Public properties (all read-only).
        public string Text
        {
            get { return this._Text; }
        }
        public Bitmap Image
        {
            get { return this._Image; }
        }
        public int Width
        {
            get { return this._Width; }
        }
        public int Height
        {
            get { return this._Height; }
        }

        // Internal properties.
        private string _Text;
        private int _Width;
        private int _Height;
        private string _FamilyName;
        private Bitmap _Image;

        // For generating random numbers.
        private Random _Random = new Random();

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width and height.
        // ====================================================================
        public CaptchaImage(string s, int width, int height)
        {
            this._Text = s;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width, height and font family.
        // ====================================================================
        public CaptchaImage(string s, int width, int height, string familyName)
        {
            this._Text = s;
            this.SetDimensions(width, height);
            this.SetFamilyName(familyName);
            this.GenerateImage();
        }

        // ====================================================================
        // This member overrides Object.Finalize.
        // ====================================================================
        ~CaptchaImage()
        {
            Dispose(false);
        }

        // ====================================================================
        // Releases all resources used by this object.
        // ====================================================================
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        // ====================================================================
        // Custom Dispose method to clean up unmanaged resources.
        // ====================================================================
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                this._Image.Dispose();
        }

        // ====================================================================
        // Sets the image width and height.
        // ====================================================================
        private void SetDimensions(int width, int height)
        {
            // Check the width and height.
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            this._Width = width;
            this._Height = height;
        }

        // ====================================================================
        // Sets the font used for the image text.
        // ====================================================================
        private void SetFamilyName(string familyName)
        {
            // If the named font is not installed, default to a system font.
            try
            {
                Font font = new Font(this._FamilyName, 11F);
                this._FamilyName = familyName;
                font.Dispose();
            }
            catch (Exception ex)
            {
                this._FamilyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }


        /// <summary>
        /// Extract only the hex digits from a string.
        /// </summary>
        public string ExtractHexDigits(string input)
        {
            // remove any characters that are not digits (like #)
            Regex isHexDigit
               = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
            string newnum = "";
            foreach (char c in input)
            {
                if (isHexDigit.IsMatch(c.ToString()))
                    newnum += c.ToString();
            }
            return newnum;
        }


        /// <summary>
        /// Convert a hex string to a .NET Color object.
        /// </summary>
        /// <param name="hexColor">a hex string: "FFFFFF", "#000000"</param>
        public Color HexStringToColor(string hexColor)
        {
            string hc = ExtractHexDigits(hexColor);
            if (hc.Length != 6)
            {
                // you can choose whether to throw an exception
                //throw new ArgumentException("hexColor is not exactly 6 digits.");
                return Color.Empty;
            }
            string r = hc.Substring(0, 2);
            string g = hc.Substring(2, 2);
            string b = hc.Substring(4, 2);
            Color color = Color.Empty;
            try
            {
                int ri
                   = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
                int gi
                   = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
                int bi
                   = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
                color = Color.FromArgb(ri, gi, bi);
            }
            catch
            {
                // you can choose whether to throw an exception
                //throw new ArgumentException("Conversion failed.");
                return Color.Empty;
            }
            return color;
        }


        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        private void GenerateImage()
        {

            
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(this._Width, this._Height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this._Width, this._Height);


            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, HexStringToColor(CaptchBackgroundColour), HexStringToColor(CaptchBackgroundColour));
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(this._FamilyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(this._Text, font);
            } while (size.Width > rect.Width);

            fontSize = fontSize - 2;
            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();

            int extraEM = 4;
            if (g.DpiX >= 120)
            {
                extraEM = 15;
            }

            // this lines writes out the text
            path.AddString(this._Text, font.FontFamily, (int)font.Style, font.Size + extraEM, rect, format);

            float v = 5F;
            PointF[] points =
			{
				new PointF(this._Random.Next(rect.Width) / v, this._Random.Next(rect.Height) / v),
				new PointF(rect.Width - this._Random.Next(rect.Width) / v, this._Random.Next(rect.Height) / v),
				new PointF(this._Random.Next(rect.Width) / v, rect.Height - this._Random.Next(rect.Height) / v ),
				new PointF(rect.Width - this._Random.Next(rect.Width) / v, rect.Height - this._Random.Next(rect.Height) / v)
			};
            Matrix matrix = new Matrix();
            matrix.Translate(3F, 3F);  // adjust this to change the warping
            path.Warp(points, rect, matrix, WarpMode.Perspective, 10F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, HexStringToColor(CaptchBackgroundColour), HexStringToColor(CaptchaForegroundTextColour));
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);

            for (int i = 0; i < (int)(rect.Width * rect.Height / 17F); i++) // increase the div number to make it easier to read
            {
                int x = this._Random.Next(rect.Width);
                int y = this._Random.Next(rect.Height);
                int w = this._Random.Next(m / 35);
                int h = this._Random.Next(m / 40);
                hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, HexStringToColor(CaptchBackgroundColour), HexStringToColor(CaptchaForegroundTextColour));
                g.FillEllipse(hatchBrush, x, y, w, h);

                //int x1 = this.random.Next(rect.Width);
                //int y1 = this.random.Next(rect.Height);
                //int w1 = this.random.Next(m / 35);
                //int h1 = this.random.Next(m / 40);
                //hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, HexStringToColor(red), HexStringToColor(red));
                //g.FillEllipse(hatchBrush, x1, y1, w1, h1);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            this._Image = bitmap;
        }
    }
}
