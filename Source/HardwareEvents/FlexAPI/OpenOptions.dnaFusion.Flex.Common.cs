namespace OpenOptions.dnaFusion.Flex.Common
{
    using System;
    using CookComputing.XmlRpc;

    /// <summary>
    /// Abstract service
    /// </summary>
    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public interface IAFlexService_Async : CookComputing.XmlRpc.IXmlRpcProxy
    {
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public interface IFlexPermissions_Async : IAFlexService_Async
    {
        /// <summary>
        /// Methods that the supplied API key can invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        [XmlRpcBegin("FlexPermissions.CanInvoke")]
        System.IAsyncResult BeginCanInvoke(string apiKey, System.AsyncCallback @__Callback, object @__UserData);
        [XmlRpcEnd()]
        string[] EndCanInvoke(System.IAsyncResult @__AsyncResult);
        /// <summary>
        /// Methods that the supplied API key can not invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        [XmlRpcBegin("FlexPermissions.CanNotInvoke")]
        System.IAsyncResult BeginCanNotInvoke(string apiKey, System.AsyncCallback @__Callback, object @__UserData);
        [XmlRpcEnd()]
        string[] EndCanNotInvoke(System.IAsyncResult @__AsyncResult);
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public interface IFlexInfo_Async : IAFlexService_Async
    {
        /// <param name="Result"></param>
        [XmlRpcBegin("FlexInfo.Clients")]
        System.IAsyncResult BeginClients(System.AsyncCallback @__Callback, object @__UserData);
        [XmlRpcEnd()]
        FlexClient[] EndClients(System.IAsyncResult @__AsyncResult);
        /// <param name="Result"></param>
        [XmlRpcBegin("FlexInfo.Version")]
        System.IAsyncResult BeginVersion(System.AsyncCallback @__Callback, object @__UserData);
        [XmlRpcEnd()]
        string EndVersion(System.IAsyncResult @__AsyncResult);
    }

    /// <summary>
    /// Abstract service
    /// </summary>
    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public partial class AFlexService : CookComputing.XmlRpc.XmlRpcClientProtocol
    {
        [System.NonSerializedAttribute()]
        public const string FlexVersion = "1.24";
        [System.NonSerializedAttribute()]
        public const string FlexLibrary = "OpenOptions.dnaFusion.Flex.Common";
        public T EndInvoke<T>(IAsyncResult asr)
        {
            return (T)EndInvoke(asr, typeof(T));
        }
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public partial class XmlRpcProxy
    {
        public static T Create<T>(string url)
            where T : CookComputing.XmlRpc.IXmlRpcProxy
        {
            T result = CookComputing.XmlRpc.XmlRpcProxyGen.Create<T>();
            result.Url = url;
            return result;
        }
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    [System.SerializableAttribute()]
    public partial class ComplexType : System.ComponentModel.INotifyPropertyChanged
    {
        public ComplexType()
        {
        }
        [field: System.NonSerialized()]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void TriggerPropertyChanged(string propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [XmlRpcMissingMapping(CookComputing.XmlRpc.MappingAction.Ignore)]
    [System.SerializableAttribute()]
    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public partial class FlexClient : ComplexType
    {
        [System.NonSerializedAttribute()]
        public const string FlexVersion = "1.24";
        [System.NonSerializedAttribute()]
        public const string FlexLibrary = "OpenOptions.dnaFusion.Flex.Common";
        private int @__SourceId;
        private string @__Name;
        private string @__Description;
        private bool @__Enabled;
        public virtual int SourceId
        {
            get
            {
                return @__SourceId;
            }
            set
            {
                if ((this.@__SourceId != value))
                {
                    @__SourceId = value;
                    this.TriggerPropertyChanged("SourceId");
                }
            }
        }
        public virtual string Name
        {
            get
            {
                return @__Name;
            }
            set
            {
                if ((this.@__Name != value))
                {
                    @__Name = value;
                    this.TriggerPropertyChanged("Name");
                }
            }
        }
        public virtual string Description
        {
            get
            {
                return @__Description;
            }
            set
            {
                if ((this.@__Description != value))
                {
                    @__Description = value;
                    this.TriggerPropertyChanged("Description");
                }
            }
        }
        public virtual bool Enabled
        {
            get
            {
                return @__Enabled;
            }
            set
            {
                if ((this.@__Enabled != value))
                {
                    @__Enabled = value;
                    this.TriggerPropertyChanged("Enabled");
                }
            }
        }
    }

    [XmlRpcMissingMapping(CookComputing.XmlRpc.MappingAction.Ignore)]
    [System.SerializableAttribute()]
    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public partial class NameValue : ComplexType
    {
        [System.NonSerializedAttribute()]
        public const string FlexVersion = "1.24";
        [System.NonSerializedAttribute()]
        public const string FlexLibrary = "OpenOptions.dnaFusion.Flex.Common";
        private string @__Name;
        private string @__Value;
        public virtual string Name
        {
            get
            {
                return @__Name;
            }
            set
            {
                if ((this.@__Name != value))
                {
                    @__Name = value;
                    this.TriggerPropertyChanged("Name");
                }
            }
        }
        public virtual string Value
        {
            get
            {
                return @__Value;
            }
            set
            {
                if ((this.@__Value != value))
                {
                    @__Value = value;
                    this.TriggerPropertyChanged("Value");
                }
            }
        }
    }
    /// <summary>
    /// Abstract service
    /// </summary>
    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public interface IAFlexService : CookComputing.XmlRpc.IXmlRpcProxy
    {
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public interface IFlexPermissions : IAFlexService
    {
        /// <summary>
        /// Methods that the supplied API key can invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        [XmlRpcMethod("FlexPermissions.CanInvoke")]
        string[] CanInvoke(string apiKey);
        /// <summary>
        /// Methods that the supplied API key can not invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        [XmlRpcMethod("FlexPermissions.CanNotInvoke")]
        string[] CanNotInvoke(string apiKey);
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public partial class FlexPermissions : AFlexService
    {
        [System.NonSerializedAttribute()]
        public new const string FlexVersion = "1.24";
        [System.NonSerializedAttribute()]
        public new const string FlexLibrary = "OpenOptions.dnaFusion.Flex.Common";
        /// <summary>
        /// Methods that the supplied API key can invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        [XmlRpcMethod("FlexPermissions.CanInvoke")]
        public virtual string[] CanInvoke(string apiKey)
        {
            return ((string[])(this.Invoke(this, "CanInvoke", apiKey)));
        }
        /// <summary>
        /// Methods that the supplied API key can invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        public virtual System.IAsyncResult BeginCanInvoke(string apiKey, System.AsyncCallback @__Callback, object @__UserData)
        {
            return this.BeginInvoke("CanInvoke", new object[] {
                        apiKey}, this, @__Callback, @__UserData);
        }
        public virtual string[] EndCanInvoke(System.IAsyncResult asyncResult)
        {
            return this.EndInvoke<string[]>(asyncResult);
        }
        /// <summary>
        /// Methods that the supplied API key can not invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        [XmlRpcMethod("FlexPermissions.CanNotInvoke")]
        public virtual string[] CanNotInvoke(string apiKey)
        {
            return ((string[])(this.Invoke(this, "CanNotInvoke", apiKey)));
        }
        /// <summary>
        /// Methods that the supplied API key can not invoke.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="apiKey">The client API Key for this application.</param>
        public virtual System.IAsyncResult BeginCanNotInvoke(string apiKey, System.AsyncCallback @__Callback, object @__UserData)
        {
            return this.BeginInvoke("CanNotInvoke", new object[] {
                        apiKey}, this, @__Callback, @__UserData);
        }
        public virtual string[] EndCanNotInvoke(System.IAsyncResult asyncResult)
        {
            return this.EndInvoke<string[]>(asyncResult);
        }
    }

    [System.Reflection.ObfuscationAttribute(Exclude = true)]
    public interface IFlexInfo : IAFlexService
    {
        /// <param name="Result"></param>
        [XmlRpcMethod("FlexInfo.Clients")]
        FlexClient[] Clients();
        /// <param name="Result"></param>
        [XmlRpcMethod("FlexInfo.Version")]
        string Version();
    }

    [System.Reflection.ObfuscationAttribute(Exclude=true)]
    public partial class FlexInfo : AFlexService {
        [System.NonSerializedAttribute()]
        public new const string FlexVersion = "1.24";
        [System.NonSerializedAttribute()]
        public new const string FlexLibrary = "OpenOptions.dnaFusion.Flex.Common";
        /// <param name="Result"></param>
        [XmlRpcMethod("FlexInfo.Clients")]
        public virtual FlexClient[] Clients() {
            return ((FlexClient[])(this.Invoke(this, "Clients")));
        }
        /// <param name="Result"></param>
        public virtual System.IAsyncResult BeginClients(System.AsyncCallback @__Callback, object @__UserData) {
            return this.BeginInvoke("Clients", new object[1], this, @__Callback, @__UserData);
        }
        public virtual FlexClient[] EndClients(System.IAsyncResult asyncResult) {
            return this.EndInvoke<FlexClient[]>(asyncResult);
        }
        /// <param name="Result"></param>
        [XmlRpcMethod("FlexInfo.Version")]
        public virtual string Version() {
            return ((string)(this.Invoke(this, "Version")));
        }
        /// <param name="Result"></param>
        public virtual System.IAsyncResult BeginVersion(System.AsyncCallback @__Callback, object @__UserData) {
            return this.BeginInvoke("Version", new object[1], this, @__Callback, @__UserData);
        }
        public virtual string EndVersion(System.IAsyncResult asyncResult) {
            return this.EndInvoke<string>(asyncResult);
        }
    }
}