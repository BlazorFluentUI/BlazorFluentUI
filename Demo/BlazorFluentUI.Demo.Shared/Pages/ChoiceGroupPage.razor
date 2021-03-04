@page "/choiceGroupPage"

@using System.Collections.Generic

<div style="margin:20px">
    <h1>ChoiceGroup</h1>
    <Demo Header="ChoiceGroup with ItemSource as List<string>" MetadataPath="ChoiceGroupPage" Key="0">
        <Stack>
            <ChoiceGroup ItemsSource="this._stringItems" @bind-Value="this._selectedStringOption" Required="true">
            </ChoiceGroup>
            <br />
            <div>
                Selected option: @(_selectedStringOption ?? "none")
            </div>
        </Stack>
    </Demo>

    <Demo Header="ChoiceGroup with ItemSource as List<IChoiceGroupOption>" MetadataPath="ChoiceGroupPage" Key="1">
        <Stack>
            <ChoiceGroup ItemsSource="this._choiceGroupOptions" @bind-Value="this._selectedChoiceGroupOption">
            </ChoiceGroup>
            <br />
            <div>
                Selected option: @(_selectedChoiceGroupOption?.Label ?? "none")
            </div>
        </Stack>
    </Demo>

    <Demo Header="ChoiceGroup with ItemSource as List<CustomObject> using OptionTemplate" MetadataPath="ChoiceGroupPage" Key="2">
        <Stack>
            <ChoiceGroup ItemsSource="this._customObjects" @bind-Value="this._selectedCustomObject" ItemAlignment="FlexDirection.Row">
                <OptionTemplate Context="item">
                    <Icon IconName="@item.IconName" Style="font-size:36px"></Icon>
                </OptionTemplate>
            </ChoiceGroup>
            <br />
            <div>
                Selected option: @(_selectedCustomObject?.Label ?? "none")
            </div>
        </Stack>

        <Stack>
            <h3>Datasource is a List&lt;CustomObject&gt; using the "OptionTemplate"</h3>
            <ChoiceGroup ItemsSource="this._customObjects2" @bind-Value="this._selectedCustomObject2" ItemAlignment="FlexDirection.Row">
                <OptionTemplate Context="item">
                    <div style="width:200px;height:90px">
                        <Icon IconName="@item.IconName" Style="font-size:36px;margin-bottom:10px"></Icon>
                        @if (item.Id == 1)
                        {
                            <DatePicker Disabled="_selectedCustomObject2?.Id != 1" @bind-Value="_selectedDate"></DatePicker>
                        }
                        else if (item.Id == 3)
                        {
                            <Dropdown ItemsSource=@_months.Select(x=>new DropdownOption { Key = x.Name, Text = x.Name })
                                         Disabled="_selectedCustomObject2?.Id != 3"
                                         Placeholder="Select a Month"
                                         @bind-SelectedOption=@_selectedMonthOption />
                        }
                    </div>
                </OptionTemplate>
            </ChoiceGroup>
            <br />
            <div>
                Selected option id: @(_selectedCustomObject2?.Id.ToString() ?? "none")
            </div>
            <div>
                Selected date: @(_selectedDate != DateTime.MinValue ? ((DateTime)_selectedDate).ToShortDateString() : "none")
            </div>
            <div>
                Selected month: @(_selectedMonthOption?.Text ?? "none")
            </div>
        </Stack>
    </Demo>

        <div style="height:150px"></div>
</div>

@code {
    private string _selectedStringOption;
    private ChoiceGroupOption _selectedChoiceGroupOption;
    private CustomObject _selectedCustomObject;
    private CustomObject _selectedCustomObject2;
    private DateTime? _selectedDate;
    //private string _selectedMonth;
    private IDropdownOption _selectedMonthOption;

    private List<string> _stringItems = new List<string>
    {
        "Option A",
        "Option B",
        "Option C",
        "Option D",
    };

    private List<ChoiceGroupOption> _choiceGroupOptions = new List<ChoiceGroupOption>
    {
        new ChoiceGroupOption{ Label = "Option A" },
        new ChoiceGroupOption{ Label = "Option B" },
        new ChoiceGroupOption{ Label = "Option C", IsDisabled = true },
        new ChoiceGroupOption{ Label = "Option D" },
    };

    private List<CustomObject> _customObjects = new List<CustomObject>
    {
        new CustomObject{ Label = "Day", IconName= "CalendarDay"},
        new CustomObject{ Label = "Week", IconName= "CalendarWeek"},
        new CustomObject{ Label = "Month", IconName= "Calendar", IsDisabled = true},
    };

    private List<CustomObject> _customObjects2 = new List<CustomObject>
    {
        new CustomObject{ Id = 1, Label = "Day", IconName= "CalendarDay"},
        new CustomObject{ Id = 2, Label = "Week", IconName= "CalendarWeek", IsDisabled = true},
        new CustomObject{ Id = 3, Label = "Month", IconName= "Calendar"},
    };

    private List<Month> _months = new List<Month>
    {
        new Month { Name = "January" },
        new Month { Name = "February" },
        new Month { Name = "March" },
        new Month { Name = "April" },
        new Month { Name = "May" },
        new Month { Name = "June" },
        new Month { Name = "July" },
        new Month { Name = "August" },
        new Month { Name = "September" },
        new Month { Name = "October" },
        new Month { Name = "November" },
        new Month { Name = "December" },
    };

    private class ChoiceGroupOption : IChoiceGroupOption
    {
        public string Label { get; set; }
        public bool IsDisabled { get; set; } = false;
        public bool IsVisible { get; set; } = true;
    }

    private class CustomObject : IChoiceGroupOption
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsDisabled { get; set; } = false;
        public bool IsVisible { get; set; } = true;
        public string IconName { get; set; }
    }

    private class Month
    {
        public string Name { get; set; }
    }
}