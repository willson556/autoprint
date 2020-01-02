using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace autoPrint
{
    public class PrintingEngine
    {
        private PrintingState printingState;

        public List<Color> ColorsToPrint { get; set; } = new List<Color> { Color.Black, Color.Cyan, Color.Magenta, Color.Yellow };

        public List<Color> LargeColors { get; set; } = new List<Color> { Color.Black };

        public Size TargetPatternSize { get; set; } = new Size(100, 100);

        public int BarGap { get; set; } = 5;

        public Size PatternGap { get; set; } = new Size(20, 20);

        public Font Font { get; set; } = new Font("Arial", 10);

        public Margins Margins = new Margins(50, 50, 50, 50);

        public void Print()
        {
            printingState = PrintingState.Load();

            var document = new PrintDocument();
            document.DefaultPageSettings.Margins = Margins;
            document.PrintPage += PrintPage;
            document.Print();

            PrintingState.Save(printingState);
        }

        private void PrintPage(object sender, PrintPageEventArgs eventArgs)
        {
            var xOffset = printingState.CurrentLocation.X == 0 ? 0 : PatternGap.Width;
            var x = eventArgs.MarginBounds.Left + printingState.CurrentLocation.X + xOffset;
            var y = eventArgs.MarginBounds.Top + printingState.CurrentLocation.Y;

            var timestamp = DateTimeOffset.Now.ToLocalTime().ToString();
            var timestampSize = eventArgs.Graphics.MeasureString(timestamp, Font);

            var patternSize = TargetPatternSize;
            patternSize.Width = (int) Math.Max(patternSize.Width, timestampSize.Width);
            patternSize.Height = (int) Math.Max(patternSize.Height, timestampSize.Height + 50);

            if (patternSize.Width + x > eventArgs.MarginBounds.Right)
            {
                x = eventArgs.MarginBounds.Left;
                y += printingState.CurrentLineHeight + PatternGap.Height;
                printingState.CurrentLineHeight = 0;

                if (y > eventArgs.MarginBounds.Bottom)
                {
                    y = 0; // Hopefully we have a new page.
                }
            }

            UpdateState(x + patternSize.Width, y, patternSize.Height, eventArgs);

            eventArgs.Graphics.DrawString(timestamp, Font, Brushes.Black, x, y);

            var barWidth = CalculateBarWidth(patternSize.Width);
            var barHeight = patternSize.Height - (timestampSize.Height + BarGap);

            foreach (var color in ColorsToPrint)
            {
                var thisBarWidth = LargeColors.Contains(color) ? barWidth * 2 : barWidth;
                eventArgs.Graphics.FillRectangle(GetBrush(color), x, y + BarGap + timestampSize.Height, thisBarWidth, barHeight);
                x += thisBarWidth + BarGap;
            }

            eventArgs.HasMorePages = false;
        }

        private void UpdateState(int x, int y, int height, PrintPageEventArgs eventArgs)
        {
            printingState.CurrentLocation = new Point(x - eventArgs.MarginBounds.Left, y - eventArgs.MarginBounds.Top);
            printingState.CurrentLineHeight = (int) Math.Max(printingState.CurrentLineHeight, height);
        }

        private int CalculateBarWidth(int targetWidth)
        {
            var numberOfBarGaps = ColorsToPrint.Count - 1.0;
            var numberOfBarWidths = ColorsToPrint.Count + LargeColors.Count;

            return (int) Math.Ceiling((targetWidth - (numberOfBarGaps * BarGap)) / numberOfBarWidths);
        }

        private Brush GetBrush(Color color)
        {
            switch (color)
            {
                case Color.Black:
                    return Brushes.Black;
                case Color.Magenta:
                    return Brushes.Magenta;
                case Color.Cyan:
                    return Brushes.Cyan;
                case Color.Yellow:
                    return Brushes.Yellow;
            }

            throw new ArgumentOutOfRangeException(nameof(Color));
        }
    }
}