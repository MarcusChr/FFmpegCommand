using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.marcuslc.ffmpegcommand;
using System;
using System.IO;
using System.Diagnostics;

namespace Unit_Tests
{
    [TestClass]
    public class FFmpegCommandTester
    {
        [TestMethod]
        public void MakeVideo()
        {
            string output = @"output.mp4";


            File.Delete(output);

            FFmpegCommand command = new FFmpegCommand(@"media/countdown.mp4", output, null)
            {
                DefaultArguments =
                {
                    "-vcodec libx264",
                    "-crf 27",
                    "-preset veryfast",
                    "-c:a copy",
                    "-s 960x540"
                }
            };

            command.AddImage(10, 10, @"media/panda.jpg", "image1");
            command.AddText(50, 50, @"hello, world", 100, "white");
            command.FFmpegOutput = (object sender, DataReceivedEventArgs e) =>
            {
                Console.WriteLine(e.Data);
            };

            Console.WriteLine(command);
            command.Execute();


            bool success = File.Exists(output);
            Assert.IsTrue(success);
        }
    }
}
