#!/bin/bash

# Script to create a clean public repository without sensitive history

echo "Creating clean public repository..."

# Step 1: Create new directory
cd ..
mkdir HL7endpointTCPlistener-public
cd HL7endpointTCPlistener-public

# Step 2: Initialize new git repository
git init
git branch -M main

# Step 3: Copy all files except .git directory
echo "Copying files..."
cp -r ../HL7endpointTCPlistener/* . 2>/dev/null || true
cp ../HL7endpointTCPlistener/.gitignore . 2>/dev/null || true
cp ../HL7endpointTCPlistener/.github/copilot-instructions.md .github/ 2>/dev/null || true

# Step 4: Add and commit as initial release
git add .
git commit -m "Initial release: HL7 TCP to HTTPS bridge service

Features:
- TCP to HTTPS message forwarding
- Windows service implementation  
- Configurable endpoints and authentication
- Event logging and error handling
- MSI installer included"

echo "Clean repository created successfully!"
echo "Next steps:"
echo "1. Create new repository on GitHub"
echo "2. git remote add origin https://github.com/yourusername/HL7endpointTCPlistener.git"
echo "3. git push -u origin main"
