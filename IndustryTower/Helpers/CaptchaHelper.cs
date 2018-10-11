using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace IndustryTower.Helpers
{
    public class CaptchaImage
    {
        public Bitmap Image
        {
            get { return this.image; }
        }

        private string text;
        private int width;
        private int height;
        private System.Drawing.FontFamily fontFamily;
        private Bitmap image;

        private Random random = new Random( );

        public CaptchaImage( int width, int height, System.Drawing.FontFamily fontFamily )
        {
            this.width = width;
            this.height = height;
            this.fontFamily = fontFamily;
        }
        public CaptchaImage( string s, int width, int height, System.Drawing.FontFamily fontFamily )
        {
            this.text = s;
            this.width = width;
            this.height = height;
            this.fontFamily = fontFamily;
        }
        public string CreateRandomText( int Length )
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ1234567890";
            char[] chars = new char[ Length ];
            Random rd = new Random( );

            for ( int i = 0; i < Length; i++ ) {
                chars[ i ] = allowedChars[ rd.Next( 0, allowedChars.Length ) ];
            }

            return new string( chars );
        }

        public void GenerateImage( )
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap( this.width, this.height, PixelFormat.Format32bppArgb );

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage( bitmap );
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle( 0, 0, this.width, this.height );

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush( HatchStyle.Shingle, Color.LightGray, Color.White );
            g.FillRectangle( hatchBrush, rect );

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do {
                fontSize--;
                font = new Font( this.fontFamily.Name, fontSize, GraphicsUnit.Pixel );
                size = g.MeasureString( this.text, font );
            } while ( size.Width > rect.Width );

            // Set up the text format.
            StringFormat format = new StringFormat( );
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath( );

            path.AddString( this.text, font.FontFamily, (int)font.Style, font.Size, rect, format );
            float v = 4F;
            PointF[] points =
			{
				new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
            Matrix matrix = new Matrix( );
            matrix.Translate( 0F, 0F );
            path.Warp( points, rect, matrix, WarpMode.Perspective, 0F );

            // Draw the text.
            hatchBrush = new HatchBrush( HatchStyle.Shingle, Color.LightGray, Color.DarkGray );
            g.FillPath( hatchBrush, path );

            // Add some random noise.
            int m = Math.Max( rect.Width, rect.Height );
            for ( int i = 0; i < (int)( rect.Width * rect.Height / 30F ); i++ ) {
                int x = this.random.Next( rect.Width );
                int y = this.random.Next( rect.Height );
                int w = this.random.Next( m / 50 );
                int h = this.random.Next( m / 50 );
                g.FillEllipse( hatchBrush, x, y, w, h );
            }

            // Clean up.
            font.Dispose( );
            hatchBrush.Dispose( );
            g.Dispose( );

            // Set the image.
            this.image = bitmap;
        }

        public void SetText( string text )
        {
            this.text = text;
        }
    }
}