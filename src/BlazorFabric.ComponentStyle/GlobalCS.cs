using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class GlobalCS : ComponentBase, IGlobalCSSheet, IDisposable, INotifyPropertyChanged
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        [Parameter]
        public object Component { get; set; }

        [Parameter]
        public ICollection<Rule> Rules
        {
            get => _rules;
            set
            {
                if (_rules != value)
                {
                    _rules = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rules"));
                }
            }
        }

        public bool HasEvent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ICollection<Rule> _rules;


        public void Dispose()
        {
            ComponentStyle.GlobalCSSheets.Remove(this);
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalCSSheets.Add(this);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rules"));
            return base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
    }
}

