# Unity HideFlags Viewer

## What’s this?

Have you ever had been bothered by ghosted GameObjects were saved in a scene with using [HideFlags](https://docs.unity3d.com/ScriptReference/Object-hideFlags.html)?  
This is a tool window for listing and editing HideFlags in the scene for Unity.  

## Supported Unity versions

Unity 2020.3 or higher.  

## Installation

Via Package Manager.  
Open the Package Manager window in your Unity editor, select `Add package from git URL...` from the `+` button in the top left, enter following and press the `Add` button.  

```text

```

Or open the `Packages/manifest.json` file and add an item like the following to the `dependencies` field.  

```json

```

## How to use

Select `Window -> HideFlags Viewer` in the menu to open the window.  

### Operations

| Item | Explanation |
| - | - |
| Scene | A dropdown to select the target scene from the currently open scenes |
| Edit HideFlags | Check this if you want to edit HideFlags |
| Exclude HideFlags.None | Check this if you want to exclude HideFlags.None from the table below |
| GameObject-HideFlags | A list of GameObjects in the scene and their HideFlags |
| Reload | Relist GameObjects in the scene |
