# COMP3000- Jonathan Andrews
## Project: 
Developing a Turn-Based Combat Game as a Tool for Strategic Skill Acquisition

## Plans & Peril
A turn-based combat system featuring strategic ability interactions, status effects, and scalable enemy AI designed for strategic skill development.

### Supervisor: 
Rory Hopcraft

## Play here
https://jandrews004.itch.io/plans-peril
---

## Project vision:
This project is for primarily young learners, whose problem is learning key skills traditional education struggles to teach in a fun and practical format. Plans & perils is a game that teaches long-term strategies and short-term tactics for a group with different strengths and weaknesses. It also teaches the importance of planning before committing to actions. It will teach them how to utilise these to perform the best against challenging and varied situations. The turn-based structure encourages this with thoughtful decision making and allows players to experiment with different approaches. The level-up and ability system allows players to pick and choose a strategy they think is best. This will help players to try a variety of strategies to think more broadly about options that may not be their favourite but are better in certain situations. The art-style will be pixel art based as this will have an aesthetic more suited to a younger audience but can also be enjoyed by older players. The enemies will have different levels of strategy to challenge the player with their strategic thinking to find flaws in strategies and utilise them effectively. It will also be designed in a way that all the controls are very basic so that it is easier for players to learn how to play with the possibility in the future to port to different platforms (e.g. mobile) to allow a wider range of people to play. 


## Overview
This project is a game where players must defeat enemies of varying degrees of difficulty ranging from random actions to coordinated attacks. The player will have a basic gameplay loop of entering a 'dungeon' defeating all the enemies within while making short- and long-term strategies. These can also be refined in the hub world where new abilities can be bought and allows the player to make a team fitting their strategy. Each character the player controls will have stats that also contribute to different aspects of gameplay such as damage or defence, making even more possibilities for numerous strategies possible. All these actions will be diligently designed and evaluated not just implemented. Throughout the game players must formulate strategies to use during combat by choosing what abilities to equip. They will need to evaluate how these abilities synergise to be able to improve their strategies over time. They will then test these strategies to ensure they work effectively in the combat scenarios and determine how they could be improved. This project is a technical implementation involving systems, architecture, testing, and evaluation.

---

## Core systems
### Combat system
 * Turn order decided by player
 * Action system with abilities, cooldowns and uses
 * Alternating player and enemy phases

### Ability system
 * Wide range of status effects (e.g buffs, debuffs, DOT, guard, reflect etc)
 * Applied and self-managed with effect class - ticks per round and is removed at 0

### Enemy AI
 * Three levels of AI (Easy, Medium, Hard)
 * Each level has different strategic capability
 * Easy - random targeting, Medium - picks best ability, Hard - picks best ability target combo
 * Score based decisions

### UI system
 * MVVM architecture for charcacter combat UI
 * Interacts with hidden buttons on character and targets for selections
 * Feedback with FX (sprite flashes, particles, SFX)

## Demo
https://youtu.be/Lhz3LPvCMnE

---

## Technical design

### System Architecture
The combat system is structured around a central Combat Manager, which controls the flow of battle through a defined turn cycle. Each combatant (player or enemy) is treated as an independent entity that exposes actions to the system rather than directly controlling flow.

Turn progression is handled through a deterministic sequence:
 * Initiative/turn order is calculated at the start of combat (based on speed or fixed order depending on design choice)
 * The Combat Manager selects the next active entity
 * That entity resolves an action (attack, ability, item, etc.)
 * Control returns to the manager to advance the turn

This ensures the system remains predictable and extendable, allowing new mechanics to be inserted without rewriting core logic.

### Ability System Design
Abilities are implemented as modular data-driven objects rather than hardcoded logic. Each ability defines:
 * Target type (single, multiple, self)
 * Cost (turn restriction)
 * Effect logic (damage, healing, status application)
 * Optional conditions (passive or active ability, element synergies etc)

This separation allows abilities to be reused across different entities without duplication, supporting scalability and rapid iteration for balancing.

### Status Effect System
Status effects are managed through a centralized effect handler attached to each combatant. Effects operate on a turn-based tick system, meaning they resolve at defined points in the combat loop (e.g. start or end of turn).

Each effect contains:
 * Duration
 * Trigger timing (end of appliers turn)
 * Effect logic (damage over time, stun, buffs, etc.)

This avoids tightly coupling effects to abilities or characters, ensuring they remain reusable and stackable.

### Enemy AI Design
Enemy behaviour is implemented using a rule-based decision system, where actions are selected based on weighted priorities rather than random choice.

Each AI evaluates:
 * Player health state
 * Self health state
 * Available abilities
 * Status conditions

Actions are then scored and the highest priority action is selected. This approach provides predictable but adaptable behaviour, making the AI easier to balance compared to fully reactive or complex planning systems.

---

## Tools and technologies
 * Unity 2021.3.38f1
 * C#
 * Unity event system
 * MVVM architecture

## How to run
All controls are through mouse and clicking. Start a battle by selecting a dungeon then start.

---

## Challenges & Design Decisions

