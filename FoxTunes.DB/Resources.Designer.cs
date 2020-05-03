﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoxTunes {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FoxTunes.DB.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WITH
        ///&quot;LibraryHierarchyItems_Lookup&quot; AS
        ///(
        ///	SELECT * 
        ///	FROM [LibraryHierarchyItems]
        ///	WHERE ((@parentId IS NULL AND [Parent_Id] IS NULL) OR [Parent_Id] = @parentId)
        ///		AND [LibraryHierarchy_Id] = @libraryHierarchyId
        ///		AND [Value] = @value
        ///		AND [IsLeaf] = @isLeaf
        ///)
        ///
        ///INSERT INTO [LibraryHierarchyItems] ([Parent_Id], [LibraryHierarchy_Id], [Value], [IsLeaf])
        ///SELECT @parentId, @libraryHierarchyId, @value, @isLeaf
        ///WHERE NOT EXISTS(SELECT * FROM &quot;LibraryHierarchyItems_Lookup&quot;);
        ///
        ///WITH
        ///&quot;LibraryHierarch [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AddLibraryHierarchyNode {
            get {
                return ResourceManager.GetString("AddLibraryHierarchyNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;LibraryItem_MetaDataItem&quot; (&quot;LibraryItem_Id&quot;, &quot;MetaDataItem_Id&quot;)
        ///SELECT @itemId, @metaDataItemId
        ///WHERE NOT EXISTS(
        ///	SELECT *
        ///	FROM &quot;LibraryItem_MetaDataItem&quot; 
        ///	WHERE &quot;LibraryItem_Id&quot; = @itemId AND &quot;MetaDataItem_Id&quot; = @metaDataItemId
        ///);.
        /// </summary>
        internal static string AddLibraryMetaDataItem {
            get {
                return ResourceManager.GetString("AddLibraryMetaDataItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;PlaylistItem_MetaDataItem&quot; (&quot;PlaylistItem_Id&quot;, &quot;MetaDataItem_Id&quot;)
        ///SELECT @itemId, @metaDataItemId
        ///WHERE NOT EXISTS(
        ///	SELECT *
        ///	FROM &quot;PlaylistItem_MetaDataItem&quot; 
        ///	WHERE &quot;PlaylistItem_Id&quot; = @itemId AND &quot;MetaDataItem_Id&quot; = @metaDataItemId
        ///);.
        /// </summary>
        internal static string AddPlaylistMetaDataItem {
            get {
                return ResourceManager.GetString("AddPlaylistMetaDataItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM &quot;LibraryItem_MetaDataItem&quot;
        ///WHERE &quot;Id&quot; IN
        ///(
        ///	SELECT &quot;LibraryItem_MetaDataItem&quot;.&quot;Id&quot;
        ///	FROM &quot;LibraryItem_MetaDataItem&quot;
        ///		JOIN &quot;MetaDataItems&quot;
        ///			ON &quot;LibraryItem_MetaDataItem&quot;.&quot;MetaDataItem_Id&quot; = &quot;MetaDataItems&quot;.&quot;Id&quot;
        ///				AND (@type &amp; &quot;MetaDataItems&quot;.&quot;Type&quot;) =  &quot;MetaDataItems&quot;.&quot;Type&quot;
        ///	WHERE &quot;LibraryItem_MetaDataItem&quot;.&quot;LibraryItem_Id&quot; = @itemId
        ///).
        /// </summary>
        internal static string ClearLibraryMetaDataItems {
            get {
                return ResourceManager.GetString("ClearLibraryMetaDataItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM &quot;PlaylistItem_MetaDataItem&quot;
        ///WHERE &quot;Id&quot; IN
        ///(
        ///	SELECT &quot;PlaylistItem_MetaDataItem&quot;.&quot;Id&quot;
        ///	FROM &quot;PlaylistItem_MetaDataItem&quot;
        ///		JOIN &quot;MetaDataItems&quot;
        ///			ON &quot;PlaylistItem_MetaDataItem&quot;.&quot;MetaDataItem_Id&quot; = &quot;MetaDataItems&quot;.&quot;Id&quot;
        ///				AND (@type &amp; &quot;MetaDataItems&quot;.&quot;Type&quot;) =  &quot;MetaDataItems&quot;.&quot;Type&quot;
        ///	WHERE &quot;PlaylistItem_MetaDataItem&quot;.&quot;PlaylistItem_Id&quot; = @itemId
        ///).
        /// </summary>
        internal static string ClearPlaylistMetaDataItems {
            get {
                return ResourceManager.GetString("ClearPlaylistMetaDataItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT &quot;MetaDataItems&quot;.*
        ///FROM &quot;MetaDataItems&quot;
        ///	JOIN &quot;LibraryItem_MetaDataItem&quot; ON &quot;LibraryItem_MetaDataItem&quot;.&quot;MetaDataItem_Id&quot; = &quot;MetaDataItems&quot;.&quot;Id&quot;
        ///WHERE &quot;LibraryItem_MetaDataItem&quot;.&quot;LibraryItem_Id&quot; = @libraryItemId 
        ///	AND (@type &amp; &quot;MetaDataItems&quot;.&quot;Type&quot;) =  &quot;MetaDataItems&quot;.&quot;Type&quot;.
        /// </summary>
        internal static string GetLibraryMetaData {
            get {
                return ResourceManager.GetString("GetLibraryMetaData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;MetaDataItems&quot; (&quot;Name&quot;, &quot;Type&quot;, &quot;Value&quot;) 
        ///SELECT @name, @type, @value
        ///WHERE NOT EXISTS(
        ///	SELECT *
        ///	FROM &quot;MetaDataItems&quot; 
        ///	WHERE &quot;Name&quot; = @name AND &quot;Type&quot; = @type AND &quot;Value&quot; = @value
        ///);
        ///
        ///SELECT &quot;Id&quot;
        ///FROM &quot;MetaDataItems&quot; 
        ///WHERE &quot;Name&quot; = @name AND &quot;Type&quot; = @type AND &quot;Value&quot; = @value;.
        /// </summary>
        internal static string GetOrAddMetaDataItem {
            get {
                return ResourceManager.GetString("GetOrAddMetaDataItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WITH &quot;CurrentSequence&quot;
        ///AS
        ///(	
        ///		SELECT &quot;Sequence&quot; 
        ///		FROM &quot;PlaylistItems&quot;
        ///		WHERE &quot;Id&quot; = @id
        ///)
        ///
        ///UPDATE &quot;PlaylistItems&quot;
        ///SET &quot;Sequence&quot; = 
        ///(
        ///	CASE WHEN (SELECT &quot;Sequence&quot; FROM &quot;CurrentSequence&quot;) &lt; @sequence THEN
        ///		&quot;Sequence&quot; - 1
        ///	ELSE
        ///		&quot;Sequence&quot; + 1
        ///	END
        ///)
        ///WHERE 
        ///(
        ///	(SELECT &quot;Sequence&quot; FROM &quot;CurrentSequence&quot;) &lt; @sequence 
        ///		AND &quot;Sequence&quot; BETWEEN (SELECT &quot;Sequence&quot; FROM &quot;CurrentSequence&quot;) 
        ///			AND @sequence
        ///) 
        ///OR 
        ///(
        ///	&quot;Sequence&quot; BETWEEN @sequence 
        ///		AND (SELECT &quot;Sequence&quot; FROM &quot;Current [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MovePlaylistItem {
            get {
                return ResourceManager.GetString("MovePlaylistItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [LibraryItems]
        ///WHERE NOT EXISTS
        ///(
        ///		SELECT *
        ///		FROM [LibraryHierarchyItem_LibraryItem]
        ///		WHERE [LibraryHierarchyItem_LibraryItem].[LibraryItem_Id] = [LibraryItems].[Id]
        ///).
        /// </summary>
        internal static string RemoveCancelledLibraryItems {
            get {
                return ResourceManager.GetString("RemoveCancelledLibraryItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [LibraryHierarchyItem_LibraryItem]
        ///WHERE [LibraryItem_Id] IN
        ///(
        ///	SELECT [Id] 
        ///	FROM [LibraryItems]
        ///	WHERE @status IS NULL OR [Status] = @status
        ///);
        ///
        ///DELETE FROM [LibraryHierarchyItems]
        ///WHERE [Id] IN
        ///(
        ///	SELECT [LibraryHierarchyItems].[Id]
        ///	FROM [LibraryHierarchyItems]
        ///		 LEFT JOIN [LibraryHierarchyItem_LibraryItem]
        ///			ON [LibraryHierarchyItems].[Id] = [LibraryHierarchyItem_LibraryItem].[LibraryHierarchyItem_Id]
        ///	WHERE [LibraryHierarchyItem_LibraryItem].[Id] IS NULL
        ///);.
        /// </summary>
        internal static string RemoveLibraryHierarchyItems {
            get {
                return ResourceManager.GetString("RemoveLibraryHierarchyItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [LibraryItem_MetaDataItem]
        ///WHERE [LibraryItem_Id] IN
        ///(
        ///	SELECT [Id]
        ///	FROM [LibraryItems]
        ///	WHERE @status IS NULL OR [LibraryItems].[Status] = @status
        ///);
        ///
        ///DELETE FROM [PlaylistItem_MetaDataItem]
        ///WHERE [PlaylistItem_Id] IN
        ///(
        ///	SELECT [PlaylistItems].[Id]
        ///	FROM [PlaylistItems]
        ///		JOIN [LibraryItems] ON [PlaylistItems].[LibraryItem_Id] = [LibraryItems].[Id]
        ///	WHERE @status IS NULL OR [LibraryItems].[Status] = @status
        ///);
        ///
        ///DELETE FROM [PlaylistItems]
        ///WHERE [Id] IN
        ///(
        ///	SELECT [PlaylistIt [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RemoveLibraryItems {
            get {
                return ResourceManager.GetString("RemoveLibraryItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [PlaylistItem_MetaDataItem]
        ///WHERE [PlaylistItem_Id] IN
        ///(
        ///	SELECT [Id]
        ///	FROM [PlaylistItems]
        ///	WHERE [Status] = @status
        ///);
        ///
        ///DELETE FROM [PlaylistItems]
        ///WHERE [Status] = @status.
        /// </summary>
        internal static string RemovePlaylistItems {
            get {
                return ResourceManager.GetString("RemovePlaylistItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO [LibraryHierarchyItem_LibraryItem] ([LibraryHierarchyItem_Id], [LibraryItem_Id])
        ///SELECT @libraryHierarchyItemId, @libraryItemId
        ///WHERE NOT EXISTS(
        ///	SELECT * 
        ///	FROM [LibraryHierarchyItem_LibraryItem]
        ///	WHERE [LibraryHierarchyItem_Id] = @libraryHierarchyItemId
        ///		AND [LibraryItem_Id] = @libraryItemId
        ///);.
        /// </summary>
        internal static string UpdateLibraryHierarchyNode {
            get {
                return ResourceManager.GetString("UpdateLibraryHierarchyNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;MetaDataItems&quot; (&quot;Name&quot;, &quot;Type&quot;, &quot;Value&quot;)
        ///SELECT @name, @type, @value
        ///WHERE NOT EXISTS(SELECT * FROM &quot;MetaDataItems&quot; WHERE &quot;Name&quot; = @name AND &quot;Type&quot; = @type AND &quot;Value&quot; = @value);
        ///
        ///WITH &quot;MetaData&quot;
        ///AS
        ///(
        ///	SELECT 
        ///		&quot;LibraryItem_MetaDataItem&quot;.&quot;LibraryItem_Id&quot; AS &quot;Id&quot;,
        ///		&quot;MetaDataItems&quot;.&quot;Name&quot;,
        ///		&quot;MetaDataItems&quot;.&quot;Value&quot;
        ///	FROM &quot;LibraryItem_MetaDataItem&quot; 
        ///		JOIN &quot;MetaDataItems&quot; 
        ///			ON &quot;MetaDataItems&quot;.&quot;Id&quot; = &quot;LibraryItem_MetaDataItem&quot;.&quot;MetaDataItem_Id&quot;
        ///),
        ///
        ///&quot;Artist&quot;
        ///AS
        ///(
        ///	SELECT &quot;Arti [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UpdateLibraryVariousArtists {
            get {
                return ResourceManager.GetString("UpdateLibraryVariousArtists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;MetaDataItems&quot; (&quot;Name&quot;, &quot;Type&quot;, &quot;Value&quot;)
        ///SELECT @name, @type, @value
        ///WHERE NOT EXISTS(SELECT * FROM &quot;MetaDataItems&quot; WHERE &quot;Name&quot; = @name AND &quot;Type&quot; = @type AND &quot;Value&quot; = @value);
        ///
        ///WITH &quot;MetaData&quot;
        ///AS
        ///(
        ///	SELECT 
        ///		&quot;PlaylistItem_MetaDataItem&quot;.&quot;PlaylistItem_Id&quot; AS &quot;Id&quot;,
        ///		&quot;MetaDataItems&quot;.&quot;Name&quot;,
        ///		&quot;MetaDataItems&quot;.&quot;Value&quot;
        ///	FROM &quot;PlaylistItem_MetaDataItem&quot; 
        ///		JOIN &quot;MetaDataItems&quot; 
        ///			ON &quot;MetaDataItems&quot;.&quot;Id&quot; = &quot;PlaylistItem_MetaDataItem&quot;.&quot;MetaDataItem_Id&quot;
        ///),
        ///
        ///&quot;Artist&quot;
        ///AS
        ///(
        ///	SELECT &quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UpdatePlaylistVariousArtists {
            get {
                return ResourceManager.GetString("UpdatePlaylistVariousArtists", resourceCulture);
            }
        }
    }
}
