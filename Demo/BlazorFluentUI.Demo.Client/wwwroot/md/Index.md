﻿@page "/oldIndex"


<h1>Icons</h1>
<BFUIcon IconName="Airplane" ClassName="iconExample" />
<BFUIcon IconName="Mail" ClassName="iconExample" />
<BFUIcon IconName="Video" ClassName="iconExample red" />
<BFUIcon IconName="EditMirrored" ClassName="iconExample red" />

<BFUPrimaryButton Text="Test" />


<h1>Image icon</h1>
<BFUIcon IconSrc="smallSampleImage.jpg" ClassName="iconExample" />
@code{
    bool isChecked = false;
}