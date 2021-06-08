using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Input
{
    public class FileInput : IFFmpegInput
    {
        private string _filepath;

        public FileInput(string filepath)
        {
            _filepath = filepath;
        }

        public string GetInputString()
        {
            return $"-i \"{_filepath}\"";
        }
    }
}
