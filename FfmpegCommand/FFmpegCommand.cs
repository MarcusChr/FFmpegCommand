using FfmpegCommand;
using FfmpegCommand.Common.Filters;
using FfmpegCommand.Common.Input;
using FfmpegCommand.Common.Output;
using FfmpegCommand.Interfaces;
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
        private IList<AbstractFFmpegInput> _inputs;
        private IList<AbstractFFmpegFilter> _filters;
        private IList<AbstractFFMpegOutput> _outputs;

        //private string _output;
        private string _ffmpegExec;

        public DataReceivedEventHandler FFmpegOutput;
        public IList<string> DefaultArguments
        {
            get; private set;
        }
        public FFmpegCommand()
        {
            _init(null);
        }

        public FFmpegCommand(string output, string ffmpegExec = null)
        {
            _init(ffmpegExec);
            this.SetSingleOutputToFile(output);
        }

        public FFmpegCommand(string input, string output, string ffmpegExec = null)
        {
            _init(ffmpegExec);
            this.SetSingleOutputToFile(output);

            //_arguments.Add("input", $"-i \"{input}\"");
            _inputs.Add(new FileInput(input, "input"));
        }

        private void _init(string ffmpegExec)
        {
            _inputs = new List<AbstractFFmpegInput>();
            _filters = new List<AbstractFFmpegFilter>();
            _outputs = new List<AbstractFFMpegOutput>();

            _ffmpegExec = ffmpegExec ?? "ffmpeg";
            //DefaultArguments = new List<string>();
        }

        public void SetFFmpegExec(string ffmpegExec)
        {
            _ffmpegExec = ffmpegExec;
        }
        /*
        public void AddCustomArgument(string argument, string key = null)
        {
            key = _handleKey(key);
            _arguments.Add(key, new FFmpegFilterDescription
            {
                source = argument
            });
        }
        */

        public void AddInput(AbstractFFmpegInput input, string key = null)
        {
            _inputs.Add(input);
        }

        public void AddFilter(AbstractFFmpegFilter filter, string key = null)
        {
            _filters.Add(filter);
        }

        public void AddOutput(AbstractFFMpegOutput output, string key = null)
        {
            _outputs.Add(output);
        }

        /*
        public void AddCustomArgument(FFmpegFilterDescription command, string key = null)
        {
            key = _handleKey(key);
            _arguments[key] = command;
        }
        */

        public void SetSingleOutputToFile(string outputFilePath)
        {
            _outputs.Add(new FileOutput(outputFilePath, "output"));
        }

        public void AddImage(string imagePath, int width, int height, int x = 0, int y = 0,  string key = null)
        {
            //_arguments.Add(key, $"-i \"{imagePath}\" -filter_complex \"overlay = {x}:{y}\"");
            _filters.Add(new ImageFilter(imagePath, width, height, key, x, y));
        }

        public void AddText(int x, int y, string text, int fontsize = 24, string fontcolor = "white", string key = null)
        {
            key = _handleKey(key);
            _filters.Add(new TextFilter(text, x, y, key)
            {
                FontSize = fontsize,
                FontColor = fontcolor
            });
            //_arguments.Add(key, $"-filter_complex \"drawtext=text='{text}':x={x}:y={y}:fontsize=24:fontcolor=white\"");
            /*
            _arguments[key] = new FFmpegFilterDescription
            {
                filterValues = $"drawtext=text='{text}':x={x}:y={y}:fontsize={fontsize}:fontcolor={fontcolor}"
            };
            */
        }
        /*
        public bool RemoveKey(string key)
        {
            return _arguments.Remove(key);
        }
        */

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
            }
        }

        public string GetArgumentsAsString()
        {
            var sourceStrings = new List<string>();
            var filterStrings = new List<string>();
            var outputString = new List<string>();

            //foreach (var pair in _arguments)
            //{
            //    var filterDesc = pair.Value;

            //    if (filterDesc.source != null)
            //    {
            //        sourceStrings.Add(filterDesc.source);
            //    }

            //    if (filterDesc.filterValues != null)
            //    {
            //        filterStrings.Add(filterDesc.filterValues);
            //    }
            //}



            //if (_output != null)
            //{
            //    postfixString.Add(" \"" + _output + "\"");
            //}

            foreach(AbstractFFmpegInput input in _inputs)
            {
                sourceStrings.Add(input.GetInputString());
            }

            foreach(AbstractFFmpegFilter filter in _filters)
            {
                sourceStrings.Add(filter.GetInputString());
                filterStrings.Add(filter.GetFilterString());
            }

            foreach(AbstractFFMpegOutput output in _outputs)
            {
                outputString.Add(output.GetOutputString());
            }


            StringBuilder argumentString = new StringBuilder();

            argumentString.Append(string.Join(" ", sourceStrings));
            argumentString.Append(" ");

            if (filterStrings.Count > 0)
            {
                argumentString.Append($"-filter_complex \"{string.Join(",", filterStrings)}\" ");
            }

            if (outputString.Count > 0)
            {
                argumentString.Append(string.Join(" ", outputString));
            }

            return argumentString.ToString();
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
