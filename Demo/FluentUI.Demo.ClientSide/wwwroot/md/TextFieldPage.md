@page "/textFieldPage"
@using System.ComponentModel.DataAnnotations

<h1>TextField</h1>

<Demo Header="Basic TextField" Key="0" MetadataPath="TextFieldPage">
    <div class="textFieldDiv">
        <TextField Label="TextField" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField with PostalCode autocomplete" AutoComplete="AutoComplete.PostalCode" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Password" InputType="InputType.Password" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Disabled" Disabled="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Readonly" ReadOnly="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Required" Required="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField w/ Icon" IconName="Home" />
    </div>
    <div class="textFieldDiv">
        <TextField Required="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="With Error" ErrorMessage="there is an error" />
    </div>
    @*<div class="textFieldDiv">
        <TextField Label="With Input Mask" MaskChar="+" />
    </div>*@
    <div class="textFieldDiv">
        <TextField Label="With Placeholder" Placeholder="placeholder text" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="Disabled Placeholder" Placeholder="placeholder text" Disabled="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="Disabled Placeholder" Placeholder="placeholder text" Disabled="true" />
    </div>
</Demo>

<Demo Header="TextField with prefix and/or suffix" Key="1" MetadataPath="TextFieldPage">
    <div class="textFieldDiv">
        <TextField Label="With Prefix (text only)" Prefix="https://" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="With PrefixContent (custom content)">
            <PrefixContent>
                <Icon IconName="Add" />
            </PrefixContent>
        </TextField>
    </div>
    <div class="textFieldDiv">
        <TextField Label="With Suffix (text only)" Suffix="bananas" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="With SuffixContent (custom content)">
            <SuffixContent>
                <Icon IconName="Home" />
            </SuffixContent>
        </TextField>
    </div>
    <div class="textFieldDiv">
        <TextField Label="With Prefix & Suffix" Prefix="Dr." Suffix="Esquire" />
    </div>
</Demo>

<Demo Header="TextField with no labels" Key="2" MetadataPath="TextFieldPage">
    <div class="textFieldDiv">
        <TextField Required="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Underlined="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Disabled="true" />
    </div>
</Demo>


<Demo Header="Multiline TextField" Key="3" MetadataPath="TextFieldPage">
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

<Demo Header="Underline &amp; Borderless TextField" Key="4" MetadataPath="TextFieldPage">
    <h2>Underline &amp; Borderless</h2>
    <div class="textFieldDiv">
        <TextField Label="TextField Underlined" Underlined="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Underlined Disabled" Underlined="true" Disabled="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Underlined Required" Underlined="true" Required="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Borderless" Borderless="true" />
    </div>
    <div class="textFieldDiv">
        <TextField Label="TextField Multiline Borderless" Multiline="true" Borderless="true" />
    </div>
</Demo>

<Demo Header="Binding Modes" Key="5" MetadataPath="TextFieldPage">
    <div class="textFieldDiv" style="display:flex; flex-direction: row">
        <TextField Label="TextField OnInput 1" @bind-Value=@onInputContent @bind-Value:event="OnInput" OnGetErrorMessage="GetErrorMessage" />
        <TextField Label="TextField OnInput 2" Value=@onInputContent />
    </div>
    <div class="textFieldDiv" style="display:flex; flex-direction: row">
        <TextField Label="TextField OnChange 1" @bind-Value=@onChangeContent @bind-Value:event="OnChange"  OnGetErrorMessage="GetErrorMessage"/>
        <TextField Label="TextField OnChange 2" Value=@onChangeContent />
    </div>
</Demo>

<Demo Header="TextField Error Message Variations" Key="6" MetadataPath="TextFieldPage">

    <h4>Hint: the input length must be less than 3.</h4>

    <div class="textFieldDiv">
            <TextField Label="String-based validation" OnGetErrorMessage="GetErrorMessage" />
        </div>
        <div class="textFieldDiv">
            <TextField Label="String-based validation on render" DefaultValue="Shows an error message on render" OnGetErrorMessage="GetErrorMessage" />
        </div>
    <div class="textFieldDiv">
        <TextField Label="String-based validation only on change" DefaultValue="Validates only on input change, not on first render" OnGetErrorMessage="GetErrorMessage" ValidateOnLoad="false" />
    </div>
    <div class="textfielddiv">
        <bfutextfield label="both description and error message" defaultvalue="shows description and error message on render" description="field description" ongeterrormessage="geterrormessage" />
    </div>
    <div class="textfielddiv">
        <bfutextfield label="deferred string-based validation" placeholder="validates after user stops typing for 2 seconds" deferredvalidationtime="2000" ongeterrormessage="geterrormessage" />
    </div>
    <div class="textfielddiv">
        <bfutextfield label="validates only on focus and blur" placeholder="validates only on input focus and blur" validateonfocusin="true" validateonfocusout="true" ongeterrormessage="geterrormessage" />
    </div>
    <div class="textfielddiv">
        <bfutextfield label="validates only on blur" placeholder="validates only on blur" validateonfocusout="true" ongeterrormessage="geterrormessage" />
    </div>
    <div class="textfielddiv">
        <bfutextfield label="underlined field " defaultvalue="this value is too long" underlined="true" ongeterrormessage="geterrormessage" />
    </div>
    <div class="textfielddiv">
        <bfutextfield label="uses the errormessage property to set an error state" placeholder="this field always has an error" errormessage="this is a statically set error message" />
    </div>
</Demo>

<Demo Header="Validation using Blazor's InputBase<string> and EditForm" Key="7" MetadataPath="TextFieldPage">
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
        </EditForm>
    </Demo>

@code {
    string onInputContent = "";
    string onChangeContent = "";

    ExampleModel exampleModel = new ExampleModel();

    class ExampleModel
    {
        [Required]
        [StringLength(5, ErrorMessage = "NameOnChange is too long.")]
        public string NameOnChange { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "NameOnInput is too long.")]
        public string NameOnInput { get; set; }
    }

    public void HandleValidSubmit()
    {
        var i = 3;
    }

    public string GetErrorMessage(string value)
    {
        return value.Length < 3 ? "" : $"Input value length must be less than 3. Actual length is {value.Length}.";
    }
}
