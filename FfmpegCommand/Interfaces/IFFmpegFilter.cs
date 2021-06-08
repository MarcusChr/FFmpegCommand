using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Interfaces
{
    public interface IFFmpegFilter
    {
        public string GetInputString();

        public string GetFilterString();
    }
}
