﻿using FoxTunes.Interfaces;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;

namespace FoxTunes
{
    public class OscilloscopeRenderer : VisualizationBase
    {
        protected override bool LoadColorPalette
        {
            get
            {
                return false;
            }
        }

        public OscilloscopeRendererData RendererData { get; private set; }

        public SelectionConfigurationElement Mode { get; private set; }

        public IntegerConfigurationElement Window { get; private set; }

        public IntegerConfigurationElement Duration { get; private set; }

        protected override void OnConfigurationChanged()
        {
            if (this.Configuration != null)
            {
                this.Mode = this.Configuration.GetElement<SelectionConfigurationElement>(
                    OscilloscopeConfiguration.SECTION,
                    OscilloscopeConfiguration.MODE_ELEMENT
                );
                this.Window = this.Configuration.GetElement<IntegerConfigurationElement>(
                    OscilloscopeConfiguration.SECTION,
                    OscilloscopeConfiguration.WINDOW_ELEMENT
                );
                this.Duration = this.Configuration.GetElement<IntegerConfigurationElement>(
                    OscilloscopeConfiguration.SECTION,
                    OscilloscopeConfiguration.DURATION_ELEMENT
                );
                this.Mode.ValueChanged += this.OnValueChanged;
                this.Duration.ValueChanged += this.OnValueChanged;
                var task = this.CreateBitmap(true);
            }
            base.OnConfigurationChanged();
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            var task = this.CreateData();
        }

        protected override bool CreateData(int width, int height)
        {
            if (this.Configuration == null)
            {
                return false;
            }
            this.RendererData = Create(
                this.OutputDataSource,
                width,
                height,
                OscilloscopeConfiguration.GetWindow(this.Window.Value),
                OscilloscopeConfiguration.GetDuration(this.Duration.Value),
                OscilloscopeConfiguration.GetMode(this.Mode.Value)
            );
            return true;
        }

