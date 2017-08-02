﻿using FoxTunes.Interfaces;
using System;
using System.Threading.Tasks;

namespace FoxTunes.Managers
{
    public class PlaybackManager : StandardManager, IPlaybackManager
    {
        public ICore Core { get; private set; }

        public IOutput Output { get; private set; }

        public IOutputStreamQueue OutputStreamQueue { get; private set; }

        public IForegroundTaskRunner ForegroundTaskRunner { get; private set; }

        public override void InitializeComponent(ICore core)
        {
            this.Core = core;
            this.Output = core.Components.Output;
            this.OutputStreamQueue = core.Components.OutputStreamQueue;
            this.OutputStreamQueue.Dequeued += this.OutputStreamQueueDequeued;
            this.ForegroundTaskRunner = core.Components.ForegroundTaskRunner;
            base.InitializeComponent(core);
        }

        protected virtual void OutputStreamQueueDequeued(object sender, OutputStreamQueueEventArgs e)
        {
            Logger.Write(this, LogLevel.Debug, "Output stream de-queued, loading it: {0} => {1}", e.OutputStream.Id, e.OutputStream.FileName);
            this.Interlocked(async () =>
            {
                if (this.CurrentStream != null)
                {
                    Logger.Write(this, LogLevel.Debug, "Unloading current stream: {0} => {1}", this.CurrentStream.Id, this.CurrentStream.FileName);
                    await this.Unload();
                }
                await this.ForegroundTaskRunner.Run(() => this.CurrentStream = e.OutputStream);
                Logger.Write(this, LogLevel.Debug, "Output stream loaded: {0} => {1}", this.CurrentStream.Id, this.CurrentStream.FileName);
            }).Wait();
        }

        public bool IsSupported(string fileName)
        {
            return this.Output.IsSupported(fileName);
        }

        private IOutputStream _CurrentStream { get; set; }

        public IOutputStream CurrentStream
        {
            get
            {
                return this._CurrentStream;
            }
            private set
            {
                this._CurrentStream = value;
                this.OnCurrentStreamChanged();
            }
        }

        protected virtual void OnCurrentStreamChanged()
        {
            if (this.CurrentStreamChanged != null)
            {
                this.CurrentStreamChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("CurrentStream");
        }

        public event EventHandler CurrentStreamChanged = delegate { };

        public Task Load(PlaylistItem playlistItem, bool immediate)
        {
            var task = new LoadOutputStreamTask(playlistItem, immediate);
            task.InitializeComponent(this.Core);
            this.OnBackgroundTask(task);
            return task.Run();
        }

        public Task Unload()
        {
            if (this.CurrentStream == null)
            {
                return Task.CompletedTask;
            }
            var task = new UnloadOutputStreamTask();
            task.InitializeComponent(this.Core);
            this.OnBackgroundTask(task);
            return task.Run();
        }

        public Task Stop()
        {
            if (this.CurrentStream == null)
            {
                return Task.CompletedTask;
            }
            var task = new StopOutputStreamTask();
            task.InitializeComponent(this.Core);
            this.OnBackgroundTask(task);
            return task.Run();
        }

        protected virtual void OnBackgroundTask(IBackgroundTask backgroundTask)
        {
            if (this.BackgroundTask == null)
            {
                return;
            }
            this.BackgroundTask(this, new BackgroundTaskEventArgs(backgroundTask));
        }

        public event BackgroundTaskEventHandler BackgroundTask = delegate { };

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed || !disposing)
            {
                return;
            }
            this.OnDisposing();
            this.IsDisposed = true;
        }

        protected virtual void OnDisposing()
        {
            this.Unload();
        }
    }
}
