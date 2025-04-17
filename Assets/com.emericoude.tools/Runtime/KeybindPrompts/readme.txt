To use this tool, you can modify the Default Keybind Sprite Asset, or create your own
in "Emericoude/Keybind Sprite Asset". This tool currently works on a per-device basis.
So you can set a Sprite Asset to be assigned to a specific device type. 
Note: We first search for direct-type reference (so without inheritance), and then check for parent types if no direct reference were found.
Because of this, make sure you put more specific types at the top of the list.

Create TMP_SpriteAssets from your sprite sheets, and then put them in a Resources/Sprite Assets/ folder.
In your TMP_SpriteAssets, you need to rename each character to the correct binding. You can see my examples.
TIP: You want bottom left pivot on your sprites. I've also felt that trimmed/tight sprites work best.

Some device don't yet exist in Unity (e.g. Steam Deck does not have a specific device type). 
This is a known limitation at the moment, but will be expanded on later as the need arises.

You can call GetActionActiveBindingToRichText() directly on a KeybindSpriteAssets reference.
This will return the rich text string value, and if you've set up your SpriteAssets properly,
TextMeshPro will display a sprite "emoji".


======


This sample uses Xelu's Free Controllers & Keyboard prompts, which can be found here 
https://thoseawesomeguys.com/prompts/ 
under the Creative Commons 0 (CC0) license (usable for any personal or commercial projects).

Sprite sheets were generated using the Code Shack's Sprite Sheet Generator, 
which can be found here https://codeshack.io/images-sprite-sheet-generator/