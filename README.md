# Markdown File

# Jenin FontAwesome Blazor

[![Nuget](https://img.shields.io/nuget/v/Jenin.FontAwesome.Blazor)](https://www.nuget.org/packages/Jenin.FontAwesome.Blazor/)
[![Nuget](https://img.shields.io/nuget/dt/Jenin.FontAwesome.Blazor)](https://www.nuget.org/packages/Jenin.FontAwesome.Blazor/)
[![GitHub](https://img.shields.io/github/license/TarekNajem04/Jenin.FontAwesome.Blazor)](https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/blob/master/LICENSE)
[![GitHub](https://img.shields.io/github/stars/TarekNajem04/Jenin.FontAwesome.Blazor)](https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/)  

* Free FontAwesome 6 components for ASP.NET Blazor
* .NET 6.0+ with Blazor WebAssembly or Blazor Server (other hosting models not tested yet, .NET 6.0 fully supported)

Getting started
===============

Prerequisites
-------------

Jenin.FontAwesome.Blazor components have the following requirements:

*   .NET 6.0 or newer
*   Blazor WebAssembly or Blazor Server hosting model (other options not tested yet)

Installation
------------

### 1\. Install the package

Find the [Jenin.FontAwesome.Blazorp](https://www.nuget.org/packages/Jenin.FontAwesome.Blazor/) package through NuGet Package Manager or install it with following command:

    dotnet add package Jenin.FontAwesome.Blazor

### 2\. Add CSS & JavaScript references

#### A. CSS

Add the following to your HTML head section, it's either `index.html`, `_Host.cshtml` or `_Layout.cshtml` depending on whether you're running WebAssembly or Server:

    <!DOCTYPE html>
    <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <base href="~/" />
    
            ...
    
            <!--load all Font Awesome styles -->
            <link href="/your-path-to-fontawesome/css/all.css" rel="stylesheet">
    
            ...
    
        </head>
    
        <body>
            ...
        </body>
    
    </html>

##### For more details of how host yourself - Font Awesome [Host Yourself - Web Fonts](https://fontawesome.com/v6/docs/web/setup/host-yourself/webfonts)

#### B. JavaScript

At the end of HTML `<body>` section of either `index.html`, `_Host.cshtml` or `_Layout.cshtml` add this:

    <!DOCTYPE html>
    <html lang="en">
    <head>
    </head>
    
    <body>
        ...
    
        <!--
            By default Font Awesome will replace elements. However, setting this value to nest will cause Font Awesome to add a child element to the existing <i> tag.
        -->
        <script src="_content/Jeninnet.FontAwesome.Blazor/js/fontawesome-Svg[nest]-config.js" type="text/javascript"></script>
    
        <!--OR -->
        <!--
            When false this will cause Font Awesome to use Webfont.
            When you set this property to false, you will not be able to use the Layers feature.
            <script src="_content/Jeninnet.FontAwesome.Blazor/js/fontawesome-Svg[false]-config.js" type="text/javascript"></script>
        -->
    
        <script src="your-path-to-fontawesome/js/all.min.js" type="text/javascript"></script>
        <script src="_content/Jeninnet.FontAwesome.Blazor/js/fontawesome-config-utilities.js" type="text/javascript"></script>
    </body>
    
    </html>

### 3\. Import namespaces

Add following to your `_Imports.razor` file:

    @using Jenin.FontAwesome.Blazor;
    @using Jenin.FontAwesome.Blazor.Extensions
    @using Jenin.FontAwesome.Blazor.Components
    @using Jenin.FontAwesome.Blazor.Transformations
    @using Jenin.FontAwesome.Blazor.Animations

**Ready, set, go!**

Are yoy ready?

    <button class="btn btn-outline-primary mb-4 ms-2 d-flex align-items-center gap-2" @onclick=GotoStartPage>
        <Icon IconStyle=IconStyle.Solid IconSize=IconSize.X02 IconName="fa-bell" Animation=@Animation.Shake() Color="gold"/>
        <span>Are yoy ready?</span>
    </button>
    
    @code{
        [Inject]
        private NavigationManager NavigationManager{ get; set; }
    
        private void GotoStartPage() => NavigationManager.NavigateTo("/icon-basics-style");
    }

Copy