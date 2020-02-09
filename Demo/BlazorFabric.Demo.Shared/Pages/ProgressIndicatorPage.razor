@page  "/progressIndicatorPage"

<h1>ProgressIndicator</h1>

<ProgressIndicator PercentComplete=@progressValue
                   Style="margin:15px;"
                   Description="Sample Description"
                   Label="Progress Indicator with PercentComplete" />

<ProgressIndicator Description="Sample Description"
                   Indeterminate="true"
                   Style="margin:15px;"
                   Label="Indeterminate Progress Indicator" />

@code {

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
