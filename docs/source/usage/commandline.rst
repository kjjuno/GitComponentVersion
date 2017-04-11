Command Line
============

GitComponentVersion is available on both nuget.org and Chocolatey

Nuget
-----

Available on nuget.org under GitComponentVersion.CommandLine

::

    nuget install GitComponentVersion.CommandLine

Chocolatey
----------

Available on Chocolatey under GitComponentVersion.Portable

::

    choco install GitComponentVersion.Portable

Output
------

By default GitComponentVersion returns a json object to stdout containing all the variables which GitComponentVersion  generates.
This works great if you want to get your build scripts to parse the json object then use the variables, but there is a simpler way.

GitVersion.exe /output buildserver will change the mode of GitComponentVersion to write out the variables to whatever build server it is running in.
You can then use those variables in your build scripts or run different tools to create versioned NuGet packages or whatever you would like to do.
See build servers for more information about this.