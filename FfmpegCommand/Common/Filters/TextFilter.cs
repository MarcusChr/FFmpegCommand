using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Filters
{
    public class TextFilter : IFFmpegFilter
    {
        public string Text;
        public int X;
        public int Y;
        public string FontColor = "white";
        public int FontSize = 24;

        public TextFilter(string text, int x, int y)
        {
            Text = text;
            X = x;
            Y = y;
        }

        

        public string GetFilterString()
        {
            return $"drawtext=text='{Text}':x={X}:y={Y}:fontsize={FontSize}:fontcolor={FontColor}";
        }

        public string GetInputString()
        {
            return null;
        }
    }
}
