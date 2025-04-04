# Rubik's Cube

![Example](https://i.imgur.com/z2ZV0DE.gif)

## Description

A small project I started to better understand and visualize the process of solving Rubik's cube. Play it [here](https://deyama.itch.io/rubik)

## Controls

### Mouse

- _Left button click and drag_ - rotate cube face
- _Right button drag_ - rotate camera around the cube
- _Scroll wheel_ - zoom
- _Middle button click_ - reset camera

### Keyboard input

- _F_, _B_, _R_, _L_, _U_, _D_ - Rotate respective faces of the cube
- _X_, _Y_, _Z_ - Rotate cube
- Hold _Left Shift_ to invert direction of the rotation
- _Ctrl+Z_ - Undo last move

### Onscreen controls

- Rotate respective faces with '_Front_', '_Back_', '_Left_', '_Right_', '_Up_', '_Down_' buttons
- Tick '_Counterclockwise_' box to rotate counterclockwise
- Use '_X_', '_Y_', '_Z_' buttons to rotate the whole cube itself on respective axis
- Use '_Undo_' to revert last move
- Use '_Undo all_' to revert all moves according to the move history
- Use '_Clear history_' to clear move history
- Use '_Scramble_' to randomize the cube
- Use '_Seq_' buttons to perform predefined sequences of moves, required to solve the cube 

## Features

- All rotations performed through onscreen controls or a keyboard are relative to the camera. However they get written to the move history in accordance with cube's initial orientation
- Scramble functionality
- History of moves with '_Undo_', '_Undo all_' and '_Clear history_' functionality
- Predefined sequences of moves

## Features to implement

- Add solving guide
- Add descriptions for each of the sequences
- Add sound effects
