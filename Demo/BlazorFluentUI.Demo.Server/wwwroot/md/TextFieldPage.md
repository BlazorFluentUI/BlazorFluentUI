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
        </div>

        <div class="subSection">
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
        </div>

        <div class="subSection">
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
        </div>


        <div class="subSection">
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
        </div>

        <div class="subSection">
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
        </div>

        <div class="subSection">
            <Demo Header="Binding Modes" Key="5" MetadataPath="TextFieldPage">
                <div class="textFieldDiv" style="display:flex; flex-direction: row">
                    <TextField Label="TextField OnInput 1" @bind-Value=@onInputContent @bind-Value:event="OnInput" OnGetErrorMessage="GetErrorMessage" />
                    <TextField Label="TextField OnInput 2" Value=@onInputContent />
                </div>
                <div class="textFieldDiv" style="display:flex; flex-direction: row">
                    <TextField Label="TextField OnChange 1" @bind-Value=@onChangeContent @bind-Value:event="OnChange" OnGetErrorMessage="GetErrorMessage" />
                    <TextField Label="TextField OnChange 2" Value=@onChangeContent />
                </div>
            </Demo>
        </div>

        <div class="subSection">
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
                <div class="textFieldDiv">
                    <TextField Label="both description and error message" DefaultValue="shows description and error message on render" Description="field description" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="deferred string-based validation" Placeholder="validates after user stops typing for 2 seconds" DeferredValidationTime="2000" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="validates only on focus and blur" Placeholder="validates only on input focus and blur" ValidateOnFocusIn="true" ValidateOnFocusOut="true" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="validates only on blur" Placeholder="validates only on blur" ValidateOnFocusOut="true" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="underlined field " DefaultValue="this value is too long" Underlined="true" OnGetErrorMessage="GetErrorMessage" />
                </div>
                <div class="textFieldDiv">
                    <TextField Label="uses the errormessage property to set an error state" Placeholder="this field always has an error" ErrorMessage="this is a statically set error message" />
                </div>
            </Demo>
        </div>

        <div class="subSection">
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
        </div>
    </div>
</div>
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

    }

    public string GetErrorMessage(string value)
    {
        return value.Length < 3 ? "" : $"Input value length must be less than 3. Actual length is {value.Length}.";
    }
}
