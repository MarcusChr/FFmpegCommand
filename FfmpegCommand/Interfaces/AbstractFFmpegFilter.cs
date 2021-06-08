using FfmpegCommand.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Interfaces
{
    public abstract class AbstractFFmpegFilter : AbstractFFmpegObject
    {
        protected AbstractFFmpegFilter(string name) : base(name)
        {

        }

        public abstract string GetInputString();

        public abstract string GetFilterString();
    }
}
