# Emeric's Unity Extensions

## Warning / Foreword
This package is a collection of utilities I use and grow as part of personal projects. **It is frequently updated with breaking changes as most of it is WIP and not necessarily meant for public usage, update at your own risk***. As this gets bigger and bigger, I may break this down into multiple smaller packages. Feel free to get inspired or copy stuff you need.

This is the repo for working in the package, see the 'How to add the package to your project' section below to install it.

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
- `[RadiansDisplayedAsDegrees]`: Displays a radians value in degrees in the inspector.
### Cinemachine
- `CinmeachineImpulseListenerAtTarget`: So let's say you have a top-down game, and your screen shake impulses are coming from way down below, you can use this to listen to them from your character's position instead of the camera's.
### Frameworks
- GameObject Pooling (specifically, a lazy gameobject pool, great for prototyping while still retaining the benefits of an object pool).
- Singleton types (lazy & persistent).
- TimeManager and TimeEffects (for things such as time stops, or timescale effects).
### Helpers
A large library of helper-type functions (extension methods) and some extra.
### Input
Some stuff for the new input system.
- `SinglePlayerDeviceHandler`: A simple utility that makes it easier to handle input devices (for instance when you want to know which type of device the user is using) for a single player context.
- `TimedHoldInteraction`/`TimedSlowTapInteraction`: A hold/SlowTap interaction which has a maximum duration. Useful if you want an input to be cancelled after a set duration.
### Input Binding Sprite
A library to have input keybinding sprites in your text as emojis. This dynamically supports different device types, and can be customized with your own icons. By default, this uses the [Xelu Controller and Keyboard Prompts](https://github.com/DJLink/Xelu_Free_Controller-Key_Prompts).
![showcase__input-binding-sprites](https://github.com/user-attachments/assets/4dc528d4-4b8d-4d14-ab91-40a4cddb7a1c)
### Pawns
A simple PlayerController/Pawn structure similar to Unreal Engine's. Basically, controllable entities are called "Pawns", and player (or AI) entities possess them.
### Physics
- `CollisionContactEvents`: A component that provides collision contacts in-between OnCollisionEnter and OnCollisionExit events. For instance, imagine you have a plank on touching the ground on one end, but not the other and is about to fall at an angle. Only the initial collision will trigger OnCollisionEnter, but if you want to let's say play a "thump" sound effect when the remainder of the plank hits the ground, you can use this component to listen for new contacts with already active collisions.
### StateMachine
A state machine framework that is code-driven. Create your state machine in one spot by building transitions.
### UI (uGUI)
- `Interactable 3D`: A framework for creating 3D UI (i.e. buttons/navigation using Unity's event system, but for 3D meshes). ![showcase__3D-UI](https://github.com/user-attachments/assets/0fac949c-b73a-4c8a-871c-8cd00a87a1e7)
- `ProjectedCanvas`: A framework to render a canvas onto a mesh and still have it be interactable, for instance if you want a 3D computer screen (or a deformable map like in this example). 
![showcase__projected-canvas](https://github.com/user-attachments/assets/a68856ab-6f39-413f-bfbe-56999b1f5722)
- `Node / NodeGraph`: A framework for creating simple node graph.
- `UILineRenderer`/`UILineRendererController`: A simple line renderer for canvas, alongside a complex controller for connecting nodes in a graph (for instance).
- `RadialLayoutGroup`: A layout group that lays out things radially. <img width="255" height="210" alt="image" src="https://github.com/user-attachments/assets/a4396857-978b-454d-97a3-fd1570c32d1c" />



## com.emericoude.sequencing
Deprecated, basically a simple state machine structure thatwas meant to be used for sequencing actions (such as rounds). I now use the StateMachine framework inside of com.emericoude.tools instead.
