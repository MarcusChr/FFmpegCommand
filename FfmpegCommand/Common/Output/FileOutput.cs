using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Output
{
    public class FileOutput : IFFMpegOutput
    {
        public string _outputPath;

        public FileOutput(string outputPath)
        {
            _outputPath = outputPath;
        }

        public string GetOutputString()
        {
            return _outputPath;
        }
    }
}
