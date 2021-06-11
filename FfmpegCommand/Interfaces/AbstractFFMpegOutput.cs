using FfmpegCommand.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Interfaces
{
    public abstract class AbstractFFMpegOutput : AbstractFFmpegObject
    {
        protected AbstractFFMpegOutput(string name) : base(name)
        {
        }

        public abstract string GetOutputString();
    }
}
