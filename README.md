# Emeric's Unity Extensions

## Warning / Foreword
This package is a collection of utilities I use as part of personal projects. It is frequently updated with breaking changes as most of it is WIP and not necessarily meant for public usage, update at your own risk.
This is the repo for working in the package, see the 'How to add the package to your project' section below to install it.
There's probably some cleanup and breaking apart that I might do as this gets bigger and bigger...

## How to add the package to your project
1. Open your Unity Project.
2. Navigate to Project Settings -> Package Manager (NOT the package manager window).
3. In Scoped Registries, if you do not have a "registry.npmjs.org" entry, add one (name: `registry.npmjs.org`, URL: `https://registry.npmjs.org`).
4. In the entry's scope(s), add the package: `com.emericoude.tools`.
5. Once this is done, the package should appear in the Package Manager window.

## com.emericoude.tools
A collection of various script (utilities, helpers, types, etc.) that I used in most projects. Note that the elements listed is not everything, mainly highlights to showcase some of the stuff in here.
### Attributes
A collection of useful attributes.
- `[BetterCurveField]`: Improved AnimationCurve fields, which displays a foldout of each key as well as "scale" properties. Scaling the curve will, in most cases, keep the shape of the curve accordingly. <br/><img width="248" height="64" alt="image" src="https://github.com/user-attachments/assets/beaff3c1-7ef0-4ffd-bb38-92aaeed218db" />
- `[DrawInDebugInfoBox]`: REQUIRES ODIN INSPECTOR. Basically a shortcut to create a group foldout where all properties are grayed out. The debug box always exists at the end of the inspector. Useful to display property values during playmode. <br/><img width="248" height="72" alt="image" src="https://github.com/user-attachments/assets/46201711-2a4a-4be2-9147-e71352adab0e" />
- `[Layer]`: Use on an int field to display as a single layer dropdown.
- `[RadiansDisplayedAsDegrees]`: It's generally better to use radians internally, but degree is easier to understand, so this does a simple conversion in editor, rather than at runtime.
### Cinemachine
- `CinmeachineImpulseListenerAtTarget`: So let's say you have a top-down game, and your screen shake impulses are coming from way down below, you can use this to listen to them from your character's position instead of the camera's.
### Frameworks
- GameObject Pooling
- Singleton types
- TimeManager and TimeEffects
### Helpers
A large library of helper-type functions (extension methods) and some extra.
### Input
Some stuff for the new input system.
- `SinglePlayerDeviceHandler`: A simple utility that makes it easier to handle input devices (for instance when you want to know which type of device the user is using) for a single player context.
- `TimedHoldInteraction`: A hold interaction which has a maximum duration.
- `TimedSlowTapInteraction`: A slow tap interaction with a maximum duration.
### Input Binding Sprite
A library to have input keybinding sprites in your text. This by default uses the [Xelu Controller and Keyboard Prompts](https://github.com/DJLink/Xelu_Free_Controller-Key_Prompts)
### Pawns
A PlayerController/Pawn structure similar to Unreal Engine's. Basically, controllable entities are called "Pawns", and player (or AI) entities possess them.
### Physics
- `CollisionContactEvents`: A component that provides collision contacts in-between OnCollisionEnter and OnCollisionExit events.
### StateMachine
A state machine framework that is code-driven. Create your state machine in one spot by building transitions.
### UI
Various uGUI scripts:
- `Interactable 3D`: A framework for creating 3D UI (i.e. buttons/navigation using Unity's event system, but for 3D meshes).
- `ProjectedCanvas`: A framework to render a canvas onto a mesh and still have it be interactable, for instance if you want a 3D computer screen.
- `Node / NodeGraph`: A framework for creating simple node graph.
- `UILineRenderer`: A simple line renderer for canvas.
- `RadialLayoutGroup`: A layout group that lays out things radially.

## com.emericoude.sequencing
Deprecated, basically a simple state machine structure thatwas meant to be used for sequencing actions (such as rounds). I now use the StateMachine framework inside of com.emericoude.tools instead.
