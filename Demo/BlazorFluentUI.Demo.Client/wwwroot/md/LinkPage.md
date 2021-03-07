@page "/linkPage"

<header class="root">
    <h1 class="title">Link</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                Links lead to another part of an app, other pages, or help articles. They can also be used to initiate commands.
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
            <p>
                When a link has an href, <Link Href="http://dev.office.com/fabric/components/link">it renders as an anchor tag.</Link> Without an
                href, <Link>the link is rendered as a button</Link>. You can also use the disabled attribute to create a
                <Link Disabled=true Href="http://dev.office.com/fabric/components/link">disabled link.</Link>
            </p>
        </div>
    </div>
</div>
@code{
    //ToDo: Add Demo sections

}