### Managing Turn Order Complexity
One challenge was ensuring that turn order remained consistent while supporting future systems such as speed modifiers, status effects, and conditional turn skipping.

A fixed sequence system was initially simple, but limited flexibility. This was improved by abstracting turn order calculation into a dedicated step within the Combat Manager, allowing dynamic modification without altering core flow logic.

### Designing a Scalable Ability System
Early iterations of the ability system were tightly coupled to specific characters, making it difficult to reuse logic across multiple entities.

This was resolved by shifting to a data-driven approach, where abilities are defined independently from characters. This improved scalability but required careful handling of references and effect execution to avoid duplication or inconsistent behaviour.

### Status Effect Ordering and Consistency
A key issue was ensuring that multiple status effects applied in the same turn resolved in a predictable order.

Without a structured pipeline, effects could trigger in inconsistent sequences, leading to unpredictable combat outcomes. This was resolved by introducing a centralised effect resolution order, ensuring all effects follow a consistent lifecycle (apply → tick → expire).

### Enemy AI Predictability vs Difficulty
Balancing AI behaviour was challenging: purely random behaviour felt weak, while overly optimal decision-making made enemies unfair.

The solution was a weighted rule-based system, allowing controllable difficulty tuning while still maintaining readable behaviour for the player.

### System Coupling Between Combat Components
Early versions had tight coupling between UI, combat logic, and entity behaviour, making changes risky and slow.

This was improved by separating concerns:
 * Combat logic handled by Combat Manager
 * Entities exposed actions but did not control flow
 * UI acted purely as a listener to combat events

This reduced dependency issues and made the system easier to extend.

---

## Future Improvements
To develop this project further some new design features will have to be implemented. These mainly focus on assets to be made to increase clarity, for example by having more enemy types with specific elemental types of the player would be able to learn them and use the information in planning a strategy. Currently, with the limited assets available, this is limited to a few enemies but would be easy to expand with enemy variants. To further the skill development of the game some additional puzzles could be added. This would help players develop some skills used less in the combat section but still required for it. This would have to be researched and designed further which this project doesn't allow time for. Additionally, a longer form of testing is required to ensure that the aim of this project is met. Currently, relevant research suggests that this product will develop the correct skills however there is no concrete evidence. A longer testing phase is required for this, however the time limit and resources available for this project does not allow for this to occur.

---

## Credits

# Credits
All assets used under their respective licenses. Full credit to original creators.

---

## Art & Visual Assets

- Skill Icons — Free Pixel Art Skill Icons Pack by Quintino Pixels  
  https://quintino-pixels.itch.io/free-pixel-art-skill-icons-pack  

- Element Icons — Elemental Skill Icons by Malkas  
  https://malkas.itch.io/elemental-skill-icons  

- Hub World Tilemap — Pixel Art Village (Top-Down RPG Asset Pack) by ZedPXL  
  https://zedpxl.itch.io/pixelart-village-top-down-rpg-asset-pack  

- Dungeon Tilemap — Tilemap Mini Dungeon by Aztharis  
  https://aztharis.itch.io/tilemap-mini-dungeon  

- Monsters & Creatures — by Luiz Melo  
  https://luizmelo.itch.io/monsters-creatures-fantasy  

- Evil Wizard — by Luiz Melo  
  https://luizmelo.itch.io/evil-wizard  

- Fire Worm — by Luiz Melo  
  https://luizmelo.itch.io/fire-worm  

- Skull Wolf — by Atari Boy  
  https://atari-boy.itch.io/skull-wolf-pixel-art  

- Golems Pack — by Monopixelart  
  https://monopixelart.itch.io/golems-pack  

- Bringer of Death — by Clembod  
  https://clembod.itch.io/bringer-of-death-free  

- Shardsoul Sprites — by Elthen  
  https://elthen.itch.io/2d-pixel-art-shardsoul-slayer-sprites  

- Skull Icon — by Skalding  
  https://skalding.itch.io/skull-sprite-002  

---

## UI & Fonts

- RPG UI Pack (Buttons, Sliders, Slots) — by Franuka  
  https://franuka.itch.io/rpg-ui-pack-demo  

- UI Extension Pack — OpenGameArt  
  https://opengameart.org/content/ui-pack-rpg-extension  

- Pixel Art UI Elements — by Quintino Pixels  
  https://quintino-pixels.itch.io/pixel-art-ui-elements  

- Font — Ruler Font by Somepx  
  https://somepx.itch.io/free-font-ruler  

---

## Audio

- Hit Sound Effects — Pixabay  
  https://pixabay.com/sound-effects/film-special-effects-punch-and-hits-310521/  

- Death Sound Effects — Pixabay  
  https://pixabay.com/sound-effects/retro-blip-sound-04-474771/  

- Button Click Sound — Pixabay  
  https://pixabay.com/sound-effects/film-special-effects-video-game-hovering-menu-sfx-417443/  

- Background Music — Fesliyan Studios  
  https://www.fesliyanstudios.com/royalty-free-music/download/8-bit-s  
