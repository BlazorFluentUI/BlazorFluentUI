@page "/checkboxPage"
@using System.ComponentModel.DataAnnotations

<h1>Checkbox</h1>

<Demo Header="Basic Checkboxes" Key="0" MetadataPath="CheckboxPage">
    <BFUStack Tokens=stackTokens>
        <BFUCheckbox Label="Unchecked checkbox (uncontrolled)" CheckedChanged=FirstChange />
        <BFUCheckbox Label="Checked checkbox (uncontrolled)" DefaultChecked="true" CheckedChanged=FirstChange />
        <BFUCheckbox Label="Disabled checkbox" Disabled="true" />
        <BFUCheckbox Label="Disabled checked checkbox" Disabled="true" DefaultChecked="true" />
    </BFUStack>
</Demo>

<Demo Header="Other Implementation Examples" Key="1" MetadataPath="CheckboxPage">
    <BFUStack Tokens=stackTokens>
        <BFULabel>Controlled checkbox is @(_isFirstChecked ? "checked" : "unchecked").</BFULabel>
        <BFUCheckbox Label="Controlled checkbox" @bind-Checked=@_isFirstChecked />
        <BFUCheckbox Label='Checkbox rendered with boxSide "end"' BoxSide="BoxSide.End" />
    </BFUStack>
</Demo>

<Demo Header="Indeterminate Checkboxes" Key="2" MetadataPath="CheckboxPage">
    <BFUStack Tokens=stackTokens>
        <BFULabel>Indeterminate checkbox (controlled) is @(_isIndeterminate ? "indeterminate" : (_isSecondChecked ? "checked" : "unchecked" )).</BFULabel>
        <BFUCheckbox Label="Indeterminate checkbox (uncontrolled)" DefaultIndeterminate="true" />
        <BFUCheckbox Label="Indeterminate checkbox which defaults to true when clicked (uncontrolled)" DefaultIndeterminate="true" DefaultChecked="true" />
        <BFUCheckbox Label="Disabled indeterminate checkbox" Disabled="true" DefaultIndeterminate="true" />
        <BFUCheckbox Label="Indeterminate checkbox (controlled)" @bind-Indeterminate=@_isIndeterminate @bind-Checked=@_isSecondChecked />
    </BFUStack>
</Demo>

<Demo Header="Blazor Forms Validation Example" Key="3" MetadataPath="CheckboxPage">
    <BFUStack Tokens=stackTokens>
        <EditForm Model="exampleModel" OnValidSubmit=@HandleValidSubmit>
            <DataAnnotationsValidator />
            <BFUValidationSummary />
            <BFULabel>Do you agree with the terms?</BFULabel>
            <BFUCheckbox Label="I agree with the terms!" @bind-Checked=@(exampleModel.IsChecked)/>
            <BFUSubmitButton Text="Submit" />
        </EditForm>
    </BFUStack>
</Demo>

@code{

    private BFUStackTokens stackTokens;
    private bool _isFirstChecked;
    private bool _isSecondChecked;
    private bool _isIndeterminate;


    protected override void OnInitialized()
    {
        _isIndeterminate = true;
        stackTokens = new BFUStackTokens { ChildrenGap = new double[] { 10 } };
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
