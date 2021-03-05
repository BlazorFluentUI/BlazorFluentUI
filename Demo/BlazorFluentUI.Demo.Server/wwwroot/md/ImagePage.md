@page  "/imagePage"
<div>
    <header class="root">
        <h1 class="title">Image</h1>
    </header>
    <div class="section" style="transition-delay: 0s;">
        <div id="overview" tabindex="-1">
            <h2 class="subHeading hiddenContent">Overview</h2>
        </div>
        <div class="content">
            <div class="ms-Markdown">
                <p>
                    An image is a graphic representation of something (e.g photo or illustration). The borders have been added to these examples in order to help visualize empty space in the image frame.
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

                <h3>ImageFit Unset</h3>
                Width set at 200
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.Unset Width="200" />
                Height set at 100
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.Unset Height="100" />
                Height set at 100, Width set at 200
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.Unset Height="100" Width="200" />

                <h3>ImageFit None</h3>
                Height set at 100, Width set at 200 => cropped to fit frame, positioned top left
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.None Height="100" Width="200" />
                Height set at 200, Width set at 200 => just placed at top left
                <Image Src="smallSampleImage.jpg" ImageFit=@ImageFit.None Height="200" Width="200" />

                <h3>ImageFit Center</h3>
                Height set at 100, Width set at 200 => cropped to fit frame, centered
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.Center Height="100" Width="200" />
                Height set at 200, Width set at 200 => centered in frame space
                <Image Src="smallSampleImage.jpg" ImageFit=@ImageFit.Center Height="200" Width="200" />

                <h3>ImageFit Contain</h3>
                Height set at 100, Width set at 200 => fits largest dimension to available frame space
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.Contain Height="100" Width="200" />
                Height set at 200, Width set at 100 => fits largest dimension to available frame space
                <Image Src="sampleImage.jpg" ImageFit=@ImageFit.Contain Height="200" Width="100" />

                <div style="height:200px;" />
            </div>
        </div>
    </div>
</div>
@code {
    //ToDo: Add Demo sections

}
