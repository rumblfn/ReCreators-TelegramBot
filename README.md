# Welcome to the Bot

Production version @BestBotEarthHadEverSeenBot <br>
This bot is hosted on [TimeWeb](https://timeweb.com/) using Docker.

## View Architecture

To view the architecture, please check out the [architecture diagram](https://miro.com/app/board/uXjVNgyRMzI=/?share_link_id=431178708886).

## Setting Up Secrets

Before starting the bot, you need to set up your secrets. Follow these steps:

1. Copy the contents of `.env.example`.
2. Create a new files named `.env.dev` and `.env.prod` in the same directory.
3. Replace the placeholder values with your actual secrets.

## Running the Bot

To start the bot in production mode (with `.env.prod`), simply run the following command:
```bash
docker compose up --build
```

To start the bot in development mode (with `.env.dev`), simply run the following commands:
```bash
cd TelegramBot
dotent run
```