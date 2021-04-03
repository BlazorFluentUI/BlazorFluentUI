@page "/textFieldPage"
@using System.ComponentModel.DataAnnotations

<header class="root">
    <h1 class="title">TextField</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                Text fields (<code>TextField</code>) give people a way to enter and edit text. They’re used in forms, modal dialogs, tables, and other surfaces where text input is required.
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
            <Demo Header="Basic TextFields" Key="0" MetadataPath="TextFieldPage">
                <div class="textFieldDiv">
                    <TextField Label="Standard" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Postal Code autocomplete" AutoComplete="AutoComplete.PostalCode" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Password" InputType="InputType.Password" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Disabled" Disabled="true" Value="I am disabled" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Read-only" ReadOnly="true" Value="I am read-only" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Required" Required="true" />
                </div>
                <div class="textFieldDiv">
                    <TextField Required="true" />
                    <span><i>Required but without a label, indicator is shown next to the TextField</i></span>
                </div>
                <div class="textFieldDiv">
                    <TextField Label="With an icon" IconName="Home" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="With Error" ErrorMessage="there is an error" />
                </div>
                @*<div class="textFieldDiv">
                       <TextField Label="With Input Mask" MaskChar="+" />
                </div>*@
                <div class="textFieldDiv">
                    <TextField Label="With Placeholder" Placeholder="Please enter text here" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Disabled with placeholder" Placeholder="I am disabled" Disabled="true" />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Multiline TextFields" Key="1" MetadataPath="TextFieldPage">
                <div class="textFieldDiv">
                    <TextField Label="TextField Multiline" Multiline="true" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="TextField Multiline Required" Multiline="true" Required="true" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="TextField Multiline Disabled" Multiline="true" Disabled="true" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Not resizable" Multiline="true" Resizable="false" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Auto-adjusting height" Multiline="true" AutoAdjustHeight="true" />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Underlined and borderless TextFields" Key="2" MetadataPath="TextFieldPage">
                <div class="textFieldDiv">
                    <TextField Label="Standard:" Underlined="true" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Disabled:" Underlined="true" Disabled="true" Value="I am disabled" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Required:" Underlined="true" Required="true" Placeholder="Enter text here" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Borderless single-line TextField" Borderless="true" Placeholder="No borders here, folks." />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Borderless multi-line TextField" Multiline="true" Borderless="true" Placeholder="No borders here, folks." />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="TextField with prefix and/or suffix" Key="3" MetadataPath="TextFieldPage">
                <div class="textFieldDiv">
                    <TextField Label="With text only Prefix" Prefix="https://" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="With custom content Prefix">
                        <PrefixContent>
                            <Icon IconName="Add" />
                        </PrefixContent>
                    </TextField>
                </div>
                <div class="textFieldDiv">
                    <TextField Label="With text only Suffix" Suffix="bananas" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="With custom content Suffix">
                        <SuffixContent>
                            <Icon IconName="Home" />
                        </SuffixContent>
                    </TextField>
                </div>
                <div class="textFieldDiv">
                    <TextField Label="With Prefix & Suffix" Prefix="Dr." Suffix="Esquire" />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Binding Modes" Key="4" MetadataPath="TextFieldPage">
                <div class="textFieldDiv" style="display:flex; flex-direction: row">
                    <TextField Label="TextField OnInput 1" @bind-Value=@onInputContent @bind-Value:event="OnInput" />
                    <TextField Label="TextField OnInput 2" Value=@onInputContent />
                </div>
                <div class="textFieldDiv" style="display:flex; flex-direction: row">
                    <TextField Label="TextField OnChange 1" @bind-Value=@onChangeContent @bind-Value:event="OnChange" />
                    <TextField Label="TextField OnChange 2" Value=@onChangeContent />
                </div>
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="TextField Error Message Variations" Key="5" MetadataPath="TextFieldPage">
                <strong>Hint: the input length must be less than 3.</strong>
                <div class="textFieldDiv">
                    <TextField Label="String-based validation" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="String-based validation on render" DefaultValue="Shows an error message on render" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="String-based validation only on change" DefaultValue="Validates only on input change, not on first render" OnGetErrorMessage="GetErrorMessage" ValidateOnLoad="false" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Both description and error message" DefaultValue="Shows description and error message on render" Description="field description" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Deferred string-based validation" Placeholder="Validates after user stops typing for 2 seconds" DeferredValidationTime="2000" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Validates only on focus and blur" Placeholder="Validates only on input focus and blur" ValidateOnFocusIn="true" ValidateOnFocusOut="true" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Validates only on blur" Placeholder="Validates only on blur" ValidateOnFocusOut="true" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Underlined field " DefaultValue="This value is too long" Underlined="true" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Uses the errormessage property to set an error state" Placeholder="This field always has an error" ErrorMessage="This is a statically set error message" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="Both DefaultValue and Value set" DefaultValue="This is a default value" Value="This is a regular value" OnGetErrorMessage="GetErrorMessage" />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Validation using Blazor's EditForm and DataAnnotations" Key="6" MetadataPath="TextFieldPage">
                <EditForm Model=@exampleModel OnValidSubmit=@HandleValidSubmit>
                    <DataAnnotationsValidator />
                    <FluentUIValidationSummary />

                    <div class="textFieldDiv">
                        <TextField Label="OnChange - Input can't be longer than 5 characters" @bind-Value=@exampleModel.NameOnChange />
                    </div>
                    <div class="textFieldDiv">
                        <TextField Label="OnInput - Input can't be longer than 5 characters" @bind-Value=@exampleModel.NameOnInput @bind-Value:event="OnInput" />
                    </div>
                    <SubmitButton Text="Submit" />
                    <br />Name on change: @(exampleModel.NameOnChange ?? "-"),
                    <br />Name on input: @(exampleModel.NameOnInput ?? "-")
                </EditForm>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="TextField Number tests" Key="7" MetadataPath="TextFieldPage">
                <h4>Numbers playground</h4>
                <p>These TextField components can be used both within and without <code>&lt;EditForm&gt;</code> block</p>
                <EditForm Model=@exampleModel OnValidSubmit=@HandleValidSubmit>
                <FluentUIValidationSummary />
                <div class="textFieldDiv">
                    <NumericTextField InputType="InputType.Number" Label="Int test" @bind-Value="exampleModel.Age" OnGetErrorMessage="ExpectedTypeError" />
                    <InputNumber @bind-Value="exampleModel.Age" ParsingErrorMessage="Number is out of range" />
                </div>
                Age: @exampleModel.Age
                <div class="textFieldDiv">
                    <NumericTextField InputType="InputType.Number" Label="Long test" @bind-Value="exampleModel.Ticks" />
                </div>
                Ticks: @exampleModel.Ticks
                <div class="textFieldDiv">
                    <NumericTextField InputType="InputType.Number" Label="Short test (unbound)" Value="Int16.MinValue" />
                </div>
                <div class="textFieldDiv">
                    <NumericTextField InputType="InputType.Number" Label="Float test (OnChange)" @bind-Value="@exampleModel.sampleFloat" @bind-Value:event="OnChange" />
                    @*<InputNumber TValue="float" @bind-Value="@exampleModel.sampleFloat" />*@
                    Sample float: @exampleModel.sampleFloat
                </div>
                <div class="textFieldDiv">
                    <NumericTextField InputType="InputType.Number" Label="Double test" @bind-Value="@exampleModel.sampleDouble" />
                    Sample double: @exampleModel.sampleDouble
                </div>
                <div class="textFieldDiv">
                    <NumericTextField InputType="InputType.Number" Label="Decimal test" @bind-Value="@exampleModel.sampleDecimal" />
                    Sample decimal: @exampleModel.sampleDecimal
                </div>
                </EditForm>
            </Demo>
        </div>
    </div>
</div>
@code {
    string onInputContent = "";
    string onChangeContent = "";
    string text = "";

    ExampleModel exampleModel = new ExampleModel();

    class ExampleModel
    {
        [Required]
        [StringLength(5, ErrorMessage = "NameOnChange is too long.")]
        public string NameOnChange { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "NameOnInput is too long.")]
        public string NameOnInput { get; set; }

        public int Age { get; set; } = int.MaxValue;
        public int Age1 { get; set; }
        public long Ticks { get; set; } = DateTime.Now.Ticks;
        public float sampleFloat { get; set; } = Single.MaxValue;
        public double sampleDouble { get; set; } = Double.MaxValue;
        public decimal sampleDecimal { get; set; } = Decimal.One / 3;
    }


    public void HandleValidSubmit()
    {

    }

    public string GetErrorMessage(string value)
    {
        return value.Length < 3 ? "" : $"Input value length must be less than 3. Actual length is {value.Length}.";
    }

    public string ExpectedTypeError(int value)
    {
        return "" ;
    }
}
