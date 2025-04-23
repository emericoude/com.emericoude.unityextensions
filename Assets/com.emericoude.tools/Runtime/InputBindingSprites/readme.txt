1. CREATE A BINDING SPRITE ASSET LIST
To use this tool, you can modify the Default Input Binding Sprite Asset List (or create your own), or create your own. 
Actually, create your own, the default may be subject to change in the future. You can find it under Emericoude/Input Binding Sprite Asset List

2. ASSIGN SPRITE ASSETS TO DEVICES
In this asset, you can assign a sprite asset (basically a fancy sprite sheet), to specific device types.
To create a sprite asset, you first import a spritesheet (set it to Sprite, and multiple, then slice it, etc),
then right click it and do Create->TextMeshPro->SpriteAsset. Once that is done you need to rename each
sprite character to their associated binding (for example, the sprite for the left joystick should be named "leftStick").
You can use the input debugger for help.

THE SPRITE ASSET MUST BE PUT INTO A "Assets/*/Resources/Sprite Assets/[put sprite assets here]" folder. 
This is how TextMeshPro can access them for rendering sprites through rich text. Look at the examples for reference.

3. CODE STUFF
Use the helper functions directly on your InputBindingSpriteAssetList. They should be well documented.
You can also use the DeviceHandler (may be something like SinglePlayerDeviceHandler) for some additional utilities.

LIMITATIONS:
1. Some device don't have specific types in Unity (e.g. Steam Deck or JoyCons). This makes sense as Unity cannot be
expected to list every single device in history. This means this system doesn't currently support some devices.
I'm investigating custom device types so that something like a joyCon could auto-register as that. Otherwise,
this system will need a way to support more custom or specific stuff.

2. There are some cases where you'd want your user to be able to set the displayed gamepad scheme manually. This is
because some 3rd-party gamepads may be registered in a non-specific manner in which case whatever is the fallback for that
will be used. I want to add support for overrides in the future.


======


My examples uses Xelu's Free Controllers & Keyboard prompts, which can be found here 
https://thoseawesomeguys.com/prompts/ 
under the Creative Commons 0 (CC0) license (usable for any personal or commercial projects).

Sprite sheets were generated using the Code Shack's Sprite Sheet Generator, 
which can be found here https://codeshack.io/images-sprite-sheet-generator/