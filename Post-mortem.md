# Foundry Frenzy Post Mortem

#### The Boomers: Ellie, Alex, Dan, Ben, Lewis, Sol

#
## Teamwork and Project Development

### GitHub

GitHub usage this time around was not as disciplined. Due to the project being more focused on polish, we weren't immediately sure about what should be polished and how. This resulted in people working on their systems after our initial meeting and moving on to add things as we thought appropriate. In hindsight, after the core systems were implemented, we should've had a group meeting to decide on what polish we were going to add to what systems and update GitHub accordingly. Milestones were added to the issues board as per improvements in the previous jam; however, these were largely ignored. This may have been due to priorities not being as evident in this project; however, they still should have been followed as a guideline for how long a task should take. 

### Communication

Communication this time around was better overall. Members of the team were more proactive at checking Slack and letting others know if there were problems. We also worked together as a group almost every time we worked on the jam. This ensured that if discussions were needed in regards to specific systems, we could quickly talk them out instead of waiting for replies on Slack. Despite this, the team would benefit from further communication issue progress and estimated completion time.

#
## The Game

### Code

The Reputation system managed the difficulty curve of the game. The player starts at zero reputation and with a minimum amount of employees. As the player completes tasks in quick succession, the reputation of the Foundry goes up. This leads to the player getting access to more employees to allot to tasks and tasks that are harder in difficulty but reward more score. At all times the reputation of the player degrades. Once it goes below a tiers threshold, the bonuses gained from that tier are lost. This was not clearly communicated and is a crucial area of future improvement.

The AI (employees) made use of Unity's NavMesh system for their pathfinding while their animations were controlled through Unity's Animator and its parameters. Unfortunately, there were a few issues with the AI with regards to setting their path. If a player dragged to a spot which was not on the NavMesh (i.e. a table blocking the floor) the visual effects and animations of an employee preparing to move to that position would have activated, but they would not move there. The cause of this is unclear. However, the whole system could have been scripted more appropriately, and it might have been avoided if multiple people or someone more experienced in AI programming worked on the task.

A PlayerTouch class was created to store all elements needed for an individual touch, such as their movement phase and what they're tapping. By having each touch contained in this way, it ensured that there was no confusion in the code over which touch was which. The method of dragging a character to set their path was a conscious decision. It made it much easier to handle multi-touch if a press and release reflected a touch.

### Design

Level Design

Since we wanted the game at its core to be a collaborative experience, we built the level it. The centre of the level is where the employees idle. Being in the centre of the screen makes it easy to reach for any player surrounding the touch screens we were briefed to work with. Then accepting and handing off tasks is to the left and work areas are on the right. This meant that if there are two players, one can focus on using employees from the centre to accept and hand off tasks, and the second player can focus on allocating employees to working on tasks. If we wanted to make it more chaotic in the future, we could mix these placements up requiring players to reach all around the screen and get in each other's way. 

Task System

UI - don't talk about the assets but do talk about their placement.

As the theme of this Games Jam was about Juice and Polish, UI was a much larger focus this time around. The Surge library was imported into the project to ensure a tweening system could be used for managing all UI animations with unique easing and animation curves. This was primarily used for spawning and despawning UI elements and could be used outside of Unity's global time. Additional effort was made to ensure UI elements had correct ordering, resized correctly for varying screen resolutions and were parented to follow the employees correctly. Each UI element that required to interact with other parts of the code base had interface functions built to populate the UI element. Still, the delay in tween animations sometimes leads to problems with spawning tasks. Also, the text font used, while retro-styled, can be challenging to read at times due to letter spacing. 

Pathing line renderer

Particle systems

Audio

#### The Foundry

Level design: open, Foundry colour palette

The first concept of the game was to be competitive with a split-screen where two teams battled to run the best Foundry, however, upon evaluation, we felt it would be more reflective of the Foundry values of connectedness and teamwork if it were cooperative.

#### Bartle Player Types

The socialiser is the most common Bartle player type. When combined with the game being multi-touch and cooperative, it made sense that people who fall under this category were our primary target audience. Additionally, the game also appeals to the achiever, with rewards and benefits from doing well in the game reflected by the reputation tracker. The other two types - killers and explorers - were not catered to by design. An expansion of the game could incorporate these, such as a competitive mode (cooperative default) or unlocking additional features such as more rooms or room types.

#
## Improvements

Further visual improvements should be made to the existing juice effects to convey more information to the player. For example, when drawing a path for an employee, the path could change colour to indicate whether the destination is valid or not. As the employees tend to sit down, they should emit an effect to inform the player that this employee is available to control. With better planning, more polished juice effects like these could have been produced earlier in the project.

In terms of UI, the "reputation" system could have been displayed with a variation of the progress bar so players can visually see the reputation fill up / decrease over time. This could be combined with additional UI particles that show stars travelling from the employees to the reputation bar, allowing players to associate completing tasks with gaining reputation. A script for this behaviour was created; however, it did not make it into the final build.

The game would also benefit from more professional and fitting assets - including models, textures and animations - which are not possible to acquire in 2 weeks.

Varied level design layouts for improved communication between multiple players

#
## Gameplay

Video and pictures here
