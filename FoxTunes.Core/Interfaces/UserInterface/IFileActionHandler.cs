﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoxTunes.Interfaces
{
    public interface IFileActionHandler : IBaseComponent
    {
        bool CanHandle(string path, FileActionType type);

        Task Handle(IEnumerable<string> paths, FileActionType type);
    }

    public enum FileActionType : byte
    {
        None,
        Playlist,
        Library
    }
}
