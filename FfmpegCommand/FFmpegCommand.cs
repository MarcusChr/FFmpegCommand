using FfmpegCommand;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.marcuslc.ffmpegcommand
{
    public class FFmpegCommand
    {
        private IDictionary<string, FFmpegFilterDescription> _arguments;
        private string _output;
        private string _ffmpegExec;

        public DataReceivedEventHandler FFmpegOutput;
        public IList<string> DefaultArguments
        {
            get; private set;
        }

        public FFmpegCommand(string output, string ffmpegExec = null)
        {
            _init(output, ffmpegExec);
        }

        public FFmpegCommand(string input, string output, string ffmpegExec = null)
        {
            _init(output, ffmpegExec);

            //_arguments.Add("input", $"-i \"{input}\"");
            _arguments.Add("input", new FFmpegFilterDescription
            {
                source = $"-i \"{input}\""
            });
        }

        private void _init(string output, string ffmpegExec)
        {
            _arguments = new Dictionary<string, FFmpegFilterDescription>();
            _output = output;
            _ffmpegExec = ffmpegExec ?? "ffmpeg";
            DefaultArguments = new List<string>();
        }

        public void SetFFmpegExec(string ffmpegExec)
        {
            _ffmpegExec = ffmpegExec;
        }

        public void AddCustomArgument(string argument, string key = null)
        {
            key = _handleKey(key);
            _arguments.Add(key, new FFmpegFilterDescription
            {
                source = argument
            });
        }

        public void AddImage(int x, int y, string imagePath, string key = null)
        {
            key = _handleKey(key);
            //_arguments.Add(key, $"-i \"{imagePath}\" -filter_complex \"overlay = {x}:{y}\"");
            _arguments[key] = new FFmpegFilterDescription
            {
                source = $"-i \"{imagePath}\"",
                filterValues = $"overlay={x}:{y}"
            };
        }

        public void AddText(int x, int y, string text, int fontsize = 24, string fontcolor = "white", string key = null)
        {
            key = _handleKey(key);
            //_arguments.Add(key, $"-filter_complex \"drawtext=text='{text}':x={x}:y={y}:fontsize=24:fontcolor=white\"");
            _arguments[key] = new FFmpegFilterDescription
            {
                filterValues = $"drawtext=text='{text}':x={x}:y={y}:fontsize={fontsize}:fontcolor={fontcolor}"
            };
        }

        public bool RemoveKey(string key)
        {
            return _arguments.Remove(key);
        }

        public void Execute()
        {
            this.ExecuteAsync().Wait();
        }

        public async Task ExecuteAsync()
        {
            using (Process ffmpeg = new Process())
            {
                ffmpeg.StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegExec,
                    Arguments = this.GetArgumentsAsString(),
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };

                ffmpeg.Start();


                ffmpeg.BeginErrorReadLine();
                if (FFmpegOutput != null)
                {
                    ffmpeg.ErrorDataReceived += FFmpegOutput;
                }
                await ffmpeg.WaitForExitAsync();
                //ffmpeg.WaitForExit();
                //Console.WriteLine(ffmpeg.StandardError.ReadToEnd());
            }
        }

        public string GetArgumentsAsString()
        {
            var sourceStrings = new List<string>();
            var filterStrings = new List<string>();
            var postfixString = new List<string>(DefaultArguments);
            //argumentString.Append(string.Join(" ", _arguments.Values));

            foreach(var pair in _arguments)
            {
                var filterDesc = pair.Value;

                if(filterDesc.source != null)
                {
                    sourceStrings.Add(filterDesc.source);
                }

                if(filterDesc.filterValues != null)
                {
                    filterStrings.Add(filterDesc.filterValues);
                }
            }

            postfixString.Add(" \"" + _output + "\"");

            return $"{string.Join(" ", sourceStrings)} -filter_complex \"{string.Join(",", filterStrings)}\" {string.Join(" ", postfixString)}";
        }

        public override string ToString()
        {
            return $"{_ffmpegExec} {this.GetArgumentsAsString()} -y";
        }

        private string _handleKey(string key)
        {
            return key ?? (new Random().Next(0, int.MaxValue - 1).ToString());
        }
    }
}
