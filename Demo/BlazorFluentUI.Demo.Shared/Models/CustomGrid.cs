using System;

namespace BlazorFluentUI.Demo.Shared.Models
{
    public class CustomGrid
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Type { get; set; }
        public int Amount { get; set; }
        public Guid? Secondid { get; set; }
        public bool Visible { get; set; }
        public string? Status { get; set; }
        public IDropdownOption? Secondstatus { get; set; }

    }
}
