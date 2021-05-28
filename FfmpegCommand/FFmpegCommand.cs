using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.marcuslc.ffmpegcommand
{
    class FFmpegCommand
    {
        private ICollection<string> _arguments;
        private string _output;
        private string _ffmpegExec;

        public FFmpegCommand(string output, string ffmpegExec = null)
        {
            _arguments = new LinkedList<string>();
            _output = output;
            _ffmpegExec = ffmpegExec ?? "ffmpeg";
        }

        public void SetFFmpegExec(string ffmpegExec)
        {
            _ffmpegExec = ffmpegExec;
        }

        public void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public async Task ExecuteAsync()
        {
            using(Process ffmpeg = new Process())
            {
                ffmpeg.StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegExec,
                    Arguments = this.GetArgumentsAsString(),
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                };

                ffmpeg.Start();
                await ffmpeg.WaitForExitAsync();
            }
        }

        public string GetArgumentsAsString()
        {
            return string.Join(" ", _arguments);
        }

        public override string ToString()
        {
            return $"{_ffmpegExec} {this.GetArgumentsAsString()}";
        }
    }
}
