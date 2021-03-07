@page  "/progressIndicatorPage"

<header class="root">
    <h1 class="title">ProgressIndicator</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                ProgressIndicators are used to show the completion status of an operation lasting more than 2 seconds. If the state of progress cannot be determined, use a Spinner instead. ProgressIndicators can appear in a new panel, a flyout, under the UI initiating the operation, or even replacing the initiating UI, as long as the UI can return if the operation is canceled or is stopped.
            </p>
            <p>
                ProgressIndicators feature a bar showing total units to completion, and total units finished. The description of the operation appears above the bar, and the status in text appears below. The description should tell someone exactly what the operation is doing. Examples of formatting include:
            </p>
            <ul>
                <li><strong>[Object]</strong> is being <strong>[operation name]</strong>, or</li>
                <li><strong>[Object]</strong> is being <strong>[operation name]</strong> to <strong>[destination name]</strong> or</li>
                <li><strong>[Object]</strong> is being <strong>[operation name]</strong> from <strong>[source name]</strong> to <strong>[destination name]</strong></li>
            </ul>
            <p>
                Status text is generally in units elapsed and total units. If the operation can be canceled, an “X” icon is used and should be placed in the upper right, aligned with the baseline of the operation name. When an error occurs, replace the status text with the error description using ms-fontColor-redDark.
            </p>
            <p>
                Real-world examples include copying files to a storage location, saving edits to a file, and more. Use units that are informative and relevant to give the best idea to users of how long the operation will take to complete. Avoid time units as they are rarely accurate enough to be trustworthy. Also, combine steps of a complex operation into one total bar to avoid “rewinding” the bar. Instead change the operation description to reflect the change if necessary. Bars moving backwards reduce confidence in the service.
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

            <ProgressIndicator PercentComplete=@progressValue
                               Style="margin:15px;"
                               Description="Sample Description"
                               Label="Progress Indicator with PercentComplete" />

            <ProgressIndicator Description="Sample Description"
                               Indeterminate="true"
                               Style="margin:15px;"
                               Label="Indeterminate Progress Indicator" />

        </div>
    </div>
</div>

@code {
    //ToDo: Add Demo sections
    decimal progressValue = 0;
    const decimal INTERVAL_INCREMENT = 0.01M;
    System.Timers.Timer timer;

    override protected Task OnInitializedAsync()
    {
        timer = new System.Timers.Timer(100);
        timer.Elapsed += TimerElapsed;

        return Task.CompletedTask;
    }

    override protected Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            timer.Start();
        }
        return Task.CompletedTask;
    }

    private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
    {
        InvokeAsync(() =>
        {
            progressValue += INTERVAL_INCREMENT;
            if (progressValue > 1)
            {
                progressValue = 0;
            }

            StateHasChanged();
        });
    }

}
