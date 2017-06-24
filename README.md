# GTA Businessmate
![Businessmate](https://i.imgur.com/StnqFIG.png)
GTA Businessmate is a program that can assist CEOs, VIPs, and Presidents in GTA Online with keeping track of what their businesses are actually doing, as well as keeping an eye on their cooldowns. It shows products and supplies of businesses without the player having to physically run to them, and smoothly updates these amounts as opposed to the confusing and inconsistent "ticks" that are used in the game. It is composed of a series of timers that the player must keep synchronized. In this way, it is merely an external organizational tool with zero hooks into the actual game - similar to a stop watch, or a phone alarm. To be clear - Businessmate is not a mod. Below we will go over the features and functionality of each of the timers.

# Top Bar
The top of Businessmate has two buttons to start and stop the program. You want to stop the program if you are not playing GTA Online, and start it when you are. This will stop all businesses and cooldowns from ticking when you are not playing the game. There is also a suggested activity up here, but we will get to that later.

# Business Timers
Businesses are represented with two timers which can be controlled with a slider. The blue product slider ticks upwards and shows how much product the business is generating. The green supply slider ticks downward and shows how many supplies are left. Each business ticks at the same rates as the actual businesses, so once synchronized this can be used to keep an eye on the progress of your businesses without physically going to them. The state of these sliders is saved if Businessmate is closed.

To synchronize a business, you must click and drag the slider for both product and supplies to the same position of those bars on the business in-game. To assist in making this process easier, when you drag the product slider, it will display the value of that product in-county, which is the same as the number displayed on the bottom right of your screen when you enter a business. Similarly, when you drag the supply slider, it will show how many "bars" of supply that represents, which you can see by checking the computer terminal in each business except the Gunrunning Bunker. Once synchronized, the business will tick normally, but you must keep it up to date in a few ways.

If at any point you start a product sale, click the blue "Sell" button, which will reset your product timer. Click the green "Resupply" button when purchased supplies arrive to fill that bar.

Businesses will not tick unless Businessmate itself is active, and the business is active. Activate or deactivate a business by checking the "Active" box on that business. Inactive businesses will not tick and will fade slightly so that you can ignore them.

You will notice small tick marks on the product and supply bars. The tick mark on the blue product bar is the sale threshold. The sale threshold can be configured, but by default, it is the product level at which a sell mission advances to two vehicles instead of one. Businessmate will warn you several minutes prior to hitting a sale threshold, so this helps solo players out. Non-solo players can pop open the configuration file and raise the sale threshold to whatever they like. The tick mark on the green supply bar is the resupply threshold. Resupplying above this amount wastes money, so wait for the supplies to fall below this amount before purchasing supplies.

# Cooldown Timers
Cooldowns are represented with a single timer, which can also be controlled with a slider. If at any point you complete or fail a mission, or perform an action associated with a cooldown, click the "Reset" button to start the timer over. If you drag the slider, it will show you a specific time so you can synchronize it if necessary. Cooldowns are not saved when Businessmate quits. The reason for this is because unlike businesses, cooldowns continue to tick when you are not playing GTA Online. They are mostly represented in Businessmate for convenience.

# Suggested Activity
The suggested activity will attempt to reconcile the state of all of the player's businesses and cooldowns, and decide what the most efficient thing the player could possibly be doing is. It will prioritize things in this order:

- Sales at businesses that are about to pass the sale threshold.
- Resupplies at businesses that are about to run out of supplies.
- Cooldowns that are ready.
- Short cooldown VIP Work, CEO Crates, Vehicle Imports, Filling Garages, etc.

Notes that as it prioritizes businesses, it will always prioritize lucrative businesses over less lucrative businesses, so cocaine over documents, for example. It will also attempt to warn the player well in advance of something happening, and bump the priority of that as the deadline gets closer. For example, a resupply is less important if we run out of supplies in half an hour as it is if we run out in 15 minutes. Another example is that if the player has nothing better to do, it will suggest a cooldown five minutes before the cooldown is actually available to allow for travel time. As always, these values are all configurable.

# Configurability
If you look in the GTABusinessmate_Data directory, you will find Configuration.json. This file can be opened and modified to configure your sale thresholds, resupply thresholds, business tick rates, warning lead times, or whatever else you desire. By default, the settings are aimed at a solo player, but this allows for a lot of flexibility if people want to use Businessmate for non-solo play. It also helps if the business tick rates change in a patch in the future.

# Have Fun
Following the suggested activity is a great way to make some good money. You no longer have to keep an eye on your businesses and can safely juggle eighteen businesses, gun running, and vehicle exports as a solo player. Awesome. Try to remember to have fun somewhere in there though. Businessmate distils the grind down to its purest form, and while this is super efficient, it's not necessarily super fun. Remember to ignore Businessmate at some point and go pop a few Hydras in your Oppressor.

# Disclaimer
Businessmate is an external organizational tool that the player must manually keep synchronized with his game. This means it is no different than a phone alarm, or a pencil and paper beside your monitor. It's just a bit easier to use. It has zero hooks to read or write data into the game executable in any way, shape, or form.

That said, Rockstar and Take Two tend to be rather unpredictable at the best of times, and I am neither an oracle or a lawyer. Therefore I hereby cover my ass and absolve myself of all damages that might arise from your usage of Businessmate. I use it myself, and there is no conceivable reason that it wouldn't be safe, but whether your computer explodes or you get banned from GTA Online, or whatever, I am not responsible.

# License
MIT License

Copyright (c) 2017 Arsonide

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.