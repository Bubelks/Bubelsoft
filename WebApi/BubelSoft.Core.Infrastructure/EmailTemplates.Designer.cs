﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BubelSoft.Core.Infrastructure {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class EmailTemplates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EmailTemplates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BubelSoft.Core.Infrastructure.EmailTemplates", typeof(EmailTemplates).Assembly);
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
        ///   Looks up a localized string similar to &lt;p&gt;
        ///                Hi,
        ///            &lt;/p&gt;
        ///            &lt;p&gt;
        ///                Your company has been invited to &lt;b&gt;#BuildingName&lt;/b&gt; as sub-contractor by &lt;b&gt;#UserFirstName #UserLastName&lt;/b&gt;.
        ///            &lt;/p&gt;
        ///            &lt;p&gt;
        ///                Click on the button below to accept invitation and registry company.
        ///            &lt;/p&gt;
        ///
        ///            &lt;a class=&quot;button&quot; href=&quot;#Link&quot;&gt;REGISTER&lt;/a&gt;
        ///
        ///            &lt;p&gt;
        ///                Please skiped this mail, if you should not get it.
        ///            &lt;/p&gt;.
        /// </summary>
        internal static string CompanyInvited {
            get {
                return ResourceManager.GetString("CompanyInvited", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///	&lt;style&gt;
        ///        body {
        ///            background-color: #f3f3f3;
        ///        }
        ///        .logo {
        ///            background-color: #048299;
        ///            color: white;
        ///            font-weight: bold;
        ///            font-size: 105px;
        ///            text-align: center;
        ///        }
        ///        .content {
        ///            background-color: white;
        ///            padding: 15px;
        ///        }
        ///        .button {
        ///            background-color: #048299;
        ///            color: white;
        ///            opacity: 0.75;
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FullBody {
            get {
                return ResourceManager.GetString("FullBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;
        ///                Hi &lt;b&gt;#UserFirstName #UserLastName&lt;/b&gt;,
        ///            &lt;/p&gt;
        ///            &lt;p&gt;
        ///                You have been added to &lt;b&gt;#CompanyName&lt;/b&gt; as &lt;b&gt;#UserCompanyRole&lt;/b&gt; by &lt;b&gt;#AdminFirstName #AdminLastName&lt;/b&gt;.
        ///            &lt;/p&gt;
        ///            &lt;p&gt;
        ///                Click on the button below to finish account creation.
        ///            &lt;/p&gt;
        ///
        ///            &lt;a class=&quot;button&quot; href=&quot;#Link&quot;&gt;REGISTER&lt;/a&gt;
        ///
        ///            &lt;p&gt;
        ///                Please skiped this mail, if you should not get it.
        ///            &lt;/p&gt;.
        /// </summary>
        internal static string WorkerAdded {
            get {
                return ResourceManager.GetString("WorkerAdded", resourceCulture);
            }
        }
    }
}
