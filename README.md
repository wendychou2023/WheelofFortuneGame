This is an individual assignment in C#.

Description of how the game works:
1. Every time you start the project, you can see a wheel with four distinct colors and the space that each color occupies is randomized.
(For some unknown reasons, the lower part of the buttons is blocked, and I could not fix this problem.)
   
2. Click “Start.” “Start” is the button on the left. 
After clicking “Start,” the wheel starts spinning. It does not stop unless “Stop” button is clicked.

3. Click “Stop.” “Stop” is the button on the right.
After clicking “Stop,” the wheel decelerates. The wheel does not stop abruptly right after “Stop” is clicked. The money that the player has does not change before the wheel stops completely.
      
4. The wheel stops at different colors results in different prizes. The position where the wheel stops at is determined by the purple line pointing to 12 o’clock. Initially, the player has $1000.
If the wheel stops at Green, the player’s wallet remains the same.
If the wheel stops at Blue, the player’s wallet adds $10.
If the wheel stops at Red, the player’s wallet adds $100.
If the wheel stops at Gray, the player’s wallet minuses $500.
 
5. If the player’s wallet has less than or equal to $500 and the wheel stops at Gray, a message box saying “You don’t have money! Game Over!!” appears. Right after the player clicks “確定,” everything, including the wheel and the wallet, is reset, so that a new wheel with randomized color sectors occurs and the player has $1000 in the wallet again. The player can start playing the game.
     
