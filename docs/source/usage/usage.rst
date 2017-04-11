Usage
=====

.. code-block:: text

    usage: gcv [<args>]

    verbs:
    init                        Sets up the initial GitComponentVersion.json file
    release                     adds a record to the history section for a component,
                                tags the repostiory with {name}_v{next},
                                and bumps the [next] version to the next minor version.
    update                      Will recursively search for all 'AssemblyInfo.cs' files in the component
                                and update them with the correct version number.

    arguments:
    -c, --component             A comma separated list of component names.
                                Makes a release entry into the history section for the named component.
    -v, --variable              Restricts output to the specified variable
    -h, --help                  Display this help screen.


Examples
--------

Display the specified variable for info for a single component

.. code-block:: text

    > gcv -c core -v NuGetVersion
    3.2.0-alpha0169

Display the specified variable for info for all components

.. code-block:: text

    > gcv -v NuGetVersionV2
    [
        {
            "Name": "core",
            "NuGetVersionV2":"3.2.0-alpha0169",
        },
        {
            "Name": "billing"
            "NuGetVersionV2":"3.1.0-alpha0077",
        }
    ]

Display the version info for a single component

.. code-block:: text

    > gcv -c core
    {
        "Major":3,
        "Minor":2,
        "Patch":0,
        "PreReleaseTag":"alpha.169",
        "PreReleaseTagWithDash":"-alpha.169",
        "PreReleaseLabel":"alpha",
        "PreReleaseNumber":169,
        "BuildMetaData":"",
        "BuildMetaDataPadded":"",
        "FullBuildMetaData":"Branch.develop.Sha.88563159817f8ff73897f47119bdb542ef9121db",
        "MajorMinorPatch":"3.2.0",
        "SemVer":"3.2.0-alpha.169",
        "AssemblySemVer":"3.2.0.0",
        "FullSemVer":"3.2.0-alpha.169",
        "InformationalVersion":"3.2.0-alpha0169",
        "BranchName":"develop",
        "Sha":"88563159817f8ff73897f47119bdb542ef9121db",
        "NuGetVersion":"3.2.0-alpha0169",
        "CommitsSinceVersionSource":169,
        "CommitsSinceVersionSourcePadded":"0169",
        "CommitDate":"2017-04-10"
    }

Display the version info for all components

.. code-block:: text

    > gcv
    [
        {
            "Name": "core",
            "Major":3,
            "Minor":2,
            "Patch":0,
            "PreReleaseTag":"alpha.169",
            "PreReleaseTagWithDash":"-alpha.169",
            "PreReleaseLabel":"alpha",
            "PreReleaseNumber":169,
            "BuildMetaData":"",
            "BuildMetaDataPadded":"",
            "FullBuildMetaData":"Branch.develop.Sha.88563159817f8ff73897f47119bdb542ef9121db",
            "MajorMinorPatch":"3.2.0",
            "SemVer":"3.2.0-alpha.169",
            "AssemblySemVer":"3.2.0.0",
            "FullSemVer":"3.2.0-alpha.169",
            "InformationalVersion":"3.2.0-alpha0169",
            "BranchName":"develop",
            "Sha":"88563159817f8ff73897f47119bdb542ef9121db",
            "NuGetVersion":"3.2.0-alpha0169",
            "CommitsSinceVersionSource":169,
            "CommitsSinceVersionSourcePadded":"0169",
            "CommitDate":"2017-04-10"
        },
        {
            "Name": "billing"
            "Major":3,
            "Minor":1,
            "Patch":0,
            "PreReleaseTag":"alpha.77",
            "PreReleaseTagWithDash":"-alpha.77",
            "PreReleaseLabel":"alpha",
            "PreReleaseNumber":77,
            "BuildMetaData":"",
            "BuildMetaDataPadded":"",
            "FullBuildMetaData":"Branch.develop.Sha.378037291d0debe840a7cf917ca7e90a914ad390",
            "MajorMinorPatch":"3.1.0",
            "SemVer":"3.1.0-alpha.77",
            "AssemblySemVer":"3.1.0.0",
            "FullSemVer":"3.1.0-alpha.77",
            "InformationalVersion":"3.1.0-alpha0077",
            "BranchName":"develop",
            "Sha":"378037291d0debe840a7cf917ca7e90a914ad390",
            "NuGetVersion":"3.1.0-alpha0077",
            "CommitsSinceVersionSource":77,
            "CommitsSinceVersionSourcePadded":"0077",
            "CommitDate":"2017-04-07"
        }
    ]


.. toctree::
   :maxdepth: 2
   :caption: More Help

   release
   update