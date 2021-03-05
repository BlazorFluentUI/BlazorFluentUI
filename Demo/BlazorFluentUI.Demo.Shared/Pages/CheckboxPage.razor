@page "/checkboxPage"
@using System.ComponentModel.DataAnnotations

<header class="root">
    <h1 class="title">Checkbox</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                Check boxes (<code>Checkbox</code>) give people a way to select one or more items from a group, or switch between two mutually exclusive options (checked or unchecked, on or off).
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
            <Demo Header="Basic Checkboxes" Key="0" MetadataPath="CheckboxPage">
                <Stack Tokens=stackTokens>
                    <Checkbox Label="Unchecked checkbox (uncontrolled)" CheckedChanged=FirstChange />
                    <Checkbox Label="Checked checkbox (uncontrolled)" DefaultChecked="true" CheckedChanged=FirstChange />
                    <Checkbox Label="Disabled checkbox" Disabled="true" />
                    <Checkbox Label="Disabled checked checkbox" Disabled="true" DefaultChecked="true" />
                </Stack>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Other Implementation Examples" Key="1" MetadataPath="CheckboxPage">
                <Stack Tokens=stackTokens>
                    <Label>Controlled checkbox is @(_isFirstChecked ? "checked" : "unchecked").</Label>
                    <Checkbox Label="Controlled checkbox" @bind-Checked=@_isFirstChecked />
                    <Checkbox Label='Checkbox rendered with boxSide "end"' BoxSide="BoxSide.End" />
                </Stack>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Indeterminate Checkboxes" Key="2" MetadataPath="CheckboxPage">
                <Stack Tokens=stackTokens>
                    <Label>Indeterminate checkbox (controlled) is @(_isIndeterminate ? "indeterminate" : (_isSecondChecked ? "checked" : "unchecked" )).</Label>
                    <Checkbox Label="Indeterminate checkbox (uncontrolled)" DefaultIndeterminate="true" />
                    <Checkbox Label="Indeterminate checkbox which defaults to true when clicked (uncontrolled)" DefaultIndeterminate="true" DefaultChecked="true" />
                    <Checkbox Label="Disabled indeterminate checkbox" Disabled="true" DefaultIndeterminate="true" />
                    <Checkbox Label="Indeterminate checkbox (controlled)" @bind-Indeterminate=@_isIndeterminate @bind-Checked=@_isSecondChecked />
                </Stack>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Blazor Forms Validation Example" Key="3" MetadataPath="CheckboxPage">
                <Stack Tokens=stackTokens>
                    <EditForm Model="exampleModel" OnValidSubmit=@HandleValidSubmit>
                        <DataAnnotationsValidator />
                        <FluentUIValidationSummary />
                        <Label>Do you agree with the terms?</Label>
                        <Checkbox Label="I agree with the terms!" @bind-Checked=@(exampleModel.IsChecked) />
                        <SubmitButton Text="Submit" />
                    </EditForm>
                </Stack>
            </Demo>
        </div>
    </div>
</div>

@code{

    private StackTokens stackTokens;
    private bool _isFirstChecked;
    private bool _isSecondChecked;
    private bool _isIndeterminate;


    protected override void OnInitialized()
    {
        _isIndeterminate = true;
        stackTokens = new StackTokens { ChildrenGap = new double[] { 10 } };
        base.OnInitialized();
    }

    private void FirstChange(bool isChecked)
    {
        Console.WriteLine($"The option has been changed to {isChecked}.");
    }

    ExampleModel exampleModel = new ExampleModel();

    class ExampleModel
    {
        [RegularExpression("True", ErrorMessage = "Must agree to terms.")]
        public bool IsChecked { get; set; }
    }

    public void HandleValidSubmit()
    {
        var i = 3;
    }

}
