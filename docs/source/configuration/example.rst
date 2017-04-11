Example Configuration File
==========================

.. code-block:: json

    {
        "components": [
            {
                "name": "calculations",
                "next": "2.0.0",
                "dirs": [
                    "Calculations",
                    "Calculations.Core"
                ],
                "assemblyinfo": [
                    "AssemblyInfo.cs",
                    "SharedAssemblyInfo.cs"
                ],
                "history": [
                    {
                        "version": "1.1.0",
                        "sha": "8c8b84ae9f1c7791b69340f3cdb90025822ba77e",
                        "notes": "blah blah blah"
                    },
                    {
                        "version": "1.0.0",
                        "sha": "8c8b84ae9f1c7791b69340f3cdb90025822ba77e",
                        "notes": "blah blah blah"
                    }
                ]
            },
            {
                "name": "billing",
                "next": "1.0.0",
                "dirs": [
                    "Billing.Core",
                    "Billing.Customer"
                ]
            }
        ]
    }