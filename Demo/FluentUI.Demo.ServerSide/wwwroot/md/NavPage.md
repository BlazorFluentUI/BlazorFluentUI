@page "/navPage"

<h1>Nav</h1>

<Nav>

    <NavLinkGroup Name="First" CollapseByDefault="false">
        <FluentUI.NavLink Name="A Link" IconName="Airplane" Url="navPage#ALink" Id="ALink" NavMatchType="NavMatchType.AnchorIncluded" />
        <FluentUI.NavLink Name="B Link" Url="navPage#BLink" NestedDepth="0" Id="BLink" NavMatchType="NavMatchType.AnchorIncluded">
            <FluentUI.NavLink Name="B SubLink 1" NestedDepth="1" Url="navPage#BLink1" Id="BLink1" NavMatchType="NavMatchType.AnchorIncluded" />
            <FluentUI.NavLink Name="B SubLink 2" NestedDepth="1" Url="navPage#BLink2" Id="BLink2" NavMatchType="NavMatchType.AnchorIncluded" />
            <FluentUI.NavLink Name="B SubLink 3" NestedDepth="1" Url="navPage#BLink3" Id="BLink3" NavMatchType="NavMatchType.AnchorIncluded" />
        </FluentUI.NavLink>
        <FluentUI.NavLink Name="C Link" Url="navPage#CLink" Id="CLink" NavMatchType="NavMatchType.AnchorIncluded" />

    </NavLinkGroup>
    <NavLinkGroup Name="Second" CollapseByDefault="true">
        <FluentUI.NavLink Name="D Link" Url="navPage#DLink" Id="DLink" NavMatchType="NavMatchType.AnchorIncluded" />
        <FluentUI.NavLink Name="E Link" Url="navPage#ELink" Id="ELink" NavMatchType="NavMatchType.AnchorIncluded" />
        <FluentUI.NavLink Name="F Link" Url="navPage#FLink" Id="FLink" NavMatchType="NavMatchType.AnchorIncluded" />
    </NavLinkGroup>
    <NavLinkGroup Name="Third" CollapseByDefault="true">
        <FluentUI.NavLink Name="G Link" Url="navPage#GLink" Id="GLink" NavMatchType="NavMatchType.AnchorIncluded" />
    </NavLinkGroup>
    <NavLinkGroup Name="Fouth" CollapseByDefault="true">
        <FluentUI.NavLink Name="H Link" Url="navPage#HLink" Id="HLink" NavMatchType="NavMatchType.AnchorIncluded" />
    </NavLinkGroup>

</Nav>

@code{


}