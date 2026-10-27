using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SlideShowMaker
{
    partial class MainWindowClass
    {
        bool _isCanceled;
        private Process? _p;
        public Process? Proc
        {
            get => _p;
            private set
            {
                _p = value;
                OnPropertyChanged(nameof(Proc));
            }
        }

        public double TotalDuration => ImagePaths.Count * (SlideDuration.Value - ((Fade ?? true) ? 0 : FadeDuration.Value)) * 1000;

        string _cs = "N/A";
        public string CurrentSpeed
        {
            get => _cs;
            set
            {
                _cs = value;
                OnPropertyChanged(nameof(CurrentSpeed));
            }
        }

        string _csz = "N/A";
        public string CurrentSize
        {
            get => _csz;
            set
            {
                _csz = value;
                OnPropertyChanged(nameof(CurrentSize));
            }
        }

        string _cb = "N/A";
        public string CurrentBitrate
        {
            get => _cb;
            set
            {
                _cb = value;
                OnPropertyChanged(nameof(CurrentBitrate));
            }
        }

        double _ct;
        public double CurrentTime
        {
            get => _ct;
            set
            {
                _ct = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        void CopyButton_Click(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(Command)) Clipboard.SetText("ffmpeg " + Command);
        }
        void RunButton_Click(object? sender, RoutedEventArgs? e)
        {
            if (!string.IsNullOrEmpty(Command))
            {
                Proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = Command,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,

                    },
                    EnableRaisingEvents = true
                };

                Proc.OutputDataReceived += (s, ea) => ParseFFmpegOutput(ea.Data);
                Proc.ErrorDataReceived += (s, ea) => ParseFFmpegOutput(ea.Data);
                Proc.Exited += (_, _) =>
                {
                    var code = Proc.ExitCode;
                    Proc = null;
                    CurrentSpeed = "N/A";
                    CurrentSize = "N/A";
                    CurrentBitrate = "N/A";
                    CurrentTime = 0;
                    if (_isCanceled)
                    {
                        _isCanceled = false;
                        return;
                    }
                    if (code == 0)
                        MessageBox.Show("Процесс успешно завершён.", "FFmpeg", MessageBoxButton.OK, MessageBoxImage.Information);
                    else if (MessageBox.Show("Процесс завершился преждевременно. Перезапустить?", "FFmpeg", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                        RunButton_Click(null, null);
                };

                Proc.Start();
                Proc.BeginOutputReadLine();
                Proc.BeginErrorReadLine();
            }
            else MessageBox.Show("Команда для FFmpeg ещё не создана.", "Не удалось запустить процесс", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (Proc == null || Proc.HasExited) return;
            if (MessageBox.Show("Вы уверены, что хотите завершить процесс преждевременно?", "FFmpeg", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                _isCanceled = true;
                Proc.Kill();
            }
        }

        private void ParseFFmpegOutput(string? data)
        {
            if (string.IsNullOrWhiteSpace(data)) return;
            Debug.WriteLine(data);

            var match = FFmpegDataRegex().Match(data);

            if (match.Success)
            {
                CurrentSize = match.Groups[1].Value;
                CurrentTime = TimeSpan.Parse(match.Groups[2].Value).TotalMilliseconds;
                CurrentBitrate = match.Groups[3].Value;
                CurrentSpeed = match.Groups[4].Value;
            }
        }

        [GeneratedRegex(@"frame=\s*\d+\s*fps=\s*[\d\.?]+.*size=\s*([\d\.]+Ki?B)\s*time=\s*([\d\:\.]+)\s*bitrate=\s*([\d\.]+kbits/s).*speed=\s*([\d\.]+x)")]
        private static partial Regex FFmpegDataRegex();
    }
}
