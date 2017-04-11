Command Line Reference
======================

::

    usage: gcv [<args>]

    verbs:
    init                Sets up the initial GitComponentVersion.json file
    release             Makes the next version permanent (TODO: This doesn't feel quite right)

    arguments:
    -c, --component     Calculates the version for the named component
    -o, --output        One of [json,buildserver]. The default is json
    -h, --help          Display this help screen.


.. toctree::
   :maxdepth: 2
   :caption: verbs

   release