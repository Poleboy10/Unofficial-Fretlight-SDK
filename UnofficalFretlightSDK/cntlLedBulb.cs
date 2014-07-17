using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace cntlLedBulb
{
    /// <summary>
    /// Original code by Steve Marsh can be found on CodeProject website
    /// http://www.codeproject.com/Articles/114122/A-Simple-Vector-Based-LED-User-Control
    /// modified for use with Unoffical Fretlight SDK project
    /// </summary>
    public partial class LedBulb : Control
    {
        #region Public and Private Members

        private Color _color;
        private bool _on = true;
        private bool _fancy = true; // or simple circle
        private Color _reflectionColor = Color.FromArgb(180, 255, 255, 255);
        private Color[] _surroundColor = new Color[] { Color.FromArgb(0, 255, 255, 255) };
        private Timer _timer = new Timer();

        /// <summary>
        /// Gets or Sets the color of the LED light
        /// </summary>
        // [DefaultValue(typeof(Color), "153, 255, 54")] // org
        [DefaultValue(typeof(Color), "Red")]
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                this.ColorLight = ControlPaint.LightLight(_color);
                this.ColorDark = ControlPaint.DarkDark(_color);
                this.Invalidate();	// Redraw the control
            }
        }

        /// <summary>
        /// Light shade of the LED color used for gradient
        /// </summary>
        public Color ColorLight { get; set; }

        /// <summary>
        /// Dark shade of the LED color used for gradient
        /// </summary>
        public Color ColorDark { get; set; }

        /// <summary>
        /// Gets or Sets whether the light is turned on
        /// </summary>
        public bool On
        {
            get { return _on; }
            set { _on = value; this.Invalidate(); }
        }

        /// <summary>
        /// Use fancy gradients (looks great) vs fast render
        /// </summary>
        public bool Fancy
        {
            get { return _fancy; }
            set { _fancy = value; }
        }

        #endregion

        #region Constructor

        public LedBulb()
        {
            SetStyle(ControlStyles.DoubleBuffer
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.UserPaint
            | ControlStyles.SupportsTransparentBackColor, true);

            // this.Color = Color.FromArgb(255, 153, 255, 54); // org
            this.Color = Color.Red;
            _timer.Tick += new EventHandler(
                (object sender, EventArgs e) => { this.On = !this.On; }
            );
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Paint event for this UserControl
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Fancy)
            {
                // Create an offscreen graphics object for double buffering
                Bitmap offScreenBmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
                using (System.Drawing.Graphics g = Graphics.FromImage(offScreenBmp))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    // Draw the control
                    drawControl(g, this.On);
                    // Draw the image to the screen
                    e.Graphics.DrawImageUnscaled(offScreenBmp, 0, 0);
                }
            }
            else
            {
                Graphics g = e.Graphics;
                // g.SmoothingMode = SmoothingMode.HighQuality;
                Color bulbColor = (this.On) ? _color : this.ColorDark;

                Brush myPen = new SolidBrush(bulbColor);
                g.FillEllipse(myPen, 0, 0, this.Width - 1, this.Height - 1);

            }
        }

        /// <summary>
        /// Renders the control to an image
        /// </summary>
        private void drawControl(Graphics g, bool on)
        {
            // Is the bulb on or off
            Color lightColor = (on) ? this.Color : Color.FromArgb(150, this.ColorLight);
            Color darkColor = (on) ? this.ColorLight : this.ColorDark;

            // Calculate the dimensions of the bulb
            int width = this.Width - (this.Padding.Left + this.Padding.Right);
            int height = this.Height - (this.Padding.Top + this.Padding.Bottom);
            // Diameter is the lesser of width and height
            int diameter = Math.Min(width, height);
            // Subtract 1 pixel so ellipse doesn't get cut off
            diameter = Math.Max(diameter - 1, 1);

            // Draw the background ellipse
            var rectangle = new Rectangle(this.Padding.Left, this.Padding.Top, diameter, diameter);
            g.FillEllipse(new SolidBrush(darkColor), rectangle);

            // Draw the glow gradient
            var path = new GraphicsPath();
            path.AddEllipse(rectangle);
            var pathBrush = new PathGradientBrush(path);
            pathBrush.CenterColor = lightColor;
            pathBrush.SurroundColors = new Color[] { Color.FromArgb(0, lightColor) };
            g.FillEllipse(pathBrush, rectangle);

            // Draw the white reflection gradient
            var offset = Convert.ToInt32(diameter * .15F);
            var diameter1 = Convert.ToInt32(rectangle.Width * .8F);
            var whiteRect = new Rectangle(rectangle.X - offset, rectangle.Y - offset, diameter1, diameter1);
            var path1 = new GraphicsPath();
            path1.AddEllipse(whiteRect);
            var pathBrush1 = new PathGradientBrush(path);
            pathBrush1.CenterColor = _reflectionColor;
            pathBrush1.SurroundColors = _surroundColor;
            g.FillEllipse(pathBrush1, whiteRect);

            // Draw the border
            g.SetClip(this.ClientRectangle);
            if (this.On) g.DrawEllipse(new Pen(Color.FromArgb(85, Color.Black), 1F), rectangle);
        }

        /// <summary>
        /// Causes the Led to start blinking
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to blink for. 0 stops blinking</param>
        public void Blink(int milliseconds)
        {
            if (milliseconds > 0)
            {
                this.On = true;
                _timer.Interval = milliseconds;
                _timer.Enabled = true;
            }
            else
            {
                _timer.Enabled = false;
                this.On = false;
            }
        }

        #endregion
    }
}
