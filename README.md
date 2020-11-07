# CG.Tools.CodeMap: 
---
[![Build Status](https://dev.azure.com/codegator/CG.Tools.CodeMap/_apis/build/status/CodeGator.CG.Tools.CodeMap?branchName=main)](https://dev.azure.com/codegator/CG.Tools.CodeMap/_build/latest?definitionId=21&branchName=main)
[![Github docs](https://img.shields.io/static/v1?label=Documentation&message=online&color=blue)](https://codegator.github.io/CG.Tools.CodeMap/index.html)
[![NuGet downloads](https://img.shields.io/nuget/dt/CG.Tools.CodeMap.svg?style=flat)](https://nuget.org/packages/CG.Tools.CodeMap)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/codegator/CG.Tools.CodeMap/21)

#### What does it do?
A tool that generates a code map from a .NET assembly.

(Yes, we know Visual Studio already does that, but we run the Community Edition of Visual Studio and it doesn't do that)

##### Quick Note:
This project uses Syncfusion controls for the UI. We took that approach, as opposed to rolling our own, or using open source alternatives, because:

* They're free, provided you sign up for the ridiculously generous Syncfusion Community License, [HERE](https://www.syncfusion.com/products/communitylicense)
* They already written, which means we can focus on this tool rather than worring about reinventing an entire diagram control, from scratch.
* They are supported, which is more than most open source control packages can say.
* Did we mention they are FREE?? Seriously, go [HERE](https://www.syncfusion.com/products/communitylicense) and sign up!

If you do get your own Syncfusion license, you'll need to add your license key to the appSettings.json file, like this:

```
{
  "Syncfusion": "Your Syncfusion license here"
}
```

##### UI walk through:
Here is the overal UI:

![The main UI](.\images\mainUI.jpg)

Opening .NET assemblies is easy using the file|open menu command, or using the file option toolbar:

![File Open](.\images\open.jpg)

Filtering unwanted assemblies out is also easy, using the filter drop down:

![File Open](.\images\filters.jpg)

Zooming the diagram up or down is easy, using the zoom controls:

![File Open](.\images\zoom.jpg)

Finally, you can layout a diagram at any time using the layout control:

![File Open](.\images\layout.jpg)


#### How do I contact you?
If you've spotted a bug in the code please use the project Issues [HERE](https://github.com/CodeGator/CG.Tools.CodeMap/issues)