        protected virtual async Task Render(OscilloscopeRendererData data)
        {
            var bitmap = default(WriteableBitmap);
            var success = default(bool);
            var info = default(BitmapHelper.RenderInfo);

            await Windows.Invoke(() =>
            {
                bitmap = this.Bitmap;
                if (bitmap == null)
                {
                    return;
                }

                success = bitmap.TryLock(LockTimeout);
                if (!success)
                {
                    return;
                }
                info = BitmapHelper.CreateRenderInfo(bitmap, BitmapHelper.GetOrCreatePalette(0, this.Color));
            }, DISPATCHER_PRIORITY).ConfigureAwait(false);

            if (!success)
            {
                //Failed to establish lock.
                this.Restart();
                return;
            }

            try
            {
                Render(info, data);
            }
            catch (Exception e)
            {
#if DEBUG
                Logger.Write(this.GetType(), LogLevel.Warn, "Failed to render oscilloscope: {0}", e.Message);
#else
                Logger.Write(this.GetType(), LogLevel.Warn, "Failed to render oscilloscope, disabling: {0}", e.Message);
                success = false;
#endif
            }

            await Windows.Invoke(() =>
            {
                bitmap.AddDirtyRect(new global::System.Windows.Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                bitmap.Unlock();
            }, DISPATCHER_PRIORITY).ConfigureAwait(false);

            if (!success)
            {
                return;
            }
            this.Restart();
        }

        protected override void OnElapsed(object sender, ElapsedEventArgs e)
        {
            var data = this.RendererData;
            if (data == null)
            {
                this.Restart();
                return;
            }
            try
            {
                if (!data.Update())
                {
                    this.Restart();
                    return;
                }
                UpdateValues(data);

                switch (data.Mode)
                {
                    default:
                    case OscilloscopeRendererMode.Mono:
                        if (!data.LastUpdated.Equals(default(DateTime)))
                        {
                            UpdateElementsSmoothMono(data.Values, data.Peaks, data.Elements, data.Width, data.Height);
                        }
                        else
                        {
                            UpdateElementsFastMono(data.Values, data.Peaks, data.Elements, data.Width, data.Height);
                        }
                        break;
                    case OscilloscopeRendererMode.Seperate:
                        if (!data.LastUpdated.Equals(default(DateTime)))
                        {
                            UpdateElementsSmoothSeperate(data.Values, data.Peaks, data.Elements, data.Width, data.Height, data.Channels);
                        }
                        else
                        {
                            UpdateElementsFastSeperate(data.Values, data.Peaks, data.Elements, data.Width, data.Height, data.Channels);
                        }
                        break;
                }

                var task = this.Render(data);
            }
            catch (Exception exception)
            {
#if DEBUG
                Logger.Write(this.GetType(), LogLevel.Warn, "Failed to update oscilloscope data: {0}", exception.Message);
                this.Restart();
#else
                Logger.Write(this.GetType(), LogLevel.Warn, "Failed to update oscilloscope data, disabling: {0}", exception.Message);
#endif
            }
        }

        private static void UpdateElementsFastMono(float[,] values, float[] peaks, Int32Point[,] elements, int width, int height)
        {
            if (values.Length == 0 || peaks[0] == 0.0f)
            {
                return;
            }
            height = height - 1;
            var center = height / 2;
            var peak = peaks[0];
            for (var x = 0; x < width; x++)
            {
                var y = default(int);
                var value = values[0, x];
                if (value < 0)
                {
                    y = center + Convert.ToInt32((Math.Abs(value) / peak) * (height / 2));
                }
                else if (value > 0)
                {
                    y = center - Convert.ToInt32((Math.Abs(value) / peak) * (height / 2));
                }
                else
                {
                    y = center;
                }
                elements[0, x].X = x;
                elements[0, x].Y = y;
            }
        }

        private static void UpdateElementsFastSeperate(float[,] values, float[] peaks, Int32Point[,] elements, int width, int height, int channels)
        {
            if (values.Length == 0 || channels == 0)
            {
                return;
            }
            height = height - 1;
            height = height / channels;
            for (var channel = 0; channel < channels; channel++)
            {
                var center = (height * channel) + (height / 2);
                var peak = peaks[channel];
                if (peak == 0.0f)
                {
                    continue;
                }
                for (var x = 0; x < width; x++)
                {
                    var y = default(int);
                    var value = values[channel, x];
                    if (value < 0)
                    {
                        y = center + Convert.ToInt32((Math.Abs(value) / peak) * (height / 2));
                    }
                    else if (value > 0)
                    {
                        y = center - Convert.ToInt32((Math.Abs(value) / peak) * (height / 2));
                    }
                    else
                    {
                        y = center;
                    }
                    elements[channel, x].X = x;
                    elements[channel, x].Y = y;
                }
            }
        }

        private static void UpdateElementsSmoothMono(float[,] values, float[] peaks, Int32Point[,] elements, int width, int height)
        {
            if (values.Length == 0 || peaks[0] == 0.0f)
            {
                return;
            }
            height = height - 1;
            height = height / 2;
            var center = height;
            var peak = peaks[0];
            var minChange = 1;
            var maxChange = height;
            for (var x = 0; x < width; x++)
            {
                var y = default(int);
                var value = values[0, x];
                if (value < 0)
                {
                    y = center + Convert.ToInt32((Math.Abs(value) / peak) * height);
                }
                else if (value > 0)
                {
                    y = center - Convert.ToInt32((Math.Abs(value) / peak) * height);
                }
                else
                {
                    y = center;
                }
                elements[0, x].X = x;
                Animate(ref elements[0, x].Y, y, center - height, center + height, minChange, maxChange);
            }
        }

        private static void UpdateElementsSmoothSeperate(float[,] values, float[] peaks, Int32Point[,] elements, int width, int height, int channels)
        {
            if (values.Length == 0 || channels == 0)
            {
                return;
            }
            height = height - 1;
            height = (height / channels) / 2;
            var minChange = 1;
            var maxChange = height;
            for (var channel = 0; channel < channels; channel++)
            {
                var center = ((height * 2) * channel) + height;
                var peak = peaks[channel];
                if (peak == 0.0f)
                {
                    continue;
                }
                for (var x = 0; x < width; x++)
                {
                    var y = default(int);
                    var value = values[channel, x];
                    if (value < 0)
                    {
                        y = center + Convert.ToInt32((Math.Abs(value) / peak) * height);
                    }
                    else if (value > 0)
                    {
                        y = center - Convert.ToInt32((Math.Abs(value) / peak) * height);
                    }
                    else
                    {
                        y = center;
                    }
                    elements[channel, x].X = x;
                    Animate(ref elements[channel, x].Y, y, center - height, center + height, minChange, maxChange);
                }
            }
        }

        protected override void OnDisposing()
        {
            if (this.Mode != null)
            {
                this.Mode.ValueChanged -= this.OnValueChanged;
            }
            if (this.Duration != null)
            {
                this.Duration.ValueChanged -= this.OnValueChanged;
            }
            base.OnDisposing();
        }

        private static void Render(BitmapHelper.RenderInfo info, OscilloscopeRendererData data)
        {
            BitmapHelper.Clear(ref info);

            if (data.SampleCount == 0)
            {
                //No data.
                return;
            }

            if (info.Width != data.Width || info.Height != data.Height)
            {
                //Bitmap does not match data.
                return;
            }

            BitmapHelper.DrawLines(ref info, data.Elements, data.Elements.GetLength(0), data.Elements.GetLength(1));
        }

        private static void UpdateValues(OscilloscopeRendererData data)
        {
            if (data.Width == 0 || data.Channels == 0 || data.SampleCount == 0)
            {
                return;
            }
            switch (data.Mode)
            {
                default:
                case OscilloscopeRendererMode.Mono:
                    UpdateValuesMono(data.Samples, data.Values, data.Width, data.SampleCount);
                    UpdateHistoryMono(data.Values, data.Peaks, data.Width, data.History, ref data.HistoryPosition, ref data.HistoryCount, data.HistoryCapacity);
                    break;
                case OscilloscopeRendererMode.Seperate:
                    UpdateValuesSeperate(data.Samples, data.Values, data.Channels, data.Width, data.SampleCount);
                    UpdateHistorySeperate(data.Values, data.Peaks, data.Channels, data.Width, data.History, ref data.HistoryPosition, ref data.HistoryCount, data.HistoryCapacity);
                    break;
            }
            data.LastUpdated = DateTime.UtcNow;
        }

        private static void UpdateValuesMono(float[,] samples, float[,] values, int width, int count)
        {
            var samplesPerValue = count / width;
            if (samplesPerValue == 1)
            {
                for (int x = 0; x < width; x++)
                {
                    values[0, x] = samples[0, x];
                }
            }
            else if (samplesPerValue > 1)
            {
                for (int a = 0, x = 0; a < count && x < width; a += samplesPerValue, x++)
                {
                    values[0, x] = 0.0f;
                    for (var b = 0; b < samplesPerValue; b++)
                    {
                        values[0, x] += samples[0, a + b];
                    }
                    values[0, x] /= samplesPerValue;
                }
            }
            else
            {
                var valuesPerSample = (float)width / count;
                for (var x = 0; x < width; x++)
                {
                    values[0, x] = samples[0, Convert.ToInt32(x / valuesPerSample)];
                }
            }
        }

        private static void UpdateHistoryMono(float[,] values, float[] peaks, int width, float[,,] history, ref int position, ref int count, int capacity)
        {
            peaks[0] = 0.1f;
            for (var x = 0; x < width; x++)
            {
                history[0, x, position] = values[0, x];
                if (count > 1)
                {
                    values[0, x] = 0;
                    for (var a = 0; a < count; a++)
                    {
                        values[0, x] += history[0, x, (a + position) % count];
                    }
                    values[0, x] /= count;
                }
                peaks[0] = Math.Max(Math.Abs(values[0, x]), peaks[0]);
            }
            if (position < capacity - 1)
            {
                position++;
            }
            else
            {
                position = 0;
            }
            if (count < capacity)
            {
                count++;
            }
        }

        private static void UpdateValuesSeperate(float[,] samples, float[,] values, int channels, int width, int count)
        {
            var samplesPerValue = (count / channels) / width;
            if (samplesPerValue == 1)
            {
                for (var x = 0; x < width; x++)
                {
                    for (var channel = 0; channel < channels; channel++)
                    {
                        values[channel, x] = samples[channel, x];
                    }
                }
            }
            else if (samplesPerValue > 1)
            {
                for (int a = 0, x = 0; a < count && x < width; a += samplesPerValue, x++)
                {
                    for (var channel = 0; channel < channels; channel++)
                    {
                        values[channel, x] = 0.0f;
                        for (var b = 0; b < samplesPerValue; b++)
                        {
                            values[channel, x] += samples[channel, a + b];
                        }
                        values[channel, x] /= samplesPerValue;
                    }
                }
            }
            else
            {
                var valuesPerSample = (float)width / count;
                for (var x = 0; x < width; x++)
                {
                    for (var channel = 0; channel < channels; channel++)
                    {
                        values[channel, x] = samples[channel, Convert.ToInt32(x / valuesPerSample)];
                    }
                }
            }
        }

        private static void UpdateHistorySeperate(float[,] values, float[] peaks, int channels, int width, float[,,] history, ref int position, ref int count, int capacity)
        {
            for (var channel = 0; channel < channels; channel++)
            {
                peaks[channel] = 0.1f;
            }
            for (var x = 0; x < width; x++)
            {
                for (var channel = 0; channel < channels; channel++)
                {
                    history[channel, x, position] = values[channel, x];
                    if (count > 1)
                    {
                        values[channel, x] = 0;
                        for (var a = 0; a < count; a++)
                        {
                            values[channel, x] += history[channel, x, (a + position) % count];
                        }
                        values[channel, x] /= count;
                    }
                    peaks[channel] = Math.Max(Math.Abs(values[channel, x]), peaks[channel]);
                }
            }
            if (position < capacity - 1)
            {
                position++;
            }
            else
            {
                position = 0;
            }
            if (count < capacity)
            {
                count++;
            }
        }

        public static OscilloscopeRendererData Create(IOutputDataSource outputDataSource, int width, int height, TimeSpan window, TimeSpan duration, OscilloscopeRendererMode mode)
        {
            var data = new OscilloscopeRendererData()
            {
                OutputDataSource = outputDataSource,
                Width = width,
                Height = height,
                Window = window,
                Duration = duration,
                Mode = mode
            };
            return data;
        }

        public class OscilloscopeRendererData
        {
            public IOutputDataSource OutputDataSource;

            public int Width;

            public int Height;

            public int Rate;

            public int Channels;

            public OutputStreamFormat Format;

            public short[] Samples16;

            public float[] Samples32;

            public float[,] Samples;

            public int SampleCount;

            public float[,,] History;

            public int HistoryPosition;

            public int HistoryCount;

            public int HistoryCapacity;

            public float[,] Values;

            public float[] Peaks;

            public Int32Point[,] Elements;

            public DateTime LastUpdated;

            public TimeSpan Window;

            public TimeSpan Duration;

            public OscilloscopeRendererMode Mode;

            public bool Update()
            {
                var rate = default(int);
                var channels = default(int);
                var format = default(OutputStreamFormat);
                if (!this.OutputDataSource.GetDataFormat(out rate, out channels, out format))
                {
                    return false;
                }
                this.Update(rate, channels, format);
                switch (this.Format)
                {
                    case OutputStreamFormat.Short:
                        this.SampleCount = this.OutputDataSource.GetData(this.Samples16);
                        for (var a = 0; a < this.SampleCount; a++)
                        {
                            this.Samples32[a] = (float)this.Samples16[a] / short.MaxValue;
                        }
                        break;
                    case OutputStreamFormat.Float:
                        this.SampleCount = this.OutputDataSource.GetData(this.Samples32);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                if (this.Rate > 0 && this.Channels > 0 && this.SampleCount > 0)
                {
                    switch (this.Mode)
                    {
                        default:
                        case OscilloscopeRendererMode.Mono:
                            this.SampleCount = DownmixMono(this.Samples, this.Samples32, this.Channels, this.SampleCount);
                            break;
                        case OscilloscopeRendererMode.Seperate:
                            this.SampleCount = Deinterlace(this.Samples, this.Samples32, this.Channels, this.SampleCount);
                            break;
                    }
                    return true;
                }
                return false;
            }

            private void Update(int rate, int channels, OutputStreamFormat format)
            {
                if (this.Rate == rate && this.Channels == channels && this.Format == format)
                {
                    return;
                }

                this.Rate = rate;
                this.Channels = channels;
                this.Format = format;

                switch (format)
                {
                    case OutputStreamFormat.Short:
                        this.Samples16 = this.OutputDataSource.GetBuffer<short>(this.Window);
                        this.Samples32 = new float[this.Samples16.Length];
                        break;
                    case OutputStreamFormat.Float:
                        this.Samples32 = this.OutputDataSource.GetBuffer<float>(this.Window);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                this.HistoryCapacity = Math.Max(Convert.ToInt32(this.Duration.TotalMilliseconds / this.Window.TotalMilliseconds), 1);

                //TODO: Only realloc if required.
                switch (this.Mode)
                {
                    default:
                    case OscilloscopeRendererMode.Mono:
                        this.History = new float[1, this.Width, this.HistoryCapacity];
                        this.Samples = new float[1, this.Samples32.Length];
                        this.Values = new float[1, this.Width];
                        this.Elements = CreateElements(1, this.Width, this.Height);
                        this.Peaks = new float[1];
                        break;
                    case OscilloscopeRendererMode.Seperate:
                        this.History = new float[this.Channels, this.Width, this.HistoryCapacity];
                        this.Samples = new float[this.Channels, this.Samples32.Length];
                        this.Values = new float[this.Channels, this.Width];
                        this.Elements = CreateElements(this.Channels, this.Width, this.Height);
                        this.Peaks = new float[this.Channels];
                        break;
                }
            }

            private Int32Point[,] CreateElements(int channels, int width, int height)
            {
                if (channels == 0)
                {
                    return null;
                }
                height = height / channels;
                var elements = new Int32Point[channels, width];
                for (var channel = 0; channel < channels; channel++)
                {
                    var center = (height * channel) + (height / 2);
                    for (var x = 0; x < width; x++)
                    {
                        elements[channel, x].X = x;
                        elements[channel, x].Y = center;
                    }
                }
                return elements;
            }
        }
    }

    public enum OscilloscopeRendererMode : byte
    {
        None,
        Mono,
        Seperate
    }
}
