﻿using FoxTunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FoxTunes
{
    public class LibraryItemScriptRunner
    {
        public LibraryItemScriptRunner(IScriptingContext scriptingContext, LibraryItem libraryItem, string script)
        {
            this.ScriptingContext = scriptingContext;
            this.LibraryItem = libraryItem;
            this.Script = script;
        }

        public IScriptingContext ScriptingContext { get; private set; }

        public LibraryItem LibraryItem { get; private set; }

        public string Script { get; private set; }

        public void Prepare()
        {
            var collections = new Dictionary<MetaDataItemType, Dictionary<string, object>>()
            {
                { MetaDataItemType.Tag, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) },
                { MetaDataItemType.Property, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) }
            };
            var fileName = default(string);
            var directoryName = default(string);
            if (this.LibraryItem != null)
            {
                if (this.LibraryItem.MetaDatas != null)
                {
                    lock (this.LibraryItem.MetaDatas)
                    {
                        foreach (var item in this.LibraryItem.MetaDatas)
                        {
                            if (!collections.ContainsKey(item.Type))
                            {
                                continue;
                            }
                            var key = item.Name.ToLower();
                            if (collections[item.Type].ContainsKey(key))
                            {
                                //Not sure what to do. Doesn't happen often.
                                continue;
                            }
                            collections[item.Type].Add(key, item.Value);
                        }
                    }
                }
                fileName = this.LibraryItem.FileName;
                directoryName = this.LibraryItem.DirectoryName;
            }
            this.ScriptingContext.SetValue("tag", collections[MetaDataItemType.Tag]);
            this.ScriptingContext.SetValue("property", collections[MetaDataItemType.Property]);
            this.ScriptingContext.SetValue("file", fileName);
            this.ScriptingContext.SetValue("folder", directoryName);
        }

        [DebuggerNonUserCode]
        public object Run()
        {
            const string RESULT = "__result";
            try
            {
                this.ScriptingContext.Run(string.Concat("var ", RESULT, " = ", this.Script, ";"));
            }
            catch (ScriptingException e)
            {
                return e.Message;
            }
            return this.ScriptingContext.GetValue(RESULT);
        }
    }
}
