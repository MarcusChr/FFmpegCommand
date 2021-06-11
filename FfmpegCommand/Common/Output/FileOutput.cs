using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Output
{
    public class FileOutput : AbstractFFMpegOutput
    {
        public string _outputPath;

        public FileOutput(string outputPath, string name) : base(name)
        {
            _outputPath = outputPath;
        }

        public override string GetOutputString()
        {
            return _outputPath;
        }
    }
}
