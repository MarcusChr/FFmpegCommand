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
        private IDictionary<string, IFFmpegInput> _inputs;
        private IDictionary<string, IFFmpegFilter> _filters;
        private IDictionary<string, IFFMpegOutput> _outputs;

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
            _inputs.Add("input", new FileInput(input));
        }

        private void _init(string ffmpegExec)
        {
            _inputs = new Dictionary<string, IFFmpegInput>();
            _filters = new Dictionary<string, IFFmpegFilter>();
            _outputs = new Dictionary<string, IFFMpegOutput>();

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

        public void AddInput(IFFmpegInput input, string key = null)
        {
            key = _handleKey(key);

            _inputs[key] = input;
        }

        public void AddFilter(IFFmpegFilter filter, string key = null)
        {
            key = _handleKey(key);
            _filters[key] = filter;
        }

        public void AddOutput(IFFMpegOutput output, string key = null)
        {
            key = _handleKey(key);

            _outputs[key] = output;
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
            _outputs["output"] = new FileOutput(outputFilePath);
        }

        public void AddImage(int x, int y, string imagePath, string key = null)
        {
            key = _handleKey(key);
            //_arguments.Add(key, $"-i \"{imagePath}\" -filter_complex \"overlay = {x}:{y}\"");
            _filters[key] = new ImageFilter(imagePath, x, y);
        }

        public void AddText(int x, int y, string text, int fontsize = 24, string fontcolor = "white", string key = null)
        {
            key = _handleKey(key);
            _filters[key] = new TextFilter(text, x, y)
            {
                FontSize = fontsize,
                FontColor = fontcolor
            };
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

            foreach(IFFmpegInput input in _inputs.Values)
            {
                sourceStrings.Add(input.GetInputString());
            }

            foreach(IFFmpegFilter filter in _filters.Values)
            {
                sourceStrings.Add(filter.GetInputString());
                filterStrings.Add(filter.GetFilterString());
            }

            foreach(IFFMpegOutput output in _outputs.Values)
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
