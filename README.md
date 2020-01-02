# AutoPrint

## Overview

AutoPrint is a simple .NET Core utility to help keep your inkjet printer's print heads from getting clogged.

AutoPrint will print to the default printer.

## Requirements

### Windows

This should run on a stock Windows install.

### macOS

`libgdiplus` from the Mono project is required. Install using `brew install mono-libgdiplus`.

### Linux

`libgdiplus` from the Mono project is required. Install on most distros with `sudo apt-get install libgdiplus`.

## Basic Usage

Checkout, build & run using standard .NET Core commands:

- `git clone https://github.com/willson556/autoprint.git`
- `cd autoprint`
- `dotnet run`

Schedule using Cron, Systemd, Windows Task Scheduler. Place the paper back into the printer between each scheduled run until it's full.

## Docker

The provided docker image is set to run AutoPrint every day at 8:00 AM. Replace the provided URI with your printer's IPP (or some other CUPS-compatible) URI.

```sh
docker pull thomaswillson/autoprint
docker run -d \
           --name autoprint \
           -e PRINTER_URI=https://192.168.1.138:631/ipp/print \
           thomaswillson/autoprint
  ```
