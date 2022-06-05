﻿using FoxTunes.Interfaces;
using ManagedBass.Gapless;
using System;
using System.Collections.Generic;
using System.IO;

namespace FoxTunes
{
    [Component("9B40FE6A-89F1-4F97-888C-05D7B34EC42A", ComponentSlots.None, priority: ComponentAttribute.PRIORITY_HIGH)]
    [ComponentDependency(Slot = ComponentSlots.Output)]
    public class BassGaplessStreamInputBehaviour : StandardBehaviour, IConfigurableComponent, IDisposable
    {
        public static string Location
        {
            get
            {
                return Path.GetDirectoryName(typeof(BassGaplessStreamInputBehaviour).Assembly.Location);
            }
        }

        public BassGaplessStreamInputBehaviour()
        {
            BassPluginLoader.AddPath(Path.Combine(Location, "Addon"));
            BassPluginLoader.AddPath(Path.Combine(Loader.FolderName, "bass_gapless.dll"));
        }

        public ICore Core { get; private set; }

        public IBassOutput Output { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public IBassStreamPipelineFactory BassStreamPipelineFactory { get; private set; }

        new public bool IsInitialized { get; private set; }

        private bool _Enabled { get; set; }

        public bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                this._Enabled = value;
                Logger.Write(this, LogLevel.Debug, "Enabled = {0}", this.Enabled);
                //TODO: Bad .Wait().
                this.Output.Shutdown().Wait();
            }
        }

        public override void InitializeComponent(ICore core)
        {
            this.Core = core;
            this.Output = core.Components.Output as IBassOutput;
            this.Configuration = core.Components.Configuration;
            this.Configuration.GetElement<SelectionConfigurationElement>(
                BassOutputConfiguration.SECTION,
                BassOutputConfiguration.INPUT_ELEMENT
            ).ConnectValue(value => this.Enabled = string.Equals(value.Id, BassGaplessStreamInputConfiguration.INPUT_GAPLESS_OPTION, StringComparison.OrdinalIgnoreCase));
            this.BassStreamPipelineFactory = ComponentRegistry.Instance.GetComponent<IBassStreamPipelineFactory>();
            this.BassStreamPipelineFactory.CreatingPipeline += this.OnCreatingPipeline;
            base.InitializeComponent(core);
        }

        protected virtual void OnCreatingPipeline(object sender, CreatingPipelineEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }
            e.Input = new BassGaplessStreamInput(this, e.Stream.Flags);
            e.Input.InitializeComponent(this.Core);
        }

        public IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            return BassGaplessStreamInputConfiguration.GetConfigurationSections();
        }

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
            if (this.BassStreamPipelineFactory != null)
            {
                this.BassStreamPipelineFactory.CreatingPipeline -= this.OnCreatingPipeline;
            }
        }

        ~BassGaplessStreamInputBehaviour()
        {
            Logger.Write(this, LogLevel.Error, "Component was not disposed: {0}", this.GetType().Name);
            try
            {
                this.Dispose(true);
            }
            catch
            {
                //Nothing can be done, never throw on GC thread.
            }
        }
    }

    public delegate void BassGaplessEventHandler(object sender, BassGaplessEventArgs e);
}