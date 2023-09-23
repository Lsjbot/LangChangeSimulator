# LangChangeSimulator
Application for simulating language change and diversification over historical time.

A world map is seeded with a number of tribes with distinct languages. The tribes are then allowed to be fruitful and multiply and spread across the map, while their languages evolve and split up. The end result is a world with thousands of languages in a number of different families.

Linguistic processes included:

*    Languages represented as word lists, with meanings and word forms.
*    Regular sound change, conditional or unconditional.
*    Lexical borrowing fron neighboring tribes.
*    Word loss and word gain.
*    Areal effects, e.g. gaining a sound that is common in the region.

Non-linguistic processes included:

    Real-world geography with topography, climate, rivers and vegetation.
    World divided into 50x50 km grid cells.
    Each cell can be home to one or more tribes.
    Populations increase and decrease in each grid cell, depending on food availability.
    Food production depends on climate, weather, proximity to coast/river, and technology.
    Technology: each tribe starts paleolithic, and gains tech (e.g. boats or agriculture) at irregular intervals.
    Migration: if cell population exceeds carrying capacity, excess population may split off and migrate to greener pastures. Will travel until they find lebensraum or starve.
    Ease of travel depends on terrain and technology, and determines both migration distance and language-contact distance.
    If multiple tribes inhabit the same grid cell, the dominant one will gradually assimilate the others.

Output:

    Swadesh lists for the resulting languages.
    Word data in other formats, suitable for automated processing.
    True tree of descent.
    True cognate classes (taking into acount borrowing).
