release
=======

The release verb adds a record to the history section for a component and bumps the ``next`` version to the next minor version.
If the next version should be a major version bump the appropriate component will need to be modified to reflect the correct next version.
This will also create a tag on the git repo with the format ``{name}_v{next}``

.. code-block:: text

    usage: gcv release [<args>]

    -c, --component            Makes a release entry into the history section for the named component
    -a, --all                  Makes a release entry into the history section for ALL changed components.
                               This argument takes precedence over the --component argument.
