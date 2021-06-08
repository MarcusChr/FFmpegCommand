using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Filters
{
    public class ImageFilter : IFFmpegFilter
    {
        private string _imagePath;
        private int _x, _y;

        public ImageFilter(string imagePath, int x, int y)
        {
            _imagePath = imagePath;
            _x = x;
            _y = y;
        }

        public string GetFilterString()
        {
            return $"overlay={_x}:{_y}";
        }

        public string GetInputString()
        {
            return $"-i \"{_imagePath}\"";
        }
    }
}
