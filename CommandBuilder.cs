using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SlideShowMaker
{
    partial class MainWindowClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Command)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalDuration)));
        }

        Fraction _sd = new(7, 1);
        public Fraction SlideDuration
        {
            get => _sd;
            set
            {
                value = value.FindNearestMultiple(_fps.Inversed);
                if (_sd == value) return;
                _sd = value;
                OnPropertyChanged(nameof(SlideDuration));
            }
        }

        Fraction _fd = new(1, 1);
        public Fraction FadeDuration
        {
            get => _fd;
            set
            {
                value = value.FindNearestMultiple(_fps.Inversed);
                if (_fd == value) return;
                _fd = value;
                OnPropertyChanged(nameof(FadeDuration));
            }
        }

        bool? _f = true;
        public bool? Fade
        {
            get => _f;
            set
            {
                if (_f == value) return;
                _f = value;
                OnPropertyChanged(nameof(Fade));
            }
        }

        string Out => Path.Combine(_od, _of + _ext);

        string _od = ".\\";
        public string OutDirectory
        {
            get => _od;
            set
            {
                if (_od == value) return;
                _od = value;
                OnPropertyChanged(nameof(OutDirectory));
            }
        }

        string _of = "out";
        public string OutFile
        {
            get => _of;
            set
            {
                if (_of == value) return;
                _of = value;
                OnPropertyChanged(nameof(OutFile));
            }
        }

        string _ext = ".avifs";
        public string OutExtension
        {
            get => _ext;
            set
            {
                if (_ext == value) return;
                _ext = value;
                OnPropertyChanged(nameof(OutExtension));
            }
        }

        Fraction _fps = new(60, 1);
        public Fraction Fps
        {
            get => _fps;
            set
            {
                if (_fps == value) return;
                _fps = value;
                FadeDuration = _fd.FindNearestMultiple(_fps.Inversed);
                SlideDuration = _sd.FindNearestMultiple(_fps.Inversed);
                OnPropertyChanged(nameof(Fps));
            }
        }

        byte _s = 4;
        public byte Speed
        {
            get => _s;
            set
            {
                if (_s == value) return;
                _s = value;
                OnPropertyChanged(nameof(Speed));
            }
        }

        byte _crf = 26;
        public byte Crf
        {
            get => _crf;
            set
            {
                if (_crf == value) return;
                _crf = value;
                OnPropertyChanged(nameof(Crf));
            }
        }

        string _cf = "yuv444";
        public string ColorFormat
        {
            get => _cf;
            set
            {
                if (_cf == value) return;
                _cf = value;
                OnPropertyChanged(nameof(ColorFormat));
            }
        }

        string _c = "av1";
        public string Codec
        {
            get => _c;
            set
            {
                if (_c == value) return;
                _c = value;
                OnPropertyChanged(nameof(Codec));
            }
        }

        ObservableCollection<string> _ip = [];
        public ObservableCollection<string> ImagePaths
        {
            get => _ip;
            set
            {
                if (_ip == value) return;
                _ip = value;
                OnPropertyChanged(nameof(ImagePaths));
            }
        }

        short? _rw;
        short? ResultWidth
        {
            get => _rw;
            set
            {
                if (_rw == value) return;
                _rw = value;
                OnPropertyChanged(nameof(Crf));
            }
        }

        short? _rh;
        short? ResultHeight
        {
            get => _rh;
            set
            {
                if (_rh == value) return;
                _rh = value;
                OnPropertyChanged(nameof(Crf));
            }
        }

        public string? Command
        {
            get
            {
                if (ImagePaths.Count < 2) return null;
                StringBuilder cmd = new("-hide_banner -y");

                cmd.Append($" -loop 1 -t {(Fade == false ? SlideDuration : SlideDuration / 2)} -i \"{ImagePaths[0]}\"");
                for (var i = 1; i < ImagePaths.Count; i++)
                {
                    cmd.Append($" -loop 1 -t {SlideDuration} -i \"{ImagePaths[i]}\"");
                }

                cmd.Append(" -filter_complex \"");

                switch (Fade)
                {
                    case true:
                    {
                        cmd.Append($"[0:v]split[v0a][v0b];[v0a]fade=t=out:st={SlideDuration / 2 - FadeDuration}:d={FadeDuration}[v0];");

                        for (var i = 1; i < ImagePaths.Count; i++)
                        {
                            cmd.Append($"[{i}:v]fade=t=in:st=0:d={FadeDuration},fade=t=out:st={SlideDuration - FadeDuration}:d={FadeDuration}[v{i}];");
                        }

                        cmd.Append($"[v0b]fade=t=in:st=0:d={FadeDuration}[v{ImagePaths.Count}];");

                        var vPrev = "[v0]";
                        for (var i = 1; i < ImagePaths.Count; i++)
                        {
                            var vCur = $"[v{i}]";
                            cmd.Append($"{vPrev}{vCur}xfade=transition=fade:duration={FadeDuration}:offset={SlideDuration / 2 - FadeDuration + (SlideDuration - FadeDuration) * (i - 1)}[v{i}a];");
                            vPrev = $"[v{i}a]";
                        }

                        cmd.Append($"{vPrev}[v{ImagePaths.Count}]xfade=transition=fade:duration={FadeDuration}:offset={SlideDuration / 2 - FadeDuration + (SlideDuration - FadeDuration) * (ImagePaths.Count - 1)}[v]");
                        break;
                    }
                    case null:
                    {
                        cmd.Append("[0:v]split[v0a][v0b];");

                        var vPrev = "[v0a]";
                        string vCur;
                        for (var i = 1; i < ImagePaths.Count; i++)
                        {
                            vCur = $"[{i}:v]";
                            cmd.Append($"{vPrev}{vCur}xfade=transition=fade:duration={FadeDuration}:offset={SlideDuration / 2 - FadeDuration + (SlideDuration - FadeDuration) * (i - 1)}[v{i-1}];");
                            vPrev = $"[v{i-1}]";
                        }

                        cmd.Append($"{vPrev}[v0b]xfade=transition=fade:duration={FadeDuration}:offset={SlideDuration / 2 - FadeDuration + (SlideDuration - FadeDuration) * (ImagePaths.Count - 1)}[v]");
                        break;
                    }
                    // Fade == false
                    default:
                    {
                        for (var i = 0; i < ImagePaths.Count; i++)
                        {
                            cmd.Append($"[{i}:v]setsar=sar=1[v{i}];");
                        }
                        for (var i = 0; i < ImagePaths.Count; i++)
                        {
                            cmd.Append($"[v{i}]");
                        }
                        cmd.Append($"concat=n={ImagePaths.Count}:v=1:a=0[v]");
                        break;
                    }
                }

                cmd.Append('"');

                cmd.Append(" -map \"[v]\"");

                switch (OutExtension)
                {
                    case ".avifs":
                        cmd.Append($" -cpu-used {Speed} -f avif");
                        break;
                    case ".mp4":
                        cmd.Append($" -c:v {Codec}");
                        break;
                }

                if (OutExtension is ".mp4" or ".avifs" or ".mkv") cmd.Append($" -crf {Crf}");
                if (OutExtension != ".gif" && OutExtension != ".apng") cmd.Append($" -pix_fmt {ColorFormat}");
                cmd.Append($" -r {Fps} \"{Out}\"");

                return cmd.ToString();
            }
        }
    }
}
