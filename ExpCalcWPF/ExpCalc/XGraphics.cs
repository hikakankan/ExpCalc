using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ExpCalc
{
    public class XGraphics
    {
        private Panel panel;
        private XFont font;
        private MVColor col;
        private bool use_image;
        private ImageSettings image_settings;

        public XGraphics(Panel panel, XFont font, bool use_image, ImageSettings image_settings)
        {
            this.panel = panel;
            this.font = font;
            this.use_image = use_image;
            this.image_settings = image_settings;
        }

        public void setColor(MVColor c)
        {
            col = c;
        }

        //public void setColor(Color c)
        //{
        //    col = c;
        //}

        //public void fillRect(int x, int y, int width, int height)
        //{
        //    System.Diagnostics.Debug.WriteLine("XGraphics.fillRect " + x.ToString() + " " + y.ToString() + " " + width.ToString() + " " + height.ToString());
        //    //SolidColorBrush brush = new SolidColorBrush(col);
        //    //graph.DrawRectangle(brush, null, new Rect(x, y, width, height));

        //    //Rectangle rect = new Rectangle();
        //    //rect.Fill = new SolidColorBrush(col);
        //    //rect.HorizontalAlignment = HorizontalAlignment.Left;
        //    //rect.VerticalAlignment = VerticalAlignment.Top;
        //    //rect.Width = width;
        //    //rect.Height = height;
        //    //rect.Margin = new Thickness(x, y, 0, 0);
        //    //panel.Children.Insert(0, rect);
        //}

        public void drawRoundRect(int x, int y, int width, int height, int d1, int d2, MVColor foreColor, MVColor backColor)
        {
            System.Diagnostics.Debug.WriteLine("XGraphics.drawRoundRect " + x.ToString() + " " + y.ToString() + " " + width.ToString() + " " + height.ToString() + " " + d1.ToString() + " " + d2.ToString());
            int r1 = d1 / 2;
            int r2 = d2 / 2;
            //Pen pen = new Pen(new SolidColorBrush(col), 1);
            //graph.DrawRoundedRectangle(null, pen, new Rect(x, y, width, height), d1, d2);

            Rectangle rect = new Rectangle();
            rect.Stroke = foreColor.XColor;
            rect.Fill = backColor.XColor;
            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Top;
            rect.Width = width;
            rect.Height = height;
            rect.Margin = new Thickness(x, y, 0, 0);
            rect.RadiusX = r1;
            rect.RadiusY = r2;
            //panel.Children.Insert(0, rect);
            panel.Children.Add(rect);
        }

        //public void fillRoundRect(int x, int y, int width, int height, int d1, int d2)
        //{
        //    System.Diagnostics.Debug.WriteLine("XGraphics.fillRoundRect " + x.ToString() + " " + y.ToString() + " " + width.ToString() + " " + height.ToString() + " " + d1.ToString() + " " + d2.ToString());
        //    int r1 = d1 / 2;
        //    int r2 = d2 / 2;
        //    SolidColorBrush brush = new SolidColorBrush(col);
        //    //graph.DrawRoundedRectangle(brush, null, new Rect(x, y, width, height), d1, d2);
        //}

        public void drawString(String str, int x, int y)
        {
            System.Diagnostics.Debug.WriteLine("XGraphics.drawString " + str + " " + x.ToString() + " " + y.ToString());
            if ( use_image )
            {
                image_settings.DrawString(str, x, y, this);
            }
            else
            {
                //SolidColorBrush brush = new SolidColorBrush(col);
                //FormattedText text = new FormattedText(str, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, font.Typeface, font.Height, brush);
                //graph.DrawText(text, new Point(x, y));

                TextBlock text = new TextBlock();
                text.FontFamily = font.Typeface.FontFamily;
                text.FontStretch = font.Typeface.Stretch;
                text.FontStyle = font.Typeface.Style;
                text.FontWeight = font.Typeface.Weight;
                text.FontSize = font.Height;
                text.Text = str;
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Top;
                text.Margin = new Thickness(x, y, 0, 0);
                //panel.Children.Insert(0, text);
                panel.Children.Add(text);
            }
        }

        public MVFont getFontMetrics()
        {
            return new MVFont(font, use_image, image_settings);
        }

        public void DrawImage(XImage ximage, int x, int y, int width, int height)
        {
            //graph.DrawImage(image.Source, new Rect(x, y, width, height));

            Image image = new Image();
            image.Source = ximage.Source;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Width = width;
            image.Height = height;
            image.Margin = new Thickness(x, y, 0, 0);
            //panel.Children.Insert(0, image);
            panel.Children.Add(image);
        }
    }
}
