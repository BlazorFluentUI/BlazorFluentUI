@page  "/imagePage"
    <div >
        <h1>Image</h1>

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

        <div style="height:200px;"/>
    </div>
@code {

}
