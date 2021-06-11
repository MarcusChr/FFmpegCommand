using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common
{
    public abstract class AbstractFFmpegObject
    {
        protected string Name
        {
            get; private set;
        }

        public AbstractFFmpegObject(string name)
        {
            Name = name ?? (new Random().Next(0, int.MaxValue - 1).ToString());
        }
    }
}
