release
=======

When no arguments are supplied it is assumed that all *changed* components should be released. Optionally you can specify
which components should be considered for release.

The release verb will tag the git repo with the format ``{name}_v{next}``
and it will add a record to the history section for a component. Then it will bump the ``next`` version to the next minor version.
If the next version should be a major version bump the appropriate component will need to be modified to reflect the correct next version.

.. code-block:: text

    usage: gcv release [<args>]

    -c, --component            A comma separated list of component names.
                               Makes a release entry into the history section for the named component.
    -n, --dry-run              Don't actually make a release, just show what components would be released.
