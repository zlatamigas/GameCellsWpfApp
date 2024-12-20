# <img src="https://github.com/user-attachments/assets/a2618840-f2e2-40b2-a745-42c31549c423" width="30" alt="icon"/> Queen

## Description

Queen is a Desktop App inspired by [LinkedIn's Queens](https://www.linkedin.com/games/queens/).

The purpose of the game is to fill cells so that the game field is covered according to the rules. There are 2 game modes:

1. Pre-build leveles, where the game is stored in database as well as game results.
2. Generated levels, where the game is randomly created based only on size.

The application is created with WPF according to MVVM pattern and supports the followwing features: database connection, localisation, storing user settings. There are also some custom styles and animations.

Currently supported languages: English and Russian.

## Project stack
.Net 6 / WPF / EntityFramework / SQLite

## Usage

The App starts at the main menu with last saved language settings where the user can change some settings or choose one of the game modes.

<p align="center">
  <kbd> <img src="https://github.com/user-attachments/assets/3b02dc70-f98b-4e3f-9729-8572413f6569" alt="Main Menu" width="400" style="border-radius:10px"\></kbd>
</p>

To generate a new game the user needs to put down the desirable size of the game from 4 to 24 inclusive. The input is validated.

A prebuilt level is stored in database and can be completed only once. So be attentive!

<p align="center">
  <kbd> <img alt="Prebuilt Game from Databased" src="https://github.com/user-attachments/assets/52ca7d99-c12f-45e9-9d94-b280ddc9a230" width="340" style="border-radius:10px"\></kbd> 
  <kbd> <img alt="Generated Game" src="https://github.com/user-attachments/assets/ded74409-afcb-414b-8b81-31ecfa1e1c91" width="340" style="border-radius:10px"\></kbd> 
</p>
<p align="center">Prebuilt Game from Databased (1) and Generated Game (2)</p>
<br>

To complete the game the user must fill the game field according to the rules (can be found at settings and under every level). The game can be paused and left at any time. If the user leaves the game without completing it the current game state won't be saved.

If the new record is set, a special notification will be shown.

<p align="center">
  <kbd> <img src="https://github.com/user-attachments/assets/31fcec98-29b4-4c20-9fec-89a213a63deb" alt="Completed level" width="340" style="border-radius:10px"\></kbd> 
  <kbd> <img src="https://github.com/user-attachments/assets/cfbd6285-c352-42e4-8331-0c0d49d8f0ad" alt="New Score" width="340" style="border-radius:10px"\></kbd>
</p>


To change some setting and check the rules the user should visit setting. The selected settings will be applied immediately and will be saved for the next App launch.

<p align="center">
  <kbd> <img alt="Settings ENG" src="https://github.com/user-attachments/assets/b1adfa27-a884-48b7-8ef2-5aebf79db42f" width="340" style="border-radius:10px"\></kbd> 
  <kbd> <img alt="Settings RUS" src="https://github.com/user-attachments/assets/39272b3f-3593-41ee-a441-e8c0a441f247" width="340" style="border-radius:10px"\></kbd>
</p>
