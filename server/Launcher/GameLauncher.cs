using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Launcher
{
    public class GameLauncher
    {
        public async Task LaunchGameAsync(string token)
        {
            string pipeName = "Launcher-Token-" + Guid.NewGuid().ToString("N");
            Process.Start("GameClient.exe", $"--pipe={pipeName}");
            using (var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            {
                await pipeServer.WaitForConnectionAsync();
                using (var writer = new StreamWriter(pipeServer))
                {
                    await writer.WriteAsync(token);
                }
            }
        }
    }
}
