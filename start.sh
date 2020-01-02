#! /bin/sh

# Start CUPS
cupsd

# Add printer
lpadmin -p defaultPrinter -v $PRINTER_URI -m everywhere

# Run Cron
/usr/sbin/cron -f