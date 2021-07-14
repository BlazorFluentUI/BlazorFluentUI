@page "/datePickerPage"
@using System
@using System.Linq
@using System.ComponentModel.DataAnnotations

<header class="root">
    <h1 class="title">DatePicker</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                Picking a date can be tough without context. A date picker (<code>DatePicker</code>) offers a drop-down control that’s optimized for picking a single date from a calendar view where contextual information like the day of the week or fullness of the calendar is important. You can modify the calendar to provide additional context or to limit available dates.
            </p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <div class="subSection">
            <Demo Header="Basic DatePicker" Key="1" MetadataPath="DatePickerPage">
                <Dropdown ItemsSource=@DaysOfWeek.Select(x=>new DropdownOption {Key=x.ToString(), Text=System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(x) })
                          Style="max-width:300px; margin:0 0 15px 0;"
                          Placeholder="Select an option"
                          @bind-SelectedOption=@selectedDayOfWeekOption
                          Label="Select the first day of the week" />
                <DatePicker AllowTextInput="false"
                            Style="max-width:300px; margin:0 0 15px 0;"
                            @bind-Value="selectedDate1"
                            Placeholder="Select a date..."
                            FirstDayOfWeek=@((DayOfWeek)Enum.Parse(typeof(DayOfWeek), selectedDayOfWeekOption?.Key!))
                            InitialPickerDate=@(DateTime.Parse("24-11-1971"))/>
                Selected date: @selectedDate1

            </Demo>
        </div>

    </div>
</div>
@code {
    DateTime? selectedDate1;
    DateTime? selectedDate2;
    DateTime? selectedDate3;
    DateTime? selectedDate4;


    //string selectedDayOfWeek = DayOfWeek.Sunday.ToString();
    IDropdownOption selectedDayOfWeekOption = new DropdownOption { Key = DayOfWeek.Sunday.ToString(), Text = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Sunday) };
    List<DayOfWeek> DaysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList(); // new System.Collections.Generic.List<DayOfWeek>();

    ExampleModel exampleModel = new ExampleModel();

    class MondayValidation : ValidationAttribute
    {
        public MondayValidation()
        {
            ErrorMessage = "Only Mondays are allowed.";
        }

        public override bool IsValid(object? value)
        {
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;

                if (dt.DayOfWeek == DayOfWeek.Monday)
                    return true;
            }
            return false;
        }
    }

    class ExampleModel
    {
        [MondayValidation]
        public DateTime? DateTime { get; set; }

    }

    public void HandleValidSubmit()
    {
        //var i = 3;
    }
}

