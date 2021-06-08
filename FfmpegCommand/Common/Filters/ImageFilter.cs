using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Filters
{
    public class ImageFilter : AbstractFFmpegFilter
    {
        public string ImagePath;
        public int X, Y;

        public int Width, Height;

        public ImageFilter(string imagePath, int width, int height, string name, int x = 0, int y = 0) : base(name)
        {
            ImagePath = imagePath;
            Width = width;
            Height = height;

            X = x;
            X = y;
        }

        public override string GetFilterString()
        {
            return $"[1:v]scale={Width}:{Height} [{Name}],[0:v][{Name}]overlay={X}:{Y}";
        }

        public override string GetInputString()
        {
            return $"-i \"{ImagePath}\"";
        }
    }
}
