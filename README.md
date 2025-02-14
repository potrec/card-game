# Recruitment task - Card management (2D)

## Task description

The task is designed to test the ability to program decks and manage animation in a game using card game mechanics.

The following game flow is implemented in the application:

1. **Shuffling the initial deck:** At the beginning of the game, a deck of cards is created, which is then shuffled.
2. **Drawing cards for hand:** From the shuffled deck, a certain number of cards (X) are drawn for the player's hand.
3. **Playing cards:** The player is allowed to play cards from his hand onto the table. After playing, the card goes to the discard pile (discard pile).
4. **End of turn:** The player's turn ends after playing any number of cards.
5. **Drawing cards after a turn:** At the beginning of the next turn, the player draws a card(s) from the deck so that he always has X cards in his hand at the beginning of the turn.
6. **Repeat cycle:** The game continues, returning to point 3, allowing cards to be played in subsequent turns.

## Functionality implemented.

- **Data structures for cards and deck:** Data structures have been created to represent a card and a deck of cards.
- **Display of cards on hand:** The cards on the player's hand are dynamically displayed on the screen.
- **Playing cards (Drag & Drop):** The implementation of the (Drag & Drop) mechanism allows intuitive playing of cards from the hand to the table. The player can drag a card from his hand and drop it in a designated area on the table, which simulates playing a card.
- **Drawing cards at the end of a turn:** At the end of a turn, cards are automatically drawn from the deck to replenish a player's hand to the desired number of cards.
- **Dragging cards between hand slots (changing the order of cards):** The user has the ability to change the order of cards in the hand by dragging cards within the hand.
- **Animations for playing cards:** A simple animation for playing a card (moving a card from the drop point to the table) has been added.
- **Returning a card to the hand on cancellation of play:** If a card is dragged but not dropped in the play zone, it returns to its original place on the hand. This prevents the card from being accidentally played and improves the user experience.
- **Illumination of the currently selected card:** When you hover the cursor over a card, the card is ejected and expanded.

## Additional features

- **Mana System** - playing a card costs mana points, which are renewed every turn. If the player does not have enough mana, the played card is returned to the hand.
- **Visualization of the deck and discarded cards** - visualization of the deck and discarded cards has been added.

## Technologies used.

- Unity.
