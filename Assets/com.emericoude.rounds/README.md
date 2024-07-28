# Emeric's Round System for Unity

## Overview
A base tool for you to create a game flow, where each step in your game is a "round". 
The codebase is small and well commented. I recommend starting at the RoundManager for context.

### Rounds
This tool expects you to implement types of Round. You can look at (or use)
TimedRound as an example.

A round should be self-contained and self-sufficient. It should manage its own state the moment it is
told to commence. Use the events available to update feedback (such as UI elements).

### Round Manager
The round manager handles the round's flow. Only one can be active at a time, and they are considered
in a queue. Note that rounds manage their own state once they are told to be active, and will automatically
notify the round manager if they conclude.

### Round Sequences
You can create round sequences presets (as Scriptable Objects) in the Context Menu 
(Emericoude -> Rounds -> Round Sequence). With our custom TypeFilterAttribute, or if you have Odin,
any non-abstract round implementation will appear as a dropdown option.
