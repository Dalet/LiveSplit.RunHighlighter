﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiveSplit.RunHighlighter.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LiveSplit.RunHighlighter.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap help {
            get {
                object obj = ResourceManager.GetObject("help", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to var count = 0;
        ///
        ///function markerLoop() {
        ///    setTimeout(function () {
        ///        $(&apos;input.start-time.string&apos;).trigger(&apos;change&apos;);
        ///        if ($(&apos;div.left-marker&apos;).attr(&apos;title&apos;) !== &quot;{start_time_str}&quot;
        ///            || $(&apos;div.right-marker&apos;).attr(&apos;title&apos;) !== &quot;{end_time_str}&quot;) {
        ///            if (!{out_of_vid}) { //no loop if the video is incomplete
        ///                markerLoop();
        ///            }
        ///        }
        ///    }, 100);
        ///}
        ///
        ///function playerLoop() {
        ///    var player = $(&apos;div#player object&apos;)[0];
        ///    setTimeout(fun [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string highlightInjection {
            get {
                return ResourceManager.GetString("highlightInjection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*--- waitForKeyElements():  A utility function, for Greasemonkey scripts,
        ///    that detects and handles AJAXed content.
        ///
        ///    Usage example:
        ///
        ///        waitForKeyElements (
        ///            &quot;div.comments&quot;
        ///            , commentCallbackFunction
        ///        );
        ///
        ///        //--- Page-specific function to do what we want when the node is found.
        ///        function commentCallbackFunction (jNode) {
        ///            jNode.text (&quot;This comment changed by waitForKeyElements().&quot;);
        ///        }
        ///
        ///    IMPORTANT: This function requires your script [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string waitForKeyElements {
            get {
                return ResourceManager.GetString("waitForKeyElements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap warning {
            get {
                object obj = ResourceManager.GetObject("warning", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
