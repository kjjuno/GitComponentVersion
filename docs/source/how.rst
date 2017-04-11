How it Works
============

TODO: Detail the process for determining the version of a component

GitComponentVersion uses a combination of a ``GitComponentVersion.json`` file at the root of your repository,
and the git history to determine the version for multiple components within a single repository.

Unlike tools like GitVersion, GitComponentVersion cannot rely completely on branches, tags, and merge commits
to determine what a component version is. Because of this there is a slight tradeoff    

A Short Example
---------------

The following ``GitComponentVersion.json`` file configures a repository with 2 different components (shipping, and billing).
Each of these components has 2 directories, for a total of 4 nuget assemblies that each produce their own nuget package.
There are no history elements in the config file, meaning that there has never been a release of either of these components.

.. code-block:: json

    {
        "components": [
            {
                "name": "shipping",
                "next": "1.0.0",
                "dirs": [
                    "shipping.core",
                    "shipping.calculations"
                ]
            },
            {
                "name": "billing",
                "next": "1.0.0",
                "dirs": [
                    "billing.core",
                    "billing.useraccounts"
                ]
            }
        ]
    }

The following command will display the ``NuGetVersion`` for each component

.. code-block:: text

    > gcv -v NuGetVersion
    [
        {
            "Name": "shipping",
            "NuGetVersionV2":"1.0.0-alpha0169",
        },
        {
            "Name": "billing"
            "NuGetVersionV2":"1.0.0-alpha0077",
        }
    ]

This means that there have been 169 commits that modified the shipping component, and 77 that have modified the billing component.

At this point we are ready to make a release of all the components in this repository. Often this means creating a release branch and merging that into the master branch.
However, it is not important to GitComponentVersion what workflow you use. From your repository, on the commit you intend to tag as a release, run the following command.

.. code-block:: text

    > gcv release

The following things just happened.

1. Each changed component now has a new tag in the format ``{name}_v{next}``
   In other words, the current commit has now been tagged with ``shiping_v1.0.0``, and ``billing_v1.0.0``
2. A new release element has been added into each components history section
3. The ``next`` variable has been bumped to the next minor version for each released component.

The config file now looks like this:

.. code-block:: json

    {
        "components": [
            {
                "name": "shipping",
                "next": "1.1.0",
                "dirs": [
                    "shipping.core",
                    "shipping.calculations"
                ],
                "history": [
                    {
                        "version": "1.0.0",
                        "sha": "bb2febab25fa1b0312bc61af0116e2de99fa0d5f"
                    }
                ]
            },
            {
                "name": "billing",
                "next": "1.1.0",
                "dirs": [
                    "billing.core",
                    "billing.useraccounts"
                ],
                "history": [
                    {
                        "version": "1.0.0",
                        "sha": "4fe97191cc24cfb1f19e3880b3cffa87d10051c7"
                    }
                ]
            }
        ]
    }

The change to GitComponentVersion.json should be committed, and the changes should be pushed to the remote, as well as pushing the new tags

.. code-block:: text

    > git add GitComponentVersion.json
    > git commit -m "Updating the GitComponentVersion.json with new release information"
    > git push
    > git push --tags

At this point your release is complete. You now need to make sure that your development branch is up to date. If you develop against the master branch
you are probably already done. However, if you use a master and develop branch you should merge this change into develop so that new development will
begin to version with the new version.