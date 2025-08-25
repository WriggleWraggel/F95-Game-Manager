# F95 Game manager

This project gives user the ability to manage their F95 library locally with a simple UI.

## Features
- View and organize your F95 library
- Launch games directly from the manager
- Detect game updates
- Delete games from the library
- Import games from your local filesystem
- Import archives from your download folder to the game library
- Manage unzipping of the imported archives
- Detecting possible archives to import
- Searching your library
- Searching F95 for games
- Creating default folders for the games

## Frameworks
- .Net 8
- Blazor
- Maui
- Fluxor
- MediatR
- NSubstitute
- XUnit
- BUnit

## Coding guidelines
- Blazor controls are minimal and have their behavior defined by the parent component.
- Blazor components orchestrate controls, to implement their specific feature
- Blazor pages are collections of components
- Blazor Components fire Fluxor actions, which trigger effects, which usually call MediatR and which then call services
- All Blazor files should have a BUnit test file