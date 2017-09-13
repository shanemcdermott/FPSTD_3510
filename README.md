# FPSTD_3510
Group Project for CSCI-3510: Advanced Game Programming

Game outline:

objective: defend a base against waves of enemies using weapons and placeable towers that create your own path for the enemies.

Player:
traits: move speed, shoot/reload speed, damage modifier, model/animation, projectiles, collider, sound
Guns- choose 2 from: Assualt Rifle, Grenade Launcher, Sniper Rifle, Shotgun
currency- each tower placement, wall placement, and upgrades cost currency that is gained from killing enemies
Tower Tech tree- Damage, Attack speed, Range
Player Tech tree- Speed, Damage, Reload Speed modifier, Magazine Capacity, HP

weapon traits:
-damage/ fire rate / range
-projectiles
-model/animation
-reload speed
-sound

Towers:
traits- Damage, radius, range, attack speed, model/animation, projectiles, placement locations, collider, sound
types- Slow(pulse field), Area of Effect(grenade tower), Machine gun, Single shot cannon
focus- first, last, strongest, weakest

walls:
traits- model, placement locations

Wave:
-number/type of mobs

level:
-number of waves
-map of placeable tiles and obstructions


enemies:
traits- move speed, attack speed, damage, model/animation, [projectiles], collider, sound





Level Outline:
1. spawn map
2. spawn player at home location
3. let player set up walls/towers/guns until round start button is pressed
4. spawn monsters at an interval, who follow an AI pattern
5. update wave score
6. display wave/level score and currency

Order of Operations:
1. create map for level 1
2. create basic UI (main menu, pause menu, load menu, end of level/score menu, in game HUD)
3. create gun script/ select model
4. create player script/ select player model
5. create monster AI/ stats/ models
