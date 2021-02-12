@page "/sliderPage"

<h1>Slider</h1>

<Demo Header="Horizontal Sliders" Key="0" MetadataPath="SliderPage">
    <Stack Horizontal="false"
              Style="width:100%;max-width:300px;"
              Tokens=@(new StackTokens() { ChildrenGap = new[] { 20.0 } })>
        <Slider />
        <Slider Label="Snapping slider example"
                   DefaultValue="20"
                   Min="0"
                   Max="50"
                   Step="10"
                   ShowValue="true"
                   SnapToStep="true"
                   />
        <Slider Label="Disabled example"
                   DefaultValue="300"
                   Min="50"
                   Max="500"
                   Step="50"
                   ShowValue="true"
                   Disabled="true"
                   />
        <Slider Label="Controlled example"
                   @bind-Value=controlledValue
                   Min="0"
                   Max="10"
                   ShowValue="true" />
        <Slider Label="Example with formatted value"
                   Min="0"
                   Max="100"
                   AriaValueText=@(val => $"{val.ToString()} percent")
                   ValueFormat=@(val => $"{val.ToString()}%")
                   ShowValue="true" 
                   />
        <Slider Label="Origin from zero"
                   Min="-5"
                   Max="5"
                   Step="1"
                   DefaultValue="2"
                   ShowValue="true"
                   OriginFromZero="true"
                   />
    </Stack>
</Demo>

<Demo Header="Vertical Sliders" Key="1" MetadataPath="SliderPage">
    <Stack Horizontal="true"
              Style="width:100%;height:200px;"
              Tokens=@(new StackTokens() { ChildrenGap = new[] { 20.0 } })>
        <Slider Label="Basic"
                   Min="1"
                   Max="5"
                   Step="1"
                   DefaultValue="2"
                   ShowValue="true"
                   Vertical="true"
                   />
        <Slider Label="Disabled"
                   DefaultValue="300"
                   Min="50"
                   Max="500"
                   Step="50"
                   ShowValue="true"
                   Disabled="true"
                   Vertical="true"
                   />
        <Slider Label="Controlled example"
                   @bind-Value=controlledValue
                   Min="0"
                   Max="10"
                   ShowValue="true" 
                   Vertical="true"
                   />
        <Slider Label="Example with formatted value"
                   Min="0"
                   Max="100"
                   AriaValueText=@(val => $"{val.ToString()} percent")
                   ValueFormat=@(val => $"{val.ToString()}%")
                   ShowValue="true" 
                   Vertical="true"/>
        <Slider Label="Origin from zero"
                   Min="-5"
                   Max="15"
                   Step="1"
                   DefaultValue="5"
                   ShowValue="true"
                   OriginFromZero="true"
                   Vertical="true"
                   />
    </Stack>
</Demo>


@code {
    double controlledValue = 0;
}
