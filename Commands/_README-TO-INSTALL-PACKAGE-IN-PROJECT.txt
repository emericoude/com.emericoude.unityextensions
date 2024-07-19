Instructions to installing packages in your Unity project:
1. Open your Unity Project.
2. Navigate to Project Settings -> Package Manager (NOT the package manager window).
3. In Scoped Registries, if you do not have a "registry.npmjs.org" entry, add one:
Name: registry.npmjs.org
URL: https://registry.npmjs.org
4. In the entry's scope(s), add your package: com.emericoude.[packagename]


Alternatively, use the recommended instructions from the package template (this doesn't work for me at times).
1. Have OpenUPM-CLI installed. https://github.com/openupm/openupm-cli#installation
2. Open the command line in your Unity project folder
3. Enter: 

	openupm --registry https://registry.npmjs.org add YOUR_PACKAGE_NAME


if anything goes wrong, refer to the package template repo this was sourced from https://github.com/IvanMurzak/Unity-Package-Template.