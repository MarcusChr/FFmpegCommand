using FfmpegCommand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FfmpegCommand.Common.Input
{
    public class FileInput : AbstractFFmpegInput
    {
        private string _filepath;

        public FileInput(string filepath, string name) : base(name)
        {
            _filepath = filepath;
        }

        public override string GetInputString()
        {
            return $"-i \"{_filepath}\"";
        }
    }
}
