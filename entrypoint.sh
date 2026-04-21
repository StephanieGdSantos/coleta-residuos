#!/bin/bash
echo "⏳ Waiting 2 minutes before starting the application..."
sleep 120
echo "✅ Starting the application now!"
exec "$@"
