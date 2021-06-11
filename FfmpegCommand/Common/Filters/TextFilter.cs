using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Filters
{
    public class TextFilter : AbstractFFmpegFilter
    {
        public string Text;
        public int X;
        public int Y;
        public string FontColor = "white";
        public int FontSize = 24;

        public TextFilter(string text, int x, int y, string name) : base(name)
        {
            Text = text;
            X = x;
            Y = y;
        }

        

        public override string GetFilterString()
        {
            return $"drawtext=text='{Text}':x={X}:y={Y}:fontsize={FontSize}:fontcolor={FontColor}";
        }

        public override string GetInputString()
        {
            return null;
        }
    }
}
