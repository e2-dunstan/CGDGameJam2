# Foundry Frenzy Post Mortem

#### The Boomers: Ellie, Alex, Dan, Ben, Lewis, Sol

#
## Teamwork and Project Development

### GitHub

Github this time around was a little more lax. Due to project being more focused on polish we weren't immediately sure on what should be polished and if it did how. This resulted in people working on their systems after our initial meeting and once they were done just slowing add things as we thought appropriate. In hindsight after we got the core systems implemented we should've had a group meeting to decide on what polish we were going to add to what systems and update Github accordingly

### Communication

Communication this time around was better overall. Members of the team were more proactive at checking slack and letting eachother know if there was going to be issues. We also worked together as a group almost every time we worked on the jam. Meaning if discussions were needed in regards to specific systems, we could quickly talk them out instead of waiting for replies on slack.

#
## The Game

### Code

Task system

Reputation system

The Reputation system managed the difficulty curve of the game. The player starts off at zero reputation and with a minimum amount of employees. As the player completes tasks in quick succession the reputation of the foundary goes up. This leads to the player getting access to more employees to allot to tasks and tasks that are harder in difficulty but reward more score. At all times the reputation of the player is degrading and once it goes below a tiers threshold then the bonuses gained from that tier are lost.

AI

Touch system

Each touch is stored in a pooled list of touches with elements such as their movement phase, what they're tapping, etc tracked without the game getting confused about which touch is which.

### Design

AI

Touch Input

Level Design

Since we wanted the game at its core to be a collabrative experience we built the level it. The center of the level is where the employees idle. Being in the center of the screen makes it easy to reach for any player surrounding the touch screens we were briefed to work with. Then accepting and handing off tasks is to the left and work areas are on the right. This meant that if there are two players one can focus on using employees from the center to accept and hand off tasks and the second player can focus on alloting employees to working on tasks. If we wanted to make it more chaotic in the future we could mix these placements up requiring players to reach all around the screen and get in each others way. 

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
