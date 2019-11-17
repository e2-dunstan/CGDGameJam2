# Foundry Frenzy Post Mortem

#### The Boomers: Ellie, Alex, Dan, Ben, Lewis, Sol

#
## Teamwork and Project Development

### GitHub

Not as good, maybe because the project was so different and the final goal was more open ended

### Communication

Improved, but once again hit crunch time due to people not able to complete things on time or stick to milestones

#
## The Game

### Code

Each touch is stored in a pooled list of touches with elements such as their movement phase, what they're tapping, etc tracked without the game getting confused about which touch is which.

### Design

AI

Touch Input

Level Design

Task System

UI

As the theme of this Games Jam was about Juice and Polish, UI was a much larger focus this time round. The Surge library was imported into the project to ensure a tweening system could be used for managing all UI animations with unique easing and animation curves. This was primarily used for spawning and despawning UI elements and could be used outside of Unity's global time. Extra effort was made to ensure UI elements had correct ordering, resized correctly for varying screen resolutions and were positioned on world space objects so notifications would follow the employees correctly. Each UI element that required to interact with other parts of the code base had interface functions built to populate the UI element but the delay in tween animations sometimes lead to problems with spawning tasks. Also the text font used, while retro styled, can be difficult to read at times due to letter spacing. 

Pathing line renderer

Particle systems

Audio

#### The Foundry

Level design: open, Foundry colour palette

Our first concept of the game had it competitive with a split screen where two teams battled to run the best foundry but, upon evaluation, we felt it would be better if it was cooperative to reflect the foundry values of connectedness, and the teamwork vibe.

#### Bartle Player Types

The Socialiser is the most common Bartle player type and when combined with the game being multi-touch and cooperative, it made sense that people who fall under this category were our primary target audience.

Additionally, the game also appeals to the achiever, with rewards and benefits from doing well in the game reflected by the reputation tracker.

#
## Improvements

More juice
Further visual improvements should be made to the existing juice effects in order to convey more information to the player. For example, when drawing a path for an employee the path could change colour to indicate whether the destination is valid or not. As the employees have a tendency to sit down, they should emit an effect to inform the player that this employee is available to control. With better planning more polished juice effects like these could have been produced earlier in the project.

In terms of UI, the "reputation" system could have been displayed with a variation of the progress bar instead of set stars so you can visually see the reputation fill up / decrease over time. This could be combined with additional UI particles that show stars traveling from the employees to the reputation bar, allowing players to associate completing tasks with gaining reputation.

Better/professionally made assets

More animations to give life to employees

Varied level design layouts for improved communication between multiple players

Game modes?

#
## Gameplay

Video and pictures here
