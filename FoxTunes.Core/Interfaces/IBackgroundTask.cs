﻿using System;
using System.Threading.Tasks;

namespace FoxTunes.Interfaces
{
    public interface IBackgroundTask : IReportsProgress
    {
        string Id { get; }

        bool Visible { get; }

        event EventHandler Started;

        event EventHandler Completed;

        Exception Exception { get; }

        event EventHandler ExceptionChanged;

        event EventHandler Faulted;

        Task Run();

        void RunSynchronously();
    }
}
