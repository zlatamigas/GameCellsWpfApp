# Queen

<p align="center">
  <img alt="Queen" src="https://github.com/user-attachments/assets/a2618840-f2e2-40b2-a745-42c31549c423" width="300"\>
</p>

## Description

Queen is a Desktop App inspired by LinkedIn Queen.

The purpose of the game is to fill cells so that the game field is covered according to the rules. There are 2 game modes:

1. Pre-build leveles, where the game is stored in database as well as game results.
2. Generated levels, where the game is randomly created based only on size.

The application is created with WPF according to MVVM pattern and supports the followwing features: database connection, localisation, storing user settings. There are also some custom styles and animations.

Currently supported languages: English and Russian.

## Project stack
.Net 6 / WPF / EntityFramework / SQLite

## Usage

The App starts at the main menu with last saved language settings where the user can change some settiong or choose one of the game modes.

<p align="center">
  <kbd> <img alt="Main Menu" src="https://github.com/user-attachments/assets/16db39b8-315e-432c-b2ce-6b26a5d359d2" width="100^" style="border-radius:10px"\></kbd> 
</p>
<p align="center">Main menu</p>
<br>

To generate a new game the user need to put down the desirable size of the game from 4 to 24 inclusive. The input is validated.

<p align="center">
  <kbd> <img alt="Generated Game" src="https://github.com/user-attachments/assets/ded74409-afcb-414b-8b81-31ecfa1e1c91" width="100^" style="border-radius:10px"\></kbd> 
</p>
<p align="center">Generated Game</p>
<br>

A prebuilt level is stored in database and can be completed only once. So be attentive!

<p align="center">
  <kbd> <img alt="Prebuilt Game from Databased" src="https://github.com/user-attachments/assets/52ca7d99-c12f-45e9-9d94-b280ddc9a230" width="100^" style="border-radius:10px"\></kbd> 
</p>
<p align="center">Prebuilt Game from Databased</p>
<br>

To complete the game the user must fill the game field according to the rules (can be found at settings and under every level). The game can be paused and left at any time. If the user leaves the game without completing it the current game state won't be saved.

If the new record is set, a special notification will be shown.

To change some setting and check the rules the user should visit Setting. The selected settings will be applied immediately and will be saved for the next App launch.

<p align="center">
  <kbd> <img alt="Settings" src="https://github.com/user-attachments/assets/5611c02a-9226-4347-89d4-ac870d386221" width="100^" style="border-radius:10px"\></kbd> 
</p>
<p align="center">Settings</p>
<br>
