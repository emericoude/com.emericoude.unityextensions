xcopy ..\README.md ..\Assets\com.emericoude.unityextensions\README.md* /Y
xcopy ..\README.md ..\Assets\com.emericoude.unityextensions\Documentation~\README.md* /Y
cd ..\Assets\com.emericoude.unityextensions
npm publish
pause