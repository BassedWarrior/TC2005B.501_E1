# User Stories

## Gameplay & Strategy

- **Play Cards [High] (2 Sprints)**
    - *User Story:* As a player, I want to drag cards from my hand to the 
        battlefield so that the card is in play.
    - *Validation:* Given a card in my hand, when I hold click on a card and 
        drag my mouse to the battlefield and release it, then the card will be 
        played.
- **End Turn [Mid] (1 Sprint)**
    - *User Story:* As a player, I want to click a button such that my turn 
        ends.
    - *Validation:* Given it's my turn, when I click on a button, then my turn 
        will end.
- **Play spells on units [Mid] (1 Sprint)**
    - *User Story:* As a player, I want to be able to click on a unit when
        playing a spell card such that the spell targets that unit.
    - *Validation:* Given that I have units in play, when I play a spell card 
        that targets a unit and click on a unit, then the spell is applied to 
        that unit.
- **Build Starting Hand [High] (1 Sprint)**
    - *User Story:* As a player, I want to be able to click on the cards of my 
        starting draw such that I can choose my starting hand.
    - *Validation:* Given that I draw a starting hand at the beginning of the 
        game, when I click on the cards I don't want to keep, then new those 
        cards will be reshuffled into my deck, and new cards will be drawn to 
        replace them and form my starting hand.
- **Last Stand [High] (1 Sprint)**
    - *User Story:* As a player, I want to keep playing such that I can play 
        with no cards left in my deck or my hand.
    - *Validation:* Given that I have no cards in my deck and hand, when I 
        have units on the board, then I can keep playing as long as I don't 
        lose all my health points.

## User Interface (UI)

- **Main Menu [High] (1 Sprint)** 
    - *User Story:* As a player, I want to have a menu to start the game.
- **Resources [Low] (1 Sprint)** 
    - *User Story:* As a player, I want a graphic to see how much resources I 
        have accumulated.
    - *Validation:* Given that I am in the middle of playing the game, when I 
        look at the screen, then I can see a numerical representation of the 
        resources I have accumulated.
- **Hand [High] (1 Sprint)**
    - *User Story:* As a player, I want to see the cards in my hand.
    - *Validation:* Given that I have cards in my hand, when I look at the 
        screen, then I can see the distinct cards that are in my hand.
- **Detailed Hand [Low] (1 Sprint)**
    - *User Story:* As a player, I want to hover the mouse over the cards to 
        bring them up and make them bigger to read them more easily.
    - *Validation:* Given that I have cards in my hand, when I hover the mouse 
        over the cards in my hand, then the card my mouse is hovering above 
        will grow in size and come to the front of all other cards in my hand.
- **Deck Cards [High] (1 Sprint)**
    - *User Story:* As a player, I want to see my deck on the screen such that 
        I can see how many cards are still in my deck.
    - *Validation:* Given that I have cards remaining in my deck, when I look
        at my deck, then a numerical graphic should be visible showing the 
        number of cards remaining.

## Deck Building

- **Deck menu [Mid] (2 Sprints)**
	- *User Story:* As a player, I want to have a menu such that I can build my 
        deck.
	- *Validation:* Given that I am in the main menu, when I click on a button 
        to customize my deck, then a menu is displayed where I can build my 
        deck.
- **Deck selection [Mid] (2 Sprints)**
	- *User Story:* As a player, I want to be able to click on cards from the 
        collection such that they get added or removed from my deck.
	- *Validation:* Given the available cards when I click on cards from my 
        deck, then they get removed from my deck, and when I click on cards 
        from my collection, then they get added to my deck.
- **Card filter [Low] (3 Sprints)**
	- *User Story:* As a player, I want to be able to filter the cards from my 
        collection such that only the cards of a certain type are.
	- *Validation:* Given I'm editing my deck, when I click to select a filter, 
        then only the cards with the characteristics I specified are shown in 
        the collection.
- **Card details [High] (2 Sprints)**
	- *User Story:* As a player, I want to be able to right-click on cards from 
        my collection or from my deck such that I get a detailed view of the 
        card.
	- *Validation:* Given I am editing my deck, when I right-click on a card, 
        then a detailed view with the card information is shown.

## Client Statistics

- **Strategy planning [Mid] (5 Sprints)**
	- *User Story:* As a client, I want to be able to see the game analytics to 
        determine the Most Effective Tactic Available (M.E.T.A.) of the game.
	- *Validation:* Given that I am on the game webpage, when I click on a 
        button to view statistics, then a menu will show the frequency and win 
        rates of the cards.
- **Node.js server [High] (5 Sprints)**
	- *User Story:* As a client, I want to be able to host the game locally 
        using a node.js server.
	- *Validation:* Given that I have the complete game installed, when I open 
        the main page, then I want to be able to play without any 
        inconvenience. 
