# Emeric's Round System for Unity

## Overview
A base tool for you to create a game flow, where each step in your game is a "Sequence Step". 
The codebase is small and well commented. I recommend starting at the Sequencer for context.

### Steps
This tool expects you to implement types of Steps. 
You can look at (or use) TimedRound as an example.

A step should be self-contained and self-sufficient. It should manage its own state the moment it is
told to begin. Use the events available to update feedback (such as UI elements).

It is primordial that you implement the Clone function to its fullest, copying over every serializable field.

### Sequencer
The Sequencer handles a sequence's flow. Only one sequence can be active at a time per sequencer.
Only one sequence step can be active at a time, and they are considered in a queue. 

Note that steps manage their own state once they are told to be active, and will automatically
notify the sequencer if they conclude.

### Sequence
You can create round sequences presets (as Scriptable Objects) in the Context Menu 
(Emericoude -> Sequencing -> Sequence). With our custom TypeFilterAttribute, or if you have Odin,
any non-abstract round implementation will appear as a dropdown option.
