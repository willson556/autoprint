#! /bin/sh

# Start CUPS
cupsd

# Add printer
lpadmin -p myPrinter -v $PRINTER_URI -m everywhere
cupsaccept myPrinter
cupsenable myPrinter
lpoptions -d myPrinter

# Run Cron
/usr/sbin/cron -f