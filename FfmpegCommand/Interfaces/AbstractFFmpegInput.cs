using FfmpegCommand.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Interfaces
{
    public abstract class AbstractFFmpegInput : AbstractFFmpegObject
    {
        protected AbstractFFmpegInput(string name) : base(name)
        {
        }

        public abstract string GetInputString();
    }
}
