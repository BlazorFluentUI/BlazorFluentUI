using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FluentUI
{
    [DataContract]
    public class TextChangedArgs
    {
        [DataMember(Name = "html")]
        public string Html { get; set; }

        private ChangeSource source;
        [DataMember(Name = "source")]
        public string RawSource
        {
            get => source.ToString().ToLowerInvariant();
            set
            {
                switch (value)
                {
                    case "user":
                        source = ChangeSource.User;
                        break;
                    case "api":
                        source = ChangeSource.Api;
                        break;
                    case "silent":
                        source = ChangeSource.Silent;
                        break;
                }
            }
        }


        public ChangeSource Source => source;

    }
}
