@page "/spinnerPage"

<header class="root">
    <h1 class="title">Spinner</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A Spinner is an outline of a circle which animates around itself indicating to the user that things are processing. A Spinner is shown when it's unsure how long a task will take making it the indeterminate version of a ProgressIndicator. They can be various sizes, located inline with content or centered. They generally appear after an action is being processed or committed. They are subtle and generally do not take up much space, but are transitions from the completed task.
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

            <h4>Normal</h4>
            <Spinner Size=@SpinnerSize.Large></Spinner>
            <Spinner Size=@SpinnerSize.Medium></Spinner>
            <Spinner Size=@SpinnerSize.Small></Spinner>
            <Spinner Size=@SpinnerSize.XSmall></Spinner>
        </div>
        <div class="subSection">
            <h4>With Label</h4>
            <Spinner Size=@SpinnerSize.Medium Label="SpinnerLabel" LabelPosition=@SpinnerLabelPosition.Top></Spinner>
            <Spinner Size=@SpinnerSize.Medium Label="SpinnerLabel" LabelPosition=@SpinnerLabelPosition.Left></Spinner>
            <Spinner Size=@SpinnerSize.Medium Label="SpinnerLabel" LabelPosition=@SpinnerLabelPosition.Bottom></Spinner>
            <Spinner Size=@SpinnerSize.Medium Label="SpinnerLabel" LabelPosition=@SpinnerLabelPosition.Right></Spinner>
        </div>
    </div>
</div>

@code {
    //ToDo: Add Demo sections
}
