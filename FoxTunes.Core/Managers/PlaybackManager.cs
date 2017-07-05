﻿using FoxTunes.Interfaces;
using System;

namespace FoxTunes.Managers
{
    public class PlaybackManager : StandardManager, IPlaybackManager
    {
        public IOutput Output { get; private set; }

        public override void InitializeComponent(ICore core)
        {
            this.Output = core.Components.Output;
            base.InitializeComponent(core);
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
            set
            {
                this.OnCurrentStreamChanging();
                this._CurrentStream = value;
                this.OnCurrentStreamChanged();
            }
        }

        protected virtual void OnCurrentStreamChanging()
        {
            if (this.CurrentStreamChanging != null)
            {
                this.CurrentStreamChanging(this, EventArgs.Empty);
            }
            this.OnPropertyChanging("CurrentStream");
        }

        public event EventHandler CurrentStreamChanging = delegate { };

        protected virtual void OnCurrentStreamChanged()
        {
            if (this.CurrentStreamChanged != null)
            {
                this.CurrentStreamChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("CurrentStream");
        }

        public event EventHandler CurrentStreamChanged = delegate { };

        public void Load(string fileName)
        {
            this.CurrentStream = this.Output.Load(fileName);
        }

        public void Unload()
        {
            this.CurrentStream.Dispose();
            this.CurrentStream = null;
        }
    }
}
