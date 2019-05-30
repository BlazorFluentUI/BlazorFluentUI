using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFabric.Test.HostedClient.Client.Models
{
    public class DataItem
    {
        public DataItem(int num)
        {
            DisplayName = num.ToString();
        }
        public string DisplayName { get; set; }
        public string ImgUrl => "/background.png";
    }
}
